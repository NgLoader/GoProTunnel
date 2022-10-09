using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    public class NetworkServerInstance
    {
        public NetworkServer? NetworkServer
        { get; init; }
        public ServerBootstrap? ServerBootstrap
        { get; set; }
        public Bootstrap? ClientBootstrap
        { get; set; }
        public MultithreadEventLoopGroup? MasterGroup
        { get; set; }
        public MultithreadEventLoopGroup? SlaveGroup
        { get; set; }
        public IChannel? ClientChannel
        { get; set; }

        public bool Alive
        { get; private set; } = true;

        public async Task Stop()
        {
            if (Alive)
            {
                Alive = false;

                if (NetworkServer != null)
                {
                    NetworkServer.networkServers.Remove(this);
                }
                if (MasterGroup != null)
                {
                    await MasterGroup.ShutdownGracefullyAsync();
                }
                if (SlaveGroup != null)
                {
                    await SlaveGroup.ShutdownGracefullyAsync();
                }
            }
        }
    }
}
