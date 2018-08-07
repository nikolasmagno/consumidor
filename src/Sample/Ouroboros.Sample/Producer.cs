using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ouroboros.Sample
{
    class Producer : IProducer<int>
    {
        public Producer()
        {
            Collection = new ConcurrentQueue<int>();
        }

        public ConcurrentQueue<int> Collection { get; set; }

        public void RefreshCollection()
        {
            for (int i = 0; i < 10; i++)
                Collection.Enqueue(i);
        }
    }
}
