using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    public class IPacketHandler : IDisposable
    {
        private readonly NetworkHandler networkHandler;

        public IPacketHandler(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public NetworkHandler GetNetworkHandler()
        {
            return this.networkHandler;
        }

        public void Dispose()
        {
        }
    }
}
