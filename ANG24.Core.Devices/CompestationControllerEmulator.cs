using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class CompestationControllerEmulator
    {
        string[] Params = File.ReadAllLines("C:\\Users\\balmasov\\Desktop\\mask.csv");
        public IDictionary dict = new Dictionary<int, string>();

        public CompestationControllerEmulator()
        {
            foreach (var item in Params)
            {
                dict.Add(int.Parse(item.Split(';')[0].Trim()), item.Split(';')[1].Trim());
            }
        }
    }
}
