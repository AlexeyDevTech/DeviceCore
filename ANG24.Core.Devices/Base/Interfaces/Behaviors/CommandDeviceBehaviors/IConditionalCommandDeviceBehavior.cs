namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    /// <summary>
    /// Интерфейс условных команд с необязательным условием
    /// </summary>
    public interface IConditionalCommandDeviceBehavior : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, Func<bool>? predicate, Action ifTrue, Action ifFalse);
        void ExecuteCommand<T>(T command, Func<object, bool>? predicate, Action ifTrue, Action ifFalse);
    }




}
