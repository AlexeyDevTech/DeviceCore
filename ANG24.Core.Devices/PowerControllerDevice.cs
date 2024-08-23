using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class PowerControllerDevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }
        public int CurrentMode { get; set; }
        public PowerControllerDevice() : base(new ReqResWithTimeCallBackDeviceBehavior() { ReqResMilliseconds = 1000 }, new OrderStrongCommandBehavior())
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
            Debug.WriteLine($"device callback: {data}");
            if (data.Contains("mode"))
            {
                var m = data.Split('=')[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
                CurrentMode = Convert.ToInt32(m);
                Debug.WriteLine($"[PS]Mode: {CurrentMode}");
            }
        }

        public void GetMode()
        {
            Execute("#GETMODE");
        }
        public void SetMode(int mode)
        {
            Execute($"#SETMODE,{mode};");
        }

        public override void Ping()
        {
            GetMode();
        }
    }
}
