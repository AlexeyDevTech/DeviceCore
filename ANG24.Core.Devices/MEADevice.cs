using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.DeviceBehaviors.MEA;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices
{


    public class MEADevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }

        public bool ModuleActive { get; set; }

        public MEADevice() : base(new AutoCallbackDeviceBehavior() { CallBackMilliseconds = 5000 },
                                  new OrderStrongCommandBehavior()) 
        {

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
        public void SetExternal(int  external) 
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

        }
        public void EnterKeys(int[] keys)
        {
            Execute($"EnterKey1,{keys[0]};", x => x.Contains("Key1 accepted"));
            Execute($"EnterKey2,{keys[0]};", x => x.Contains("Key2 accepted"));
            Execute($"EnterKey3,{keys[0]};", x => x.Contains("Key3 accepted"));
            Execute($"EnterKey4,{keys[0]};", x => x.Contains("Key4 accepted"));
        }

        public FazeType GetFazeType()
        {
            return FazeType.ThreeFaze;
        }


        public void SetModule(LabModules module)
        {
            switch (module)
            {
                case LabModules.HVMAC:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    break;
                case LabModules.HVMDC:
                    Execute("#HVM:DC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMDC);
                    break;
                case LabModules.HVMDCHi:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    break;
                case LabModules.HVBurn:
                    Execute("#HVBURN:START,MANUAL;", () => ControllerData.Module == LabModules.HVBurn);
                    break;
                case LabModules.Burn:
                    Execute("#BURN:START,MANUAL;", () => ControllerData.Module == LabModules.Burn);
                    break;
                case LabModules.JoinBurn:
                    Execute("#JOINTBURN:START,MANUAL;", () => ControllerData.Module == LabModules.JoinBurn);
                    break;
                case LabModules.Bridge:
                    Execute("#BRIDGE:START,MANUAL;", () => ControllerData.Module == LabModules.Bridge);
                    break;
                case LabModules.GVI:
                    Execute("#HVBURN:START,MANUAL;", () => ControllerData.Module == LabModules.GVI);
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

        }
        public void VoltDown()
        {

        }
        public void PowerOn()
        {
            Execute("#POWERUP", new PowerControlOptionalBehavior() 
            { FaultCallback = 6000, 
              ProcessingAction = () =>
              {
                  Console.WriteLine("[??? power in progress... ???]");
              },
              SuccessAction = () =>
              {
                  Console.WriteLine("[+++ power in success +++]");
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
                Timeout = 6000, TimeoutAfter = 1000
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
                Timeout = 6000,
                TimeoutAfter = 1000
            });
        }
    }
}
