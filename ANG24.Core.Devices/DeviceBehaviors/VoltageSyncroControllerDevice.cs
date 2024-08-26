using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors
{
    public class VoltageSyncroControllerDevice : DeviceBase
    {
        public int Mode { get; set; }
        public VoltageSyncroControllerDevice() : base(new ReqResDeviceBehavior(), new OrderStrongCommandBehavior())
        {
        }
        public override async void Connect()
        {
            if (await Find("#LAB?", "Voltage Regulator Synchronizer"))
                base.Connect();
        }
        public override void Ping()
        {
            
        }

        protected override void ProcessData(string data)
        {
            if (data.Contains("RNSMODE"))
            {
                Mode = Int32.Parse(data.Split(':')[1]);
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

    }
}
