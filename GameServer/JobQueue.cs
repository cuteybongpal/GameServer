using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class JobQueue<T> where T : Delegate
    {
        readonly object queueLocker = new object();
        private Queue<T> job = new Queue<T>();


        public void Push(T action)
        {
            lock (queueLocker)
            {
                job.Enqueue(action);
            }
        }
        public T Pop()
        {
            T action;
            try
            {
                action = job.Dequeue();
            }
            catch (Exception ex)
            {
                action = null; 
            }
            return action;
        }
    }
}
