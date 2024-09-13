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
    }

    public abstract class ManagedDeviceBase : DeviceBase
    {
        IConnectionDeviceBehavior ConnectionBehavior;
        ICommandDeviceBehavior CommandBehavior;


        protected ManagedDeviceBase() { }
    }

    /// <summary>
    /// Стандартный интерфейс для реализации классов поведения подключения
    /// </summary>
   
    /// <summary>
    /// Ответвление, предлагающее методы для переопределяемых командных паттернов 
    /// </summary>
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
