using ANG24.Core.Devices.Base;
using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class IKS40ASDevice : DeviceBase
    {
        public IKS40ASDevice() : base(new ReqResDeviceBehavior(), new OrderStrongCommandBehavior())
        {
        }

        public void Identify()
        {
                       
        }
        public override void Ping()
        {
            
        }

        protected override void ProcessData(string data)
        {
            
        }
    }
}
