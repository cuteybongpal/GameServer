using GameServer.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Game.System
{
    internal class RigidBodySystem : GameSystem
    {
        public List<RigidBody> Components = new List<RigidBody>();

        public override void Execute()
        {
            foreach (RigidBody r in Components)
            {
                r.Entity.Position += r.Velocity * Game.DeltaTime;
            }
        }
        public RigidBodySystem(Action updateAction) : base(updateAction)
        {

        }
    }
}
