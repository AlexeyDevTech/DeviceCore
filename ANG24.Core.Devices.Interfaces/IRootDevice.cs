using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Interfaces
{
    public interface IRootDevice
    {
        T SelectDevice<T>() where T : class, IDevice, new();
    }
}
