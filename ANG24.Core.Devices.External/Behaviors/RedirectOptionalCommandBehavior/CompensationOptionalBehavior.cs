using ANG24.Core.Devices.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External.Behaviors.RedirectOptionalCommandBehavior
{
    public class CompensationOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(CompensationOptionalBehavior);

        public override void ProcessData(object data)
        {
            if ((data as string).Contains("Voltage must by 15..25V"))
            {
                OnProcess();
            }
            if ((data as string).Contains("Error"))
            {
                OnFail();
            }
            if ((data as string).Contains("Result"))
            {
                OnSuccess();
            }
        }
    }
}
