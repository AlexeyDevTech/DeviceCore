using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External.Behaviors.RedirectOptionalCommandBehavior
{
    public class PowerControlOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(PowerControlOptionalBehavior);

        public bool PowerMode { get; set; } = false; //0 -- включение, 1 -- выключение
        public MKZStateInfo MKZStateInfo { get; set; } = new MKZStateInfo();
        public MagnetStateInfo MagnetStateInfo { get; set; } = new MagnetStateInfo();
        public bool HVSwitchIncorrect { get; set; }
        public override void ProcessData(object data)
        {
            var msg = data as string;
            if (!PowerMode)
            {
                if (msg.Contains("shorter is up")) OnProcess();
                if (msg.Contains("shorter is down")) OnFail();
                if (msg.Contains("power is ready")) OnSuccess();
                if (msg.Contains("power is fault")) OnFail();
                if (msg.Contains("MKZ:"))
                {
                    var spl = msg.Split(':');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("Door 2")) MKZStateInfo.DoorRight = true;
                    if (spl_desc.Contains("Door 1")) MKZStateInfo.DoorLeft = true;
                    if (spl_desc.Contains("Ground")) MKZStateInfo.Ground = true;
                    if (spl_desc.Contains("Dangerous potential")) MKZStateInfo.DangerousPotencial = true;
                    if (spl_desc.Contains("Key")) MKZStateInfo.SafeKey = true;
                    if (spl_desc.Contains("Stop button")) MKZStateInfo.Stop = true;
                    OnProcess();
                }
                if (msg.Contains("Check"))
                {
                    var spl = msg.Split(' ');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                    if (spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                    if (spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                    OnProcess();
                }
                if (msg.Contains("MKZ error")) OnFail();
                if (msg.Contains("HVSwitch mode is incorrect"))
                {
                    HVSwitchIncorrect = true;
                    OnFail();
                }
            }
            else
            {
                if (msg.Contains("Check"))
                {
                    var spl = msg.Split(' ');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                    if (spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                    if (spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                    OnProcess();
                }
                if (msg.Contains("Power off is OK")) OnSuccess();
                if (msg.Contains("Power off is fault")) OnFail();
            }
        }
        public override void OnFail()
        {
            if (PowerMode)
            {
                new Timer(state =>
                {
                    base.OnFail();
                }).Change(1000, Timeout.Infinite);
            }
            else
            {
                base.OnFail();
            }

        }
        public override void OnSuccess()
        {
            new Timer(state =>
            {
                base.OnSuccess();
            }).Change(300, Timeout.Infinite);
            
        }

    }
}
