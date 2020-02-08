using System;
using System.Threading;

namespace Multithreading
{
    public static class ThreadActions
    {
        public const string Release = "release";
        public const string Exit = "exit";
    }
    
    public class BasicWaitHandle
    {
        static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        
        public static void Start(string[] args)
        {
            var t = new Thread(Waiter);
            t.Start();
            //
            var input = String.Empty;
            while (input.ToLower() != ThreadActions.Exit)
            {
                Console.Out.Write("What do you intend to do? ");
                input = Console.In.ReadLine();
                Console.WriteLine($"Received: [{input}]");
                // Actions
                switch (input)
                {
                    case ThreadActions.Release:
                        Console.WriteLine("Releasing thread...");
                        _autoResetEvent.Set();
                        Console.WriteLine("Thread released!");
                        break;
                }
            }
        }

        static void Waiter()
        {
            Console.WriteLine("Before wait...");
            _autoResetEvent.WaitOne(5000); // This thread won't wait any longer than 5 seconds to be released here
            Console.WriteLine("After wait! Ready to go!");
        }
    }
}