using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.DataSources;
using ANG24.Core.Devices.External.Behaviors.CommandBehavior;
using ANG24.Core.Devices.External.Behaviors.ConnectionBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External
{
    public class MockDevice : ExecutableManagedDeviceBase
    {
        public MockDevice()
        {
            SetDataSource(new MockDataSource());
            source.SetDataReceivedType(typeof(string));
            CommandBehavior = new OrderStrongCommandDeviceBehavior();
            CommandBehavior.SetDevice(this);
            ConnectionBehavior = new AutoCallbackConnectionDeviceBehavior();
            ConnectionBehavior.SetDevice(this);

            
        }
        protected override void OnData(object data)
        {
            base.OnData(data);
        }
        public void Ping() => Execute("PING");
    }
}
