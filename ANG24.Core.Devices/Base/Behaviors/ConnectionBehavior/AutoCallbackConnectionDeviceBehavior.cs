using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.Interfaces.Behaviors.ConnectionDeviceBehaviors;

namespace ANG24.Core.Devices.Base.Behaviors.ConnectionBehavior
{
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
