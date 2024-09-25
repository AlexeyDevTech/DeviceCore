using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Base.Interfaces;
using ANG24.Core.Devices.Base.Interfaces.Behaviors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base
{
    public abstract class DeviceBase
    {
        protected IDataSource source;

        public DeviceBase()
        {
            
        }

        private void Source_OnData(object obj)
        {
            Console.WriteLine($"device received data: {obj}");
            OnData(obj);
        }
        private void DeviceBase_OnDisconnect()
        {
            Console.WriteLine("device offline");
        }
        private void DeviceBase_OnConnect()
        {
            Console.WriteLine("device online");
        }

        public void SetDataSource(IDataSource source)
        {
            if(this.source != null)
            {
                this.source.OnData -= Source_OnData;
                ((IConnectable)this.source).OnConnect -= DeviceBase_OnConnect;
                ((IConnectable)this.source).OnDisconnect -= DeviceBase_OnDisconnect;
            }

            this.source = source;
            this.source.OnData += Source_OnData;
            ((IConnectable)this.source).OnConnect += DeviceBase_OnConnect;
            ((IConnectable)this.source).OnDisconnect += DeviceBase_OnDisconnect;
        }

        public void Connect()
        {
            ((IConnectable)source)?.Connect();
        }
        public void Disconnect()
        {
            ((IConnectable)source)?.Disconnect();
        }

        public void Write<T>(T msg) => source.Write(msg);
        protected abstract void OnData(object data);
    }

    public abstract class ManagedDeviceBase : DeviceBase
    {
        IConnectionDeviceBehavior ConnectionBehavior; //операция Reconnect
        public CommandDeviceBehaviorBase CommandBehavior; //операции SetCommand, Check
        public OptionalBehaviorManager OptionalBehavior; //дополнительный анализ 

        protected ManagedDeviceBase() : base()
        { 
        
        }

        protected override void OnData(object data)
        {
            ConnectionBehavior.HandleData(data);            //для коннект-менеджера
            CommandBehavior.HandleData(data);               //для команд-менеджера
            OptionalBehavior.HandleData(data);              //для опционал менеджера
        }
    }
    public abstract class ExecutableManagedDeviceBase : ManagedDeviceBase
    {
        public void Execute<T>(T command) => CommandBehavior.ExecuteCommand(command);
        public void Execute<T>(T command, Func<bool>? predicate, Action? IfTrue, Action? IfFalse) => CommandBehavior.ExecuteCommand(command, predicate, IfTrue, IfFalse);
        public void Execute<T>(T command, Func<object, bool> predicate, Action? IfTrue, Action? IfFalse) => CommandBehavior.ExecuteCommand(command, predicate, IfTrue, IfFalse);
        public void Execute<T>(T command, IOptionalCommandBehavior behavior) => CommandBehavior.ExecuteCommand(command, behavior);
    }

    public class BehaviorManager
    {
        protected BehaviorManager() { }

    }

    public interface IOptionalCommandBehavior : IOptionalBehavior
    {
        OptionalBehaviorState State { get; }
        public int FaultCallback { get; set; }
        OptionalBehaviorState OperationCheck(object data);
        Action ProcessingAction { get; set; }
        Action SuccessAction { get; set; }
        Action<IOptionalCommandBehavior> FailureAction { get; set; }
        void Start();
        void Stop();
        void OnSuccess();
        void OnFail();
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
