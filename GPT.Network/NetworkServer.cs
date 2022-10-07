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
using DotNetty.Handlers.Tls;
using System.Collections.Concurrent;

namespace GPT.Network
{
    public class NetworkServer : IDisposable
    {
        internal readonly ConcurrentBag<NetworkHandler> networkHandlers = new();

        private readonly EndPoint address;

        private ServerBootstrap? bootstrap;
        private MultithreadEventLoopGroup? masterGroup;
        private MultithreadEventLoopGroup? slaveGroup;

        private volatile bool running = false;

        public NetworkServer(EndPoint address)
        {
            this.address = address;

            AppDomain.CurrentDomain.ProcessExit += (s, e) => this.Dispose();
        }

        public async Task<bool> Start<PacketHandler>(ActionChannelInitializer<ISocketChannel> initializer, PacketHandler packetHandler) where PacketHandler : Type
        {
            if (running)
            {
                return false;
            }
            running = true;

            masterGroup = new MultithreadEventLoopGroup();
            slaveGroup = new MultithreadEventLoopGroup();
            bootstrap = new ServerBootstrap();
            bootstrap
                .Group(masterGroup, slaveGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>((channel) =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    NetworkHandler networkHandler = new(this, packetHandler);
                    networkHandlers.Add(networkHandler);

                    pipeline.AddLast(new PacketEncoder(), new PacketDecoder(), networkHandler);
                }));

            await bootstrap.BindAsync(this.address);
            return true;
        }

        public async Task<bool> Stop()
        {
            if (!running)
            {
                return false;
            }

            if (masterGroup != null)
            {
                await masterGroup.ShutdownGracefullyAsync();
            }
            if (slaveGroup != null)
            {
                await slaveGroup.ShutdownGracefullyAsync();
            }

            bootstrap = null;
            masterGroup = null;
            slaveGroup = null;

            running = false;
            return true;
        }

        public async void Dispose()
        {
            await Stop();
        }
    }
}
