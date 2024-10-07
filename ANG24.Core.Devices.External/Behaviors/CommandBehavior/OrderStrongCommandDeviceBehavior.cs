using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External.Behaviors.CommandBehavior
{
    public class OrderStrongCommandDeviceBehavior : CommandDeviceBehaviorBase
    {
        public override void HandleData(object data)
        {
            base.HandleData(data);
            //check...
            //Check(data);
        }

        public override void RequestData()
        {
            //[not required] request...
        }

        protected override void CommandTick()
        {
            base.CommandTick();
            //set command...
            Console.Write("[]");
        }

       

    }
}
