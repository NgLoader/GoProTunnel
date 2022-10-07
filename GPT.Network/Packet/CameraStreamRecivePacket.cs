using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network.Packet
{
    public class CameraStreamRecivePacket : IPacket<DefaultPacketHandler>
    {

        public int? CameraId
        { get; set; }
        public byte[]? Data
        { get; set; }

        public void Read(IByteBuffer buffer)
        {
            CameraId = buffer.ReadInt();
            Data = ByteBufferUtil.ReadByteArray(buffer);
        }

        public void Write(IByteBuffer buffer)
        {
            if (CameraId != null)
            {
                buffer.WriteInt((int)CameraId);
            }
            if (Data != null)
            {
                ByteBufferUtil.WriteByteArray(buffer, Data);
            }
        }
        public void Handle(DefaultPacketHandler handler)
        {
            handler.HandleCameraStreamRecivePacket(this);
        }
    }
}
