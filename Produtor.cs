using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialIdea
{
    public class Produtor
    {
        public Produtor()
        {
            Colecao = new ConcurrentQueue<int>();
        }

        public ConcurrentQueue<int> Colecao { get; set; }
    }
}