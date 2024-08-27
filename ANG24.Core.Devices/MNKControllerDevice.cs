using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class MNKControllerDevice : DeviceBase
    {
        public MNKControllerTypes MNKControllerType;
        public MNKActionTypes MNKConnection;
        public bool OperationSuccess { get; set; } = false;
        public bool Error { get; set; } = false;
        public MNKControllerDevice() : base(new ReqResWithTimeCallBackDeviceBehavior(), new OrderStrongCommandBehavior())
        {
        }

        public override async void Connect()
        {
            if (await Find("#LAB?", "MNC"))
                base.Connect();
        }

        public override void Ping()
        {
            GetState();
        }

        protected override void ProcessData(string data)
        {
            if (data.Contains("State"))
            {
                var msg = data.Split('=');
                if (msg.Length > 0)
                {
                    int state = Convert.ToInt32(msg[1].Replace('\r', ' ').Replace('\n', ' ').Trim());
                    if (state == 0)
                    {
                        MNKConnection = MNKActionTypes.None;
                        MNKControllerType = MNKControllerTypes.None;
                    }
                    else
                    {
                        if ((state & 0x01) != 0) MNKConnection = MNKActionTypes.AB;
                        if ((state & 0x02) != 0) MNKConnection = MNKActionTypes.AC;
                        if ((state & 0x04) != 0) MNKConnection = MNKActionTypes.BA;
                        if ((state & 0x08) != 0) MNKConnection = MNKActionTypes.BC;
                        if ((state & 0x10) != 0) MNKConnection = MNKActionTypes.CA;
                        if ((state & 0x20) != 0) MNKConnection = MNKActionTypes.CB;
                        if ((state & 0x40) != 0) MNKConnection = MNKActionTypes.NA;
                        if ((state & 0x80) != 0) MNKConnection = MNKActionTypes.NB;
                        if ((state & 0x100) != 0) MNKConnection = MNKActionTypes.NC;
                        
                        if ((state & 0x200) != 0) MNKControllerType = MNKControllerTypes.Ind;
                        if ((state & 0x400) != 0) MNKControllerType = MNKControllerTypes.Meas;
                        if ((state & 0x800) != 0) MNKControllerType = MNKControllerTypes.MO;
                        if ((state & 0x1000) != 0) MNKControllerType = MNKControllerTypes.Ref;   
                        
                    }
                }
            }
            else if (data.Contains("OK"))
            {
                OperationSuccess = true;
            }
            else if (data.Contains("ERROR"))
            {
                OperationSuccess = false;
                Error = true;
            }
        }


        public void Reset()
        {
            Execute("#Reset");
        }
        public void GetState()
        {
            Execute("#GetState");
        }
        public void SetMode(MNKControllerTypes type, MNKActionTypes action)
        {
            Execute($"#{type}{action}");
        }
        public enum MNKActionTypes
        {
            None,
            AB,
            AC,
            BA,
            BC,
            CA,
            CB,
            NA,
            NB,
            NC
        }
        public enum MNKControllerTypes
        {
            None,
            Ind,
            Meas,
            MO,
            Ref
        }
    }
}
