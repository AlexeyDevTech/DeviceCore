using ANG24.Core.Devices;
using ANG24.Core.Devices.Types;
using static ANG24.Core.Devices.BridgeCommutatorDevice;

namespace DeviceCore
{

    internal class Program
    {
        //static double KT = 200;
        static async Task Main(string[] args)
        {

            var device = new PowerControllerDevice();
            device.Connect();
            
            //for(int i = 1000; i < 62000; i+=1000)
            //{
            //    var val = GetPwmFromVoltage(i);
            //    Console.WriteLine($"pwm = {val}({(val / 1982) * 100})");
            //}


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

        //public static double GetPwmFromVoltage(int voltage)
        //{

        //    return Math.Round(voltage / (KT * Math.Sqrt(2) * (220.0 / 1982.0)), 0);
        //}
    }

}

