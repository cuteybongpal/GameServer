using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Game
{
    internal class Game
    {
        Dictionary<Scenes, List<Session>> UserInScenes = new Dictionary<Scenes, List<Session>>();
        Dictionary<int, PingPong> gameRooms = new Dictionary<int, PingPong>();

        public static float DeltaTime = 1f / 24f;
        public void SendChatting(byte[] packet)
        {
            List<Session> users = UserInScenes[Scenes.Lobby];

            foreach (Session userSession in users)
            {
                GameServer.sendSocket.SendPacket(packet, userSession.Client);
            }
        }

        public void EnterScene(Session session, Scenes scenes)
        {
            var keys = UserInScenes.Keys;

            foreach (var key in keys)
            {
                List<Session> sessions = UserInScenes[key];
                foreach (Session _session in sessions)
                {
                    if (session == _session)
                    {
                        sessions.Remove(session);
                    }
                }
            }
            UserInScenes[scenes].Add(session);
        }
    }

}
