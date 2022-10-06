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
    internal class PacketEncoder : MessageToByteEncoder<IPacket<IPacketHandler>>
    {
        protected override void Encode(IChannelHandlerContext context, IPacket<IPacketHandler> packet, IByteBuffer output)
        {
            int packetId = PacketRegistry.GetId(packet);
            ByteBufferUtil.WriteVarInt(output, packetId);
            packet.Write(output);
        }
    }
}
