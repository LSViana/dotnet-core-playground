using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class UnixSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            Console.WriteLine("[Posting callback] ");
            // This causes the execution in the same Main Thread
            SynchronizationContextPlayground.Actions.Enqueue(() => d(state));
            // This causes the execution in the Worker Thread
            //base.Post(d, state); // or d(state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Console.WriteLine("[Sending callback] ");
            base.Send(d, state);
        }
    }
    
    public class SynchronizationContextPlayground
    {
        private static bool _running = false;
        private static int _iteration = 0;
        private static Thread _mainThread;
        internal static Queue<Action> Actions = new Queue<Action>();
        private static object _locker = new object();

        public static void Start(string[] args)
        {
            StartLooper();
            lock (_locker)
            {
                Actions.Enqueue(async () =>
                {
                    Console.WriteLine($"Greetings, universe! [{Thread.CurrentThread.ManagedThreadId}]");
                    // Calling ConfigureAwait(false) here simply takes custom SynchronizationContext out of scenes
                    await Task.Delay(500);
                    Console.WriteLine($"Adios, universe! [{Thread.CurrentThread.ManagedThreadId}]");
                });
            }
            _mainThread.Join();
        }

        private static void StartLooper()
        {
            _mainThread = new Thread(() => {
                var sc = new UnixSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(sc);
                Console.WriteLine($"[1] Current thread: {Thread.CurrentThread.ManagedThreadId}");
                _running = true;
                while (_running)
                {
                    if (_iteration > 20)
                        _running = false;
                    else
                    {
                        Action action = null;
                        lock (_locker)
                        {
                            if (Actions.Count > 0)
                                action = Actions.Dequeue();
                        }
                        Console.WriteLine($"@[{Thread.CurrentThread.ManagedThreadId}] ");
                        Task.Delay(1000).Wait();
                        action?.Invoke();
                        Thread.Sleep(100); // Simulate UI drawing work
                    }
                    _iteration++;
                }
                Console.WriteLine();
                Console.WriteLine($"[2] Current thread: {Thread.CurrentThread.ManagedThreadId}");
            });
            _mainThread.Start();
        }
    }
}