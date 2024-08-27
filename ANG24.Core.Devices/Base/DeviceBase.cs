using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Interfaces;
using System.IO.Ports;

namespace ANG24.Core.Devices.Base
{
    public abstract class DeviceBase : IDevice
    {
        protected SerialPort _port;                                                       //непосредственно порт подключения
        private Timer _reconnectTimer;                                          //таймер для попытки подключиться снова
        public string _portName;                                              //именованный источник (имя порта)
        private readonly IDeviceBehavior _behavior;                             //хендл поведения устройства
        internal readonly ICommandBehavior _commandBehavior;                     //хендл поведения отправки команд устройством
        public List<IOptionalBehavior> _optionalBehaviors;                     //опциональные хэндлы, определяющие специфику поведения конкретного устройства
        List<ProcessAction> processedActions = new List<ProcessAction>();       //список действий-опций для пост-обработки (помимо основного действия)
        public string DeviceStatus { get; } = "None";
        public string DataBuffer { get; private set; } = string.Empty; //для хранения полученных сообщений по требованию    

        public event EventHandler? Connected;
        public event EventHandler? Disconnected;
        public event DREventHandler DeviceDataReceived;

        //device online state
        public bool Online => _port?.IsOpen ?? false;

        protected DeviceBase(IDeviceBehavior behavior, ICommandBehavior commandBeahvior)
        {
            _reconnectTimer = new Timer(Reconnect, null, Timeout.Infinite, Timeout.Infinite);
            _optionalBehaviors = new List<IOptionalBehavior>();
            _behavior = behavior;
            _behavior.SetDevice(this);
            _commandBehavior = commandBeahvior;
            _commandBehavior.SetDevice(this);
            _port = new SerialPort();
            _port.DataReceived += OnDataReceived;
        }

