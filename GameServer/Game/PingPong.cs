using GameServer.Game.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Game
{
    internal class PingPong
    {
        static int NextRoomID = 0;
        public int RoomID = 0;
        public Session[] Users = new Session[2];
        public Action UpdateAction;
        public Action StartAction;
        int deltaTime;

        public void Start()
        {
            deltaTime = (int)Math.Round(Game.DeltaTime * 1000);
            StartAction?.Invoke();
            Task.Delay(deltaTime);
            Update();
        }
        public void Update()
        {
            int deltaTime = (int)Math.Round(Game.DeltaTime * 1000);
            while (true)
            {
                UpdateAction?.Invoke();
                Task.Delay(deltaTime);
            }
        }
        public PingPong()
        {
            RoomID = NextRoomID;
            NextRoomID++;
        }
    }
}
