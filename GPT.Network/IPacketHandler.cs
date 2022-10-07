using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    public class IPacketHandler : IDisposable
    {
        internal readonly NetworkHandler networkHandler;

        public IPacketHandler(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public void Dispose()
        {
        }
    }
}
