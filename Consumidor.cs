using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialIdea
{
    class Consumidor
    {
        public string Nome { get; set; }
        public void Processar(int numero) => Console.WriteLine($"Consumidor {Nome} - Processei o item {numero}");
    }
}