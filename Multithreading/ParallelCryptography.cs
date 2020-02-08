using System;
using System.Linq;
using System.Security.Cryptography;

namespace Multithreading
{
    public class ParallelCryptography
    {
        public static void Start(string[] args)
        {
            var keyPairs = ParallelEnumerable.Range(0, 6)
                // This didn't work in .NET Core running on Ubuntu 18.04
                .Select(i => RSA.Create().ToXmlString(true))
                .ToArray();
            //
            foreach (var keyPair in keyPairs)
            {
                Console.WriteLine("Key pair: {0}", keyPair);
            }
        }
    }
}