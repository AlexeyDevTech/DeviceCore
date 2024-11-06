using ANG24.Core.Interfaces.External;
using ANG24.Infrastructure.Middleware.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.External
{
    public class AOTDevice : ExecutableManagedDeviceBase, ISimpleDeviceBase
    {
        public string Name { get; set; } = "AOT";
    }
}
