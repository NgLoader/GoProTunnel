using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network.Packet
{
    public class DisconnectPacket : IPacket<DefaultPacketHandler>
    {
        public string? Reason
        { get; set; }

        public void Read(IByteBuffer buffer)
        {
            Reason = ByteBufferUtil.ReadString(buffer);
        }

        public void Write(IByteBuffer buffer)
        {
            if (Reason != null)
            {
                ByteBufferUtil.WriteString(buffer, Reason);
            }
        }

        public void Handle(DefaultPacketHandler handler)
        {
            handler.HandleDisconnectPacket(this);
        }
    }
}
