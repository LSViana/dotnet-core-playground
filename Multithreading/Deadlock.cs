using System;
using System.Threading;

namespace Multithreading
{
    public class Deadlock
    {
        public static void Start(string[] args)
        {
            object locker1 = new object();
            object locker2 = new object();
 
            new Thread (() => {
                lock (locker1)
                {
                    Console.WriteLine("2");
                    Thread.Sleep (1000);
                    Console.WriteLine("4");
                    lock (locker2) // Deadlock
                    {
                        
                    }
                    Console.WriteLine("6");
                }
            }).Start();
            lock (locker2)
            {
                Console.WriteLine("1");
                Thread.Sleep (1000);
                Console.WriteLine("3");
                lock (locker1) // Deadlock
                {
                    
                }
                Console.WriteLine("5");
            }
        }
    }
}