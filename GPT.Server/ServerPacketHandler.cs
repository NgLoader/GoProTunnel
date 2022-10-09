using GPT.Network;
using GPT.Network.Packet;

namespace GPT.Server
{
    internal class ServerPacketHandler : DefaultPacketHandler
    {
        private static readonly Dictionary<int, GoProStream> cameras = new Dictionary<int, GoProStream>();

        public ServerPacketHandler(NetworkHandler networkHandler) : base(networkHandler)
        {
        }

        public override void HandleCameraStreamFollowPacket(CameraStreamFollowPacket packet)
        {
            int cameraId = (int)packet.CameraId!;
            Console.WriteLine($"Client {this.GetNetworkHandler().GetName()} Following: {cameraId}");

            if (cameras.TryGetValue(cameraId, out GoProStream? stream))
            {
                stream.Add(GetNetworkHandler());
            }
            else
            {
                GoProStream streamInstance = new GoProStream();
                streamInstance.Add(GetNetworkHandler());
                cameras.Add(cameraId, streamInstance);
            }
        }

        public override void HandleCameraStreamRecivePacket(CameraStreamRecivePacket packet)
        {
            int cameraId = (int)packet.CameraId!;
            if (cameras.TryGetValue(cameraId, out GoProStream? stream))
            {
                stream.Broadcast(packet);
            } else
            {
                cameras.Add(cameraId, new GoProStream());
            }
        }

        public override void HandleCameraStreamUnfollowPacket(CameraStreamUnfollowPacket packet)
        {
            int cameraId = (int)packet.CameraId!;
            Console.WriteLine($"Client {this.GetNetworkHandler().GetName()} Unfollowing: {cameraId}");

            if (cameras.TryGetValue(cameraId, out GoProStream? stream))
            {
                stream.Remove(GetNetworkHandler());
            }
        }

        public override void HandleDisconnectPacket(DisconnectPacket packet)
        {
            Dispose();
        }
    }
}
