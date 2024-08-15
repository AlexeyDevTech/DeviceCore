using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors
{
    /// <summary>
    /// Класс, определяющий поведение отправки команд как строго упорядоченное, 
    /// в порядке очереди и через определенный интервал времени.
    /// </summary>
    public class OrderStrongCommandBehavior : ICommandBehavior
    {
        private IDevice Device { get; set; }
        public int TickMilliseconds { get; set; } = 400;
        public int DefaultCallbackCommandMilliseconds { get; set; } = 500;

        private Queue<CommandElement> commandQueue;
        private string lastData = string.Empty;

        CommandProcessor commandProcessor;
        private bool isProcessActive = false;
        private Task commandTask;

        public OrderStrongCommandBehavior()
        {
            commandQueue = new Queue<CommandElement>();
            commandProcessor = new CommandProcessor()
            {
                commandExecuteAction = SendCommandToDevice,
                CommandNextAction = () => commandQueue.Dequeue()
            };
            StartCommandProcess();
        }

        private void StartCommandProcess()
        {
            isProcessActive = true;
            commandTask = Task.Factory.StartNew(async () =>
            {
                while (isProcessActive)
                {
                    //redirected.WaitOne();
                    await Task.Delay(TickMilliseconds);

                    if (commandQueue.Count == 0)
                    {
                        StopCommandProcess();
                        continue;
                    }
                    if (!Device.Online) continue;
                    ProcessNextCommand();
                }
            });
        }

        private void ProcessNextCommand()
        {
            if (commandProcessor.Busy) return;          //если занято -- пропускаем итерацию
            commandProcessor.Set(commandQueue.Peek());  //задаем команду
            commandProcessor.Action();
        }

        private void SendCommandToDevice(string command)
        {
            Console.WriteLine($"<- {command}");
            Device.SetCommand(command);
        }
        public void ExecuteCommand(string command)
        {
            commandQueue.Enqueue(new CommandElement { Command = command });
            if (!isProcessActive) StartCommandProcess();
        }
        public void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new CommandCondition(predicate, ifTrue, ifFalse)
            });
            if (!isProcessActive) StartCommandProcess();
        }
        public void ExecuteCommand(string command, Func<string, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null)
        {
            commandQueue.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new ParametrizedCommandCondition(predicate, ifTrue, ifFalse)
            });
            if (!isProcessActive) StartCommandProcess();
        }
        public void ExecuteCommand(string command, IOptionalCommandBehavior redirectedBehavior) => new BehaviorCommandElement
        {
            Command = command,
            RedirectedBehavior = redirectedBehavior,
        };
        public void HandleData(string data)
        {
            Console.WriteLine($"-> {data}");
            commandProcessor.HandleData(data);
        }
        public void SetDevice(IDevice device) => Device = device;
        private void StopCommandProcess() => isProcessActive = false;
    }


    public class BehaviorCommandElement : CommandElement
    {
        public IOptionalCommandBehavior RedirectedBehavior { get; set; }
    }
    public class CommandElement
    {
        public string Command { get; set; }
        public CommandCondition? Condition { get; set; }
        public virtual bool Execute(string data = "") => Condition?.Execute(data) ?? true;
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

        public virtual bool Execute(string data)
        {
            bool result = condition?.Invoke() ?? true;
            if (result) ifTrue?.Invoke();
            else ifFalse?.Invoke();
            return result;
        }
    }

    public class ParametrizedCommandCondition : CommandCondition
    {
        private readonly Func<string, bool>? condition;

        public ParametrizedCommandCondition(Func<string, bool>? condition, Action? ifTrue, Action? ifFalse)
            : base(null, ifTrue, ifFalse)
        {
            this.condition = condition;
        }

        public override bool Execute(string data)
        {
            bool result = condition?.Invoke(data) ?? true;
            if (result) ifTrue?.Invoke();
            else ifFalse?.Invoke();
            return result;
        }
    }

    public class CommandProcessor
    {
        private Timer FallTimer;
        private Timer CommandExecutedTimer;

        int FaultTimeMilliseconds = 500;
        int CommandExecutedMilliseconds = 300;
        const int Attempts = 3;
        int local_attempts = Attempts;

        string last_data = "";
        private CommandElement? _currentCommand;
        private IOptionalCommandBehavior? _behavior;

        public CommandElement? CurrentCommand {
            get => _currentCommand;
            set
            {
                _currentCommand = value;
                if (_currentCommand is BehaviorCommandElement)
                {
                    if((value as BehaviorCommandElement).RedirectedBehavior != null)
                       _behavior = (value as BehaviorCommandElement).RedirectedBehavior;
                }
                else _behavior = null;
            }

        }
        public bool Busy { get; set; } = false;
        public required Action<string> commandExecuteAction { get; set; }
        public required Action CommandNextAction { get; set; }
        public CommandProcessor() 
        { 
            FallTimer = new Timer(TimeoutCallback, null, Timeout.Infinite, Timeout.Infinite);
            CommandExecutedTimer = new Timer(ExecuteCommandCallback, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void TimeoutCallback(object? state)
        {
            Check(last_data, true);
        }
        private void ExecuteCommandCallback(object? state)
        {
            local_attempts = Attempts;
            Busy = false;
            CommandNextAction.Invoke();
        }

        public void Set(CommandElement? command)
        {
            CurrentCommand = command;
        }
        public void Action()
        {
            commandExecuteAction.Invoke(CurrentCommand.Command);
            Busy = true;
        }
        //решает, освобождать выполнение или нет 
        private void Check(string msg, bool timeout = false)
        {
            if (_behavior == null)
            {
                if (!timeout)
                {
                    var result = CurrentCommand?.Execute(msg) ?? true;
                    if (result)
                    {
                        //если условие команды выполнено
                        CommandExecutedTimer.Change(CommandExecutedMilliseconds, Timeout.Infinite);
                    }
                    else
                    {
                        //если нет -- надо выполнить ещё раз
                        local_attempts--;
                        if (local_attempts == 0) //пока не исчерпаем количество попыток
                        {
                            CommandExecutedTimer.Change(CommandExecutedMilliseconds, Timeout.Infinite);
                        }
                    }
                }
                else
                {
                    local_attempts--;
                    if (local_attempts == 0) //пока не исчерпаем количество попыток
                    {
                        CommandExecutedTimer.Change(CommandExecutedMilliseconds, Timeout.Infinite);
                    }
                }
            }
            else BehaviorCheck(msg, timeout);
        }
        public void HandleData(string data)
        {
            Check(data);
        }

        private void BehaviorCheck(string data, bool timeout)
        {
            var state = _behavior.OperationCheck(data);
            switch (state)
            {
                case MEA.OptionalBehaviorState.Processing:
                    if (timeout)
                    {
                        _behavior.OnFail();
                    }
                    break;
                case MEA.OptionalBehaviorState.Success:

                    break;
                case MEA.OptionalBehaviorState.Fail:
                    if (!timeout)
                    {

                    }
                    break;
            }

        }
    }

}
