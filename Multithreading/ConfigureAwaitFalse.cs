using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class ConfigureAwaitFalse
    {
        public static void Start(string[] args)
        {
            Console.WriteLine($"Starting SYNC method: [{Thread.CurrentThread.ManagedThreadId}]");
            var taskAwaiter = RunAsync().GetAwaiter();
            taskAwaiter.OnCompleted(() =>
            {
                Console.WriteLine($"Finished ASYNC method: [{Thread.CurrentThread.ManagedThreadId}]");    
            });
            taskAwaiter.GetResult();
            Console.WriteLine($"Finishing SYNC method: [{Thread.CurrentThread.ManagedThreadId}]");
        }

        private static async Task RunAsync()
        {
            Console.WriteLine($"Starting ASYNC method: [{Thread.CurrentThread.ManagedThreadId}]");
            var ts = TaskScheduler.FromCurrentSynchronizationContext();
            await Task.Delay(1000).ConfigureAwait(true);
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Finishing ASYNC method: [{Thread.CurrentThread.ManagedThreadId}]");
            }, CancellationToken.None, TaskCreationOptions.None, ts).Wait();
        }
    }
}