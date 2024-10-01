using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLab.PubSubTypes
{
    public class SelectControllerSendCommandPubSubObject
    {
        public string Controller { get; set; }
        public string PortName { get; set; }
        public string Command { get; set; }
    }
}
