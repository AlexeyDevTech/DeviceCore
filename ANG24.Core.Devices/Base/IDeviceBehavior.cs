namespace ANG24.Core.Devices.Base
{
    /// <summary>
    /// Стандартный интерфейс всех паттернов поведения
    /// </summary>
    public interface IDeviceBehavior
    {
        void HandleData(object data);

        void SetDevice(DeviceBase device);
    }
}
