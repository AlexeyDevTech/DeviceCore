namespace ANG24.Core.Devices.Base.Interfaces
{
    public interface IDataSource
    {
        string Name { get; }
        Type DataReceivedType { get; }
        event Action<object> OnData;

        void SetDataReceivedType(Type dataReceivedType);
        T Read<T>();
        object Read(Type type);
        void Write<T>(T data);
        void Write(Type type, object data);

    }
}
