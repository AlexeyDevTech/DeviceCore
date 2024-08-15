using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using ANG24.Core.Devices.Types;

namespace ANG24.Core.Devices.DeviceBehaviors.MEA
{
    public class PowerControlOptionalBehavior : IOptionalCommandBehavior
    {
        private IDevice _device;
        public bool IsUsage { get; set; } = true;
        public bool PowerMode { get; set; } = false; //0 -- включение, 1 -- выключение
        public string Name => nameof(PowerControlOptionalBehavior);
        public OptionalBehaviorState State { get; private set; } = OptionalBehaviorState.NotStarted;
        public int FaultCallback { get; set; } = 5000;
        public Action ProcessingAction { get; set; }
        public Action SuccessAction { get; set; }
        public Action<IOptionalCommandBehavior> FailureAction { get; set; }
        
        public MKZStateInfo MKZStateInfo { get; set; } = new MKZStateInfo();
        public MagnetStateInfo MagnetStateInfo { get; set; } = new MagnetStateInfo();
        public bool HVSwitchIncorrect { get; set; }

        public void HandleData(string data)
        {
            if (IsUsage)
            {
                if (State >= OptionalBehaviorState.Starting && State < OptionalBehaviorState.Success)
                {
                    if (!PowerMode)
                    {
                        if (data.Contains("shorter is up")) OnProcess();
                        if (data.Contains("shorter is down")) OnFail();
                        if (data.Contains("power is ready")) OnSuccess();
                        if (data.Contains("power is fault")) OnFail();
                        if (data.Contains("MKZ:"))
                        {
                            var spl = data.Split(':');
                            var spl_desc = spl[1].Trim();
                            if(spl_desc.Contains("Door 2")) MKZStateInfo.DoorRight = true;
                            if(spl_desc.Contains("Door 1")) MKZStateInfo.DoorLeft = true;
                            if(spl_desc.Contains("Ground")) MKZStateInfo.Ground = true;
                            if(spl_desc.Contains("Dangerous potential")) MKZStateInfo.DangerousPotencial = true;
                            if(spl_desc.Contains("Key")) MKZStateInfo.SafeKey = true;
                            if(spl_desc.Contains("Stop button")) MKZStateInfo.Stop = true;
                            OnProcess();
                        }
                        if (data.Contains("Check"))
                        {
                            var spl = data.Split(' ');
                            var spl_desc = spl[1].Trim();
                            if(spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                            if(spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                            if(spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                            OnProcess();
                        }
                        if (data.Contains("MKZ error")) OnFail();
                        if (data.Contains("HVSwitch mode is incorrect"))
                        {
                            HVSwitchIncorrect = true;
                            OnFail();
                        }
                    } else
                    {
                        if (data.Contains("Check"))
                        {
                            var spl = data.Split(' ');
                            var spl_desc = spl[1].Trim();
                            if (spl_desc.Contains("MVK")) MagnetStateInfo.MVKMagnetFault = true;
                            if (spl_desc.Contains("SVI")) MagnetStateInfo.SVIMagnetFault = true;
                            if (spl_desc.Contains("MSK")) MagnetStateInfo.MSKMagnetFault = true;
                            OnProcess();
                        }
                        if (data.Contains("Power off is OK")) OnSuccess();
                        if (data.Contains("Power off is fault")) OnFail();
                    }

                }
            }
        }

        private void OnProcess()
        {
            State = OptionalBehaviorState.Processing;
            ProcessingAction?.Invoke();
        }
        public void OnFail()
        {
            if(PowerMode)
            {
                new Timer(state =>
                {
                    State = OptionalBehaviorState.Fail;
                    FailureAction?.Invoke(this);
                }).Change(1000, Timeout.Infinite);
            }
            else
            {
                State = OptionalBehaviorState.Fail;
                FailureAction?.Invoke(this);
            }
            
        }
        public void OnSuccess()
        {
            State = OptionalBehaviorState.Success;
            SuccessAction?.Invoke();
        }
        public void Off()
        {
            IsUsage = false;
        }

        public void On()
        {
            IsUsage = true;
        }

        public void SetDevice(IDevice device)
        {
            _device = device;
        }
        public OptionalBehaviorState OperationCheck(string data) => State;
        public void Start()
        {
            State = OptionalBehaviorState.Starting;
        }
        public void Stop()
        {
            State = OptionalBehaviorState.NotStarted;
        }

    }

    public enum OptionalBehaviorState : int
    {
        NotStarted = 0,
        Starting = 1,
        Processing = 2,
        Success = 3,
        Fail = 4
    }
}
