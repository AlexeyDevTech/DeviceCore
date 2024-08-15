using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Types
{
    public class MagnetStateInfo
    {
        public bool MVKMagnetFault { get; set; }
        public bool SVIMagnetFault { get; set; }
        public bool MSKMagnetFault { get; set; }
    }
}
