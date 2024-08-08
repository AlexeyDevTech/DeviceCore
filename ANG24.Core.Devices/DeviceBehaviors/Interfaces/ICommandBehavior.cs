using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors.Interfaces
{
    public interface ICommandBehavior : IDeviceBehavior
    {
        void ExecuteCommand(string command); //метод для обработки отправленных команд
        void ExecuteCommand(string command, Func<bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null);
        void ExecuteCommand<T>(string command, Func<T, bool>? predicate = null, Action? ifTrue = null, Action? ifFalse = null);
    }
}
