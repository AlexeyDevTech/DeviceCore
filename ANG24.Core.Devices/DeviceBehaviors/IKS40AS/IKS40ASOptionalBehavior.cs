using ANG24.Core.Devices.DeviceBehaviors.Base;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors.IKS40AS
{
    public class IKS40ASOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(IKS40ASOptionalBehavior);

        public override void ProcessData(string data)
        {
            
        }
    }
}
