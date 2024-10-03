namespace ANG24.Core.Devices.Base.Abstract.Types
{
    #region ProcessAction types
    public class PredicatedProcessAction : ProcessAction
    {
        public Func<object, bool> Predicate { get; set; }

        public override void Execute(object val)
        {
            if (Usage)
                if (Predicate != null && Predicate.Invoke(val))
                    base.Execute(val);
        }
    }
    #endregion
}
