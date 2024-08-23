using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors
{
    public class ReqResWithTimeCallBackDeviceBehavior : ReqResDeviceBehavior
    {
        Timer timerCallback;
        
        public int ReqResMilliseconds { get; set; } = 10000;
        public ReqResWithTimeCallBackDeviceBehavior() : base()
        {
            timerCallback = new Timer(GetCommand, null, CallBackMilliseconds, ReqResMilliseconds);
        }

        public void GetCommand(object? state)
        {
            base.device.Ping();
        }

      
       
    }
}
