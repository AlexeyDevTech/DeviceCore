using ANG24.Core.Devices.Base.Abstract.Types;
using ANG24.Core.Devices.Base.Interfaces;
using ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors;
using ANG24.Core.Devices.Types;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace ANG24.Core.Devices.Base.Abstract
{
    #region CommandDeviceBehaviorBase
    /*
     * Это базовый класс для реализации паттернов поведения при обработке команд
     */
    public abstract class CommandDeviceBehaviorBase : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior, IConditionalCommandDeviceBehavior, IRedirectedCommandDeviceBehavior, IObjectiveCommandDeviceBehavior
    {
        protected CommandElement Command;
        protected bool Busy;
        protected int attempts = 3;
        int attempt_lost = 3;
        int timeout_milliseconds = 5000;

        public bool AutoResponse { get; set; } = false;
        public int MessageAttempts { get; set; } = 3;

        public event Action OnSuccessEvent;
        public event Action OnProcessingEvent;
        public event Action OnFailureEvent;



        protected int CommandTickTime = 1000;

        protected Queue<CommandElement> cmds;
        protected DeviceBase device;

        bool Active = false;
        Task _commandTask;
        CancellationTokenSource CT_cts;

        protected CommandDeviceBehaviorBase()
        {
            cmds = new Queue<CommandElement>();
        }

        public void ExecuteCommand<T>(T command)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command
            });
            Start();
        }

        public void ExecuteCommand<T>(T command, Func<bool>? predicate, Action? ifTrue, Action? ifFalse)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new CommandCondition(predicate, ifTrue, ifFalse),
            });
            Start();
        }

        public void ExecuteCommand<T>(T command, Func<object, bool>? predicate, Action? ifTrue, Action? ifFalse)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new ParametrizedCommandCondition(predicate, ifTrue, ifFalse),
            });
            Start();
        }

        public void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Behavior = redirectedBehavior
            });
            Start();
        }
        public void ExecuteCommand(CommandElement command)
        {
            cmds.Enqueue(command);
            Start();
        }

        public virtual void HandleData(object data)
        {
            Check(data);
        }
        private void Check(object msg)
        {
            if (Busy && Command != null)
            {
                var result = Command.Execute(msg);
                if (!Command.Redirected) //если команда простая
                {
                    if (result) OnSuccess();
                    else
                    {
                        if (AutoResponse)
                        {
                            if (MessageAttempts == 0)
                                OnFailure();
                            MessageAttempts--;
                        }
                        else
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
        }
        public abstract void RequestData();


        #region output events
        public virtual void OnSuccess()
        {
            Reset();
            OnSuccessEvent?.Invoke();
        }

        public virtual void OnFailure()
        {
            if(attempt_lost <= 0) //пока попытки не исчерпались
            {
                attempt_lost--;
            }
            else                 //а вот когда исчерпались
            {
                Reset();
                OnFailureEvent?.Invoke();
            }
            
        }
        public virtual void OnProcessing()
        {
            OnProcessingEvent?.Invoke();
        }
        #endregion
        public virtual void Clear()
        {
            cmds.Clear();
        }
        public void DropCommand()
        {
            if (cmds.Count > 0)
            {
                cmds.Dequeue();
            }
        }

        public void Start()
        {
            if (!Active)
            {
                Console.WriteLine("[CDBB] -> Start process...");
                CT_cts = new CancellationTokenSource();
                _commandTask = Task.Factory.StartNew(async () => await CommandProcess(CT_cts.Token), CT_cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                Active = true;
            }
        }
        public void Stop()
        {
            CT_cts?.Cancel();
            Active = false;
            _commandTask = null;
        }

        private async Task CommandProcess(CancellationToken token)
        {
            Stopwatch sw = new Stopwatch();
            while (cmds.Count > 0 || token.IsCancellationRequested)
            {
                Console.WriteLine($"[task] -> cmds Count = {cmds.Count}");
                await Task.Delay(CommandTickTime);
                if (!(cmds.Count > 0 || token.IsCancellationRequested)) break;
                if (!device.Online) continue;
                if (Command == null) //то, что команды не установлено, означает, что ничего не выполняется
                {
                    if (!Busy)
                    {
                        Console.WriteLine($"[CDBB] -> PROCESS ITERATION");
                        Command = Set();
                        if (Command.Redirected) Command.Behavior.Start(); //задаем сигнал для переопределяемого обработчика команды
                        sw.Restart();
                        Console.WriteLine($"Command -> {Command.Command}");
                        Busy = true; //не допускаем выполнение нового CommandTick
                        CommandTick(); //"тиккер" команды
                    }
                }
                else //если команда не null и охарактеризованно её выполнение
                {
                   if(sw.ElapsedMilliseconds > timeout_milliseconds) //если команда опоздала, но ещё не выполнилась
                   {
                        OnFailure();
                   }
                }
            }
            Console.WriteLine($"[CDBB] -> command process stop. queue is empty or cancel operation");
        }
        /// <summary>
        /// собственно, "тиккер" команды
        /// </summary>
        protected virtual void CommandTick()
        {
            if(Command != null)
            {
                var type = Command.Command.GetType();
                device.Write(type, Command.Command); //отправка команды
            }


            
        }
        private CommandElement Set()
        {
            
            try
            {
                attempt_lost = attempts;
                return cmds.Peek();
            } 
            catch { }
            return default(CommandElement);
        }
        protected void Reset(bool dropCommand = true)
        {
            Console.WriteLine("[CDBB] -> command reset");
            if (dropCommand)
                DropCommand();
            MessageAttempts = 3;
            Command = null;
            Busy = false;
        }
       

        public void SetDevice(DeviceBase device) => this.device = device;
    }

#endregion
}
