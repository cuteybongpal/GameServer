using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    internal class GameServer
    {
        //static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static AcceptSocket acceptSocket;
        static Initializer initializer = new Initializer();
        public static List<Socket> users = new List<Socket>();
        public static JobQueue JobQueue = new JobQueue();
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            initializer.Init();
            acceptSocket = new AcceptSocket(10, endPoint);
            Task.Run(() => { acceptSocket.StartAccpet(); });
            //serverSocket.Bind(endPoint);
            while (true)
            {
                JobQueue.Popup()?.Invoke();
            }

        }
    }
}
