using ANG24.Core.Interfaces.ConnectionDeviceBehaviors;
using ANG24.Infrastructure.Logical.ConnectionBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Middleware.Factories
{
    public abstract class ConnectBehaviorFactory 
    {
    }
    public class AutoCallbackConnectionBahviorFactory : ConnectBehaviorFactory
    {
        public IConnectionDeviceBehavior Create()
        {
            return new AutoCallbackConnectionDeviceBehavior();
        }
    }
}
