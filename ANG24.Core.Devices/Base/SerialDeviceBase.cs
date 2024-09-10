using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ANG24.Core.Devices.Base
{

    //public delegate void ProcessDataEventHandler

    //public abstract class BlankDeviceBase
    //{

    //    public string DeviceStatus { get; set; } = string.Empty;

    //    public event EventHandler? Connected;
    //    public event EventHandler? Disconnected;
    //    public event DREventHandler DeviceDataReceived;
    //    public Action<object> ProcessData;
    //    protected BlankDeviceBase() { }
    //    public virtual bool Online { get; set; }
    //    public string Name { get; set; }

    //    protected virtual void Connect()
    //    {
    //        Online = true;
    //        Connected?.Invoke(this, new EventArgs());

    //    }
    //    protected void Disconnect()
    //    {
    //        Online = false;
    //        Disconnected?.Invoke(this, new EventArgs());
    //    }
    //    protected abstract void SetCommand(string command); //пока много вопросов по этому методу в данном классе, но оставлю

    //}

    //public abstract class SerialDeviceBase : BlankDeviceBase
    //{
    //    public override bool Online => _port?.IsOpen ?? false;

    //    protected SerialPort _port;
    //    private bool _autoDR;



    //    //для обобщения свойство вынесено в базу
    //    public Action<SerialPort> ReadProcessAction { get; set; }

    //    protected bool AutoDR
    //    {
    //        get => _autoDR;
    //        set
    //        {
    //            if (_autoDR != value)
    //            {
    //                if (value) OnAutoDR();
    //                else OffAutoDR();
    //            }
    //            _autoDR = value;
    //        }
    //    }

    //    protected SerialDeviceBase()
    //    {
    //        _port = new SerialPort();
    //    }

    //    private void OnAutoDR() => _port.DataReceived += port_DR;
    //    private void OffAutoDR() => _port.DataReceived -= port_DR;


    //    //реализация будет зависеть от способа чтения данных
    //    private void port_DR(object sender, SerialDataReceivedEventArgs e)
    //    {
    //        if (Online) DRProcess(); //не допускает чтение неподключенного устройства
    //    }
    //    protected abstract void DRProcess();
    //    /*
    //     * подключение -- примитивное;
    //     * если порт смог открыться -- считай подключились
    //     * только стоит учесть, что есть порты, которые могут открыться, но дальше пусто
    //     * поэтому метод виртуальный, предполагающий расширение функционала
    //     * 
    //     * 
    //     * by Prozorov
    //     */
    //    protected new virtual void Connect()
    //    {
    //        try
    //        {
    //            if (_port != null && !Online)
    //            {
    //                _port.Open();
    //                base.Connect();
    //            }
    //        }
    //        catch { }
    //    }
    //    protected new virtual void Disconnect()
    //    {
    //        try
    //        {
    //            if (_port != null && Online)
    //            {
    //                _port.Close();
    //                base.Disconnect();
    //            }
    //        }
    //        catch { }
    //    }
    //    protected virtual void Reconnect()
    //    {
    //        Connect();
    //    }
    //    protected virtual async Task<bool> Find(string exceptedRequest, string exceptedResponce, int baudRate = 9600)
    //    {
    //        var name = await SerialPortFinder.FindDeviceAsync(exceptedRequest, exceptedResponce, baudRate);
    //        if (name == null) return false;
    //        if (name.Contains("device not found")) return false;
    //        _port.PortName = name;
    //        //_portName = name;
    //        return true;
    //    }

    //    protected override void SetCommand(string command)
    //    {

    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }
    //    }
    //    protected virtual void SetCommand(byte[] command, int offset = 0, int count = 1)
    //    {

    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command, 0, count);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }

    //    }
    //    protected virtual void SetCommand(char[] command, int offset = 0, int count = 1)
    //    {
    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command, 0, count);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }
    //    }
    //    public void SetReadProcess(Action<SerialPort> action) => ReadProcessAction = action;

    //}
    //public class StringSerialDeviceBase : SerialDeviceBase
    //{
    //    public StringSerialReadMode ReadMode { get; set; } = StringSerialReadMode.Line;
    //    public string TerminatorString { get; set; } = "\n";
    //    public int ReadCount { get; set; } = 1;

    //    protected StringSerialDeviceBase()
    //    {

    //    }
    //    protected override void DRProcess()
    //    {
    //        try
    //        {
    //            if (_port.BytesToRead > 0)
    //            {
    //                string dat = string.Empty;
    //                switch (ReadMode)
    //                {
    //                    case StringSerialReadMode.Line:
    //                        dat = _port.ReadLine().Replace('\r', ' ').Replace('\n', ' ').Trim();
    //                        break;
    //                    case StringSerialReadMode.Existing:
    //                        dat = _port.ReadExisting();
    //                        break;
    //                    case StringSerialReadMode.OptionalToString:
    //                        dat = _port.ReadTo(TerminatorString);
    //                        break;
    //                    case StringSerialReadMode.OptionalToBytes:
    //                        var buf = new byte[ReadCount];
    //                        _port.Read(buf, 0, ReadCount);
    //                        dat = Encoding.UTF8.GetString(buf);
    //                        break;
    //                    case StringSerialReadMode.Action:
    //                        ReadProcessAction?.Invoke(_port);
    //                        break;
    //                }
    //                ProcessData?.Invoke(dat);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {

    //        }
    //    }
    //    //protected abstract void ProcessData(string data);
    //    public enum StringSerialReadMode : int
    //    {
    //        Line,
    //        Existing,
    //        OptionalToString,
    //        OptionalToBytes,
    //        Action,
    //    }
    //}
    //public class ByteSerialDeviceBase : SerialDeviceBase
    //{
    //    public ByteSerialReadMode ReadMode { get; set; } = ByteSerialReadMode.Signal;
    //    public int ReadCount { get; set; } = 1;
    //    protected ByteSerialDeviceBase() { }
    //    protected override void DRProcess()
    //    {
    //        try
    //        {
    //            switch (ReadMode)
    //            {
    //                case ByteSerialReadMode.Signal:
    //                    ProcessData?.Invoke(new byte[1]);
    //                    break;
    //                case ByteSerialReadMode.Packet:
    //                    var buf = new byte[ReadCount];
    //                    _port.Read(buf, 0, ReadCount);
    //                    ProcessData?.Invoke(buf);
    //                    break;
    //                case ByteSerialReadMode.Action:
    //                    ReadProcessAction?.Invoke(_port);
    //                    ProcessData?.Invoke(new byte[1]);
    //                    break;
    //            }
    //        }
    //        catch (InvalidOperationException ioex) { }
    //    }
    //    public enum ByteSerialReadMode : int
    //    {
    //        Signal,
    //        Packet,
    //        Action
    //    }
    //}

    //public class ManagedDevice
    //{
    //    public BlankDeviceBase Device { get; }
    //    private readonly IDeviceBehavior _behavior;                             //хендл поведения устройства
    //    private readonly ICommandBehavior _commandBehavior;                     //хендл поведения отправки команд устройством


    //    protected ManagedDevice(IDeviceBehavior manageBehavior, ICommandBehavior commandBehavior)
    //    {
    //        Device.ProcessData = Process;
    //        _behavior = manageBehavior;
    //        _commandBehavior = commandBehavior;
    //    }

    //    private void Process(object obj)
    //    {
    //        _behavior.HandleData()
    //    }
    //}

    public interface IPort
    {
        IDeviceOperational deviceReader { get; }
        IDeviceOperational deviceWriter { get; }

        public event Action Connected;
        public event Action Disconnected;
        event Action OnData;
        bool Online { get; }
        bool DataExist { get; }
        void Connect();
        void Disconnect();
        T Read<T>();
        void Write<T>(T msg);
    }
    public interface IDeviceReader<T> : IDeviceOperational
    {
        event Action<T> OnDataReceived;
    }
    public interface IDeviceWriter<T> : IDeviceOperational
    {
        void Write(T message);
        event Action OnDataRequested;
    }
    public interface IDeviceOperational
    {
        IPort port { get; }
        void SetPort(IPort port);
        void Command(object command);
    }




    public abstract class BlankDeviceBase
    {
        IPort port;
        protected BlankDeviceBase()
        {

        }

        public void SetPort(IPort port)
        {
            this.port = port;
        }

        protected abstract void ProcessData(object data);
        protected abstract void RequestData();
    }
    public abstract class ManagedDeviceBase : BlankDeviceBase
    {
        DeviceBehaviorManager dbm = new DeviceBehaviorManager();
        public ManagedDeviceBase(IDeviceBehavior deviceBehavior, ICommandBehavior commandBehavior)
        {
            dbm.deviceBehavior = deviceBehavior;
            dbm.commandBehavior = commandBehavior;
            dbm.device = this;
        }
        protected override void ProcessData(object data)
        {
            dbm.DataProcess(data);
        }
        protected override void RequestData()
        {
            dbm.RequestData();
        }


    }
    public class DeviceBehaviorManager
    {
        public IDeviceBehavior deviceBehavior;
        public ICommandBehavior commandBehavior;
        public ManagedDeviceBase device;
        public List<IOptionalBehavior> _optionalBehaviors;
        List<ProcessAction> processedActions = new List<ProcessAction>();       //список действий-опций для пост-обработки (помимо основного действия)

        public DeviceBehaviorManager()
        {
            _optionalBehaviors = new List<IOptionalBehavior>();
        }
        public void DataProcess(object data)
        {
            deviceBehavior.HandleData(data);
            commandBehavior.HandleData(data);
            if (_optionalBehaviors.Count > 0)
                foreach (var behavior in _optionalBehaviors) behavior.HandleData(data);
            if (processedActions.Count > 0)
                foreach (ProcessAction action in processedActions)
                {
                    action.Execute(data);
                    if (action.ExecutedOnce)
                        processedActions.Remove(action);
                }

        }

        public void RequestData()
        {
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
        protected void Option(string Name, Action<object> action, bool isExecutedOnce = false, bool Active = true) => processedActions.Add(new ProcessAction
        {
            Name = Name,
            ProcessedAction = action,
            ExecutedOnce = isExecutedOnce,
            Usage = Active
        });
        protected void PredicatedOption(string Name, Func<object, bool> predicate, Action<object> action, bool isExecutedOnce = false, bool Active = true) => processedActions.Add(new PredicatedProcessAction
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

    }

    public class StringDeviceReader : IDeviceReader<string>
    {
        public IPort port { get; private set; }

        public event Action<string> OnDataReceived;

        public void Command(object command)
        {
            //emtpy...
        }

        public void SetPort(IPort port)
        {
            this.port = port;
            this.port.OnData += DR;
        }
        //DataReceived
        private void DR()
        {
            while (port.DataExist)
            {
                var data = port.Read<string>();
                OnDataReceived?.Invoke(data);
            }
        }
    }
    public class StringDeviceWriter : IDeviceWriter<string>
    {
        public IPort port { get; private set; }

        public event Action OnDataRequested;

        public void Command(object command)
        {
            Write(command as string);
        }

        public void SetPort(IPort port)
        {
            this.port = port;
        }

        public void Write(string message)
        {
            port.Write(message);
            OnDataRequested?.Invoke();
        }
    }
    public class PortSerial : PortBase
    {
        SerialPort _port;
        public bool Online => _port.IsOpen;
        public bool DataExist => _port.BytesToRead > 0;


        public event Action OnData;

        public PortSerial()
        {
            _port = new SerialPort();
            _port.DataReceived += _port_DataReceived;
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OnData?.Invoke();
        }

        public override void Connect()
        {
            if (!Online)
            {
                try
                {
                    _port.Open();
                    base.Connect();
                }
                catch
                {

                }
            }
        }

        public override void Disconnect()
        {
            if (Online)
            {
                try
                {
                    _port.Close();
                    base.Disconnect();
                }
                catch
                {

                }
            }
        }

        public override T Read<T>()
        {
            if (Online && _port.BytesToRead > 0)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)_port.ReadLine();
                    }
                    else if (typeof(T) == typeof(byte[]))
                    {
                        var buf = new byte[_port.BytesToRead];
                        _port.Read(buf, 0, _port.BytesToRead);
                        return (T)(object)buf;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    return default;
                }
            }
            else return default;
        }

        public override void Write<T>(T msg)
        {
            if (Online)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        var m = msg as string;
                        _port.Write(m);
                    }
                    else if (typeof(T) == typeof(byte[]))
                    {
                        _port.Write(msg as byte[], 0, (msg as byte[]).Length);
                    }
                }
                catch (InvalidOperationException ioex)
                {

                }
                catch (NullReferenceException nrex)
                {

                }
                catch (Exception ex)
                {

                }

            }
        }
    }
    public abstract class PortBase : IPort
    {
        public bool Online { get; }
        public bool DataExist { get; }
        public IDeviceOperational deviceReader { get; }
        public IDeviceOperational deviceWriter { get; }

        public event Action Connected;
        public event Action Disconnected;
        public event Action OnData;
        protected PortBase(IDeviceOperational reader, IDeviceOperational writer)
        {
            reader.SetPort(this);
            deviceReader = reader;
            writer.SetPort(this);
            deviceWriter = writer;
        }
        public virtual void Connect()
        {
            Connected?.Invoke();
        }

        public virtual void Disconnect()
        {
            Disconnected?.Invoke();
        }

        public abstract T Read<T>();

        public abstract void Write<T>(T msg);
    }

    public class DeviceSettings
    {
       public required IPort Port { get; set; }
       public required IDeviceOperational Reader { get; set; }
       public required IDeviceOperational Writer { get; set; }
       public required ICommandBehavior commandBehavior { get; set; }
       public required IDeviceBehavior DeviceBehavior { get; set; }


    }
}
