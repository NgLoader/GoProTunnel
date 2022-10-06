using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    internal class PacketDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (input.ReadableBytes != 0)
            {
                int packetId = ByteBufferUtil.ReadVarInt(input);
                IPacket<IPacketHandler> packet = PacketRegistry.GetPacket(packetId);
                packet.Read(input);

                if (input.ReadableBytes != 0)
                {
                    throw new IOException($"Packet {packet.GetType().Name} has unreaded bytes");
                }

                output.Add(packet);
            }
        }
    }
}
