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

        IDevice Device { get; set; }
        public int TickMilliseconds { get; set; } = 200;
        public int CallbackCommandMilliseconds { get; set; } = 500;

        

        const int Attempts = 3;                                         //количество попыток
        bool Busy = false;
        bool BusyHold = false;
        CommandElement? CurrentCommand;
        Queue<CommandElement> _commands;

        bool ProcessActive = false;                                     //флаг, указывающий активность задачи обработчика команд
        Task commandTask;

        public OrderStrongCommandBehavior()
        {
            _commands = new Queue<CommandElement>();
            ProcessActive = true;
            commandTask = Task.Factory.StartNew(CommandProcess);
        }

        private async void CommandProcess()
        {
            while (ProcessActive)
            {
                await Task.Delay(TickMilliseconds);         //задающий темп через задержку
                if (_commands.Count == 0)                   // если команд нет -- остановка
                {
                    Stop(); continue;
                }
                if (!Device.Online) continue;               //если устройство отключено -- пропустить до следующего "тика"
                CommandTick();                              //отправка команды
            }
        }


        //логика обработки одной команды
        protected virtual void CommandTick()
        {
           
            //predicate
            if (Busy) { return; }              //холдер отправки команды. Указывает на то, что сейчас выполняется какая-то команда
            CurrentCommand = _commands.Peek(); //забираем команду
            DeviceSendCommand(CurrentCommand);
        }

        private void CommandCheck<T>(T message)
        {
            if (CurrentCommand != null)        //в итерации может не быть команды, поэтому нужна проверка
            {
                if(CurrentCommand is CommandElement) 
                    CurrentCommand.Execute();
                else if (CurrentCommand is ParametrizedCommandElement<T>)
                    (CurrentCommand as ParametrizedCommandElement<T>)?.Execute(message);
            }
        }
        private void DeviceSendCommand(CommandElement cur_command)
        {
            Busy = true;
            Device.SetCommand(cur_command.Command);    //отправляем команду

        }
        public virtual void ExecuteCommand(string command)
        {
            _commands.Enqueue(new CommandElement { Command = command});
            Start();
        }
        public void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null)
        {
            _commands.Enqueue(new CommandElement
            {
                Command = command,
                Condition = predicate,
                ActionIfTrueCondition = ifTrue,
                ActionIfFalseCondition = ifFalse
            });
            Start();
        }
        public void ExecuteCommand<T>(string command, Func<T, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null)
        {
            _commands.Enqueue(new ParametrizedCommandElement<T>
            {
                Command = command,
                Condition = predicate,
                ActionIfTrueCondition = ifTrue,
                ActionIfFalseCondition = ifFalse
            });
            Start();
        }

        public virtual void HandleData(string data)
        {
            //if (!BusyHold)
            //    Busy = false; //если данные не придут -- надо что-то делать...

        }

        private void Start()
        {
            if (!ProcessActive)
            {
                ProcessActive = true;
                commandTask = Task.Factory.StartNew(CommandProcess);
            }
        }
        private void Stop()
        {
            ProcessActive = false;
        }
        public void SetDevice(IDevice device) => Device = device;
    }

    public class CommandElement
    {
        public required string Command { get; set; }
        public Func<bool>? Condition { get; set; }
        public Action? ActionIfTrueCondition { get; set; }
        public Action? ActionIfFalseCondition { get; set; }

        public void Execute()
        {
            if(Condition?.Invoke() ?? true)
                ActionIfTrueCondition?.Invoke();
            else ActionIfFalseCondition?.Invoke();
        }

    }
    public class ParametrizedCommandElement<T> : CommandElement
    {
        public new Func<T, bool>? Condition { get; set; }

        public void Execute(T param)
        {
            if (Condition?.Invoke(param) ?? true)
                ActionIfTrueCondition?.Invoke();
            else ActionIfFalseCondition?.Invoke();
        }
    }
}
