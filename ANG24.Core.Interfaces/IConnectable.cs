namespace ANG24.Core.Interfaces
{
    public interface IConnectable
    {
        event Action OnConnect;
        event Action OnDisconnect;
        void Connect();
        void Disconnect();

    }
}
