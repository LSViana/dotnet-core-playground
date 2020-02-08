using System;
using System.Threading;

namespace Multithreading
{
    public class SimpleMethodBeginInvoke
    {
        // Not supported in .NET Core
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        
        public static void Start(string[] args)
        {
            Console.WriteLine($"Running on Thread [1]: {Thread.CurrentThread.ManagedThreadId}");
            //
            var name = "Lucas";
            Func<string, int> method = Work;
            var asyncResult = method.BeginInvoke(name, WorkCallback, method);
            Console.WriteLine($"Started running on Thread: {Thread.CurrentThread.ManagedThreadId}");
            _autoResetEvent.WaitOne();
        }

        private static void WorkCallback(IAsyncResult ar)
        {
            Console.WriteLine($"Running on Thread [2]: {Thread.CurrentThread.ManagedThreadId}");
            var method = (Func<string, int>) ar.AsyncState;
            var result = method.EndInvoke(ar);
            Console.WriteLine($"String length is: {result}");
            Console.WriteLine($"Thread signaling...");
            _autoResetEvent.Set();
            Console.WriteLine($"Thread signaled!");
        }

        private static int Work(string content)
        {
            return content.Length;
        }
    }
}