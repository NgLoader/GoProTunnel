using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    internal interface IPacket<T> where T : IPacketHandler
    {
        void Read(IByteBuffer buffer);

        void Write(IByteBuffer buffer);

        void Handle(T handler);
    }
}
