using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameRoom
{
    internal class Panel : Entity
    {
        public Panel(Action updateAction)
        {
            updateAction += Update;
        }
        void Update()
        {
            //if ()
        }
    }
}
