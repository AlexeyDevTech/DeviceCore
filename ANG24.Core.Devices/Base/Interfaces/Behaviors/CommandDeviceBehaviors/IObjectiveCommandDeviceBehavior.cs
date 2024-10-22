using ANG24.Core.Devices.Base.Abstract.Types;

namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    public interface IObjectiveCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand(CommandElement command);
    }




}
