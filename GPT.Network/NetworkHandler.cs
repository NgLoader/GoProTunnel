using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using GPT.Network.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    public class NetworkHandler : SimpleChannelInboundHandler<IPacket<IPacketHandler>>, IDisposable
    {
        internal readonly NetworkServer networkServer;
        private readonly IPacketHandler packetHandler;

        private IChannel? channel;
        private bool IsAlive => channel != null && channel.IsWritable;

        public long LastRecivedPacket
        { get; private set; }

        public NetworkHandler(NetworkServer networkServer, IChannel channel, Type packetHandler)
        {
            this.networkServer = networkServer;
            this.packetHandler = (IPacketHandler) Activator.CreateInstance(packetHandler, this)!;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            channel = context.Channel;
            Console.WriteLine($"{channel!.Id} connected");

            SendPacket(new CameraStreamUnfollowPacket()
            {
                CameraId = 9999
            });
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            Console.WriteLine($"{channel!.Id} lost connection");

            networkServer.networkHandlers.Remove(this);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            base.ChannelReadComplete(context);
            context.Flush();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
            Console.WriteLine(exception);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket<IPacketHandler> msg)
        {
            LastRecivedPacket = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            msg.Handle(packetHandler);
        }

        public async void SendPacket<PacketHandler>(IPacket<PacketHandler> packet) where PacketHandler : IPacketHandler
        {
            if (IsAlive)
            {
                await channel!.WriteAndFlushAsync(packet);
            }
        }

        public string GetName()
        {
            return channel!.Id.AsShortText() ?? "Unknown";
        }

        public async void Dispose()
        {
            if (IsAlive)
            {
                SendPacket(new DisconnectPacket() { Reason = "Disconnected" });
                await channel!.CloseAsync();
            }

            packetHandler.Dispose();
        }
    }
}
