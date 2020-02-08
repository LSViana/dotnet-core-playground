using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class TaskCompletionSourceTest
    {
        private static TaskCompletionSource<bool> _tcs;

        public static void Start(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            var task = GetProcessedValue();
            Console.WriteLine("Waiting for result to arrive...");
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                Console.WriteLine($"Thread didn't block! {i}");
            }

            Console.WriteLine(task.Result);
        }

        private static Task<bool> GetProcessedValue()
        {
            _tcs = new TaskCompletionSource<bool>();
            new Thread(() => Wait(1000)).Start();
            return _tcs.Task;
        }

        private static bool Wait(int timeout)
        {
            if(Thread.CurrentThread.Name is null)
                Thread.CurrentThread.Name = "Worker";
            Thread.Sleep(timeout);
            var result = true;
            _tcs.SetResult(true);
            return result;
        }
    }
}