using ANG24.Core.Devices;
using ANG24.Core.Devices.Types;
using static ANG24.Core.Devices.BridgeCommutatorDevice;

namespace DeviceCore
{

    internal class Program
    {
        static async Task Main(string[] args)
        {

            //var device = new PowerControllerDevice();
            //device.Connect();
            

            
            //Task.Run(async () =>
            //{
            //    await Task.Delay(4000);
            //    device.GetMode();
            //});
            //Console.WriteLine("Hello, World!");
            //var device = new MEADevice();
            //device.Connect();
            //device.SetModule(LabModules.HVMAC);
            //device.PowerOn();
            //await Task.Delay(10000);
            //device.PowerOff();
            //device.SetModule(LabModules.HVMDC);
            //await Task.Delay(10000);
            ////device.PowerOff();
            //device.ResetModule();
            //// device.SetModule(LabModules.HVMDC);
            //device.SetModule(LabModules.Burn);
            //device.SetModule(LabModules.Reflect);
            //device.SetModule(LabModules.JoinBurn);
            //device.SetModule(LabModules.HVBurn);
            //device.SetModule(LabModules.Bridge);
            //device.ResetModule();
            //Task.Run(async () =>
            //{
            //    await Task.Delay(4000);
            //    device.SetModule(LabModules.Burn);
            //    device.SetModule(LabModules.Reflect);
            //    device.SetModule(LabModules.JoinBurn);
            //    device.SetModule(LabModules.HVBurn);
            //    device.SetModule(LabModules.Bridge);

            //});


            Console.ReadKey();

        }
    }

}

