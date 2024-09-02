
using ANG24.Core.Devices.DeviceBehaviors.Base;

namespace ANG24.Core.Devices.DeviceBehaviors.Interfaces
{
    public interface IOptionalBehavior : IDeviceBehavior
    {
        public string Name { get; }
        bool IsUsage { get; set; }
        void On();
        void Off();
    }
    public interface IOptionalCommandBehavior : IOptionalBehavior
    {
        OptionalBehaviorState State { get; }
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
