using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class TaskWaitMethods
    {
        public static void Start(string[] args)
        {
            var tasks = Enumerable
                .Range(1, 10)
                .Select(x => new Task(async () =>
                {
                    Console.WriteLine("Started {0} on {1}",
                        x,
                        Thread.CurrentThread.ManagedThreadId);
                    await Task.Delay(1000);
                    Console.WriteLine("Finished {0} on {1}",
                        x,
                        Thread.CurrentThread.ManagedThreadId);
                }))
                .ToArray();
            Console.WriteLine("Started waiting on {0}",
                Thread.CurrentThread.ManagedThreadId);
            // Executing in WaitAll
            foreach (var task in tasks)
                task.Start();
            //Task.WaitAll(tasks);
            // Executing in WaitAny
            var result = Task.WaitAny(tasks);
            Console.WriteLine("Finished {0} waiting on {1}",
                result,
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}