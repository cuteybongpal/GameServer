using GameServer.GameRoom.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameRoom.System
{
    internal class ColliderSystem : GameSystem
    {
        public List<Collider> Colliders = new List<Collider>();
        public Dictionary<(int, int), List<Collider>> ColliderGroup = new Dictionary<(int, int), List<Collider>>();
        public override void Execute()
        {

            foreach (List<Collider> colliders in ColliderGroup.Values)
            {
                if (colliders.Count <= 1)
                    continue;
                for (int i = 0; i < colliders.Count; i++)
                {
                    for (int j = i + 1; j < colliders.Count; j++)
                    {
                        float distX = Math.Abs(colliders[j].Entity.Position.X - colliders[i].Entity.Position.X);
                        float distY = Math.Abs(colliders[j].Entity.Position.Y - colliders[j].Entity.Position.Y);
                        float offsetX = colliders[j].Offset.X + colliders[i].Offset.X;
                        float offsetY = colliders[j].Offset.Y + colliders[i].Offset.Y;
                        if (distY <= offsetY && distX <= offsetX)
                        {
                            colliders[i].Callback?.Invoke(colliders[j]);
                            colliders[j].Callback?.Invoke(colliders[i]);
                        }
                    }
                }
            }

        }
        public ColliderSystem(Action updateAction) : base(updateAction)
        {
            
        }
    }
}
