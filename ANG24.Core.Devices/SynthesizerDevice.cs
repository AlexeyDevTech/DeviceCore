using ANG24.Core.Devices.Base;
using ANG24.Core.Devices.DeviceBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices
{
    public class SynthesizerDevice : ParallelDeviceBase
    {
        public int Mode { get; set; }
        public int Voltage { get; set; }

        public SynthesizerDevice() : base(new ReqResWithTimeCallBackDeviceBehavior { CallBackMilliseconds = 1000, ReqResMilliseconds = 500},
                                          new OrderStrongCommandBehavior{ AutoResponse = false, TickMilliseconds = 50 })
        {
            
        }
        public override void Ping()
        {
            GetVoltage();
        }

        protected override void ProcessData(string data)
        {
            try
            {
                if (data.Contains("Mode"))
                {
                    var splt = data.Split('=');
                    var str_mode = splt[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
                    Mode = int.Parse(str_mode);
                }
                if (data.Contains("Voltage"))
                {
                    var splt = data.Split('=');
                    var str_mode = splt[1].Replace('\r', ' ').Replace('\n', ' ').Trim();
                    var vol_pwm = int.Parse(str_mode);
                    Voltage = vol_pwm / 3500 * 220;
                }
            } catch(FormatException) { }
        }
        public override async void Connect()
        {
            if(await Find("8#Get_mode", "Mode", 115200))
               base.Connect();
        }

        public void SetThreeFaze() => Execute("8#Set_3F_mode", x => x.Contains("On_3F_mode"), () => Mode = 3);
        public void SetOneFaze() => Execute("8#Set_1F_mode", x => x.Contains("On_1F_mode"), () => Mode = 1);
        public void UpVoltage() => Execute("8#Up_voltage"/*, x => x.Contains("Up_slow")*/);
        public void DownVoltage() => Execute("8#Down_voltage"/*, x => x.Contains("Down_slow")*/);
        public void DownVoltageFast() => Execute("8#Down_voltag_fast"/*, x => x.Contains("Down_fast")*/);
        public void UpVoltageFast() => Execute("8#Up_voltag_fast"/*, x => x.Contains("Up_fast")*/);
        public void VolReset() => Execute("8#Stop_mode", x => x.Contains("Stop"));
        public void GetMode() => Execute("8#Get_mode", x => x.Contains("Mode"));
        public void GetVoltage() => Execute("8#Get_voltage", x => x.Contains("Voltage"));
        public void SetVoltage(int voltage)
        {
            var pwm = (int)(Math.Round(voltage / 220.0 * 3500.0, 0));
            Execute($"8#Set_Vol_Level:{pwm};", x => x.Contains("Vol_Level"));
        }
    }
}
