using System;
using System.Diagnostics;
using System.Threading;

namespace Multithreading
{
    class LockAndCaptureVariables
    {
        private static bool done;
        private static object locker = new object();
        private static Stopwatch sw = new Stopwatch();
        
        public static void Start(string[] args)
        {
//            for (int i = 0; i < 10; i++)
//                new Thread (() => Console.Write (i)).Start();
//            return;
            //
            sw.Start();
            var t = new Thread(WriteValue);
            t.Start();
            t.Join();
            var elapsedMilliseconds = sw.ElapsedMilliseconds;

            WriteValue();
            Thread.Yield();
            Console.WriteLine();
            Console.WriteLine($"[{elapsedMilliseconds}]");
        }

        static void WriteValue()
        {
            lock (locker)
            {
                if (!done)
                {
                    Console.WriteLine("Done");
                    done = true;
                }
            }
        }

        static void WriteNumbers()
        {
            for(var i = 0; i < 5; i++)
                Console.Write(i);
        }
    }
}