namespace ANG24.Core.Devices.Types
{
    public class ModuleWorkAnalyser
    {
        public ModulePowerState WorkState { get; set; }
        private List<bool> TargetFlags { get; set; }
        private bool MainTarget { get; set; }
        public ModuleWorkAnalyser(LabModules CurrentModule, PowerInfo powerInfo)
        {
            switch (CurrentModule)
            {
                case LabModules.Bridge:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.BridgeEnable
                        //powerInfo.HVMEnable
                    };
                    MainTarget = powerInfo.BridgeEnable;
                    break;
                case LabModules.Burn:
                    TargetFlags = new List<bool>
                   {
                       powerInfo.BurnEnable,
                       powerInfo.MVKUp,
                       powerInfo.VREnable
                   };
                    MainTarget = powerInfo.BurnEnable;
                    break;
                case LabModules.GP500:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.MVKUp,
                        powerInfo.GP500Enable,
                        powerInfo.KM3_MKZEnable
                    };
                    MainTarget = powerInfo.GP500Enable;
                    break;
                case LabModules.GVI:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.MVKUp,
                        powerInfo.GVIEnable,
                        powerInfo.VREnable
                    };
                    MainTarget = powerInfo.GVIEnable;
                    break;
                case LabModules.HVBurn:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.MVKUp,
                        powerInfo.SVIPowerEnable,
                        powerInfo.ProtectedDrosselEnable,
                        powerInfo.VREnable,
                        powerInfo.HVMEnable
                    };
                    MainTarget = powerInfo.HVMEnable;
                    break;
                case LabModules.HVMAC:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.HVMEnable,
                        powerInfo.SVIPowerEnable,
                        powerInfo.VREnable,
                        powerInfo.KM1_MKZEnable
                    };
                    MainTarget = powerInfo.HVMEnable;
                    break;
                case LabModules.HVMDC:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.HVMEnable,
                        powerInfo.MVKUp,
                        powerInfo.SVIPowerEnable,
                        powerInfo.VREnable,
                        powerInfo.KM1_MKZEnable
                    };
                    MainTarget = powerInfo.HVMEnable;
                    break;
                case LabModules.JoinBurn:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.MVKUp,
                        powerInfo.SVIPowerEnable,
                        powerInfo.MSKEnable,
                        powerInfo.ProtectedDrosselEnable,
                        powerInfo.VREnable,
                        powerInfo.HVMEnable,
                        powerInfo.JoinBurnEnable
                    };
                    MainTarget = powerInfo.JoinBurnEnable;
                    break;
                case LabModules.LVMeas:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.LVMEnable,
                        powerInfo.VREnable
                    };
                    MainTarget = powerInfo.LVMEnable;
                    break;
                case LabModules.Meas:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.MVKUp,
                        powerInfo.MeasEnable
                    };
                    MainTarget = powerInfo.MeasEnable;
                    break;
                case LabModules.SA540:
                case LabModules.SA540_1:
                case LabModules.SA540_3:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.SA540Enable
                    };
                    MainTarget = powerInfo.SA540Enable;
                    break;
                case LabModules.SA640:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.SA640Enable
                    };
                    MainTarget = powerInfo.SA640Enable;
                    break;
                case LabModules.VLF:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.VLFEnable
                    };
                    MainTarget = powerInfo.VLFEnable;
                    break;
                case LabModules.Tangent2000:
                    TargetFlags = new List<bool>
                    {
                        powerInfo.Tangent2000Enable
                    };
                    MainTarget = powerInfo.Tangent2000Enable;
                    break;
                default:
                    TargetFlags = new List<bool>();
                    MainTarget = false;
                    break;
            }
            if (!TargetFlags.Contains(false)) WorkState = ModulePowerState.Enable;
            else if (MainTarget == true) WorkState = ModulePowerState.EnableFail;
            else WorkState = ModulePowerState.Disable;
        }

    }
}
