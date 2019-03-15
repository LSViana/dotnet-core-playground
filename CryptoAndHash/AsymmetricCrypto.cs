using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CryptoAndHash
{
    public static class AsymmetricCrypto
    {
        private static Stopwatch stopwatch;

        public static string Word { get; private set; }
        public static byte[] WordBytes { get; private set; }

        public static void Run()
        {
            Word = "Lucas Viana";
            WordBytes = Encoding.UTF8.GetBytes(Word);
            //
            Console.Write("Hash source is: ");
            ConsoleHelper.WriteHightlight(Word);
            Console.WriteLine();
            // Creating a stopwatch instance to measure performance
            stopwatch = new Stopwatch();
            //
            UseRSA();
        }

        private static void UseRSA()
        {
            var algorithm = RSA.Create();
            ConsoleHelper.WriteHightlight($"# Starting {nameof(RSA)} cryptography\n");
            // Continue here
            ConsoleHelper.WriteHightlight($"# Finishing {nameof(RSA)} cryptography\n");
            Console.WriteLine();
        }
    }
}
