using ANG24.Core.Devices.DeviceBehaviors.Base;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Diagnostics;

namespace ANG24.Core.Devices.DeviceBehaviors
{
    #region comments
    /// <summary>
    /// Класс, определяющий поведение отправки команд как строго упорядоченное, 
    /// в порядке очереди и через определенный интервал времени.
    /// </summary>


    #endregion

    public class OrderStrongCommandBehavior : ICommandBehavior
    {
        private Queue<CommandElement> commandQueue;
        private string lastData = string.Empty;
        private bool isProcessActive = false;
        int attempts = 3;
        int messageAttempts = 3;
        private Task commandTask;

        Timer callbackTimeoutTimer;
        private object _syncRoot = new object();
        private bool FailureCommand;
        private bool CommandFailProcess;

        IDevice Device { get; set; }
        public bool AutoResponse { get; set; } = true;
        public int Attempts { get; set; } = 3;
        public int MessageAttempts { get; set; } = 3;
        public int TickMilliseconds { get; set; } = 350;
        public int TickAfter { get; set; } = 0;
        public bool Busy { get; private set; }

        public CommandElement Command { get; set; }

        public OrderStrongCommandBehavior()
        {
            commandQueue = new Queue<CommandElement>();
            callbackTimeoutTimer = new Timer(CallbackTimeout, null, Timeout.Infinite, Timeout.Infinite);
        }


