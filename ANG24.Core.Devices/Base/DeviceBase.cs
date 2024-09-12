using System;
using System.Collections.Generic;
using System.Diagnostics;
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


    public interface IConnectable 
    {
        event Action OnConnect;
        event Action OnDisconnect;
        void Connect();
        void Disconnect();

    }
    public interface IDataSource
    {
        string Name { get; }
        Type DataReceivedType { get; }
        event Action OnData;
        T Read<T>();
        object Read(Type type);
        void Write<T>(T data);

    }
    public interface IDataSourceAdapter<T>
    {
        T Read();
        void Write(T data);
    }
   

    public class SerialDataSource : DataSourceBase
    {
        SerialPort port;
        public string Name { get; }
        public override bool Online => port.IsOpen;

        public event Action OnData;

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
            while(port.BytesToRead > 0)
            {
                Console.WriteLine(Read(DataReceivedType)); 
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

    public abstract class DataSourceBase : IDataSource, IConnectable
    {
        public string Name { get; }
        public abstract bool Online { get; }
        public Type DataReceivedType { get; protected set; }


        public event Action OnData;
        public event Action OnConnect;
        public event Action OnDisconnect;


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
        protected object GetAdapter(Type type)
        {
            if (adapters.TryGetValue(type, out var adapter))
            {
                return adapter;
            }

            throw new InvalidOperationException($"No adapter found for type {type.Name}");
        }
        public void SetDataReceivedType(Type type) => DataReceivedType = type;
        public virtual T Read<T>()
        {
            return GetAdapter<T>().Read();
        }
        public virtual object Read(Type type)
        {
            var adapter = GetAdapter(type);

            // Используем reflection, чтобы вызвать метод Read у адаптера
            var method = adapter.GetType().GetMethod("Read");
            var result = method.Invoke(adapter, null);
            // Приводим результат к нужному типу
            return Convert.ChangeType(result, type);
        }
        public virtual void Write<T>(T data)
        {
            GetAdapter<T>().Write(data);
        }
        public virtual void Connect()
        {
            OnConnect?.Invoke();
        }
        public virtual void Disconnect()
        {
            OnDisconnect?.Invoke();
        }
    }



}
