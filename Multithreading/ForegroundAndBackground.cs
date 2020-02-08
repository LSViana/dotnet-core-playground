using System;
using System.Diagnostics;
using System.Threading;

namespace Multithreading
{
    public class ForegroundAndBackground
    {
        public static void Start(string[] args)
        {
            Thread worker = new Thread ( () =>
            {
                var input = Console.ReadLine();
                Console.WriteLine(input);
            });
            worker.IsBackground = false;
            worker.Start();
        }
    }
}