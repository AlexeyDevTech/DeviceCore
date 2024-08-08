using ANG24.Core.Devices.Interfaces;

namespace ANG24.Core.Devices.DeviceBehaviors.Interfaces
{
    public interface IDeviceBehavior
    {
        void HandleData(string data); //метод для обработки данных
        void SetDevice(IDevice device);
    }
}
