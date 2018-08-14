using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor.Sample
{
    class Consumer : IConsumer<int>
    {
        public string Id { get; set; }

        public void Process(int item) => Console.WriteLine($"Consumer {Id} - processing item {item}");
    }
}
