using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ouroboros.Sketch
{
    class Program
    {
        static void Main(string[] args)
        {
            var produtor = new Produtor();
            var cancelationTokenS = new CancellationTokenSource();
            var cancelationToken = cancelationTokenS.Token;
            var consumidores = new List<Consumidor>()
            {
                 new Consumidor { Nome = "Consumidor 1" },
                 new Consumidor { Nome = "Consumidor 3" },
                 new Consumidor { Nome = "Consumidor 4" },
                 new Consumidor { Nome = "Consumidor 5" },
                 new Consumidor { Nome = "Consumidor 6" },
            };

            for (int i = 0; i < 200000; i++)
                produtor.Colecao.Enqueue(i);

            var processoConsumidores = consumidores.Select(c => new Task(() => Dequeue(c, produtor, cancelationToken)));

            foreach (var processo in processoConsumidores)
                processo.Start();

            Console.ReadLine();
            cancelationTokenS.Cancel();
            Console.ReadLine();
        }

        private static void Dequeue(Consumidor c, Produtor produtor, CancellationToken cancelationToken)
        {
            int item;

            while (!cancelationToken.IsCancellationRequested)
            {
                if (produtor.Colecao.TryDequeue(out item))
                    c.Processar(item);

                Thread.Sleep(1000);
            }
            Console.WriteLine($"Consumidor {c.Nome} - Morri");
        }
    }
}