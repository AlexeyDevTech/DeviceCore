using ANG24.Core.Devices;

namespace DeviceCore
{

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var device = new MEADevice();
            device.Connect();

            Console.ReadKey();

        }
    }
   
}
   
