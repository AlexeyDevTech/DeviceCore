using ANG24.Core.Devices.DeviceBehaviors;
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
    public class BridgeCommutatorDevice : DeviceBase
    {
        public ControllerData CurrentData { get; set; }
        public BridgeCommutatorState CurrentState { get; set; } = BridgeCommutatorState.Zero_State;
        public BridgeCommutatorPhase CurrentPhase { get; set; } = BridgeCommutatorPhase.Normal;
        public BridgeCommutatorDevice() : base(new ReqResDeviceBehavior(), new OrderStrongCommandBehavior())
        {
        }

        public override async void Connect()
        {
            if (await Find("#LAB?", "BridgeCommutator", 115200))
                base.Connect();
        }
        protected override void ProcessData(string data)
        {
            CurrentData = new ControllerData(data);
            ControllerLogger.WriteString($"device callback: {data}");
            //нужна информация о том что прилетает с устройства

            //if (data.Contains("State"))
            //{
            //    var splt = data.Split('=');
            //    var stateStr = splt[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
            //    if(Int32.TryParse(stateStr, out int state))
            //    {

            //    }

            //}


        }

        public void GetState()
        {
            Execute("#GET_STATE");
        }
        public void SetState(BridgeCommutatorState state)
        {
            Execute($"#SET_STATE:{state}");
            CurrentState = state;
        }

        public void ResetPhase()
        {
            Execute("#RESET_PHASE", () => CurrentPhase == BridgeCommutatorPhase.Reverse, () => CurrentPhase = BridgeCommutatorPhase.Normal);
            //if (CurrentPhase == BridgeCommutatorPhase.Reverse)
            //{
            //    CurrentPhase = BridgeCommutatorPhase.Normal;
            //}
        }
        public void FlopPhase()
        {
            Execute("#FLOP_PHASE", () => CurrentPhase == BridgeCommutatorPhase.Normal,
                                   () => CurrentPhase = BridgeCommutatorPhase.Reverse,
                                   () => CurrentPhase = BridgeCommutatorPhase.Normal);
            //if (CurrentPhase == BridgeCommutatorPhase.Normal)
            //{
            //    CurrentPhase = BridgeCommutatorPhase.Reverse;
            //}
            //else
            //{
            //    CurrentPhase = BridgeCommutatorPhase.Normal;
            //}
        }

        public override void Ping()
        {

        }

        public enum BridgeCommutatorState : int
        {
            Zero_State = 0,
            First_State = 1,
            Second_State = 2,
            Third_State = 4,
        }
        public enum BridgeCommutatorPhase : int
        {
            Normal = 0x00,
            Reverse = 0x08,
        }
    }
}
