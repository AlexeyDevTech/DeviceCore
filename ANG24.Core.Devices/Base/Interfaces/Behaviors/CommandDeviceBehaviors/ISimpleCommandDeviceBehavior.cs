namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    /// <summary>
    /// Интерфейс простых команд
    /// </summary>
    public interface ISimpleCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command);
    }



}
