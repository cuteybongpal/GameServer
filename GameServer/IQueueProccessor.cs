using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal interface IQueueProccessor<T> where T : Delegate
    {
        public JobQueue<T> JobQueue {get; set;}

        public Task Process();

        public void Push(T item);

        public T Pop();
    }
}
