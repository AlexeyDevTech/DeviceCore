namespace ANG24.Core.Entities.Logical
{
    #region ProcessAction types
    public class ProcessAction
    {
        public string Name { get; set; }
        public Action<object> ProcessedAction { get; set; }
        public bool ExecutedOnce { get; set; }
        public bool Usage { get; set; }

        public virtual void Execute(object val)
        {
            if (Usage)
                ProcessedAction?.Invoke(val);
        }
    }
    #endregion
}
