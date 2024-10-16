using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Interfaces
{
    public interface ICompensationDevice
    {
        public int Voltage { get; set; }
        public int Current { get; set; }
        public int Combination { get; set; }
        public bool IsMatched { get; set; }
        public void GetVoltage();
        public void GetCurrent();
        public void SetCoilCombination(int combination);
        public void GetCoilState();
        public void StartCoilSelect();
    }
}
