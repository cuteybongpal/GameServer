using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static GameServer.PacketUtil;

namespace GameServer
{
    internal class AcceptSocket
    {
        Socket acceptSocket;
        SocketAsyncEventArgs args;
        public async Task StartAccpet()
        {
            AcceptAsync();
        }
        void AcceptAsync()
        {
            args = new SocketAsyncEventArgs();
            bool pending = acceptSocket.AcceptAsync(args);
            if (!pending)
            {
                OnAccept(null, args);
                return;
            }
            args.Completed += OnAccept;
        }
        void OnAccept(object? sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                GameServer.JobQueue.Push(() => { GameServer.users.Add(args.ConnectSocket); });
                AcceptAsync();
            }
            else
            {
                Console.WriteLine("접속 요청을 받을 수 없습니다.");
            }
        }
        public AcceptSocket(int MaxPerson, IPEndPoint ipEndPoint)
        {
            acceptSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            acceptSocket.Bind(ipEndPoint);
            acceptSocket.Listen(MaxPerson);
        }
    }
}

