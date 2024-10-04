using ANG24.Core.Devices.Base.Interfaces;
using System.IO.Ports;

namespace ANG24.Core.Devices.Base.DataAdapters
{

    //абстракция для обобщения устройств для COM порта 
    public abstract class SerialAdapter<T> : IDataSourceAdapter<T>
    {
        protected SerialPort port;
        protected SerialAdapter(SerialPort port)
        {
            this.port = port;
        }

        public abstract T Read();

        public abstract void Write(T data);
    }

    public class StringSerialAdapter : SerialAdapter<string>
    {

        public StringSerialAdapter(SerialPort port) : base(port) { }

        public override string Read()
        {
            var res = string.Empty;
            try
            {
                res = port.ReadLine();
            }
            catch (UnauthorizedAccessException uaex) { }
            catch (InvalidOperationException ioex) { }
            catch (Exception ex) { }
            return res;
        }

        public override void Write(string data)
        {
            port.Write(data);
        }
    }
    public class ByteSerialAdapter : SerialAdapter<byte[]>
    {
        public ByteSerialAdapter(SerialPort port) : base(port) { }

        public override byte[] Read()
        {

            var arr = new byte[port.BytesToRead];
            try
            {
                port.Read(arr, 0, arr.Length);
            }
            catch (UnauthorizedAccessException uaex) { }
            catch (InvalidOperationException ioex) { }
            catch (Exception ex) { }
            return arr;
        }

        public override void Write(byte[] data)
        {
            port.Write(data, 0, data.Length);
        }
    }


}
