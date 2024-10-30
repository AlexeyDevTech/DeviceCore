using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Interfaces.CommandLogical
{
    public interface ICommandCondition
    {
        bool Execute(object data);
    }
}
