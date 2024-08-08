using System;

namespace ANG24.Core.Devices.Types
{

    public class ControllerData
    {
        public static BurnKoef BurnKoefs { get; set; }
        private int oldBatteryVoltage;
        public static bool SVI_Power_On = false;
        public static LabModules Module { get; set; }
        public static int Step_Faze { get; set; }
        public static WorkMode workMode { get; set; }
        public static int Step { get; set; }
        public static double Voltage { get; set; }
        public static double Current { get; set; }
        public static double BurnCurrent => GetBurnCurrent(Current);
        public static double BurnCurrentStep => GetBurnCurrent(Current, Step);
        public static double BurnVoltage => GetBurnVoltage(Voltage, Step, Current);
        public static GVIFireMode FireMode { get; set; }
        public static int BatteryVoltage { get; set; }
        public static GVIErrorInfo GVIErrorMessage { get; set; }
        public static double TempTransformator { get; set; }
        public static double PowerOutValue { get; set; }
        public static int CurrentProtect { get; set; }
        public static BurnPower PowerBurn { get; set; }
        public static int FireDelay { get; set; }
        public static int CountFire { get; set; }
        public static PowerInfo PowerInfoMessage { get; set; }
        public static double StableVoltage { get; set; }
        public static double StableCurrent { get; set; }
        public static int VoltageType => GetVoltageType(Voltage, StableVoltage);
        public static bool ConnectionEnable { get; set; }
        public static string ErrorCode1 { get; set; }
        public static string ErrorCode2 { get; set; }
        public static string ErrorCode3 { get; set; }
        public static string ErrorCode4 { get; set; }
        public static MKZStateInfo MKZState { get; set; }
        public static double OutRNVoltage { get; set; }
        public static double OutRNCurrent { get; set; }
        public string Message { get; set; } = string.Empty;
        public static HVSwitchState HVSwitch1 { get; set; }
        public static HVSwitchState HVSwitch2 { get; set; }
        public static HVSwitchState HVSwitch3 { get; set; }
        public static bool Simulation { get; set; }
        public static string line { get; set; }

        public ControllerData(string receivedData)
        {
            receivedData = receivedData.Trim('\r', '\n').Trim();
            //Console.WriteLine(receivedData);
            var vals = receivedData.Split(',');
            if (vals[0].IndexOf("\0") != -1)
            {
                vals[0] = vals[0].Replace("\0", "");
            }
            int lenVals = vals.Length;
            switch (lenVals)
            {
                case 4:
                    if (receivedData.StartsWith("HV_SWITCH"))
                    {
                        HVSwitch1 = GetHVSwitch(vals[1]);
                        HVSwitch2 = GetHVSwitch(vals[2]);
                        HVSwitch3 = GetHVSwitch(vals[3]);
                    }
                    else Message = receivedData;  // Данные для калибровки СВИ
                    break;
                case 19:
                    Module = GetCurrentModule(vals[0]);
                    Step_Faze = GetPhase(vals[1]);
                    workMode = GetWorkMode(vals[2]);
                    Step = GetStep(vals[3]);
                    Voltage = Convert.ToDouble(vals[4]);
                    StableVoltage = Convert.ToDouble(vals[10]);
                    Current = Convert.ToDouble(vals[5]);
                    StableCurrent = Convert.ToDouble(vals[11]);
                    FireMode = GetFireMode(vals[5]);
                    BatteryVoltage = GetBatteryVoltage(Convert.ToInt32(vals[6]));
                    GVIErrorMessage = new GVIErrorInfo(vals[6]);
                    TempTransformator = Convert.ToDouble(vals[6]);
                    PowerOutValue = Convert.ToDouble(vals[7]);
                    FireDelay = Convert.ToInt32(vals[7]);
                    PowerBurn = GetPowerBurn(vals[7]);
                    CurrentProtect = Convert.ToInt32(vals[7]);
                    CountFire = Convert.ToInt32(vals[8]);
                    PowerInfoMessage = new PowerInfo(vals[9]);

                    ConnectionEnable = GetConnectionInfo(vals[12]);
                    ErrorCode1 = vals[12];
                    ErrorCode2 = vals[13];
                    ErrorCode3 = vals[14];
                    ErrorCode4 = vals[15];
                    MKZState = new MKZStateInfo(vals[16]);
                    OutRNVoltage = Convert.ToDouble(vals[17]);
                    OutRNCurrent = Convert.ToDouble(vals[18]);
                    break;
                default:
                    if (receivedData.StartsWith("VOL")) Message = receivedData; // Данные для калибровки СВИ
                    else Message = vals[0];
                    break;
            }
            //ControllerLogger.WriteString(vals);
            //ControllerLogger.WriteString(receivedData);
            line = receivedData;
        }
        public ControllerData()
        {

        }

