using ANG24.Core.Devices.Base;
using ANG24.Core.Devices.DeviceBehaviors;
using ANG24.Core.Devices.DeviceBehaviors.Compensation;
using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class CompensationControllerDevice : DeviceBase
    {
        public int Voltage { get; set; }
        public int Current { get; set; }
        public int Combination { get; set; }
        public bool IsMatched { get; set; }
        public CompensationControllerDevice() : base(new ReqResDeviceBehavior(), new OrderStrongCommandBehavior())
        {
        }
        public override async void Connect()
        {
            if (await Find("#LAB?", "CompensationSystem_4bits"))
                base.Connect();
        }

        protected override void ProcessData(string data)
        {

            ControllerLogger.WriteString($"device callback: {data}");
            if (data.Contains("Voltage="))
            {
                Voltage = Int32.Parse(data.Split('=')[1]);
            }
            if (data.Contains("Current="))
            {
                Current = Int32.Parse(data.Split('=')[1]);
            }
            if (data.Contains("CoilState="))
            {
                Combination = Int32.Parse(data.Split('=')[1]);
            }
            if (data.Contains("Voltage must by 15..25V"))
            {

            }
            if (data.Contains("Set"))
            {
                var spl = data.Split(' ');
                Combination = Int32.Parse(spl[1]);
            }
            if (data.Contains("Error"))
            {
                var spl = data.Split(' ');
                Combination = Int32.Parse(spl[1]);
            }
            if (data.Contains("Result"))
            {
                var spl = data.Split(' ');
                Combination = Int32.Parse(spl[1]);
            }
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
        public void GetCoilState()
        {
            Execute($"#GET_COIL_STATE");
        }
        public void StartCoilSelect()
        {
            Execute("#START_COIL_SELECT", new CompensationControllerOptionalBehavior()
            {
                ProcessingAction = () =>
                {
                    Console.WriteLine("[??? Processing Action ???]");
                },
                FailureAction = a =>
                {
                    Console.WriteLine("[--- Failure Action ---]");
                },
                SuccessAction = () =>
                {
                    IsMatched = true;
                    Console.WriteLine("[+++ Success Action +++]");
                },

            });

        }
    }
}
