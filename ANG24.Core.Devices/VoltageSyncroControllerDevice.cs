using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class VoltageSyncroControllerDevice : DeviceBase
    {
        public int Mode { get; set; }
        //[Prozorov] здесь предпочтителен способ с пингом
        //[Prozorov] указать параметры behavior
        public VoltageSyncroControllerDevice() : base(new ReqResWithTimeCallBackDeviceBehavior() { CallBackMilliseconds = 1000, ReqResMilliseconds = 2000}, new OrderStrongCommandBehavior())
        {
        }
        public override async void Connect()
        {
            if (await Find("#LAB?", "Voltage Regulator Synchronizer"))
                base.Connect();
        }
        public override void Ping()
        {
            //[Prozorov] указать метод пинга
            GetMode();
        }

        protected override void ProcessData(string data)
        {
            if (data.Contains("RNSMODE"))
            {
                Mode = int.Parse(data.Split(':')[1]);
            }
        }

        public void GetMode()
        {
            Execute("#GETMODE");
        }
        public void SetMode(int mode)
        {
            //[Prozorov] указать ответ 
            Execute($"#SETMODE,{mode};", () => Mode == mode);
        }

    }
}
