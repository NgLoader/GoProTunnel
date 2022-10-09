using GPT.Network;
using GPT.Network.Packet;
using GPT.Server;
using System.Net;

namespace GPT.Client
{
    internal class StreamClient
    {
        private readonly NetworkServer networkServer = new NetworkServer();
        private NetworkServerInstance instance;

        public async Task Start(IPEndPoint address)
        {
            this.instance = await networkServer.StartClient(address, typeof(ClientPacketHandler));
        }

        public void follow(int camera, int port)
        {
            if (this.instance != null)
            {
                instance.ClientChannel?.WriteAndFlushAsync(new CameraStreamFollowPacket()
                {
                    CameraId = camera
                });
            }
        }

        public void unfollow(int camera)
        {

        }

        public async Task Stop()
        {
            await networkServer.Stop();
        }
    }
}
