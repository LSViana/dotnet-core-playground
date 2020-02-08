using System;
using System.Threading.Tasks;

namespace Multithreading
{
    public class ParallelForForEach
    {
        public static void Start(string[] args)
        {
            bool executeFor = false, executeForEach = true;
            #region For

            if (executeFor)
            {
                // Standard For
                Console.Write("    Standard For:");
                for (int i = 1; i < 11; i++)
                {
                    Console.Write("{0,3}", i);
                }
                Console.WriteLine();
                // Parallel For
                Console.Write("    Parallel For:");
                Parallel.For(1, 11, i =>
                {
                    Console.Write("{0,3}", i);
                });
                Console.WriteLine();
            }
            #endregion

            #region ForEach
            if (executeForEach)
            {
                var word = "Greetings, universe!";
                // Standard ForEach
                Console.Write("Standard ForEach:");
                foreach (var letter in word)
                {
                    Console.Write("[{0}]", letter);
                }
                Console.WriteLine();
                // Parallel ForEach
                Console.Write("Parallel ForEach:");
                Parallel.ForEach(word, letter =>
                {
                   Console.Write("[{0}]", letter); 
                });
                Console.WriteLine();
            }
            #endregion
        }
    }
}