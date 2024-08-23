using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class PowerControllerDevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }
        public PowerControllerDevice() : base(new ReqResWithTimeCallBackDeviceBehavior("#GETMODE"),new OrderStrongCommandBehavior())
        {

        }
        public override async void Connect()
        {
            if (await Find("#LAB?", "Power Selector", 115200))
                base.Connect();
        }
        protected override void ProcessData(string data)
        {
            CurrentData = new ControllerData(data);
            Console.WriteLine($"device callback: {data}");

        }

        public void GetMode()
        {
            Execute("#GETMODE");
        }
        public void SetMode()
        {
            Execute("#SETMODE");
        }
    }
}
