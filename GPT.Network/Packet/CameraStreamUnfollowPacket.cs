using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network.Packet
{
    internal class CameraStreamUnfollowPacket : IPacket<DefaultPacketHandler>
    {
        public int? CameraId
        { get; set; }

        public void Read(IByteBuffer buffer)
        {
            CameraId = buffer.ReadInt();
        }

        public void Write(IByteBuffer buffer)
        {
            if (CameraId != null)
            {
                buffer.WriteInt((int)CameraId);
            }
        }
        public void Handle(DefaultPacketHandler handler)
        {
            handler.handleCameraStreamUnfollowPacket(this);
        }
    }
}
