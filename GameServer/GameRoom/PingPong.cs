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
        public int[] Scores = new int[2];

        ColliderSystem colliderSystem;
        RigidBodySystem rigidBodySystem;

        public static float CameraOffsetX = 9.8f;
        public static float CameraOffsetY = 5.4f;

        public async Task Start()
        {
            colliderSystem = new ColliderSystem(UpdateAction);
            rigidBodySystem = new RigidBodySystem(UpdateAction);

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

            Start();
        }
    }
}
