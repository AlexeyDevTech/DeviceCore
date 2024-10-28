namespace ANG24.Core.Interfaces.CommandBehaviors
{
    /// <summary>
    /// Интерфейс простых команд
    /// </summary>
    public interface ISimpleCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command);
    }



}
