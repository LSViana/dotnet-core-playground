using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    public class ParallelSumSqrtTenMillions
    {
        private static object _locker = new object();
        private static double _total = 0;
        private const int MaxValue = 20_000_000;
        
        public static void Start(string[] args)
        {
            var elapsed = 0L;
            // First method (poor)
            _total = 0;
            Stopwatch sw = Stopwatch.StartNew();
            Parallel.For(1, MaxValue,
                (i) =>
                {
                    lock (_locker)
                        _total += Math.Sqrt(i);
                });
            sw.Stop();
            elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine("Method 1: {0,12} | {1}", _total, elapsed);
            // Second method (excellent)
            _total = 0;
            sw.Restart();
            Parallel.For(1, MaxValue,
                () => 0.0,
                (i, state, local) => local + Math.Sqrt(i),
                (local) => _total += local);
            sw.Stop();
            elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine("Method 2: {0,12} | {1}", _total, elapsed);
            // Third method (good)
            _total = 0;
            sw.Restart();
            _total = ParallelEnumerable.Range(1, MaxValue - 1)
                .Sum(i => Math.Sqrt(i));
            sw.Stop();
            elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine("Method 3: {0,12} | {1}", _total, elapsed);
        }
    }
}