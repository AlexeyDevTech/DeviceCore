using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Interfaces
{
    public interface IDeviceBase
    {
        event Action OnConnect;
        event Action OnDisconnect;

        bool Online { get; }

        void SetDataSource(IDataSource source);
        void Connect();
        void Disconnect();
        void Write<T>(T msg);
        void Write(Type type, object msg);
    }
}
