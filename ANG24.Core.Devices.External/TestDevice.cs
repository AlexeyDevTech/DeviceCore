using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.DataSources;
using ANG24.Core.Devices.Extensions;
using ANG24.Core.Devices.External.Behaviors.CommandBehavior;
using ANG24.Core.Devices.External.Behaviors.ConnectionBehavior;
using ANG24.Core.Devices.External.Behaviors.RedirectOptionalCommandBehavior;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices.External
{
    public class TestDevice : ExecutableManagedDeviceBase
    {
        private ControllerData CurrentData;

        public TestDevice()
        {
            //SetDataSource(new SerialDataSource("COM4"));
            //source.SetDataReceivedType(typeof(string));
            //CommandBehavior = new OrderStrongCommandDeviceBehavior();
            //CommandBehavior.SetDevice(this);
            //ConnectionBehavior = new AutoCallbackConnectionDeviceBehavior();
            //ConnectionBehavior.SetDevice(this);
            this.SelectDataSource(new SerialDataSource("COM4"))
                .SetDataReceivedType(typeof(string))
                .SetCommandBehavior(new OrderStrongCommandDeviceBehavior())
                .SetConnectionBehavior(new AutoCallbackConnectionDeviceBehavior(), Disconnect, Connect);
        }
        protected override void OnData(object data)
        {
            /*
             * Если в условиях команд присутствуют локальные переменные контроллера -- необходимо
             * сначала их обновить. Для этого прописать base.OnData(data) -- последним
             * Иначе -- порядок не важен
             */

            //Console.WriteLine("[[data updated to controller]]");
            if (data != null)
                CurrentData = new ControllerData(data as string);
            Console.WriteLine(data);
            base.OnData(data); //в этом случае -- сначала обновляем котроллер потом OnData
        }
        public void SetModule(LabModules module)
        {
            Console.WriteLine($"module set -> {module}");
            //SetDefault();
            switch (module)
            {
                case LabModules.HVMAC:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    //SetVoltageLimit(100000, 5000);
                    break;
                case LabModules.HVMDC:
                    Execute("#HVM:DC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMDC);
                    //SetVoltageLimit(70000, 5000);
                    break;
                case LabModules.HVMDCHi:
                    Execute("#HVM:AC,MANUAL,START;", () => ControllerData.Module == LabModules.HVMAC);
                    //SetVoltageLimit(140000, 10000);
                    break;
                case LabModules.HVBurn:
                    Execute("#HVBURN:START,MANUAL;", () => ControllerData.Module == LabModules.HVBurn);
                    //SetVoltageLimit(70000, 5000);
                    break;
                case LabModules.Burn:
                    Execute("#BURN:START,MANUAL;", () => ControllerData.Module == LabModules.Burn);
                    //SetVoltageLimit(220, 15);
                    //SetVoltageRNLimit(230);
                    //SetCurrentRNLimit(300);
                    break;
                case LabModules.JoinBurn:
                    Execute("#JOINTBURN:START,MANUAL;", () => ControllerData.Module == LabModules.JoinBurn);
                    //SetVoltageLimit(45000, 5000);
                    //SetVoltageRNLimit(230);
                    //SetCurrentRNLimit(300);
                    break;
                case LabModules.Bridge:
                    Execute("#BRIDGE:START,MANUAL;", () => ControllerData.Module == LabModules.Bridge);
                    //something...
                    break;
                case LabModules.GVI:
                    Execute("#HVPULSE:START,MANUAL;", () => ControllerData.Module == LabModules.GVI);
                    //SetVoltageLimit(9000, 500);
                    //SetVoltageRNLimit(230);
                    // SetCurrentRNLimit(260);
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
        public void PowerOn() => Execute("#POWERUP", new PowerControlOptionalBehavior());
        public void PowerOff() => Execute("#POWERDOWN", new PowerControlOptionalBehavior() { PowerMode = true });
    }
}
