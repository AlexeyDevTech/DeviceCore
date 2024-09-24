namespace ANG24.Core.Devices.Base
{
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
