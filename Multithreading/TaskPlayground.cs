using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class TaskPlayground
    {
        public static void Start(string[] args)
        {
            Console.WriteLine("Before task");
            var task = AsyncOperation();
            Console.WriteLine("Task started");
            task.Wait();
            Console.WriteLine("Finished task");
        }

        static async Task AsyncOperation()
        {
            Console.WriteLine("[in] Task started");
            await Task.Delay(1000);
            Console.WriteLine("[in] Running in thread pool: " + Thread.CurrentThread.IsThreadPoolThread);
            await Task.Delay(1000);
            Console.WriteLine("[in] Finished task");
        }
    }
}