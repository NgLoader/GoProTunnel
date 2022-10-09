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
    public class NetworkServer
    {
        internal readonly HashSet<NetworkHandler> networkHandlers = new();
        internal readonly HashSet<NetworkServerInstance> networkServers = new();

        public NetworkServer()
        {
            AppDomain.CurrentDomain.ProcessExit += async (s, e) => await Stop();
        }

        public async Task<NetworkServerInstance> StartServer<PacketHandler>(EndPoint address, PacketHandler packetHandler) where PacketHandler : Type
        {
            MultithreadEventLoopGroup masterGroup = new();
            MultithreadEventLoopGroup slaveGroup = new();
            ServerBootstrap bootstrap = new();
            bootstrap
                .Group(masterGroup, slaveGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>((channel) =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    NetworkHandler networkHandler = new(this, channel, packetHandler);
                    networkHandlers.Add(networkHandler);

                    pipeline.AddLast("decoder", new PacketDecoder());
                    pipeline.AddLast("encoder", new PacketEncoder());
                    pipeline.AddLast("handler", networkHandler);
                }));

            await bootstrap.BindAsync(address);

            NetworkServerInstance instance = new NetworkServerInstance()
            {
                NetworkServer = this,
                ServerBootstrap = bootstrap,
                MasterGroup = masterGroup,
                SlaveGroup = slaveGroup
            };
            return instance;
        }

        public async Task<NetworkServerInstance> StartClient<PacketHandler>(EndPoint address, PacketHandler packetHandler) where PacketHandler : Type
        {
            Bootstrap bootstrap = new();
            MultithreadEventLoopGroup slaveGroup = new();
            bootstrap
                .Group(slaveGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Handler(new ActionChannelInitializer<ISocketChannel>((channel) =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    NetworkHandler networkHandler = new(this, channel, packetHandler);
                    networkHandlers.Add(networkHandler);

                    pipeline.AddLast("decoder", new PacketDecoder());
                    pipeline.AddLast("encoder", new PacketEncoder());
                    pipeline.AddLast("handler", networkHandler);
                }));

            IChannel channel = await bootstrap.ConnectAsync(address);

            NetworkServerInstance instance = new NetworkServerInstance()
            {
                NetworkServer = this,
                ClientBootstrap = bootstrap,
                SlaveGroup = slaveGroup,
                ClientChannel = channel
            };
            return instance;
        }

        public async Task Stop()
        {
            foreach (NetworkServerInstance instance in networkServers)
            {
                await instance.Stop();
            }
        }
    }
}
