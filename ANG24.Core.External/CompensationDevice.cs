using ANG24.Core.Interfaces.External;
using ANG24.Infrastructure.Middleware.Base;
using ANG24.Infrastructure.Middleware.RedirectOptionalCommandBehavior;

namespace ANG24.Core.External
{
    public class CompensationDevice : ExecutableManagedDeviceBase, ISimpleDeviceBase
    {
        public int Voltage { get; set; }
        public int Current { get; set; }
        public int Combination { get; set; }
        public bool IsMatched { get; set; }
        public CompensationDevice() { }

        protected override void OnData(object data)
        {
            //ControllerLogger.WriteString($"device callback: {data}");
            if ((data as string).Contains("Voltage="))
            {
                Voltage = int.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("Current="))
            {
                Current = int.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("CoilState="))
            {
                Combination = int.Parse((data as string).Split('=')[1]);
            }
            if ((data as string).Contains("Voltage must by 15..25V"))
            {

            }
            if ((data as string).Contains("Set"))
            {
                var spl = (data as string).Split(' ');
                Combination = int.Parse(spl[1]);
            }
            if ((data as string).Contains("Error"))
            {
                var spl = (data as string).Split(' ');
                Combination = int.Parse(spl[1]);
            }
            if ((data as string).Contains("Result"))
            {
                var spl = (data as string).Split(' ');
                Combination = int.Parse(spl[1]);
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
