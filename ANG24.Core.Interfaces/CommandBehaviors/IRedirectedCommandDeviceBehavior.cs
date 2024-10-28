namespace ANG24.Core.Interfaces.CommandBehaviors
{
    /// <summary>
    /// Интерфейс команд с обрабатываемым внешним паттерном поведением
    /// </summary>
    public interface IRedirectedCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior);
    }



}
