using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class JobQueueProcessor<T> : IQueueProccessor<T> where T : Delegate
    {
        JobQueue<T> job = new JobQueue<T>();
        public JobQueue<T> JobQueue { get { return job; } set {  } }

        

        public async Task Process()
        {
            while (true)
            {
                T task =  job.Pop();

                if (typeof(T) == typeof(Func<Task>))
                {
                    Func<Task> t = task as Func<Task>;
                    if (t != null)
                        await t.Invoke();
                }
                else
                {
                    Action action = task as Action;
                    action?.Invoke();
                }
            }
        }

        public void Push(T item)
        {
            JobQueue.Push(item);
        }
        public T Pop()
        {
            return JobQueue.Pop();
        }
    }
}
