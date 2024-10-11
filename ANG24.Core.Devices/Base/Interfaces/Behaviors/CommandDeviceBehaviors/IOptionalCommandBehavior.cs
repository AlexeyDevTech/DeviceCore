using ANG24.Core.Devices.Base.Interfaces.Behaviors;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    public interface IOptionalCommandBehavior : IOptionalBehavior
    {
        OptionalBehaviorState State { get; protected set; }
        public int FaultCallback { get; set; }
        OptionalBehaviorState OperationCheck(object data);
        Action ProcessingAction { get; set; }
        Action SuccessAction { get; set; }
        Action<IOptionalCommandBehavior> FailureAction { get; set; }
        void Start();
        void Stop();
        void OnSuccess();
        void OnFail();
    }
}
