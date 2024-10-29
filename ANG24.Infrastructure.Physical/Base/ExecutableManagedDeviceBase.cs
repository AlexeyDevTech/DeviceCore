using ANG24.Core.Interfaces.CommandBehaviors;

namespace ANG24.Infrastructure.Physical.Base
{
    public abstract class ExecutableManagedDeviceBase : ManagedDeviceBase
    {
        public void Execute<T>(T command) => CommandBehavior.ExecuteCommand(command);
        public void Execute<T>(T command, Func<bool>? predicate, Action? IfTrue = default, Action? IfFalse = default)
        {

            CommandBehavior.ExecuteCommand(command, predicate, IfTrue, IfFalse);
        }
        public void Execute<T>(T command, Func<object, bool> predicate, Action? IfTrue = default, Action? IfFalse = default) => CommandBehavior.ExecuteCommand(command, predicate, IfTrue, IfFalse);
        public void Execute<T>(T command, IOptionalCommandBehavior behavior) => CommandBehavior.ExecuteCommand(command, behavior);
    }
}
