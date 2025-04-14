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
                GameServer.ConnectJobProcessor.Push(() => 
                {
                    GameServer.users.Add(args.ConnectSocket);
                    Console.WriteLine("연결에 성공했습니다!!!");
                });
                AcceptAsync();
            }
            else
            {
                Console.WriteLine("접속 요청을 받을 수 없습니다.");
            }
        }
        public AcceptSocket(Socket socket)
        {
            acceptSocket = socket;
        }
    }
}

