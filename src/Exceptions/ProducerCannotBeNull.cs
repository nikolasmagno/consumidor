﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumidor.Exceptions
{
    public class ProducerCannotBeNull : Exception
    {
        public ProducerCannotBeNull(): base("Producer cannot be null.") { }
    }
}
