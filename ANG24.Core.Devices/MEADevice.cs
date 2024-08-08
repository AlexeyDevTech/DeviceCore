using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices
{


    public class MEADevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }

        public MEADevice() : base(new AutoCallbackDeviceBehavior() { CallBackMilliseconds = 5000 },
                                  new OrderStrongCommandBehavior()) { }

        public override async void Connect()
        {
            if (await Find("#LAB?", "AngstremLabController"))
                base.Connect();
        }
        protected override void ProcessData(string data)
        {
            CurrentData = new ControllerData(data);
            Console.WriteLine($"device callback: {data}");
        }

        public void SetModule(string module) => Execute($"#{module}:START,MANUAL;", () => nameof(ControllerData.Module) == module, () => Console.WriteLine("Module is changed success"));
    }
}
