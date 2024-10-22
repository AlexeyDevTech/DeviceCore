namespace ANG24.Core.Devices.Base.Interfaces.Behaviors.CommandDeviceBehaviors
{
    /// <summary>
    /// Стандартный интерфейс командных интерфейсов
    /// </summary>
    public interface ICommandDeviceBehavior : IDeviceBehavior
    {
        void RequestData();
    }
}
