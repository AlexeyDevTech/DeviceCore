namespace ANG24.Core.Devices.Interfaces
{
    public delegate void DREventHandler(string data);
    public interface IDevice
    {
        event EventHandler Connected;
        event EventHandler Disconnected;
        event DREventHandler DeviceDataReceived;
        bool Online { get; }
        string DeviceStatus { get; }
        void Connect();
        void Disconnect();
        void SetCommand(string command);
        void Ping();
    }
    


}
