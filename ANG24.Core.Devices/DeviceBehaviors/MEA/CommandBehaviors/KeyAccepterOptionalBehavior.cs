using ANG24.Core.Devices.DeviceBehaviors.Base;

namespace ANG24.Core.Devices.DeviceBehaviors.MEA.CommandBehaviors
{
    public class KeyAccepterOptionalBehavior : OptionalCommandBehaviorBase
    {
        public override string Name => nameof(KeyAccepterOptionalBehavior);

        public override void ProcessData(string data)
        {
            if (data.Contains("accepted")) OnProcess();
            if (data.Contains("The keys are wrong")) OnFail();
            if (data.Contains("The keys are correct")) OnSuccess();
        }
    }
}
