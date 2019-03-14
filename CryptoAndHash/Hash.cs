using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;

namespace CryptoAndHash
{
    public class Hash
    {
        public static string Word { get; private set; }
        public static byte[] WordBytes { get; private set; }
        public static Stopwatch StopwatchCrypto { get; private set; }

        public static void Run()
        {
            Word = "Lucas Viana";
            WordBytes = Encoding.UTF8.GetBytes(Word);
            //
            Console.Write("Hash source is: ");
            ConsoleHelper.WriteHightlight(Word);
            Console.WriteLine();
            // MD5 is Message Digest 5 (generates 128-bit, 16 bytes, hashes)
            UseHashAlgorithm(HashAlgorithmName.MD5);
            // SHA1 is Secure Hash Algorithm 1 (generates 160-bit, 20 bytes, hashes)
            UseHashAlgorithm(HashAlgorithmName.SHA1);
            // SHA256 is analogous to SHA1 (generates 256-bit, 32 bytes, hashes)
            UseHashAlgorithm(HashAlgorithmName.SHA256);
            // SHA384 is analogous to SHA1 (generates 384-bit, 48 bytes, hashes)
            UseHashAlgorithm(HashAlgorithmName.SHA384);
            // SHA512 is analogous to SHA1 (generates 512-bit, 64 bytes, hashes)
            UseHashAlgorithm(HashAlgorithmName.SHA512);
        }

        private static void UseHashAlgorithm(HashAlgorithmName hashAlgorithmName)
        {
            var algorithm = HashAlgorithm.Create(hashAlgorithmName.Name);
            ConsoleHelper.WriteHightlight($"# Starting {hashAlgorithmName.Name} Hashing\n");
            // Divide this value by 8 to get the number in bytes
            var buffer = new byte[algorithm.HashSize / 8];
            var bytesWritten = 0;
            if (StopwatchCrypto is null)
                StopwatchCrypto = new Stopwatch();
            StopwatchCrypto.Start();
            if (algorithm.TryComputeHash(WordBytes, buffer, out bytesWritten))
            {
                StopwatchCrypto.Stop();
                Console.WriteLine($"[{bytesWritten} bytes in {StopwatchCrypto.ElapsedTicks} ticks]");
                Console.Write("\t");
                foreach (var hashByte in buffer.Take(bytesWritten))
                {
                    var firstHalf = (byte)(hashByte & 0b1111_0000);
                    var secondHalf = (byte)(hashByte & 0b0000_1111);
                    ConsoleHelper.WriteSuccess(string.Format("{0:x}{1:x}", firstHalf / 16, secondHalf));
                }
                Console.WriteLine();
            }
            //
            ConsoleHelper.WriteHightlight($"# Finishing {hashAlgorithmName.Name} Hashing\n");
            Console.WriteLine();
        }
    }
}
