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
        //패킷의 크기가 1024바이트 이상이면 세그먼트로 분리해서 보내야 하기 때문에 필요한 변수임
        const int MAXPACKETLENGTH = 1024;
        public SendSocket(IPEndPoint endPoint)
        {
            sendSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            sendSocket.Bind(endPoint);
        }

        public void Send(byte[] data)
        {
            int segmentCount = data.Length / MAXPACKETLENGTH;
            if (segmentCount <= 0)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(data, 0, data.Length);
                sendSocket.SendAsync(segment);
            }
            for (int i = 0; i < segmentCount; i++)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>();
            }
        }
    }
}
