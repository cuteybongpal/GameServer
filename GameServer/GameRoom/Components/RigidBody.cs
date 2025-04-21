using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameRoom.Components
{
    internal class RigidBody : Component
    {
        public Vector2 Velocity;
        public Vector2 Dir { get
            {
                if (Speed == 0)
                    return Vector2.Zero;
                Vector2 dir = new Vector2((float)(Velocity.X / Speed), (float)(Velocity.Y / Speed));
                return dir;
            }
        }
        public double Speed
        {
            get
            {
                double speed = Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2));
                return speed;
            }
        }
    }
}
