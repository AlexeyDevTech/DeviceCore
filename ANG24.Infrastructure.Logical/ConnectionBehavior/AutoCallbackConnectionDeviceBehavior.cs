using ANG24.Core.Interfaces;
using ANG24.Core.Interfaces.ConnectionDeviceBehaviors;
using ANG24.Infrastructure.Physical.Base;

namespace ANG24.Infrastructure.Logical.ConnectionBehavior
{
    public class AutoCallbackConnectionDeviceBehavior : IConnectionDeviceBehavior
    {
        IDeviceBase device;
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
                        if (device.Online) //открыт порт
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

        public void SetDevice(IDeviceBase device) => this.device = device;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
