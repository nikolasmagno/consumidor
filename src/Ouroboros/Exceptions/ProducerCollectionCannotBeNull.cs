using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ouroboros.Exceptions
{
    public class ProducerCollectionCannotBeNull : Exception
    {
        public ProducerCollectionCannotBeNull(): base("Producer collection cannot be null.") { }
    }
}