        private HVSwitchState GetHVSwitch(string value)
        {
            var result = HVSwitchState.Empty;
            try
            {
                value = value.Substring(4);
                if (!string.IsNullOrEmpty(value)) value = value.Substring(0, 1);
                switch (value)
                {
                    case "1":
                        result = HVSwitchState.HVM;
                        break;
                    case "2":
                        result = HVSwitchState.Burn;
                        break;
                    case "3":
                        result = HVSwitchState.GVI;
                        break;
                    case "4":
                        result = HVSwitchState.Meg;
                        break;
                    case "5":
                        result = HVSwitchState.Ground;
                        break;
                    case "0":
                        result = HVSwitchState.NoMode;
                        break;
                }
            }
            catch
            {

            }
            return result;
        }

        private LabModules GetCurrentModule(string value)
        {
            switch (value)
            {
                case "MAIN":
                    return LabModules.Main;
                case "HVMAC":
                    return LabModules.HVMAC;
                case "HVMDC":
                    return LabModules.HVMDC;
                case "BURN":
                    return LabModules.Burn;
                case "JOINTBURN":
                    return LabModules.JoinBurn;
                case "HVBURN":
                    return LabModules.HVBurn;
                case "HVPULSE":
                    return LabModules.GVI;
                case "MEAS":
                    return LabModules.Meas;
                case "GP500":
                    return LabModules.GP500;
                case "LVM":
                    return LabModules.LVMeas;
                case "Tangent2000":
                    return LabModules.Tangent2000;
                case "VLF":
                    return LabModules.VLF;
                case "SA640":
                    return LabModules.SA640;
                case "SA540_1":
                    return LabModules.SA540_1;
                case "SA540_3":
                    return LabModules.SA540_3;
                case "BRIDGE":
                    return LabModules.Bridge;
                default:
                    return LabModules.NoMode;
            }
        }

        private GVIFireMode GetFireMode(string value)
        {
            switch (value)
            {
                case "1":
                    return GVIFireMode.Single;
                case "2":
                    return GVIFireMode.Multiple;
                case "3":
                    return GVIFireMode.InfinityMultiple;
                default:
                    return GVIFireMode.NoFireMode;

            }
        }

        private BurnPower GetPowerBurn(string value)
        {
            switch (value)
            {
                case "50":
                    return BurnPower.Power50;
                case "100":
                    return BurnPower.Power100;
                default:
                    return BurnPower.NoPower;

            }
        }

        private WorkMode GetWorkMode(string value)
        {
            switch (value)
            {
                case "HAND":
                    return WorkMode.Manual;
                case "AUTO":
                    return WorkMode.Auto;
                default:
                    return WorkMode.NoMode;

            }
        }

        private int GetBatteryVoltage(int value)
        {
            int voltageBattery = value;
            int range = 0;
            if (oldBatteryVoltage > voltageBattery)
            {
                // понижение напряжения
                if (value < 600)
                {
                    range = 0;
                }
                else if (value >= 600 && value < 900)
                {
                    range = 1;
                }
                else if (value >= 850 && value < 1150)
                {
                    range = 2;
                }
                else if (value >= 1100 && value < 1350)
                {
                    range = 3;
                }
                else if (value >= 1300)
                {
                    range = 4;
                }

                oldBatteryVoltage = voltageBattery;
            }
            else
            {
                // повышение напряжения
                if (value < 700)
                {
                    range = 0;
                }
                else if (value >= 650 && value < 950)
                {
                    range = 1;
                }
                else if (value >= 900 && value < 1200)
                {
                    range = 2;
                }
                else if (value >= 1150 && value < 1450)
                {
                    range = 3;
                }
                else if (value >= 1450)
                {
                    range = 4;
                }
            }


            return range;
        }

