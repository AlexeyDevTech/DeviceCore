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
        string command = "";
        public int ReqResMilliseconds { get; set; } = 10000;
        public ReqResWithTimeCallBackDeviceBehavior(string command) : base()
        {
            this.command = command;
            timerCallback = new(GetCommand, null, base.CallBackMilliseconds, ReqResMilliseconds);
        }

        public void GetCommand(object? state)
        {
            base.device.SetCommand(command);
        }
        public void HandleData(string data)
        {
            base.HandleData(data);
            timerCallback.Change(CallBackMilliseconds,ReqResMilliseconds);
        }
    }
}
