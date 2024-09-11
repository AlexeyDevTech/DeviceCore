using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base
{
    public class DeviceBase
    {
        IDataSource source;
    }

    public interface IDataSource
    {
        string Name { get; }
        Type DataReceivedType { get; set; }
        event Action OnData;
        T Read<T>();
        void Write<T>(T data);

    }
    public interface IDataSourceAdapter<T>
    {
        T Read();
        void Write(T data);
    }


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
           return port.ReadLine();
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
            port.Read(arr, 0, arr.Length);
            return arr;
        }

        public override void Write(byte[] data)
        {
            port.Write(data, 0, data.Length);
        }
    }

    public class SerialDataSource : DataSourceBase
    {
        SerialPort port;
        public string Name { get; }

        public event Action OnData;

        public SerialDataSource(SerialPort port) : base() 
        {
            this.port = port;
            adapters = new Dictionary<Type, object>
            {
                { typeof(string), new StringSerialAdapter(port) },
                { typeof(byte[]), new ByteSerialAdapter(port) },
            };
        }
        public SerialDataSource(string portName, int baudRate = 9600) : this(new SerialPort(portName, baudRate)) { }
        public override T Read<T>()
        {
            return GetAdapter<T>().Read();
        }

        public override void Write<T>(T data)
        {
            GetAdapter<T>().Write(data);
        }
    }

    public abstract class DataSourceBase : IDataSource
    {
        public string Name { get; }
        public Type DataReceivedType { get; set; }

        public event Action OnData;
        protected Dictionary<Type, object> adapters;

        protected DataSourceBase()
        {
          adapters = new Dictionary<Type, object>();
        }

        protected IDataSourceAdapter<T> GetAdapter<T>()
        {
            if (adapters.TryGetValue(typeof(T), out var adapter))
            {
                return (IDataSourceAdapter<T>)adapter;
            }

            throw new InvalidOperationException($"No adapter found for type {typeof(T).Name}");
        }

        public abstract T Read<T>();

        public abstract void Write<T>(T data);
    }



}
