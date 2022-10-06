using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network
{
    internal class PacketRegistry
    {
        private static readonly Dictionary<int, Type> IdToPacket = new Dictionary<int, Type>();
        private static readonly Dictionary<Type, int> PacketToId = new Dictionary<Type, int>();

        static PacketRegistry() {
        }

        private static void RegisterPacket(Type packet)
        {
            int id = IdToPacket.Count;
            IdToPacket.Add(id, packet);
            PacketToId.Add(packet, id);
        }

        public static IPacket<IPacketHandler> GetPacket(int packetId)
        {
            if (IdToPacket.TryGetValue(packetId, out var packet))
            {
                object? instance = Activator.CreateInstance(packet);
                if (instance != null)
                {
                    return (IPacket<IPacketHandler>)instance;
                }

                throw new IOException($"Unable to create packet instance for packet id '{packetId}'");
            }
            throw new IOException($"Unable to find packet id '{packetId}'");
        }

        public static int GetId(IPacket<IPacketHandler> packet)
        {
            if (packet == null)
            {
                throw new IOException($"Packet is null");
            }

            if (PacketToId.TryGetValue(packet.GetType(), out var packetId))
            {
                return packetId;
            }

            throw new IOException($"Unable to find packet id for '{packet.GetType()}'");
        }
    }
}
