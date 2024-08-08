using ANG24.Core.Devices.DeviceBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class TestDevice : DeviceBase
    {
        string ActiveModule = string.Empty;
        public TestDevice() : base(new AutoCallbackDeviceBehavior() { CallBackMilliseconds = 1000}, new OrderStrongCommandBehavior())
        {

        }
        //аналог DataReceived для пост-обработки полученных данных для конкретного устройства
        //(обработка DataReceived есть в родительском классе)
        protected override async void ProcessData(string data) 
        {
           await Console.Out.WriteLineAsync(data);
           
        }
    }
}
