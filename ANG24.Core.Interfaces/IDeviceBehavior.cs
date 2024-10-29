namespace ANG24.Core.Interfaces
{
    /// <summary>
    /// Стандартный интерфейс всех паттернов поведения
    /// </summary>
    public interface IDeviceBehavior
    {
        void HandleData(object data);

        void SetDevice(IDeviceBase device);
    }
}
