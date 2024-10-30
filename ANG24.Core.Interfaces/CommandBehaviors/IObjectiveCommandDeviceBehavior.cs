using ANG24.Core.Interfaces.CommandLogical;

namespace ANG24.Core.Interfaces.CommandBehaviors
{
    public interface IObjectiveCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand(ICommandElement command);
    }




}
