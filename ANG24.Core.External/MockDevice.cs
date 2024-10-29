namespace ANG24.Core.External
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
