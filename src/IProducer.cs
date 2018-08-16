using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor
{
    public interface IProducer<T>
    {
        ConcurrentQueue<T> Collection { get; set; }
        IList<T> Processing { get; set; }
        void RefreshCollection();
    }
}
