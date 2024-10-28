namespace ANG24.Core.Interfaces.ConnectionDeviceBehaviors
{
    public interface IConnectionDeviceBehavior : IDeviceBehavior
    {
        Action OnlineAction { get; set; }
        Action OfflineAction { get; set; }
        void Start();
        void Stop();
    }
}
