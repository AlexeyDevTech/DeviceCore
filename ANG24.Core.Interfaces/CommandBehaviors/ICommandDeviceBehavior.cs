namespace ANG24.Core.Interfaces.CommandBehaviors
{
    /// <summary>
    /// Стандартный интерфейс командных интерфейсов
    /// </summary>
    public interface ICommandDeviceBehavior : IDeviceBehavior
    {
        void RequestData();
    }
}