        #region Main cycle
        private async void CommandProcess()
        {
            while (isProcessActive)
            {
                await Task.Delay(TickMilliseconds);
                if (commandQueue.Count == 0)        //если команд нет
                {
                    Console.WriteLine("Queue is empty. Stop");
                    Stop();
                    continue;
                }
                if (!Device.Online) continue;
                CommandTick();
            }
        }
        private void CommandTick()
        {
            if (Busy) return;       //если занят или пустая очередь -- пропустить
            var _command = commandQueue.Peek();
            Console.WriteLine($"command set(lost = {commandQueue.Count})");
            Command = _command;                                 //устанавливаем текущую команду
            OnProcessing();                                     //задаем сигнал, чтобы не допускать повторной отправки команды
            var tb = 0;                                         //значение для задержки перед отправкой
            var tm = 500;                                         //значение для таймера таймаута
            messageAttempts = MessageAttempts;
            if(!CommandFailProcess)
               attempts = Attempts;

            if(Command.Settings != null)
            {
                tm = Command.Settings.Timeout;
                messageAttempts = Command.Settings.MessageAttempts;
                if(!CommandFailProcess)
                   attempts = Command.Settings.Attempts;
            }
            if (Command.Redirected) Command.Behavior.Start();
            //if (tb > 0)
            //    await Task.Delay(tb); //по заказу -- задержка перед отправкой команды
            Device.SetCommand(Command.Command);                 //отправляем
            
            Console.WriteLine($"<--- {Command.Command}");
            callbackTimeoutTimer.Change(tm, Timeout.Infinite); //и запускаем таймер ожидания обратной связи
        }
        #endregion
        #region start\stop 
        private void Stop()
        {
            isProcessActive = false;
        }
        private void Start()
        {
            if (!isProcessActive)
            {
                isProcessActive = true;
                commandTask = Task.Run(CommandProcess);
            }
        }
        #endregion
        #region ExecuteCommand realizations
        public void ExecuteCommand(string command, CommandElementSettings settings)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
            });
            if (!isProcessActive) Start();
        }
        public void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings settings = null)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new CommandCondition(predicate, ifTrue, ifFalse),
                Settings = settings
            });
            if (!isProcessActive) Start();
        }
        public void ExecuteCommand(string command, Func<string, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings settings = null)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new ParametrizedCommandCondition(predicate, ifTrue, ifFalse),
                Settings = settings
            });
            if (!isProcessActive) Start();
        }
        public void ExecuteCommand(string command, IOptionalCommandBehavior behavior, CommandElementSettings settings = null)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
                Behavior = behavior,
                Settings = settings
            });
        }
        #endregion
        public void HandleData(object data)
        {
            if (Command != null)
            {
                if (Command?.Redirected ?? false) Command.Behavior.HandleData(data);
                Check(data);
            }
        }

        private void Check(object msg)
        {
            var result = Command.Execute(msg);
            if (!Command.Redirected) //если команда простая
            {
                if (result) OnSuccess();
                else
                {
                    if (AutoResponse)
                    {
                        if (messageAttempts == 0)
                            OnFailure();
                        messageAttempts--;
                    } else
                    {
                        OnFailure();
                    }
                }
            }
            else
            {
                var state = Command.GetState();
                switch (state)
                {
                    case OptionalBehaviorState.Success:
                        OnSuccess();
                        Console.Out.WriteLine("[behavior success]");
                        break; //якорь завершения операции
                    case OptionalBehaviorState.Processing:
                        OnProcessing();
                        Console.Out.WriteLine("[behavior processing]");
                        break; //продолжение операции и задержка главного цикла
                    case OptionalBehaviorState.Fail:
                        OnFailure();
                        Console.Out.WriteLine("[behavior failure]");
                        break; //Якорь завершения операции с ошибкой (если не исчерпались попытки -- повторение)
                }
            }
        }

        private void CallbackTimeout(object? state)
        {
            Console.WriteLine("[[has timeout]]");
            OnFailure();
        }

        public void SetDevice(IDevice device) => Device = device;

        public void OnProcessing() => Busy = true;
        public void OnSuccess()
        {
            callbackTimeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.WriteLine("[[Success]]");
            DropCommand();
            Busy = false;
        }
        public void OnFailure()
        {
            callbackTimeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            CommandFailProcess = true;
            if (attempts <= 0)
            {
                DropCommand();
                CommandFailProcess = false;
            }
            attempts--;
            Console.WriteLine($"[[Fail(Attempts = {attempts})]]");
            Busy = false; //впускает для повторения операции
        }
        public void DropCommand()
        {
            commandQueue.Dequeue();
            Command = null;
        }

        public void RequestData(string data)
        {
            
        }
    }
    #region Command element realization (and conditions)
    public class CommandElement
    {
        public string Command { get; set; }
        public CommandElementSettings Settings { get; set; }
        public CommandCondition? Condition { get; set; }
        public IOptionalCommandBehavior Behavior { get; set; }
        public bool Redirected => Behavior != null;
        public virtual bool Execute(object data)
        {
            if(Behavior == null)
               return Condition?.Execute(data) ?? true;
            else
            {
                var res = Behavior.OperationCheck(data);
                if (res == OptionalBehaviorState.Processing || res == OptionalBehaviorState.Fail) return false;
                else return true;
            }
        }
        public OptionalBehaviorState GetState()
        {
            return Behavior?.State ?? OptionalBehaviorState.Success;
        }
    }
    public class CommandCondition
    {
        private readonly Func<bool>? condition;
        protected readonly Action? ifTrue;
        protected readonly Action? ifFalse;

        public CommandCondition(Func<bool>? condition, Action? ifTrue, Action? ifFalse)
        {
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        public virtual bool Execute(object data)
        {
            bool result = condition?.Invoke() ?? true;
            if (result) ifTrue?.Invoke();
            else ifFalse?.Invoke();
            return result;
        }
    }
    public class ParametrizedCommandCondition : CommandCondition
    {
        private readonly Func<object, bool>? condition;

        public ParametrizedCommandCondition(Func<object, bool>? condition, Action? ifTrue, Action? ifFalse)
            : base(null, ifTrue, ifFalse)
        {
            this.condition = condition;
        }

        public override bool Execute(object data)
        {
            bool result = condition?.Invoke(data) ?? true;
            if (result) ifTrue?.Invoke();
            else ifFalse?.Invoke();
            return result;
        }
    }
    #endregion
}
