using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.DeviceBehaviors.MEA;
using ANG24.Core.Devices.DeviceBehaviors.MEA.CommandBehaviors;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices
{


    public class MEADevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }
        public bool ModulePower { get; set; } = false;
        public bool Trial { get; set; } = true;
        public FazeType CurrentFazeType { get; set; } = FazeType.ThreeFaze;

        public MEADevice() : base(new AutoCallbackDeviceBehavior() { CallBackMilliseconds = 5000 },
                                  new OrderStrongCommandBehavior())
        {
            this.UseBehavior(new VoltageOverloadManagerBehavior() { RNVoltageLimit = 220, RNCurrentLimit = 260, IsUsage = false });
        }

        public override async void Connect()
        {
            if (await Find("#LAB?", "AngstremLabController"))
                base.Connect();
        }
        protected override void ProcessData(string data)
        {
            CurrentData = new ControllerData(data);
            Console.WriteLine($"device callback: {data}");
            if (data.Contains("Lab phase"))
            {
                var splt = data.Split('-');
                var pht = splt[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
                if (int.Parse(pht) == 1) CurrentFazeType = FazeType.OneFaze;
                if (int.Parse(pht) == 3) CurrentFazeType = FazeType.ThreeFaze;
            }
        }


        public void SetLabPh(FazeType fazeType)
        {
            if (fazeType == FazeType.OneFaze)
            {
                Execute("#WriteLabPh, 1;");

            }
            if (fazeType == FazeType.ThreeFaze)
            {
                Execute("#WriteLabPh, 3;");
            }
        }
        public void SetExternal(int external)
        {
            Execute($"#WriteExternal,{external};");
        }
        public void SetNewMagn()
        {
            Execute("#SET_NEW_MAGN,1;");
        }
        public void SetStep(int step)
        {
            Execute("#BURN:STEP," + step + ";", () => ControllerData.Step == step);
        }
        public void SetPower(BurnPower power)
        {
            var p = 0;
            switch (power)
            {
                case BurnPower.Power50:
                    p = 50;
                    break;
                case BurnPower.Power100:
                    p = 100;
                    break;
                default:
                    p = 0;
                    break;
            }
            Execute("#BURN:POWER," + p + ";", () => ControllerData.PowerBurn == power);
        }

        public void GVI_OneFire()
        {
            Execute("#HVPULSE:ONEFIRE;");
        }
        public void GVI_LineFire(int count)
        {
            Execute($"#HVPULSE:LINEFIRE,{count};", () => ControllerData.CountFire == count);
        }
        public void GVI_FireStop()
        {
            Execute("#HVPULSE:LINESTOP;", () => ControllerData.FireMode == GVIFireMode.NoFireMode);
        }
        public void GVI_SetDelay(int delay)
        {
            Execute($"#HVPULSE:PAUSE,{delay};", () => ControllerData.FireDelay == delay);
        }
        public void GetTrial()
        {
            Execute("#ReadTrial", x => x.Contains("Full version"), () => Trial = false, () => Trial = true);
        }
        public void EnterKeys(int[] keys)
        {
            Execute($"EnterKey1,{keys[0]};", x => x.Contains("Key1 accepted"));
            Execute($"EnterKey2,{keys[0]};", x => x.Contains("Key2 accepted"));
            Execute($"EnterKey3,{keys[0]};", x => x.Contains("Key3 accepted"));
            Execute($"EnterKey4,{keys[0]};", new KeyAccepterOptionalBehavior());
        }

        public void GetFazeType()
        {
            Execute("#ReadLabPh", x => x.Contains("Lab Phase"));
        }
        public void SetModule(LabModules module)
        {
            SetDefault();
            switch (module)
            {
                case LabModules.HVMAC:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    SetVoltageLimit(100000, 5000);
                    break;
                case LabModules.HVMDC:
                    Execute("#HVM:DC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMDC);
                    SetVoltageLimit(70000, 5000);
                    break;
                case LabModules.HVMDCHi:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    SetVoltageLimit(140000, 10000);
                    break;
                case LabModules.HVBurn:
                    Execute("#HVBURN:START,MANUAL;", () => ControllerData.Module == LabModules.HVBurn);
                    SetVoltageLimit(70000, 5000);
                    break;
                case LabModules.Burn:
                    Execute("#BURN:START,MANUAL;", () => ControllerData.Module == LabModules.Burn);
                    SetVoltageLimit(220, 15);
                    SetVoltageRNLimit(230);
                    SetCurrentRNLimit(300);
                    break;
                case LabModules.JoinBurn:
                    Execute("#JOINTBURN:START,MANUAL;", () => ControllerData.Module == LabModules.JoinBurn);
                    SetVoltageLimit(45000, 5000);
                    SetVoltageRNLimit(230);
                    SetCurrentRNLimit(300);
                    break;
                case LabModules.Bridge:
                    Execute("#BRIDGE:START,MANUAL;", () => ControllerData.Module == LabModules.Bridge);
                    //something...
                    break;
                case LabModules.GVI:
                    Execute("#HVPULSE:START,MANUAL;", () => ControllerData.Module == LabModules.GVI);
                    SetVoltageLimit(9000, 500);
                    SetVoltageRNLimit(230);
                    SetCurrentRNLimit(260);
                    break;
                case LabModules.GP500:
                    Execute("#GP500:START,MANUAL;", () => ControllerData.Module == LabModules.GP500);
                    break;
                case LabModules.LVMeas:
                    Execute("#LVM:START,MANUAL;", () => ControllerData.Module == LabModules.LVMeas);
                    break;
                case LabModules.Meas:
                    Execute("#MEASURE:START,MANUAL;", () => ControllerData.Module == LabModules.Meas);
                    break;
                case LabModules.Reflect:
                    Execute("#MEASURE:START,MANUAL;", () => ControllerData.Module == LabModules.Meas);
                    break;
                case LabModules.SA640:
                    Execute("#SA640:START", () => ControllerData.Module == LabModules.SA640);
                    break;
                case LabModules.SA540_1:
                    Execute("#SA540_1:START", () => ControllerData.Module == LabModules.SA540_1);
                    break;
                case LabModules.SA540_3:
                    Execute("#SA540_3:START", () => ControllerData.Module == LabModules.SA540_3);
                    break;
                case LabModules.VLF:
                    Execute("#VLF:START", () => ControllerData.Module == LabModules.VLF);
                    break;
                case LabModules.Tangent2000:
                    Execute("#BRIDGE:START,MANUAL;", () => ControllerData.Module == LabModules.Bridge);
                    break;
                case LabModules.Parma:
                    Execute("#BRIDGE:START,MANUAL;", () => ControllerData.Module == LabModules.Bridge);
                    break;
            }
        }
        public void ResetModule()
        {
            Execute("#HVM:STOP;", () => ControllerData.Module == LabModules.Main);
        }
        public void RegulatorOn()
        {
            Execute("#VOLTAGE_REGULATOR_ENABLE", () => ControllerData.PowerInfoMessage.VREnable);
        }
        public void RegulatorOff()
        {
            Execute("#VOLTAGE_REGULATOR_DISABLE", () => !ControllerData.PowerInfoMessage.VREnable);
        }
        public void RegulatorFast()
        {
            Execute("#SPEED_FAST", d => d.Contains("Speed_fast"));
        }
        public void RegulatorSlow()
        {
            Execute("#SPEED_SLOW", d => d.Contains("Speed_slow"));
        }

        public void VoltUp()
        {
            Execute("#VOLT_STEP_UP");
        }
        public void VoltDown()
        {
            Execute("#VOLT_STEP_DOWN");
        }
        public void PowerOn()
        {
            Execute("#POWERUP", new PowerControlOptionalBehavior()
            {
                ProcessingAction = () =>
                {
                    Console.WriteLine("[??? power in progress... ???]");
                },
                SuccessAction = () =>
                {
                    Console.WriteLine("[+++ power in success +++]");
                    ModulePower = true;
                    this.EnableBehavior("VoltageOverloadManagerBehavior"); //включает контроль перегрузки напряжения и тока РН
                },
                FailureAction = pcob =>
                {
                    Console.WriteLine("[--- power in Fail ---]");
                    var mkz = (pcob as PowerControlOptionalBehavior).MKZStateInfo;
                    var msz = (pcob as PowerControlOptionalBehavior).MagnetStateInfo;
                    var hvinc = (pcob as PowerControlOptionalBehavior).HVSwitchIncorrect;
                    if (mkz.DoorLeft) Console.WriteLine(">>> error in door left");
                    if (mkz.DoorRight) Console.WriteLine(">>> error in door right");
                    if (mkz.DangerousPotencial) Console.WriteLine(">>> error in DP");
                    if (mkz.SafeKey) Console.WriteLine(">>> error in Key");
                    if (mkz.Ground) Console.WriteLine(">>> error in Ground");
                    if (mkz.Stop) Console.WriteLine(">>> error in stop key");
                    if (msz.MVKMagnetFault) Console.WriteLine(">>> error in [MVK Magnet] -> down");
                    if (msz.SVIMagnetFault) Console.WriteLine(">>> error in [SVI Magnet] -> down");
                    if (msz.MSKMagnetFault) Console.WriteLine(">>> error in [MSK Magnet] -> down");
                    if (hvinc) Console.WriteLine(">>> check HVSwitch!");
                }
            }, new CommandElementSettings
            {
                Timeout = 6000
            });
        }
        public void PowerOff()
        {
            Execute("#POWERDOWN", new PowerControlOptionalBehavior()
            {
                FaultCallback = 5000,
                PowerMode = true,
                ProcessingAction = () =>
                {
                    Console.WriteLine("[??? power off in progress... ???]");
                },
                SuccessAction = () =>
                {
                    Console.WriteLine("[+++ power off in success +++]");
                    ModulePower = false;
                    this.DisableBehavior("VoltageOverloadManagerBehavior");
                },
                FailureAction = pcob =>
                {
                    var msz = (pcob as PowerControlOptionalBehavior).MagnetStateInfo;
                    if (msz.MVKMagnetFault) Console.WriteLine(">>> error in [MVK Magnet] -> down");
                    if (msz.SVIMagnetFault) Console.WriteLine(">>> error in [SVI Magnet] -> down");
                    if (msz.MSKMagnetFault) Console.WriteLine(">>> error in [MSK Magnet] -> down");
                    Console.WriteLine("[--- power off in Fail ---]");
                }
            },
            new CommandElementSettings
            {
                Timeout = 6000
            });
        }

        public void SetVoltageLimit(double voltageLimit, double hysteresis) 
        {
            var voltageOverloadManager = this.GetOptionalBehavior<VoltageOverloadManagerBehavior>("VoltageOverloadManagerBehavior");
            voltageOverloadManager.SetVoltageLimit(voltageLimit, hysteresis);
        }
        public void SetVoltageRNLimit(int voltageRNLimit)
        {
            var voltageOverloadManager = this.GetOptionalBehavior<VoltageOverloadManagerBehavior>("VoltageOverloadManagerBehavior");
            voltageOverloadManager.SetRNVoltageLimit(voltageRNLimit);
        }
        public void SetCurrentRNLimit(int currentLimit)
        {
            var voltageOverloadManager = this.GetOptionalBehavior<VoltageOverloadManagerBehavior>("VoltageOverloadManagerBehavior");
            voltageOverloadManager.SetRNCurrentLimit(currentLimit);
        }
        public void SetDefault()
        {
            var voltageOverloadManager = this.GetOptionalBehavior<VoltageOverloadManagerBehavior>("VoltageOverloadManagerBehavior");
            voltageOverloadManager.SetDefault();
        }
    }
}
