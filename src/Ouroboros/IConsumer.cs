using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ouroboros
{
    public interface IConsumer<T>
    {
        string Id { get; set; }
        void Process<T>(T item);
    }
}
