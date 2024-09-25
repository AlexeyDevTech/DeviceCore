namespace ANG24.Core.Devices.Base.Interfaces.Behaviors
{
    public interface IConnectionDeviceBehavior : IDeviceBehavior
    {
        void Reconnect();
    }
    public class AutoCallbackConnectionDeviceBehavior : IConnectionDeviceBehavior
    {
        DeviceBase device;

        public AutoCallbackConnectionDeviceBehavior()
        {

        }

        public void HandleData(object data)
        {

        }

        public void Reconnect()
        {

        }

        public void SetDevice(DeviceBase device) => this.device = device;
    }
}
