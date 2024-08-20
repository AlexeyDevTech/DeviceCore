using ANG24.Core.Devices.DeviceBehaviors.Base;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices.DeviceBehaviors.MEA.CommandBehaviors
{
    public class PowerControlOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(PowerControlOptionalBehavior);

        public bool PowerMode { get; set; } = false; //0 -- включение, 1 -- выключение
        public MKZStateInfo MKZStateInfo { get; set; } = new MKZStateInfo();
        public MagnetStateInfo MagnetStateInfo { get; set; } = new MagnetStateInfo();
        public bool HVSwitchIncorrect { get; set; }
        public override void ProcessData(string data)
        {
            if (!PowerMode)
            {
                if (data.Contains("shorter is up")) OnProcess();
                if (data.Contains("shorter is down")) OnFail();
                if (data.Contains("power is ready")) OnSuccess();
                if (data.Contains("power is fault")) OnFail();
                if (data.Contains("MKZ:"))
                {
                    var spl = data.Split(':');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("Door 2")) MKZStateInfo.DoorRight = true;
                    if (spl_desc.Contains("Door 1")) MKZStateInfo.DoorLeft = true;
                    if (spl_desc.Contains("Ground")) MKZStateInfo.Ground = true;
                    if (spl_desc.Contains("Dangerous potential")) MKZStateInfo.DangerousPotencial = true;
                    if (spl_desc.Contains("Key")) MKZStateInfo.SafeKey = true;
                    if (spl_desc.Contains("Stop button")) MKZStateInfo.Stop = true;
                    OnProcess();
                }
                if (data.Contains("Check"))
                {
                    var spl = data.Split(' ');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                    if (spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                    if (spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                    OnProcess();
                }
                if (data.Contains("MKZ error")) OnFail();
                if (data.Contains("HVSwitch mode is incorrect"))
                {
                    HVSwitchIncorrect = true;
                    OnFail();
                }
            }
            else
            {
                if (data.Contains("Check"))
                {
                    var spl = data.Split(' ');
                    var spl_desc = spl[1].Trim();
                    if (spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                    if (spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                    if (spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                    OnProcess();
                }
                if (data.Contains("Power off is OK")) OnSuccess();
                if (data.Contains("Power off is fault")) OnFail();
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

    }


}
