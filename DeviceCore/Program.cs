using ANG24.Core.Devices.Base;

namespace DeviceCore
{

    internal class Program
    {

        static async Task Main(string[] args)
        {
            var main = new SerialDataSource("COM3");
            main.SetDataReceivedType(typeof(string));
            main.OnConnect += () => Console.WriteLine("device <MAIN> connected");
            main.OnDisconnect += () => Console.WriteLine("device <MAIN> offline");
            main.Connect();
            new Timer(state =>
            {
                main.SetDataReceivedType(typeof(byte[]));
            }).Change(5000, Timeout.Infinite);
            Console.ReadKey();

        }
    }
}

