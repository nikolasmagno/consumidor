using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor
{
    public interface IConsumer<T>
    {
        string Id { get; set; }
        void Process(T item);
    }
}
