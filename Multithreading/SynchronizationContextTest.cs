using System;
using System.Threading;

namespace Multithreading
{
    public static class SynchronizationContextTest
    {
        public static void Start(string[] args)
        {
            DeadlockSync dead1 = new DeadlockSync(1);
            DeadlockSync dead2 = new DeadlockSync(2);
            dead1.Other = dead2;
            dead2.Other = dead1;
            //
            new Thread (dead1.Demo).Start();
            dead2.Demo();
        }
    }
    
    
    public class DeadlockSync : ContextBoundObject
    {
        private readonly int _number;
        public DeadlockSync Other;

        public DeadlockSync(int number)
        {
            _number = number;
        }
        
        public void Demo() { Thread.Sleep (1000); Other.Hello(); }
        void Hello()       { Console.WriteLine ($"hello {_number}");        }
    }
}