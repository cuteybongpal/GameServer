using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class AcceptSocket
    {
        Socket acceptSocket;
        public async Task StartAccpet()
        {
            AcceptAsync();
        }
        void AcceptAsync()
        {
            Task<Socket> task = acceptSocket.AcceptAsync();
            task.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    Socket socket = t.Result;
                    OnAccept(socket);
                }
                else
                {
                    Console.WriteLine("소켓 연결 실패");
                }
            });     
        }
        void OnAccept(Socket socket)
        {
            GameServer.JobQueue.Push(() => { GameServer.users.Add(socket); });
            AcceptAsync();
        }
        public AcceptSocket(int MaxPerson, IPEndPoint ipEndPoint)
        {
            acceptSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            acceptSocket.Bind(ipEndPoint);
            acceptSocket.Listen(MaxPerson);
        }
    }
}
