# Consumidor [![](https://badge.fury.io/nu/Consumidor.svg)](https://www.nuget.org/packages/Consumidor/)

### A simple library to implements producer-consumer pattern

* [Overview](#overview)
* [Usage](#usage)

## Overview
It's a library to give a simple away for implement producer and consumer pattern. Allowing the complex away from your code and time to focus in your task. All consumers and producer will run in a separate task, doing concurrency management for you.

## Usage
### Producer
```cs
 class Producer : IProducer<int>
    {
        public Producer()
        {
            Collection = new ConcurrentQueue<int>();
        }

        public ConcurrentQueue<int> Collection { get; set; }

        public void RefreshCollection()
        {
            for (int i = 0; i < 15; i++)
                Collection.Enqueue(i);
        }
    }
```
### Consumer
```cs
 class Consumer : IConsumer<int>
    {
        public string Id { get; set; }

        public void Process<T>(T item) => Console.WriteLine($"Consumer {Id} - processing item {item}");
    }
```

### Starting
```cs
            var producer = new Producer();
            var consumers = new List<Consumer>()
            {
                new Consumer { Id = "Consumer 1" },
                new Consumer { Id = "Consumer 2" },
                new Consumer { Id = "Consumer 3" },
                new Consumer { Id = "Consumer 4" },
                new Consumer { Id = "Consumer 5" },
            };

            var service = new Service<int>(producer, consumers);            
            service.Start();
            Console.WriteLine("Press something to stop producer");
            Console.ReadLine();
            service.ProducerCancelationTokenSource.Cancel();

            Console.WriteLine("Press something to stop consumers");
            Console.ReadLine();
            service.ConsumerCancelationTokenSource.Cancel();
            Console.WriteLine("Done!");
            Console.ReadKey();
```

## Options
You can define:
  * ConsumerIntervalDelay;
  * ProducerRefreshIntervalDelay;
  * ProducerRefreshTimes;
  * ThrowExceptionOnConsumerError ;
  * ReturnToQueueOnError;
