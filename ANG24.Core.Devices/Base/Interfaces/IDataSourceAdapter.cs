namespace ANG24.Core.Devices.Base.Interfaces
{
    public interface IDataSourceAdapter<T>
    {
        T Read();
        void Write(T data);
    }


}
