using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.Interfaces.Behaviors.ConnectionDeviceBehaviors;

namespace ANG24.Core.Devices.External.Behaviors.ConnectionBehavior
{
    public class AutoCallbackConnectionDeviceBehavior : IConnectionDeviceBehavior
    {
        DeviceBase device;
        int attemptLost = 5;
        bool IsRunning = false;
        public Action OnlineAction { get; set; }
        public Action OfflineAction { get; set; }

        public AutoCallbackConnectionDeviceBehavior()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    if (IsRunning)
                    {
                        if (device.Online)
                        {
                            if (attemptLost > 0)
                                attemptLost--;
                            else
                                OfflineAction?.Invoke();
                        }
                        else
                            OnlineAction?.Invoke();
                    }
                }
            });
        }

        

        public void HandleData(object data)
        {
            attemptLost = 5;
        }

        public void SetDevice(DeviceBase device) => this.device = device;
    }
}
