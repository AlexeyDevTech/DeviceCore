using ANG24.Core.Devices.Interfaces;

namespace ANG24.Core.Devices.Creators
{
    public class DeviceCreator : IDeviceCreator
    {

        public DeviceCreator() { }

        public T Create<T>() where T : class, new() 
        {
            return new T();
        }
    }
}
