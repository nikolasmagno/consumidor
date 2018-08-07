using Ouroboros.Exceptions;
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

        public CancellationTokenSource ConsumerCancelationTokenSource { get; set; }
        public CancellationTokenSource ProducerCancelationTokenSource { get; set; }

        /// <summary>
        /// Milliseconds between consumer request
        /// </summary>
        public int ConsumerIntervalDelay { get; set; }

        public int ProducerRefreshIntervalDelay { get; set; }
        public int? ProducerRefreshTimes { get; set; }

        public Service(IProducer<T> producer, IEnumerable<IConsumer<T>> consumers)
        {
            ProducerInitialize(producer);
            ConsumerInitialize(consumers);
        }

        private void ProducerInitialize(IProducer<T> producer)
        {
            _producer = producer;
            ProducerCancelationTokenSource = new CancellationTokenSource();
        }
        private void ConsumerInitialize(IEnumerable<IConsumer<T>> consumers)
        {
            _consumers = consumers;
            ConsumerCancelationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            BasicValidations();
            consumersProcess = _consumers.Select(c => new Task(() => Dequeue(c, _producer)));

            foreach (var processo in consumersProcess)
                processo.Start();
        }

        private void BasicValidations()
        {
            if (_producer == null)
                throw new ProducerCannotBeNull();

            if (_producer.Collection == null)
                throw new ProducerCollectionCannotBeNull();
        }

        private void ProducerRefresh()
        {
            _producer.RefreshCollection();

            if (ProducerRefreshTimes > 1)
                new Task(() => ProducerRefreshRecurrent()).Start();
        }

        private void ProducerRefreshRecurrent()
        {
            while (!ProducerCancelationTokenSource.Token.IsCancellationRequested || ProducerRefreshTimes == 0)
            {
                _producer.RefreshCollection();
                DecreaseRefreshTime();
                Thread.Sleep(ProducerRefreshIntervalDelay);
            }
        }

        private void DecreaseRefreshTime()
        {
            if (ProducerRefreshTimes.HasValue)
                ProducerRefreshTimes--;
        }

        private void Dequeue(IConsumer<T> consumer, IProducer<T> producer)
        {
            T item;
            while (!ProducerCancelationTokenSource.Token.IsCancellationRequested)
            {
                if (producer.Collection.TryDequeue(out item))
                    consumer.Process(item);

                Thread.Sleep(ConsumerIntervalDelay);
            }
        }
    }
}
