using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor
{
    public abstract class BaseProducer<T> : IProducer<T>
    {
        public BaseProducer()
        {
            Collection = new ConcurrentQueue<T>();
            Processing = new List<T>();
        }

        public virtual ConcurrentQueue<T> Collection { get; set; }
        public virtual IList<T> Processing { get; set; }

        public abstract void RefreshCollection();
    }
}
