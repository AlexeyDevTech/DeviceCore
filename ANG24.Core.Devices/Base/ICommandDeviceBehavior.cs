using System.ComponentModel.Design;

namespace ANG24.Core.Devices.Base
{

    #region interfaces CommandDeviceBehavior
    /// <summary>
    /// Стандартный интерфейс командных интерфейсов
    /// </summary>
    public interface ICommandDeviceBehavior : IDeviceBehavior
    {
        void RequestData();
    }
    /// <summary>
    /// Интерфейс простых команд
    /// </summary>
    public interface ISimpleCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command);
    }
    /// <summary>
    /// Интерфейс условных команд с необязательным условием
    /// </summary>
    public interface IConditionalCommandDeviceBehavior : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, Func<bool>? predicate, Action ifTrue, Action ifFalse);
        void ExecuteCommand<T>(T command, Func<object, bool>? predicate, Action ifTrue, Action ifFalse);
    }
    /// <summary>
    /// Интерфейс команд с обрабатываемым внешним паттерном поведением
    /// </summary>
    public interface IRedirectedCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior);
    }
    #endregion

    #region implements CommandDeviceBehavior
    /*
     * Это базовый класс для реализации паттернов поведения при обработке команд
     */
    public abstract class CommandDeviceBehaviorBase : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior, IConditionalCommandDeviceBehavior, IRedirectedCommandDeviceBehavior
    {
        public event Action OnSuccessEvent;
        public event Action OnProcessingEvent;
        public event Action OnFailureEvent;



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
                _commandTask = Task.Factory.StartNew(CommandProcess, CT_cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private void CommandProcess()
        {

        }

        protected abstract void CommandTick();

        public void SetDevice(DeviceBase device) => this.device = device;
    }

    public class OptionalBehaviorManager
    {
        List<ProcessAction> ProcessActions { get; set; }
        public List<IOptionalBehavior> _optionalBehaviors { get; set; }
        public OptionalBehaviorManager() 
        { 
            ProcessActions = new List<ProcessAction>();
            _optionalBehaviors = new List<IOptionalBehavior>();
        }

        public void HandleData(object data)
        {
            if (_optionalBehaviors.Count > 0)
            {
                foreach (var behavior in _optionalBehaviors)
                {
                    behavior.HandleData(data);
                }
            }
            //экспериментальная функция, выполняет дополнительные действия, добавляемые и включаемые по требованию
            if (ProcessActions.Count > 0)
                foreach (ProcessAction action in ProcessActions)
                {
                    action.Execute(data);
                    if (action.ExecutedOnce)
                        ProcessActions.Remove(action);
                }
        }

        #region option management
        public void AddOption(string Name, Action<object> action, bool Active = true) => ProcessActions.Add(new ProcessAction
        {
            Name = Name,
            ProcessedAction = action,
            ExecutedOnce = false,
            Usage = Active
        });
        public void AddPredicatedOption(string Name, Func<object, bool> predicate, Action<object> action, bool Active = true) => ProcessActions.Add(new PredicatedProcessAction
        {
            Name = Name,
            Predicate = predicate,
            ProcessedAction = action,
            ExecutedOnce = false,
            Usage = Active
        });
        public void Clear() => ProcessActions.Clear();
        public void RemoveOption(string Name) => ProcessActions.Remove(ProcessActions.First(x => x.Name == Name));
        public void DisableOption(string Name)
        {
            var r = ProcessActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = false;
        }
        public void EnableOption(string Name)
        {
            var r = ProcessActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = true;
        }
        #endregion
    }
    #endregion

    #region Command element realization (and conditions)
    public class CommandElement
    {
        public object Command { get; set; }
        public CommandCondition? Condition { get; set; }
        public IOptionalCommandBehavior Behavior { get; set; }
        public bool Redirected => Behavior != null;
        public virtual bool Execute(object data)
        {
            if (Behavior == null)
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

    #region ProcessAction types
    public class ProcessAction
    {
        public string Name { get; set; }
        public Action<object> ProcessedAction { get; set; }
        public bool ExecutedOnce { get; set; }
        public bool Usage { get; set; }

        public virtual void Execute(object val)
        {
            if (Usage)
                ProcessedAction?.Invoke(val);
        }
    }
    public class PredicatedProcessAction : ProcessAction
    {
        public Func<object, bool> Predicate { get; set; }

        public override void Execute(object val)
        {
            if (Usage)
                if (Predicate != null && Predicate.Invoke(val))
                    base.Execute(val);
        }
    }
    #endregion

    #region Extensions for DeviceBase
    public static class DeviceBaseExtensions
    {
        public static DeviceBase UseBehavior(this DeviceBase device, IOptionalBehavior behavior)
        {
            (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.Add(behavior);
            behavior.SetDevice(device);
            return device;
        }
        public static DeviceBase EnableBehavior(this DeviceBase device, string Name)
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            //var item = device._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.On();
            return device;
        }
        public static DeviceBase DisableBehavior(this DeviceBase device, string Name)
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.Off();
            return device;
        }
        public static T GetBehavior<T>(this DeviceBase device, string Name) where T : class, IOptionalBehavior
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null)
            {
                return item as T;
            }
            else
            {
                throw new NullReferenceException($"Object {nameof(T)} was not found!!!");
            }
        }
        public static DeviceBase UseOption(this DeviceBase device, string Name, Action<object> action, bool Active = true)
        {
            (device as ManagedDeviceBase).OptionalBehavior.AddOption(Name, action, Active);
            return device;
        }
        public static DeviceBase UseOption(this DeviceBase device, string Name, Func<object, bool> predicate, Action<object> action, bool Active = true)
        {
            (device as ManagedDeviceBase).OptionalBehavior.AddPredicatedOption(Name, predicate, action, Active);
                return device;
        }
        public static DeviceBase ClearOptionList(this DeviceBase device)
        {
            (device as ManagedDeviceBase).OptionalBehavior.Clear();
            return device;
        }
        public static DeviceBase RemoveOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.RemoveOption(Name);
            return device;
        }
        public static DeviceBase DisableOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.DisableOption(Name);
            return device;
        }
        public static DeviceBase EnableOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.EnableOption(Name);
            return device;
        }

    }
    #endregion

}
