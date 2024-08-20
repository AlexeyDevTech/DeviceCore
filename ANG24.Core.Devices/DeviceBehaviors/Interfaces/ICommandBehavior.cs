namespace ANG24.Core.Devices.DeviceBehaviors.Interfaces
{
    public interface ICommandBehavior : IDeviceBehavior
    {
        int TickMilliseconds { get; internal set; }
        void ExecuteCommand(string command, CommandElementSettings? settings = null); //метод для обработки отправленных команд
        void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings? settings = null);
        void ExecuteCommand(string command, Func<string, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null, CommandElementSettings? settings = null);
        void ExecuteCommand(string command, IOptionalCommandBehavior behavior, CommandElementSettings? settings = null);
    }

    public class CommandElementSettings
    {
        public int Timeout { get; set; }
        public int MessageAttempts { get; set; }
        public int Attempts { get; internal set; }
    }
}
