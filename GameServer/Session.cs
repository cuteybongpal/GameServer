using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class Session
    {
        static int NextSessionId = 0;
        //세션끼리 구분 시켜줄 아이디
        public int SessionId = 0;
        public Socket Client;
        RecvSocket recvSocket;
        
        public void StartRecieve()
        {
            Task.Run(recvSocket.CheckRecvDatas);
        }
        public Session (Socket client)
        {
            SessionId = NextSessionId;
            NextSessionId++;
            this.Client = client;
            recvSocket = new RecvSocket(client);
            StartRecieve();
        }
    }
}
