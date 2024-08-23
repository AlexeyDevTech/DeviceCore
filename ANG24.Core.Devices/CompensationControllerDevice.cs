using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Compensation;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class CompensationControllerDevice : DeviceBase
    {
        public CompensationControllerDevice(): base(new ReqResDeviceBehavior() , new OrderStrongCommandBehavior())
        {
        }
        public override async void Connect()
        {
            if (await Find("#LAB?", "CompensationSystem_4bits"))
                base.Connect();
        }

        protected override void ProcessData(string data)
        {
            
        }
        public override void Ping()
        {
            
        }

        public void GetVoltage()
        {
            Execute("#GET_VOLTAGE");
        }
        public void GetCurrent()
        {
            Execute("#GET_CURRENT");
        }
        public void SetCoilCombination(int combination)
        {
            Execute($"#SET_COIL_COMBINATION:{combination};");
        }
        public void StartCoilSelect()
        {
            Execute("#START_COIL_SELECT", new CompensationControllerOptionalBehavior()
            {
                
            });
            
        }
    }
}
