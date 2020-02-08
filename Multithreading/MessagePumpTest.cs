using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Multithreading
{
    public class MessagePumpTest
    {
        private static readonly Queue<Func<Task>> _tasks = new Queue<Func<Task>>();

        private static readonly object _locker = new object();

        private static int _count = 1;
        
        public static void Start(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            _tasks.Enqueue(Work);
            while (true)
            {
                Console.WriteLine("Fetching task...");
                Func<Task> nextTask = null;
                lock (_locker)
                {
                    if(_tasks.Count > 0)
                        nextTask = _tasks.Dequeue();
                }    
                if (nextTask is null)
                {
                    Console.WriteLine("Finishing message pump");
                    return;
                }
                Console.WriteLine($"Starting task in [{Thread.CurrentThread.ManagedThreadId}]");
                await nextTask.Invoke().ConfigureAwait(false);
                Console.WriteLine($"Finished task in [{Thread.CurrentThread.ManagedThreadId}]");
            }
        }

        private static async Task Work()
        {
            Console.WriteLine($"Working {_count}");
            Console.WriteLine($"Worked {_count}");
            await Task.Delay(500);
            _count++;
            if (_count <= 100)
            {
                lock (_locker)
                {
                    Monitor.Pulse(_locker);
                    _tasks.Enqueue(Work);
                }
            }
            else
            {
                lock (_locker)
                {
                    Monitor.Pulse(_locker);
                    _tasks.Enqueue(null);
                }
            }
        }
    }
}