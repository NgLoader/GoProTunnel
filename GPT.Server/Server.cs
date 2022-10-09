using GPT.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Server
{
    public class Server {

        private readonly NetworkServer networkServer = new NetworkServer();

        public async Task Start(EndPoint address)
        {
            await networkServer.StartServer(address, typeof(ServerPacketHandler));
        }

        public async Task Stop()
        {
            await networkServer.Stop();
        }
    }
}
