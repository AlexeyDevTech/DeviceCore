namespace ANG24.Core.Devices.DeviceBehaviors.Interfaces
{
    public interface ICommandBehavior : IDeviceBehavior
    {
        void ExecuteCommand(string command); //метод для обработки отправленных команд
        void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null);
        void ExecuteCommand(string command, Func<string, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null);
        void ExecuteCommand(string command, IOptionalCommandBehavior behavior);
    }
}
