using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoAndHash
{
    public static class SymmetricCrypto
    {
        private static Stopwatch stopwatch;

        public static string Word { get; private set; }
        public static byte[] WordBytes { get; private set; }
        public static byte[] Key { get; private set; }
        public static byte[] IV { get; private set; }

        public static void Run()
        {
            // Creating data to be encrypted
            Word = "Lucas Viana";
            WordBytes = Encoding.UTF8.GetBytes(Word);
            // Defining keys
            GenerateKeys((int)Math.Pow(2, sizeof(byte) * 8));
            // Printing the encryption message
            Console.Write("Symmetric crypto source is: ");
            ConsoleHelper.WriteHightlight(Word);
            Console.WriteLine();
            // Creating the Stopwatch to measure performance
            stopwatch = new Stopwatch();
            //
            UseCrypto(nameof(Aes));
            // DES stands for "Data Encryption Standard"
            UseCrypto(nameof(TripleDES));
            // This isn't browsable through IntelliSense
            UseCrypto(nameof(DES));
            UseCrypto(nameof(RC2));
        }

        private static void GenerateKeys(int length)
        {
            Key = new byte[length];
            IV = new byte[length];
            //
            for (int i = 0; i < 256; i++)
            {
                IV[i] = Key[i] = (byte)i;
            }
        }

        private static void UseCrypto(string algorithmName)
        {
            ConsoleHelper.WriteHightlight($"# Starting {algorithmName} cryptography\n");
            // Creating MemoryStream to read WordBytes
            using (var wordStream = new MemoryStream(WordBytes))
            {
                var algorithm = SymmetricAlgorithm.Create(algorithmName);
                // Creating ICryptoTransform to perform the cryptography
                using (var encryptTransform = algorithm.CreateEncryptor(Key.Take(algorithm.KeySize / 8).ToArray(), IV.Take(algorithm.BlockSize / 8).ToArray()))
                {
                    // Stream used to encrypt bytes
                    using (var encryptStream = new CryptoStream(wordStream, encryptTransform, CryptoStreamMode.Read))
                    {
                        // Stream used as buffer to keep the encrypted data
                        using (var encryptedStream = new MemoryStream(256))
                         {
                            stopwatch.Start();
                            encryptStream.CopyTo(encryptedStream);
                            stopwatch.Stop();
                            Console.Write("Encrypted: ");
                            WriteStream(encryptedStream);
                            // Stream to decrypt data
                            using (var decryptTransform = algorithm.CreateDecryptor(Key.Take(algorithm.KeySize / 8).ToArray(), IV.Take(algorithm.BlockSize / 8).ToArray()))
                            {
                                // Seeking Stream current Position to decrypt
                                encryptedStream.Seek(0, SeekOrigin.Begin);
                                // Stream used to decrypt bytes
                                using (var decryptStream = new CryptoStream(encryptedStream, decryptTransform, CryptoStreamMode.Read))
                                {
                                    // Stream used as buffer to keep the decrypted data
                                    using (var decryptedStream = new MemoryStream(256))
                                    {
                                        decryptStream.CopyTo(decryptedStream);
                                        Console.Write("Decrypted: ");
                                        WriteStream(decryptedStream, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //
            ConsoleHelper.WriteHightlight($"# Finishing {algorithmName} cryptography\n");
            Console.WriteLine();
        }

        private static void WriteStream(Stream stream, bool asHex = true)
        {
            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;
            // Printing the result in hightlight colors
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var nextByte = stream.ReadByte();
                if (nextByte != -1)
                {
                    do
                    {
                        if (asHex)
                            Console.Write("{0:X}", nextByte);
                        else
                            Console.Write("{0}", (char)nextByte);
                        nextByte = stream.ReadByte();
                    }
                    while (nextByte != -1);
                }
            }
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write($" [{stream.Length} bytes in {stopwatch.ElapsedTicks} ticks]");
            Console.WriteLine();
        }
    }
}
