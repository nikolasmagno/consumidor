using Ouroboros.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ouroboros
{
    public class Service<T>
    {
        public const int DEFAULT_PRODUCER_REFRESH_INTERVAL = 2000;
        public const int DEFAULT_CONSUMER_REFRESH_INTERVAL = 1000;

        private IProducer<T> _producer { get; set; }
        private IEnumerable<IConsumer<T>> _consumers { get; set; }
        private IEnumerable<Task> _consumersProcess { get; set; }

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
            ProducerRefreshIntervalDelay = DEFAULT_PRODUCER_REFRESH_INTERVAL;
        }
        private void ConsumerInitialize(IEnumerable<IConsumer<T>> consumers)
        {
            _consumers = consumers;
            ConsumerCancelationTokenSource = new CancellationTokenSource();
            ConsumerIntervalDelay = DEFAULT_CONSUMER_REFRESH_INTERVAL;
        }

        public void Start()
        {
            BasicValidations();
            ProducerRefresh();
            InitializeConsumers();
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
            new Task(() => ProducerRefreshRecurrent()).Start();
        }

        private void ProducerRefreshRecurrent()
        {
            while (!ProducerCancelationTokenSource.Token.IsCancellationRequested && ProducerRefreshTimes != 0)
            {
                _producer.RefreshCollection();
                DecreaseRefreshTime();
                Thread.Sleep(ProducerRefreshIntervalDelay);
            }
            Console.WriteLine($"{_producer.Collection.Count} Items to be consumed");
        }

        private void DecreaseRefreshTime()
        {
            if (ProducerRefreshTimes.HasValue)
                 ProducerRefreshTimes --;
        }

        private void InitializeConsumers()
        {
            _consumersProcess = _consumers.Select(c => new Task(() => Dequeue(c, _producer)));

            foreach (var processo in _consumersProcess)
                processo.Start();
        }

        private void Dequeue(IConsumer<T> consumer, IProducer<T> producer)
        {
            T item;
            while (!ConsumerCancelationTokenSource.Token.IsCancellationRequested)
            {
                if (producer.Collection.TryDequeue(out item))
                    consumer.Process(item);
                else
                    Console.WriteLine($"Consumer {consumer.Id} has nothing to process");

                Thread.Sleep(ConsumerIntervalDelay);
            }
        }
    }
}
