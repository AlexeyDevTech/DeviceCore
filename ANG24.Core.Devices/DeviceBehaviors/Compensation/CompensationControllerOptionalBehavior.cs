using ANG24.Core.Devices.DeviceBehaviors.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors.Compensation
{
    public class CompensationControllerOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(CompensationControllerOptionalBehavior);

        public override void ProcessData(object data)
        {
            var res = string.Empty;
            if(data is string) res = data as string;
            if (res.Contains("Voltage must by 15..25V"))
            {
                OnProcess();
            }
            if (res.Contains("Error"))
            {
                OnFail();
            }
            if (res.Contains("Result"))
            {
                OnSuccess();
            }
        }
    }
}
