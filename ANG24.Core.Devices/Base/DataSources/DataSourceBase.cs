using ANG24.Core.Devices.Base.Interfaces;

namespace ANG24.Core.Devices.Base.DataSources
{
    public abstract class DataSourceBase : IDataSource, IConnectable
    {
        public string Name { get; }
        public abstract bool Online { get; protected set; }
        public Type DataReceivedType { get; protected set; }


        public event Action<object> OnData;
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
        public void Write(Type type, object data)
        {
            var adapter = GetAdapter(type);
            var method = adapter.GetType().GetMethod("Write");
            var result = method?.Invoke(adapter, [ data ]);
        }
        public virtual void DataRec(object data) => OnData?.Invoke(data);
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
