namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    /// <summary>
    /// Интерфейс команд с обрабатываемым внешним паттерном поведением
    /// </summary>
    public interface IRedirectedCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior);
    }



}
