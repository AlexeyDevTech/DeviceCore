using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Interfaces
{
    public interface IDeviceCreator
    {
        T Create<T>();
    }
}
