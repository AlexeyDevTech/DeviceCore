namespace ANG24.Core.Devices.Base
{
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
        void ExecuteCommand<T>(T command, Func<T, bool>? predicate, Action ifTrue, Action ifFalse);
    }
    /// <summary>
    /// Интерфейс команд с обрабатываемым внешним паттерном поведением
    /// </summary>
    public interface IRedirectedCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior);
    }

    public abstract class CommandDeviceBehaviorBase : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior, IConditionalCommandDeviceBehavior, IRedirectedCommandDeviceBehavior
    {

        
        public void ExecuteCommand<T>(T command)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCommand<T>(T command, Func<bool>? predicate, Action ifTrue, Action ifFalse)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCommand<T>(T command, Func<T, bool>? predicate, Action ifTrue, Action ifFalse)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior)
        {
            throw new NotImplementedException();
        }

        public void HandleData()
        {
            throw new NotImplementedException();
        }

        public void RequestData()
        {
            throw new NotImplementedException();
        }
    }

    #region Command element realization (and conditions)
    public class CommandElement
    {
        public string Command { get; set; }
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

}
