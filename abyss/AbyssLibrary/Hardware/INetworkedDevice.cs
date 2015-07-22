using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public interface INetworkedDevice
    {
        string MacAddress { get; }
        string IpAddress { get; }
        bool IsConnected { get; }

        void RefreshIpAddress(bool scanNetworkIfNotPresent);
    }
}
