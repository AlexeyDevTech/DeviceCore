using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Interfaces.Base
{
    public interface IDevice
    {
        event Action OnConnect;
        event Action OnDisconnect;
        void Connect();
        void Disconnect();
        void Write<T>(T msg);
        void Write(Type type, object msg);
    }
    public interface IManagedDevice : IDevice
    {
        void Start();
        void Stop();
    }
}
