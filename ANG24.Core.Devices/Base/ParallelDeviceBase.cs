using ANG24.Core.Devices.DeviceBehaviors.Interfaces;

namespace ANG24.Core.Devices.Base
{
    public abstract class ParallelDeviceBase : DeviceBase
    {
        public int Address { get; set; }
        protected ParallelDeviceBase(IDeviceBehavior behavior, ICommandBehavior commandBeahvior) : base(behavior, commandBeahvior)
        {
        }




    }
}
