using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class JobQueue
    {
        readonly object queueLocker = new object();
        private Queue<Action> job = new Queue<Action>();


        public void Push(Action action)
        {
            lock (queueLocker)
            {
                job.Enqueue(action);
            }
        }
        public Action Popup()
        {
            Action action;
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
