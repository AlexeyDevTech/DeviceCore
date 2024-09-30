using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANG24.Core.Devices.Interfaces;

namespace ANG24.Core.Devices.Creators
{
    public class DeviceCreator : IDeviceCreator
    {

        public DeviceCreator() { }

        public T Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}
