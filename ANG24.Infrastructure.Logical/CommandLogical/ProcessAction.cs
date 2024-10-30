using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Logical.CommandLogical
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
