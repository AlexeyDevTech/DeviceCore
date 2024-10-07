using ANG24.Core.Devices.Base.DataAdapters;
using System.IO.Ports;

namespace ANG24.Core.Devices.Base.DataSources
{
    public class SerialDataSource : DataSourceBase
    {
        SerialPort port;
        public override bool Online { get => port.IsOpen; protected set => Online = value; }
        public SerialDataSource(SerialPort port) : base()
        {
            this.port = port;
            port.DataReceived += Port_DataReceived;
            adapters = new Dictionary<Type, object>
            {
                { typeof(string), new StringSerialAdapter(port) },
                { typeof(byte[]), new ByteSerialAdapter(port) },
            };
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (port.BytesToRead > 0)
            {
                var d = Read(DataReceivedType);
                Convert.ChangeType(d, DataReceivedType);
                base.DataRec(d);
            }
        }
        public SerialDataSource(string portName, int baudRate = 9600) : this(new SerialPort(portName, baudRate)) { }
        public override void Connect()
        {
            if (!Online)
            {
                try
                {
                    port.Open();
                    base.Connect();
                }
                catch { Console.WriteLine("device connect with error"); }
            }
        }
        public override void Disconnect()
        {
            if (Online)
            {
                try
                {
                    port.Close();
                    base.Disconnect();
                }
                catch { Console.WriteLine("device offline with error"); }
            }
        }
    }





}
