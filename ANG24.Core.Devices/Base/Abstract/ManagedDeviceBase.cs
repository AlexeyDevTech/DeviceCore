using ANG24.Core.Devices.Base.Interfaces.Behaviors.ConnectionDeviceBehaviors;

namespace ANG24.Core.Devices.Base.Abstract
{
    public abstract class ManagedDeviceBase : DeviceBase
    {
        public IConnectionDeviceBehavior ConnectionBehavior; //операция Reconnect
        public CommandDeviceBehaviorBase CommandBehavior; //операции SetCommand, Check
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
