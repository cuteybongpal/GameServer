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
        PacketProcessor processor = new PacketProcessor();
        public RecvSocket(Socket socket)
        {
            recvSocket = socket;
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
                return;
            }
            e.Completed += OnRecvData;
            
        }

        void OnRecvData(object? sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                byte[] buffer = e.Buffer;
                Socket _sender = e.ConnectSocket;
                Task.Run(() =>
                {
                    processor.ProcessPacket(buffer, _sender);
                });
            }
            else
            {
                Console.WriteLine("소켓 에러 : 소켓에서 받지 못했음ㅋ");
            }
        }
    }
}
