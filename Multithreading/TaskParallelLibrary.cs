using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace Multithreading
{
    public class TaskParallelLibrary
    {
        public static void Start(string[] args)
        {
            // TIP: Set a high value to minimum threads per core (default is 1) to avoid delay on thread creations when some work is needed!
            ThreadPool.SetMinThreads(20, 20);
            try
            {
                // Wait is needed, because running methods through Task Parallel Library use background threads (pooled threads)
                var task = Task.Factory.StartNew(Go);
                Console.WriteLine("We're in a pooled thread? " + Thread.CurrentThread.IsThreadPoolThread);
                var result = task;
                Console.WriteLine("Result is: " + result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().Name);
            }
        }

        static bool Go()
        {
            Console.WriteLine("We're in a pooled thread? " + Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Going to sleep...");
            Thread.Sleep(1000);
            throw null;
//            Console.WriteLine("Slept!");
//            return false;
        }
    }
}