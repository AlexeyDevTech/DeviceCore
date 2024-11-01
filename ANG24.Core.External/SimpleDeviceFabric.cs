using ANG24.Core.External.Types;
using ANG24.Infrastructure.Middleware.Base;

namespace ANG24.Core.External
{
    public static class SimpleDeviceFabric
    {
        public static SimpleDeviceBase Create(ControllerNames ControllerName)
        {
            switch (ControllerName)
            {
                case ControllerNames.Main:
                    return new MEADevice();
                case ControllerNames.Compensation:
                    return new CompensationDevice();
                case ControllerNames.MNK:
                    return new MNKDevice();
                default:
                    throw new ArgumentException(nameof(ControllerName));
            }
        }
        public static SimpleDeviceBase Create(string ControllerName)
        {
            switch (ControllerName)
            {
                case "MainController":
                    return new MEADevice();
                case "Compensation":
                    return new CompensationDevice();
                case "MNK":
                    return new MNKDevice();
                case "AOT":
                    return new AOTDevice();
                default:
                    throw new ArgumentException(nameof(ControllerName));
            }
        }
    }
}