        private bool GetConnectionInfo(string value)
        {
            bool res = false;
            try
            {
                var i = int.Parse(value);
                if (i == 0) res = true;
                else res = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        private int GetPhase(string value)
        {
            switch (value)
            {
                case "PHASE1":
                    return 1;
                case "PHASE2":
                    return 2;
                case "PHASE3":
                    return 3;
                default:
                    return 0;
            }
        }

        private int GetStep(string value)
        {
            switch (value)
            {
                case "ST1":
                    return 1;
                case "ST2":
                    return 2;
                case "ST3":
                    return 3;
                case "ST4":
                    return 4;
                case "ST5":
                    return 5;
                case "ST6":
                    return 6;
                case "ST7":
                    return 7;
                default:
                    return 0;

            }
        }

        private static double GetBurnCurrent(double cur)
        {

            var percent = 100 * (cur / 290.0);
            return percent;

        }

        private static double GetBurnCurrent(double cur, int step)
        {
            var coef = BurnKoefs;
            switch (step)
            {
                case 1:
                    return cur / coef.BurnCurrentCoef1 * 34;
                case 2:
                    return cur / coef.BurnCurrentCoef2 * 65;
                case 3:
                    return cur / coef.BurnCurrentCoef3 * 130;
                case 4:
                    return cur / coef.BurnCurrentCoef4 * 260;
                case 5:
                    return cur / coef.BurnCurrentCoef5 * 700;
                case 6:
                    return cur / coef.BurnCurrentCoef6 * 2400;
                case 7:
                    return cur / coef.BurnCurrentCoef7 * 9100;
                default:
                    return 0.0;
            }
        }

        private static double GetBurnVoltage(double Uval, int step, double CurrentBurn)
        {

            double res = 0;
            switch (step)
            {
                case 1:
                    res = (Uval - CurrentBurn * 0.431) * 68.18;
                    if (res < 0) res = 0.0;
                    return res;
                case 2:
                    res = (Uval - CurrentBurn * 0.431) * 36.36;
                    if (res < 0) res = 0.0;
                    return res;
                case 3:
                    res = (Uval - CurrentBurn * 0.431) * 18.18;
                    if (res < 0) res = 0.0;
                    return res;
                case 4:
                    res = (Uval - CurrentBurn * 0.431) * 9.09;
                    if (res < 0) res = 0.0;
                    return res;
                case 5:
                    res = (Uval - CurrentBurn * 0.788) * 3.4;
                    if (res < 0) res = 0.0;
                    return res;
                case 6:
                    res = Uval - CurrentBurn * 0.788;
                    if (res < 0) res = 0.0;
                    return res;
                case 7:
                    res = (Uval - CurrentBurn * 0.788) * 0.263;
                    if (res < 0) res = 0.0;
                    return res;
                default:
                    return 0.0;
            }

        }

        private static int GetVoltageType(double VoltageAC, double VoltageDC)
        {
            // 1 - Переменка; 2 - Постоянка; 3 - Колебания
            var minKV = 0.4; //заглушка
            var maxKV = 0.7; //заглушка
            int result = 0;
            var cal = Math.Abs((VoltageAC - VoltageDC) / (VoltageAC + VoltageDC));
            if (cal > maxKV) result = 1;
            if (cal < minKV) result = 2;
            if (cal >= minKV && cal <= maxKV) result = 3;
            return result;
        }
    }

    public enum HVSwitchState
    {
        Empty,
        NoMode,
        HVM,
        Burn,
        GVI,
        Meg,
        Ground,
        GP500,
        Bridge,
    }
    public enum LabModules
    {
        Main,
        HVMAC,
        HVMDC,
        HVMDCHi,
        HVBurn,
        Burn,
        JoinBurn,
        Bridge,
        GVI,
        GP500,
        LVMeas,
        Meas,
        Reflect,
        NoMode,
        SA640,
        SA540,
        SA540_1,
        SA540_3,
        VLF,
        Tangent2000,
        Activation,
        Parma

    }
    public enum FazeType : int
    {
        NoFaze = 0,
        OneFaze = 1,
        ThreeFaze = 3
    }
    public class BurnKoef
    {
        public double BurnCurrentCoef1 { get; set; } = 290;
        public double BurnCurrentCoef2 { get; set; } = 290;
        public double BurnCurrentCoef3 { get; set; } = 290;
        public double BurnCurrentCoef4 { get; set; } = 290;
        public double BurnCurrentCoef5 { get; set; } = 290;
        public double BurnCurrentCoef6 { get; set; } = 290;
        public double BurnCurrentCoef7 { get; set; } = 290;
    }
}
