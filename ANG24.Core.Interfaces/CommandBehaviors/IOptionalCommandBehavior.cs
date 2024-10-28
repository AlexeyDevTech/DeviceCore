namespace ANG24.Core.Interfaces.CommandBehaviors
{
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
