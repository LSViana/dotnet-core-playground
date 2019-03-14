using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CryptoAndHash
{
    /// <summary>
    /// In some places, this is known as Keyed-Hash MAC (Message Authentication Code), then it can be also called HMAC
    /// </summary>
    public static class KeyedHash
    {
        private static Stopwatch stopwatch;

        public static string Word { get; private set; }
        public static byte[] WordBytes { get; private set; }
        public static byte[] Key { get; private set; }

        public static void Run()
        {
            Word = "Lucas Viana";
            WordBytes = Encoding.UTF8.GetBytes(Word);
            // Generating Keys to use in the encryption
            GenerateKeys(255);
            // Creating stopwatch to measure performance
            stopwatch = new Stopwatch();
            // Showing the base message to be encrypted
            Console.Write("Keyed Hash source is: ");
            ConsoleHelper.WriteHightlight(Word);
            Console.WriteLine();
            //
            UseKeyedHash(nameof(HMACMD5));
        }

        private static void UseKeyedHash(string algorithmName)
        {
            var algorithm = KeyedHashAlgorithm.Create(algorithmName);
            ConsoleHelper.WriteHightlight($"# Starting {algorithmName} Hashing\n");
            ConsoleHelper.WriteHightlight($"# Finishing {algorithmName} Hashing\n");
        }

        private static void GenerateKeys(byte length)
        {
            Key = new byte[length];
            //
            for (byte i = 0; i < length; i++)
            {
                Key[i] = i;
            }
        }
    }
}
