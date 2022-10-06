using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    internal class NetworkHandler : SimpleChannelInboundHandler<IPacket<IPacketHandler>>
    {
        private readonly IPacketHandler packetHandler;

        private readonly ISocketChannel channel;

        public long LastRecivedPacket
        { get; private set; }

        public NetworkHandler(IPacketHandler packetHandler, ISocketChannel channel)
        {
            this.packetHandler = packetHandler;
            this.channel = channel;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket<IPacketHandler> msg)
        {
            LastRecivedPacket = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            msg.Handle(packetHandler);
        }

        public async void SendPacket(IPacket<IPacketHandler> packet)
        {
            if (channel.Active)
            {
                await channel.WriteAndFlushAsync(packet);
            }
        }
    }
}
