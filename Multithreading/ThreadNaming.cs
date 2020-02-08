using System;
using System.Threading;

namespace Multithreading
{
    public class ThreadNaming
    {
        public static void Start()
        {
            Thread.CurrentThread.Name = "main";
            Thread worker = new Thread (Go);
            worker.Name = "worker";
            worker.Start();
            Go();
        }
 
        static void Go()
        {
            Console.WriteLine ("Hello from " + Thread.CurrentThread.Name);
        }
    }
}