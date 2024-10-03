using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLab.PubSubTypes
{
    public class SettingsEvent
    {
        public bool Apply { get; set; }
        public int BaudRate { get; set; }
    }
}
