using ANG24.Core.Devices.Base.Abstract;
using System.ComponentModel.Design;

namespace ANG24.Core.Devices.Base.Interfaces.Behaviors
{

#region interfaces CommandDeviceBehavior
    /// <summary>
    /// Стандартный интерфейс командных интерфейсов
    /// </summary>
    public interface ICommandDeviceBehavior : IDeviceBehavior
    {
        void RequestData();
    }
    /// <summary>
    /// Интерфейс простых команд
    /// </summary>
    public interface ISimpleCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command);
    }
    /// <summary>
    /// Интерфейс условных команд с необязательным условием
    /// </summary>
    public interface IConditionalCommandDeviceBehavior : ICommandDeviceBehavior, ISimpleCommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, Func<bool>? predicate, Action ifTrue, Action ifFalse);
        void ExecuteCommand<T>(T command, Func<object, bool>? predicate, Action ifTrue, Action ifFalse);
    }
    /// <summary>
    /// Интерфейс команд с обрабатываемым внешним паттерном поведением
    /// </summary>
    public interface IRedirectedCommandDeviceBehavior : ICommandDeviceBehavior
    {
        void ExecuteCommand<T>(T command, IOptionalCommandBehavior redirectedBehavior);
    }
    public interface IObjectiveCommandDeviceBehavior
    {
        void ExecuteCommand(CommandElement command);
    }
#endregion



}
