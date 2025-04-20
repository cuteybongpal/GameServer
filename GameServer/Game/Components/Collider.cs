using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Game.Components
{
    /// <summary>
    /// 사각형 충돌 판정을 해주는 컴포넌트
    /// </summary>
    internal class Collider : Component
    {
        public Vector3 Offset;
        public Action<Collider> Callback;

        public Collider(Vector3 offset, Action<Collider> callback)
        {
            Offset = offset;
            Callback = callback;
        }
    }
}
