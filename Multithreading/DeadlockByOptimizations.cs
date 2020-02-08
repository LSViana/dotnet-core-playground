using System.Threading;

namespace Multithreading
{
    public class DeadlockByOptimizations
    {
        public static void Start(string[] args)
        {
            bool complete = false; 
            var t = new Thread (() =>
            {
                bool toggle = false;
                while (!complete) toggle = !toggle;
            });
            t.Start();
            Thread.Sleep (1000);
            complete = true;
            t.Join();        // Blocks indefinitely
        }
    }
}