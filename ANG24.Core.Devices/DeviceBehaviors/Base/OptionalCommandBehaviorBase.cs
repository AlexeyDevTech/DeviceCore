﻿using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors.Base
{
    public abstract class OptionalCommandBehaviorBase : IOptionalCommandBehavior
    {
        private IDevice _device;

        public OptionalBehaviorState State { get; protected set; } = OptionalBehaviorState.NotStarted;
        public int FaultCallback { get; set; }
        public Action ProcessingAction { get; set; }
        public Action SuccessAction { get; set; }
        public Action<IOptionalCommandBehavior> FailureAction { get; set; }
        public abstract string Name { get; }
        public bool IsUsage { get; set; } = true;



        public virtual void HandleData(string data)
        {
            if (IsUsage)
            {
                if (State >= OptionalBehaviorState.Starting && State < OptionalBehaviorState.Success)
                {
                    ProcessData(data);
                }
            }
        }
        public abstract void ProcessData(string data);

        public virtual void Off()
        {
            IsUsage = false;
        }

        public virtual void On()
        {
            IsUsage = true;
        }

        public virtual void OnFail()
        {
            State = OptionalBehaviorState.Fail;
            FailureAction?.Invoke(this);
        }

        public virtual void OnSuccess()
        {
            State = OptionalBehaviorState.Success;
            SuccessAction?.Invoke();
        }
        protected virtual void OnProcess()
        {
            State = OptionalBehaviorState.Processing;
            ProcessingAction?.Invoke();
        }

        public virtual OptionalBehaviorState OperationCheck(string data) => State;

        public void SetDevice(IDevice device)
        {
            _device = device;
        }

        public void Start()
        {
            State = OptionalBehaviorState.Starting;
        }

        public void Stop()
        {
            State = OptionalBehaviorState.NotStarted;
        }

        public void RequestData(string data)
        {
            
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
