using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class SendSocket
    {
        Socket sendSocket;

        public SendSocket(IPEndPoint endPoint)
        {
            sendSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            sendSocket.Bind(endPoint);
        }

        public void Send(byte[] data)
        {
            sendSocket.Send(data);
        }

    }
}
