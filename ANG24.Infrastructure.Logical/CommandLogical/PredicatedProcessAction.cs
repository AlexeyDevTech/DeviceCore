using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Infrastructure.Logical.CommandLogical
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
