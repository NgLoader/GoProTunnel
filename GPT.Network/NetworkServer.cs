using DotNetty.Common.Concurrency;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.ConstrainedExecution;

namespace GPT.Network
{
    internal class NetworkServer : IDisposable
    {
        private readonly EndPoint address;

        private ServerBootstrap bootstrap;
        private MultithreadEventLoopGroup masterGroup;
        private MultithreadEventLoopGroup slaveGroup;

        private volatile bool running = false;

        public NetworkServer(EndPoint address, IPacketHandler packetHandler)
        {
            this.address = address;

            this.masterGroup = new MultithreadEventLoopGroup();
            this.slaveGroup = new MultithreadEventLoopGroup();

            this.bootstrap = new ServerBootstrap();
            this.bootstrap
                .Group(masterGroup, slaveGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>((channel) =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new PacketEncoder(), new PacketDecoder(), new NetworkHandler(packetHandler, channel));
                }));

            AppDomain.CurrentDomain.ProcessExit += (s, e) => this.Dispose();
        }

        public async Task<bool> Start()
        {
            if (this.running)
            {
                return false;
            }

            this.running = true;
            await this.bootstrap.BindAsync(this.address);
            return true;
        }

        public async Task<bool> Stop()
        {
            if (!this.running)
            {
                return false;
            }

            await this.masterGroup.ShutdownGracefullyAsync();
            await this.slaveGroup.ShutdownGracefullyAsync();

            this.running = false;
            return true;
        }

        public async void Dispose()
        {
            await this.Stop();
        }
    }
}
