using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    public interface IPacket<PacketHandler> where PacketHandler : IPacketHandler
    {
        void Read(IByteBuffer buffer);

        void Write(IByteBuffer buffer);

        void Handle(PacketHandler handler);
    }
}
