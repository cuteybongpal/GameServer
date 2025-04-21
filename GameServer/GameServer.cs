using GameServer.GameRoom;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    internal class GameServer
    {
        static Socket serverSocket;
        static AcceptSocket acceptSocket;
        public static RecvSocket recvSocket;
        public static SendSocket sendSocket;
        static Initializer initializer = new Initializer();
        public static List<Session> Users = new List<Session>();
        //public static JobQueue JobQueue = new JobQueue();
        public static JobQueueProcessor<Func<Task>> SendPacketJobProcessor =  new JobQueueProcessor<Func<Task>>();
        public static JobQueueProcessor<Action> ConnectJobProcessor = new JobQueueProcessor<Action>();
        public static JobQueueProcessor<Action> PacketProcessor = new JobQueueProcessor<Action>();
        public static Game Game = new Game();

        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            Console.WriteLine($"{ipHost.AddressList[0].ToString()}:{endPoint.Port}");
            serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(10);

            initializer.Init();
            acceptSocket = new AcceptSocket(serverSocket);
            sendSocket = new SendSocket();

            Task.Run(() => { acceptSocket.StartAccpet(); });
            Task.Run(() => { ConnectJobProcessor.Process(); });
            Task.Run(() => { SendPacketJobProcessor.Process(); });
            Task.Run(() => PacketProcessor.Process());

            //serverSocket.Bind(endPoint);
            while (true)
            {
                
            }

        }
    }
}
