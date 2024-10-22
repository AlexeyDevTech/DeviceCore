using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.External.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External
{
    public class MNKDevice : ExecutableManagedDeviceBase
    {
        public MNKControllerTypes MNKControllerType;
        public MNKActionTypes MNKConnection;
        public bool OperationSuccess { get; set; } = false;
        public bool Error { get; set; } = false;

        protected override void OnData(object data)
        {
            if ((data as string).Contains("State"))
            {
                var msg = (data as string).Split('=');
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
            else if ((data as string).Contains("OK"))
            {
                var msg = (data as string).Split('#');
                var phase = msg[1].Substring(msg[1].Length - 2);
                var type = msg[1].Substring(0, msg[1].Length - 2);
                switch (phase)
                {
                    case "AB":
                        MNKConnection = MNKActionTypes.AB;
                        break;
                    case "AC":
                        MNKConnection = MNKActionTypes.AC;
                        break;
                    case "BA":
                        MNKConnection = MNKActionTypes.BA;
                        break;
                    case "BC":
                        MNKConnection = MNKActionTypes.BC;
                        break;
                    case "CA":
                        MNKConnection = MNKActionTypes.CA;
                        break;
                    case "CB":
                        MNKConnection = MNKActionTypes.CB;
                        break;
                    case "NA":
                        MNKConnection = MNKActionTypes.NA;
                        break;
                    case "NB":
                        MNKConnection = MNKActionTypes.NB;
                        break;
                    case "NC":
                        MNKConnection = MNKActionTypes.NC;
                        break;
                }
                switch (type)
                {
                    case "Ind":
                        MNKControllerType = MNKControllerTypes.Ind;
                        break;
                    case "Meas":
                        MNKControllerType = MNKControllerTypes.Meas;
                        break;
                    case "MO":
                        MNKControllerType = MNKControllerTypes.MO;
                        break;
                    case "Ref":
                        MNKControllerType = MNKControllerTypes.Ref;
                        break;
                }
                OperationSuccess = true;
            }
            else if ((data as string).Contains("ERROR"))
            {
                OperationSuccess = false;
                Error = true;
            }
            else if ((data as string).Contains("RESET"))
            {
                MNKConnection = MNKActionTypes.None;
                MNKControllerType = MNKControllerTypes.None;
            }

            base.OnData(data);
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
        public void SetMode(MNKActionTypes action)
        {
            SetMode(MNKControllerType, action);
        }
    }
}
