using ANG24.Core.Devices.Base.Interfaces.Behaviors;

namespace ANG24.Core.Devices.Base
{
    #region implements CommandDeviceBehavior

    public class OptionalBehaviorManager
    {
        List<ProcessAction> ProcessActions { get; set; }
        public List<IOptionalBehavior> _optionalBehaviors { get; set; }
        public OptionalBehaviorManager()
        {
            ProcessActions = new List<ProcessAction>();
            _optionalBehaviors = new List<IOptionalBehavior>();
        }

        public void HandleData(object data)
        {
            if (_optionalBehaviors.Count > 0)
            {
                foreach (var behavior in _optionalBehaviors)
                {
                    behavior.HandleData(data);
                }
            }
            //экспериментальная функция, выполняет дополнительные действия, добавляемые и включаемые по требованию
            if (ProcessActions.Count > 0)
                foreach (ProcessAction action in ProcessActions)
                {
                    action.Execute(data);
                    if (action.ExecutedOnce)
                        ProcessActions.Remove(action);
                }
        }

        #region option management
        public void AddOption(string Name, Action<object> action, bool Active = true) => ProcessActions.Add(new ProcessAction
        {
            Name = Name,
            ProcessedAction = action,
            ExecutedOnce = false,
            Usage = Active
        });
        public void AddPredicatedOption(string Name, Func<object, bool> predicate, Action<object> action, bool Active = true) => ProcessActions.Add(new PredicatedProcessAction
        {
            Name = Name,
            Predicate = predicate,
            ProcessedAction = action,
            ExecutedOnce = false,
            Usage = Active
        });
        public void Clear() => ProcessActions.Clear();
        public void RemoveOption(string Name) => ProcessActions.Remove(ProcessActions.First(x => x.Name == Name));
        public void DisableOption(string Name)
        {
            var r = ProcessActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = false;
        }
        public void EnableOption(string Name)
        {
            var r = ProcessActions.FirstOrDefault(x => x.Name == Name);
            if (r != null) r.Usage = true;
        }
        #endregion
    }
    #endregion
}
