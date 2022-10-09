using GPT.Network;

namespace GPT.Server
{
    internal class GoProStream
    {
        private HashSet<NetworkHandler> listening = new();

        public void Broadcast<PacketHandler>(IPacket<PacketHandler> packet)
            where PacketHandler : IPacketHandler
        {
            foreach (NetworkHandler networkHandler in listening)
            {
                networkHandler.SendPacket(packet);
            }
        }

        public void Add(NetworkHandler handler)
        {
            listening.Add(handler);
        }
        public void Remove(NetworkHandler handler)
        {
            listening.Remove(handler);
        }
    }
}
