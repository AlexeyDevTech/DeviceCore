namespace ANG24.Core.Devices.Base.Interfaces
{
    public interface IConnectable
    {
        event Action OnConnect;
        event Action OnDisconnect;
        void Connect();
        void Disconnect();

    }
}
