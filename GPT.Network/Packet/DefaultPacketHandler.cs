using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT.Network.Packet
{
    public abstract class DefaultPacketHandler : IPacketHandler
    {
        protected DefaultPacketHandler(NetworkHandler networkHandler) : base(networkHandler)
        {
        }

        public abstract void HandleCameraStreamFollowPacket(CameraStreamFollowPacket packet);

        public abstract void HandleCameraStreamUnfollowPacket(CameraStreamUnfollowPacket packet);

        public abstract void HandleCameraStreamRecivePacket(CameraStreamRecivePacket packet);

        public abstract void HandleDisconnectPacket(DisconnectPacket packet);
    }
}
