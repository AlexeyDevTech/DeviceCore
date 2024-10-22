namespace ANG24.Core.Entities.DataTypes
{
    public class ModulePowerInfo
    {
        public ModulePowerState BurnModuleEnable { get; set; }
        public ModulePowerState HVMDCModuleEnable { get; set; }
        public ModulePowerState HVMACModuleEnable { get; set; }
        public ModulePowerState GVIModuleEnable { get; set; }
        public ModulePowerState GP500ModuleEnable { get; set; }
        public ModulePowerState MeasureModuleEnable { get; set; }
        public ModulePowerState JoinBurnModuleEnable { get; set; }
        public ModulePowerState HVBurnModuleEnable { get; set; }
        public ModulePowerState LVMModuleEnable { get; set; }
        public ModulePowerState BridgeModuleEnable { get; set; }
        public ModulePowerState SA540MoudleEnable { get; set; }
        public ModulePowerState SA640MoudleEnable { get; set; }
        public ModulePowerState VLFModuleEnable { get; set; }
        public ModulePowerState Tangent2000Enable { get; set; }

        public ModulePowerInfo(PowerInfo info)
        {
            BurnModuleEnable = new ModuleWorkAnalyser(LabModules.Burn, info).WorkState;
            HVMDCModuleEnable = new ModuleWorkAnalyser(LabModules.HVMDC, info).WorkState;
            HVMACModuleEnable = new ModuleWorkAnalyser(LabModules.HVMAC, info).WorkState;
            GVIModuleEnable = new ModuleWorkAnalyser(LabModules.GVI, info).WorkState;
            GP500ModuleEnable = new ModuleWorkAnalyser(LabModules.GP500, info).WorkState;
            MeasureModuleEnable = new ModuleWorkAnalyser(LabModules.Meas, info).WorkState;
            JoinBurnModuleEnable = new ModuleWorkAnalyser(LabModules.JoinBurn, info).WorkState;
            HVBurnModuleEnable = new ModuleWorkAnalyser(LabModules.HVBurn, info).WorkState;
            LVMModuleEnable = new ModuleWorkAnalyser(LabModules.LVMeas, info).WorkState;
            BridgeModuleEnable = new ModuleWorkAnalyser(LabModules.Bridge, info).WorkState;
            SA540MoudleEnable = new ModuleWorkAnalyser(LabModules.SA540, info).WorkState;
            SA540MoudleEnable = new ModuleWorkAnalyser(LabModules.SA540_1, info).WorkState;
            SA540MoudleEnable = new ModuleWorkAnalyser(LabModules.SA540_3, info).WorkState;
            SA640MoudleEnable = new ModuleWorkAnalyser(LabModules.SA640, info).WorkState;
            VLFModuleEnable = new ModuleWorkAnalyser(LabModules.VLF, info).WorkState;
            Tangent2000Enable = new ModuleWorkAnalyser(LabModules.Tangent2000, info).WorkState;
        }
    }
}
