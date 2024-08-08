using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors
{
    public class AutoCallbackDeviceBehavior : IDeviceBehavior
    {
        private IDevice device;
        Timer timer;
        public int CallBackMilliseconds { get; set; } //время ожидания ответа
        public AutoCallbackDeviceBehavior()
        {
            timer = new Timer(Reconnect, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void SetDevice(IDevice device) => this.device = device;
        private void Reconnect(object? state)
        {
            device.Disconnect();
            (device as DeviceBase).StartReconnectTimer();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void HandleData(string data)
        {
            timer.Change(CallBackMilliseconds, Timeout.Infinite);
        }
    }
}
