using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class RecvSocket
    {
        Socket recvSocket;
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        public RecvSocket(IPEndPoint endPoint)
        {
            recvSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            recvSocket.Bind(endPoint);
        }

        public async Task CheckRecvDatas()
        {
            RecvAsync();
        }

        void RecvAsync()
        {
            e = new SocketAsyncEventArgs();
            bool isPending = recvSocket.ReceiveAsync(e);
            if (!isPending)
            {
                OnRecvData(null, e);
            }
            else
            {
                e.Completed += OnRecvData;
            }
        }

        void OnRecvData(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                byte[] buffer = e.Buffer;
            }
            else
            {
                Console.WriteLine("소켓 에러 : 소켓에서 받지 못했음ㅋ");
            }
        }
    }
}
