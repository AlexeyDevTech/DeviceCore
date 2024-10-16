using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.External.Behaviors.RedirectOptionalCommandBehavior;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.External
{
    public class CompensationDevice : ExecutableManagedDeviceBase, ICompensationDevice
    {
        public int Voltage { get; set; }
        public int Current { get; set; }
        public int Combination { get; set; }
        public bool IsMatched { get; set; }
        public CompensationDevice() { }

        protected override void OnData(object data)
        {
            ControllerLogger.WriteString($"device callback: {data}");
            if ((data as string).Contains("Voltage="))
            {
                Voltage = Int32.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("Current="))
            {
                Current = Int32.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("CoilState="))
            {
                Combination = Int32.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("Voltage must by 15..25V"))
            {

            }
            if ((data as string).Contains("Set"))
            {
                var spl = (data as string).Split(' ');
                Combination = Int32.Parse(spl[1]);
            }
            if ((data as string).Contains("Error"))
            {
                var spl = (data as string).Split(' ');
                Combination = Int32.Parse(spl[1]);
            }
            if ((data as string).Contains("Result"))
            {
                var spl = (data as string).Split(' ');
                Combination = Int32.Parse(spl[1]);
            }

            base.OnData(data);
        }
        // Find("#LAB?", "CompensationSystem_4bits"))


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
            Execute("#START_COIL_SELECT", new CompensationOptionalBehavior()
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
