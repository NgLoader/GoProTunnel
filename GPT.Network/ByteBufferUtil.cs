using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Code coped from Ingrim4
 */
namespace GPT.Network
{
    internal class ByteBufferUtil
    {
        public static int ReadVarInt(IByteBuffer buffer)
        {
            int outgoing = 0;
            int bytes = 0;
            byte current;
            while (true)
            {
                current = buffer.ReadByte();
                outgoing |= (current & 0x7F) << (bytes++ * 7);
                if (bytes > 5)
                {
                    throw new ArgumentException("Attempt to read int bigger than 5 bytes");
                }
                if ((current & 0x80) != 0x80)
                {
                    break;
                }
            }
            return outgoing;
        }

        public static void WriteVarInt(IByteBuffer buffer, int value)
        {
            byte part;
            uint uValue = (uint)value;
            int uValueAsInt = value;
            while (true)
            {
                part = (byte)(uValueAsInt & 0x7F);

                uValue >>= 7;
                uValueAsInt = (int)uValue;

                if (uValueAsInt != 0)
                    part |= 0x80;
                buffer.WriteByte(part);
                if (uValueAsInt == 0)
                    break;
            }
        }

        public static byte[] ReadByteArray(IByteBuffer buffer)
        {
            byte[] array = new byte[ReadVarInt(buffer)];
            buffer.ReadBytes(array);
            return array;
        }

        public static void WriteByteArray(IByteBuffer buffer, byte[] array)
        {
            WriteVarInt(buffer, array.Length);
            buffer.WriteBytes(array);
        }

        public static string ReadString(IByteBuffer buffer)
        {
            byte[] bytes = ReadByteArray(buffer);
            return Encoding.UTF8.GetString(bytes);
        }

        public static void WriteString(IByteBuffer buffer, String message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            WriteByteArray(buffer, bytes);
        }
    }
}
