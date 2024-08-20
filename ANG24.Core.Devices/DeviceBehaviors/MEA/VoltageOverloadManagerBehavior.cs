using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Interfaces;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.DeviceBehaviors.MEA
{
    internal class VoltageOverloadManagerBehavior : IOptionalBehavior
    {
        MEADevice _device;
        const int DefaultRNVoltageLimit = 220;
        const int DefaultRNCurrentLimit = 260;
        const double DefaultOutVoltage = 70000;
        const double DefaultGist = 5000;
        public string Name => nameof(VoltageOverloadManagerBehavior);
        public bool IsUsage { get; set; }
        public bool VoltageOverloaded { get; set; }
        public bool CurrentOverloaded { get; set; }
        public int RNVoltageLimit { get; set; } = 220;
        public int RNCurrentLimit { get; set; } = 260;
        public int V_Gist { get; set; } = 15;
        public double OutV_Gist { get; set; } = 5000;              //гистерезис для выходного напряжения
        public int I_Gist { get; set; } = 15;
        public double TargetVoltageLimit { get; private set; }
        public double TargetVoltage { get; set; }

        public void HandleData(string data)
        {
            if (IsUsage)
            {
                TargetVoltage = ControllerData.Voltage;
                if (!CheckRNVoltage()) return;
                CheckTargetVoltage();
            }
        }

        private bool CheckTargetVoltage()
        {
            if (!VoltageOverloaded)
            {
                if (ControllerData.Voltage > TargetVoltageLimit)
                {
                    _device.RegulatorOff();
                    VoltageOverloaded = true;
                    return false;
                }
                return true;
            }
            else
            {
                if (ControllerData.Voltage < (TargetVoltageLimit - OutV_Gist))
                {
                    _device.RegulatorOn();
                    VoltageOverloaded = false;
                    return true;
                }
                return false;
            }
        }

        private bool CheckRNVoltage()
        {
            if (!VoltageOverloaded)
            {
                if (ControllerData.OutRNVoltage > RNVoltageLimit)
                {
                    _device.RegulatorOff();
                    VoltageOverloaded = true;
                    return false;
                }
                return true;
            }
            else
            {
                if (ControllerData.OutRNVoltage < (RNVoltageLimit - V_Gist))
                {
                    _device.RegulatorOn();
                    VoltageOverloaded = false;
                    return true;
                }
                return false;
            }
        }

        public void Off()
        {
            IsUsage = false;   
        }

        public void On()
        {
            IsUsage = true;
        }

        public void SetVoltageLimit(double value) => TargetVoltageLimit = value;
        public void SetVoltageLimit(double value, double gisteresis)
        {
            TargetVoltageLimit = value;
            OutV_Gist = gisteresis;
        }
        public void SetDefaultVoltage()
        {
            TargetVoltageLimit = DefaultOutVoltage;
            OutV_Gist = DefaultGist;
        }
        public void SetDefaultRN()
        {
            RNVoltageLimit = DefaultRNVoltageLimit;
            RNCurrentLimit = DefaultRNCurrentLimit;
        }
        public void SetDefault()
        {
            SetDefaultVoltage();
            SetDefaultRN();
        }

        public void SetRNVoltageLimit(int value) => RNVoltageLimit = value;
        public void SetRNCurrentLimit(int value) => RNCurrentLimit = value;
        public void SetDevice(IDevice device)
        {
            _device = (device as MEADevice);
        }
    }
}
