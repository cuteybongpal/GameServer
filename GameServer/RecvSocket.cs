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
            e = new SocketAsyncEventArgs();
            e.SetBuffer(new byte[1024],0, 1024);
            e.Completed += OnRecvData;
        }

        public async Task CheckRecvDatas()
        {
            RecvAsync();
        }

        void RecvAsync()
        {
            bool isPending = recvSocket.ReceiveAsync(e);

            if (!isPending)
            {
                Console.WriteLine("pending is true");
                OnRecvData(null, e);
                return;
            }
        }

        void OnRecvData(object? sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine(e.SocketError);
            Console.WriteLine(e.BytesTransferred);
            if (e.SocketError == SocketError.Success)
            {
                if (e.BytesTransferred == 0)
                    return;
                byte[] buffer = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesTransferred);
                Socket _sender = recvSocket;
                GameServer.PacketProcessor.Push(() =>
                {
                    processor.ProcessPacket(buffer, _sender);
                });
            }
            else
            {
                Console.WriteLine("소켓 에러 : 소켓에서 받지 못했음ㅋ");
                recvSocket.Close();
                return;
            }
            RecvAsync();
        }
    }
}
