﻿using ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices.Base.Abstract.Types
{
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
}