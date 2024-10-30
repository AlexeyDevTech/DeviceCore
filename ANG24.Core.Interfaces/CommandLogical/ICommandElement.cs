using ANG24.Core.Entities.DataTypes;
using ANG24.Core.Interfaces.CommandBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Interfaces.CommandLogical
{
    public interface ICommandElement
    {
        object Command { get; set; }
        ICommandCondition? Condition { get; set; }
        IOptionalCommandBehavior Behavior { get; set; }
        bool Redirected => Behavior != null;
        bool Execute(object data);
        OptionalBehaviorState GetState();
    }
}
