using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class ServiceLocator
    {
        static Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void Register<T>(object service)
        {
            services[typeof(T)] = service;
        }

        public T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
