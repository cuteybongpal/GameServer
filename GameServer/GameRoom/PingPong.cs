using GameServer.GameRoom.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameRoom
{
    internal class PingPong
    {
        static int NextRoomID = 0;
        public int RoomID = 0;
        public Session[] Users = new Session[2];
        public Action UpdateAction;
        public Action StartAction;
        int deltaTime;

        public async Task Start()
        {
            deltaTime = (int)Math.Round(Game.DeltaTime * 1000);
            StartAction?.Invoke();
            await Task.Delay(deltaTime);
            Update();
        }
        public async Task Update()
        {
            int deltaTime = (int)Math.Round(Game.DeltaTime * 1000);
            while (true)
            {
                UpdateAction?.Invoke();
                await Task.Delay(deltaTime);
            }
        }
        public PingPong()
        {
            RoomID = NextRoomID;
            NextRoomID++;
        }
    }
}
