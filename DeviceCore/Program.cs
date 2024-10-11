using ANG24.Core.Devices.Base;
using ANG24.Core.Devices.Base.DataSources;
using ANG24.Core.Devices.External;
using ANG24.Core.Devices.Types;
using System;

namespace DeviceCore
{

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.SetOut(new LoggedConsole("log.log"));

            var d = new TestDevice();
            d.Connect();
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    d.SetModule(LabModules.Burn);
                    d.SetModule(LabModules.HVBurn);
                    d.SetModule(LabModules.Burn);
                    d.SetModule(LabModules.HVMAC);
                    d.ResetModule();
                    await Task.Delay(5000);
                }
            });
            Console.ReadLine();
        }
    }

    class LoggedConsole : StreamWriter
    {
        StreamWriter sw;
        FileStream fs;

        public LoggedConsole(string logfile) : base(Console.OpenStandardOutput(), Console.OutputEncoding)
        {
            base.AutoFlush = true;
            fs = new FileStream(logfile, FileMode.Append);
            sw = new StreamWriter(fs, Console.Out.Encoding);
            sw.AutoFlush = true;
        }
        public override void WriteLine()
        {
            sw.WriteLine();
            base.WriteLine();
        }

        public override void WriteLine(string line)
        {
            sw.WriteLine(line);
            base.WriteLine(line);
        }

        public override void Write(string line)
        {
            sw.Write(line);
            base.Write(line);
        }
        //и остальные методы какие надо логировать


        public void Dispose()
        {
            sw.Close();
            sw.Dispose();
            fs.Dispose();
            base.Dispose();
        }
    }
}

