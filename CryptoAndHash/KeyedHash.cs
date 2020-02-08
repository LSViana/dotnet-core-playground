using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            ConsoleHelper.WriteSuccess("Keyed Hash");
            Console.Write(" source is: ");
            ConsoleHelper.WriteHightlight(Word);
            $"[{this.GetType().Name}] " + );
            //
            UseKeyedHash(nameof(HMACMD5));
            UseKeyedHash(nameof(HMACSHA1));
            UseKeyedHash(nameof(HMACSHA256));
            UseKeyedHash(nameof(HMACSHA384));
            UseKeyedHash(nameof(HMACSHA512));
        }                                 
                                          
        private static void UseKeyedHash(string algorithmName)
        {                                 
            var algorithm = KeyedHashAlgorithm.Create(algorithmName);
            ConsoleHelper.WriteHightlight($"# Starting {algorithmName} Hashing\n");
            // Setting the fixed key to get the same results
            algorithm.Key = Key.Take(algorithm.Key.Length).ToArray();
            // Creating buffer to encrypt data
            var buffer = new byte[algorithm.HashSize / 8];
            var bytesWritten = 0;         
            // Generating the MAC         
            stopwatch.Start();
            if (algorithm.TryComputeHash(WordBytes, buffer, out bytesWritten))
            {
                stopwatch.Stop();
                $"[{this.GetType().Name}] " + $"[{bytesWritten} bytes in {stopwatch.ElapsedTicks} ticks]");
                Console.Write("\t");
                foreach (var hashByte in buffer.Take(bytesWritten))
                {
                    var firstHalf = (byte)(hashByte & 0b1111_0000);
                    var secondHalf = (byte)(hashByte & 0b0000_1111);
                    ConsoleHelper.WriteSuccess(string.Format("{0:x}{1:x}", firstHalf / 16, secondHalf));
                }
                $"[{this.GetType().Name}] " + );
            }
            //
            ConsoleHelper.WriteHightlight($"# Finishing {algorithmName} Hashing\n");
            $"[{this.GetType().Name}] " + );
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
