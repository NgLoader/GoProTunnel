using GPT.Network;
using GPT.Network.Packet;

namespace GPT.Server
{
    internal class ClientPacketHandler : DefaultPacketHandler
    {
        private static readonly Dictionary<int, GoProStream> cameras = new Dictionary<int, GoProStream>();

        public ClientPacketHandler(NetworkHandler networkHandler) : base(networkHandler)
        {
        }

        public override void HandleCameraStreamFollowPacket(CameraStreamFollowPacket packet)
        {
            Console.WriteLine("Follow");
        }

        public override void HandleCameraStreamRecivePacket(CameraStreamRecivePacket packet)
        {
            int cameraId = (int)packet.CameraId!;
            if (cameras.TryGetValue(cameraId, out GoProStream? stream))
            {
                stream.sendStream(packet.Data!);
            }
        }

        public override void HandleCameraStreamUnfollowPacket(CameraStreamUnfollowPacket packet)
        {
            Console.WriteLine("Unfollow");
        }

        public override void HandleDisconnectPacket(DisconnectPacket packet)
        {
            Dispose();
        }
    }
}
