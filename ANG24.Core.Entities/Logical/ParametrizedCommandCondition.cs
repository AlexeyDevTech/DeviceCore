namespace ANG24.Core.Entities.Logical
{
    #region Command element realization (and conditions)
    public class ParametrizedCommandCondition : CommandCondition
    {
        private readonly Func<object, bool>? condition;

        public ParametrizedCommandCondition(Func<object, bool>? condition, Action? ifTrue, Action? ifFalse)
            : base(null, ifTrue, ifFalse)
        {
            this.condition = condition;
        }

        public override bool Execute(object data)
        {
            bool result = condition?.Invoke(data) ?? true;
            if (result) ifTrue?.Invoke();
            else ifFalse?.Invoke();
            return result;
        }
    }
    #endregion
}
