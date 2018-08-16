using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor.Sample
{
    class Producer : BaseProducer<int>
    {
        public Producer()
        {
        }

        public override void RefreshCollection()
        {
            for (int i = 0; i < 15; i++)
                Collection.Enqueue(i);
        }
    }
}