        protected void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_port.IsOpen)
            {
                try
                {
                    while (_port.BytesToRead > 0) //пока в буфере количество байт не будет равно 0
                    {
                        string cur_data = _port.ReadLine();     //чтение по строке
                        ProcessData(cur_data);                  //в зависимости от реализации, вызываем пост-обработку данных 
                        _behavior.HandleData(cur_data);         //в зависимости от поведения -- производим обработку данных
                        _commandBehavior.HandleData(cur_data);  //для обработчика команд -- то же самое
                        if (_optionalBehaviors.Count > 0)
                        {
                            foreach (var behavior in _optionalBehaviors)
                            {
                                behavior.HandleData(cur_data);
                            }
                        }
                        //экспериментальная функция, выполняет дополнительные действия, добавляемые и включаемые по требованию
                        if (processedActions.Count > 0)
                            foreach (ProcessAction action in processedActions)
                            {
                                action.Execute(cur_data);
                                if (action.ExecutedOnce)
                                    processedActions.Remove(action);
                            }
                        DeviceDataReceived?.Invoke(cur_data);
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
        }
        protected abstract void ProcessData(string data); //абстрактный метод для обработки полученных данных
        public virtual void SetCommand(string command)
        {
            try
            {
                if (Online)
                {
                    _port.Write(command); //попытка отправить данные
                    _behavior.RequestData(command);
                }
            }
            catch (InvalidOperationException ioex)
            {
                //когда порт закрыт (да, такое бывает)
                OnDisconnected();
            }
            catch (TimeoutException tex)
            {
                //когда время отправки команды истекло
                //(или когда входной буфер занят)
                Console.WriteLine("Timeout write command");
                //ничего не делаем
                //(технически, можно считать количество неудачных попыток)
            }
            catch (Exception ex)
            {
                //во всех остальных случаях 
                //считаем, что устройство сломалось
                OnDisconnected();
            }
        }
        protected void Execute(string command) => _commandBehavior.ExecuteCommand(command, null);
        protected void Execute(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings? settings = null)
        {
            _commandBehavior.ExecuteCommand(command, predicate, ifTrue, ifFalse, settings);
        }
        protected void Execute(string command, Func<string, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings? settings = null)
        {
            _commandBehavior.ExecuteCommand(command, predicate, ifTrue, ifFalse, settings);
        }
        protected void Execute(string command, IOptionalCommandBehavior behavior, CommandElementSettings? settings = null) => _commandBehavior.ExecuteCommand(command, behavior, settings);

        #region Option management
        protected void Option(string Name, Action<string> action, bool isExecutedOnce = false, bool Active = true) => processedActions.Add(new ProcessAction
        {
            Name = Name,
            ProcessedAction = action,
            ExecutedOnce = isExecutedOnce,
            Usage = Active
        });
        protected void PredicatedOption(string Name, Func<string, bool> predicate, Action<string> action, bool isExecutedOnce = false, bool Active = true) => processedActions.Add(new PredicatedProcessAction
        {
            Name = Name,
            Predicate = predicate,
            ProcessedAction = action,
            ExecutedOnce = isExecutedOnce,
            Usage = Active
        });
        protected void OptionClear() => processedActions.Clear();
        protected void OptionRemove(string Name)
        {
            processedActions.Remove(processedActions.First(x => x.Name == Name));
        }
        protected void OptionOff(string Name)
        {
            var r = processedActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = false;
        }
        protected void OptionOn(string Name)
        {
            var r = processedActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = true;
        }
        #endregion

        #region connection logic
        public virtual void Connect()
        {
            try
            {

                //примитивная реализация подключения
                if (!_port.IsOpen)
                {
                    _port.Open();
                    OnConnected();
                }
            }
            catch
            {
                StartReconnectTimer();
            }
        }
        public void Disconnect()
        {
            //примитивная реализация отключения
            try
            {
                if (_port.IsOpen)
                    _port.Close();
                StopReconnectTimer();
            }
            catch
            {
                //обрабатывать нечего пока, поэтому пусто  (29.07)
            }

        }
        protected async Task<bool> Find(string exceptedRequest, string exceptedResponce, int baudRate = 9600)
        {
            var name = await SerialPortFinder.FindDeviceAsync(exceptedRequest, exceptedResponce, baudRate);
            if (name == null) return false;
            if (name.Contains("device not found")) return false;
            _port.PortName = name;
            _portName = name;
            return true;
        }

        private void Reconnect(object? state) => Connect();
        #endregion 

        #region connect reactions
        private void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
            StopReconnectTimer();
        }
        private void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            StartReconnectTimer();
        }
        #endregion

        #region service methods
        internal void StartReconnectTimer() => _reconnectTimer.Change(2000, Timeout.Infinite);
        internal void StopReconnectTimer() => _reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
        #endregion

        public abstract void Ping();
    }

    public class ProcessAction
    {
        public string Name { get; set; }
        public Action<string> ProcessedAction { get; set; }
        public bool ExecutedOnce { get; set; }
        public bool Usage { get; set; }

        public virtual void Execute(string val)
        {
            if (Usage)
                ProcessedAction?.Invoke(val);
        }
    }
    public class PredicatedProcessAction : ProcessAction
    {
        public Func<string, bool> Predicate { get; set; }

        public override void Execute(string val)
        {
            if (Usage)
                if (Predicate != null && Predicate.Invoke(val))
                    base.Execute(val);
        }
    }

    public static class DeviceBaseExtensions
    {
        public static DeviceBase UseBehavior(this DeviceBase device, IOptionalBehavior behavior)
        {
            device._optionalBehaviors.Add(behavior);
            behavior.SetDevice(device);
            return device;
        }
        public static DeviceBase EnableBehavior(this DeviceBase device, string Name)
        {
            var item = device._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.On();
            return device;
        }
        public static DeviceBase DisableBehavior(this DeviceBase device, string Name)
        {
            var item = device._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.Off();
            return device;
        }
        public static T GetOptionalBehavior<T>(this DeviceBase device, string Name) where T : class, IOptionalBehavior
        {
            var item = device._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null)
            {
                return item as T;
            }
            else
            {
                throw new NullReferenceException($"Object {nameof(T)} was not found!!!");
            }
        }
    }
}

