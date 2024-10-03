using ANG24.Core.Devices.Base.Abstract.Types;
using ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors;

namespace ANG24.Core.Devices.Base.Abstract
{
    #region CommandDeviceBehaviorBase
    /*
     * Это базовый класс для реализации паттернов поведения при обработке команд
     */
    public abstract class CommandDeviceBehaviorBase : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior, IConditionalCommandDeviceBehavior, IRedirectedCommandDeviceBehavior, IObjectiveCommandDeviceBehavior
    {
        public event Action OnSuccessEvent;
        public event Action OnProcessingEvent;
        public event Action OnFailureEvent;

        protected int CommandTickTime = 100;

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
        }

        public void ExecuteCommand<T>(T command, Func<bool>? predicate, Action? ifTrue, Action? ifFalse)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new CommandCondition(predicate, ifTrue, ifFalse),
            });
        }

        public void ExecuteCommand<T>(T command, Func<object, bool>? predicate, Action? ifTrue, Action? ifFalse)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Condition = new ParametrizedCommandCondition(predicate, ifTrue, ifFalse),
            });
        }

        public void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior)
        {
            cmds.Enqueue(new CommandElement
            {
                Command = command,
                Behavior = redirectedBehavior
            });
        }
        public void ExecuteCommand(CommandElement command)
        {
            cmds.Enqueue(command);
        }

        public abstract void HandleData(object data);
        public abstract void RequestData();


        #region output events
        public virtual void OnSuccess()
        {
            OnSuccessEvent?.Invoke();
        }

        public virtual void OnFailure()
        {
            OnFailureEvent?.Invoke();
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
                CT_cts = new CancellationTokenSource();
                _commandTask = Task.Factory.StartNew(async () => await CommandProcess(CT_cts.Token), CT_cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private async Task CommandProcess(CancellationToken token)
        {
            while (cmds.Count > 0 || token.IsCancellationRequested)
            {
                CommandTick();
                await Task.Delay(CommandTickTime);
            }
        }
        protected abstract void CommandTick();

        public void SetDevice(DeviceBase device) => this.device = device;
    }

#endregion
}
