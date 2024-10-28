namespace ANG24.Core.Interfaces
{
    public interface IDataSourceAdapter<T>
    {
        T Read();
        void Write(T data);
    }


}
