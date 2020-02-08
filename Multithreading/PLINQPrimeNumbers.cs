using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Multithreading
{
    public class PLINQPrimeNumbers
    {
        public static void Start(string[] args)
        {
            IEnumerable<int> numbers = Enumerable.Range (3, (int)1e6);
            var parallelQuery = 
                from n in numbers.AsParallel()
                where Enumerable.Range (2, (int) Math.Sqrt (n)).All (i => n % i > 0)
                select n;
            int[] primes = parallelQuery.ToArray();
            var index = 0;
            var lowest = primes[index];
            index++;
            var other = primes[index];
            while (index < primes.Length - 1 && lowest < other)
            {
                index++;
                lowest = other;
                other = primes[index];
            }
            
            Console.WriteLine("Index found: " + index + " | Length: " + primes.Length);
//            Console.WriteLine($"Lowest: {primes[index - 1]} | Other: {primes[index]}");
//            Console.WriteLine($"Lowest: {lowest} | Other: {other}");

            // These following lines show that PLINQ may return items out of order 
            for (int i = index - 10; i < index + 10; i++)
            {
                Console.WriteLine($"p[{i}] => {primes[i]}");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"p[{i}] => {primes[i]}");
            }
        }
    }
}