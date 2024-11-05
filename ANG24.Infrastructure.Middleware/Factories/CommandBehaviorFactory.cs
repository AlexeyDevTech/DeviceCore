using ANG24.Core.Interfaces.CommandBehaviors.Realizations;
using ANG24.Infrastructure.Logical.Base;
using ANG24.Infrastructure.Logical.CommandBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Middleware.Factories
{
    public abstract class CommandBehaviorFactory
    {
    }
    public class OrderStrongCommandBehaviorFactory : CommandBehaviorFactory
    {
        public CommandDeviceBehaviorBase Create()
        {
            return new OrderStrongCommandDeviceBehavior();
        }
    }
}
