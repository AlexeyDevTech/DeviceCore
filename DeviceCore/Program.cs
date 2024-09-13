using ANG24.Core.Devices.Base;

namespace DeviceCore
{

    internal class Program
    {

        static async Task Main(string[] args)
        {
            var main = new MEADevice();
            main.Connect();
            main.Write("#HVM:STOP");
            Console.ReadKey();

        }
    }
}

