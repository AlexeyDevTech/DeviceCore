namespace ANG24.Core.Devices.Base
{
    /// <summary>
    /// Стандартный интерфейс всех паттернов поведения
    /// </summary>
    public interface IDeviceBehavior
    {
        void HandleData();
    }
    public interface IConnectionDeviceBehavior : IDeviceBehavior
    {
        void Reconnect();
    }

    /// <summary>
    /// Интерфейс для реализации всех опциональных паттернов
    /// </summary>
    public interface IOptionalBehavior : IDeviceBehavior
    {
        public string Name { get; }
        bool IsUsage { get; set; }
        void On();
        void Off();
    }
}
