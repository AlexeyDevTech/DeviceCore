using ANG24.Core.Devices.Base.DataAdapters;

namespace ANG24.Core.Devices.Base.DataSources
{
    public class MockDataSource : DataSourceBase
    {
        public override bool Online { get; protected set; }

        public MockDataSource() 
        {
            adapters = new Dictionary<Type, object>
            {
                { typeof(string), new StringMockAdapter() },
            };
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(300);
                    DataRec("Data simulation");
                }
            });
        }
        public override void Connect()
        {
            Online = true;
            base.Connect();
        }
        public override void Disconnect()
        {
            Online = false;
            base.Disconnect();
        }
    }
}
