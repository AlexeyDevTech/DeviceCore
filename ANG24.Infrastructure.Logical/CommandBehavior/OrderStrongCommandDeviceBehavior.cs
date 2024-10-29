using ANG24.Infrastructure.Logical.Base;

namespace ANG24.Infrastructure.Logical.CommandBehavior
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
            //Console.Write("[]");
        }



    }
}
