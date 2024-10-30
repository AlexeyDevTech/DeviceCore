using ANG24.Core.Interfaces.CommandBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Middleware.Base
{
    public class ExecutableManagedDeviceBase : ManagedDeviceBase
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
