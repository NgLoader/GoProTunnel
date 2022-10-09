using GPT.Network;
using System.Net;
using System.Net.Sockets;

namespace GPT.Server
{
    internal class GoProStream
    {
        private readonly int camera;
        private UdpClient client;

        public GoProStream(int camera, int port)
        {
            this.camera = camera;
            client = new UdpClient(IPAddress.Loopback.ToString(), port);
        }

        public void sendStream(byte[] data)
        {
            client.SendAsync(data);
        }
    }
}
