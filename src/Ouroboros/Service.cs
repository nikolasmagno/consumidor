using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ouroboros
{
    public class Service<T>
    {
        private IProducer<T> _producer { get; set; }
        private IEnumerable<IConsumer<T>> _consumers { get; set; }
        private IEnumerable<Task> consumersProcess { get; set; }

        public CancellationTokenSource CancelationTSource { get; set; }

        /// <summary>
        /// Milliseconds between consumer request
        /// </summary>
        public int ConsumerIntervalDelay { get; set; }

        public Service(IProducer<T> producer, IEnumerable<IConsumer<T>> consumers)
        {
            _producer = producer;
            _consumers = _consumers;
            CancelationTSource = new CancellationTokenSource();
        }

        public void Start()
        {
            consumersProcess = _consumers.Select(c => new Task(() => Dequeue(c, _producer, CancelationTSource.Token)));

            foreach (var processo in consumersProcess)
                processo.Start();
        }

        private void Dequeue(IConsumer<T> consumer, IProducer<T> producer, CancellationToken token)
        {
            T item;
            while (!token.IsCancellationRequested)
            {
                if (producer.Collection.TryDequeue(out item))
                    consumer.Process(item);

                Thread.Sleep(ConsumerIntervalDelay);
            }
        }
    }
}
