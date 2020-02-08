using System;
using System.Linq;

namespace Multithreading
{
    public class PLINQForAllTest
    {
        public static void Start(string[] args)
        {
            "abcdefghijklmnopqrstuvwxyz"
                .AsParallel()
                .Select(c => char.ToUpper(c))
                .ForAll(Console.Write);
        }
    }
}