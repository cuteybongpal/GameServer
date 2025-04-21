using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.GameRoom.Components;
using System.Numerics;

namespace GameServer.GameRoom
{
    internal class Ball : Entity
    {

        public Ball(Action updateAction)
        {
            Components.Add(new RigidBody());

            Components.Add(new Collider(new Vector3(.1f, .1f, 0), (collider) =>
            {
                if (collider.Entity.Name == "Panel")
                {
                    RigidBody rigidBody = GetComponent<RigidBody>();
                    rigidBody.Velocity.X *= -1;
                }
            }));
            updateAction += Update;
        }
        void Update()
        {

        }
    }
}
