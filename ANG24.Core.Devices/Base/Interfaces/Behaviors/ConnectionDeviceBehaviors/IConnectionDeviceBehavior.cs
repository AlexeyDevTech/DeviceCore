namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.ConnectionDeviceBehaviors
{
    public interface IConnectionDeviceBehavior : IDeviceBehavior
    {
        Action OnlineAction { get; set; }
        Action OfflineAction { get; set; }
    }
}
