using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.Interfaces.Behaviors;

namespace ANG24.Core.Devices.Extensions
{



    #region Extensions for DeviceBase
    public static class DeviceBaseExtensions
    {
        public static DeviceBase UseBehavior(this DeviceBase device, IOptionalBehavior behavior)
        {
            (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.Add(behavior);
            behavior.SetDevice(device);
            return device;
        }
        public static DeviceBase EnableBehavior(this DeviceBase device, string Name)
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            //var item = device._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.On();
            return device;
        }
        public static DeviceBase DisableBehavior(this DeviceBase device, string Name)
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null) item.Off();
            return device;
        }
        public static T GetBehavior<T>(this DeviceBase device, string Name) where T : class, IOptionalBehavior
        {
            var item = (device as ManagedDeviceBase).OptionalBehavior._optionalBehaviors.FirstOrDefault(x => x.Name == Name);
            if (item != null)
            {
                return item as T;
            }
            else
            {
                throw new NullReferenceException($"Object {nameof(T)} was not found!!!");
            }
        }
        public static DeviceBase UseOption(this DeviceBase device, string Name, Action<object> action, bool Active = true)
        {
            (device as ManagedDeviceBase).OptionalBehavior.AddOption(Name, action, Active);
            return device;
        }
        public static DeviceBase UseOption(this DeviceBase device, string Name, Func<object, bool> predicate, Action<object> action, bool Active = true)
        {
            (device as ManagedDeviceBase).OptionalBehavior.AddPredicatedOption(Name, predicate, action, Active);
            return device;
        }
        public static DeviceBase ClearOptionList(this DeviceBase device)
        {
            (device as ManagedDeviceBase).OptionalBehavior.Clear();
            return device;
        }
        public static DeviceBase RemoveOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.RemoveOption(Name);
            return device;
        }
        public static DeviceBase DisableOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.DisableOption(Name);
            return device;
        }
        public static DeviceBase EnableOption(this DeviceBase device, string Name)
        {
            (device as ManagedDeviceBase).OptionalBehavior.EnableOption(Name);
            return device;
        }

    }
    #endregion
}
