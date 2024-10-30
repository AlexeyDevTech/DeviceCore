using ANG24.Core.Interfaces.ConnectionDeviceBehaviors;
using ANG24.Infrastructure.Physical.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Middleware.Base
{
    public abstract class ManagedDeviceBase : SimpleDeviceBase
    {
        public IConnectionDeviceBehavior ConnectionBehavior; //операция Reconnect
        public ICommandDeviceBehaviorBase CommandBehavior; //операции SetCommand, Check
        public OptionalBehaviorManager OptionalBehavior; //дополнительный анализ 

        protected ManagedDeviceBase() : base()
        {
        }

        protected override void OnData(object data)
        {
            ConnectionBehavior?.HandleData(data);            //для коннект-менеджера
            CommandBehavior?.HandleData(data);               //для команд-менеджера
            OptionalBehavior?.HandleData(data);              //для опционал менеджера
        }

        public void Start() => ConnectionBehavior?.Start();
        public void Stop() => CommandBehavior?.Stop();

    }
}
