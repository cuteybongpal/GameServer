using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameRoom.System
{
    internal abstract class GameSystem
    {
        public abstract void Execute();

        public GameSystem(Action updateAction)
        {
            updateAction += Execute;
        }
    }
}
