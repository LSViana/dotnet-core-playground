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
            ConsoleHelper.WriteSuccess("Asymmetric Cryptography");
            Console.Write(" source is: ");
            ConsoleHelper.WriteHightlight(Word);
            Console.WriteLine();
            // Creating a stopwatch instance to measure performance
            stopwatch = new Stopwatch();
            //
            UseRSA();
        }

        private static void UseRSA()
        {
            RSAParameters exportedParameters;
            byte[] encryptedBuffer;
            var receiver = RSA.Create();
            exportedParameters = receiver.ExportParameters(false);
            var sender = RSA.Create();
            sender.ImportParameters(exportedParameters);
            //
            EncryptData(sender, out encryptedBuffer);
            //
            DecryptData(receiver, encryptedBuffer);
            ConsoleHelper.WriteHightlight($"# Finishing {nameof(RSA)} cryptography");
        }

        private static void DecryptData(RSA rsa, byte[] encryptedBuffer)
        {
            stopwatch.Restart();
            var decryptedBuffer = rsa.Decrypt(encryptedBuffer, RSAEncryptionPadding.OaepSHA1);
            stopwatch.Stop();
            Console.Write("Decrypted: ");
            WriteBytes(decryptedBuffer, false);
            Console.WriteLine();
        }

        private static void EncryptData(RSA rsa, out byte[] encryptedBuffer)
        {
            ConsoleHelper.WriteHightlight($"# Starting {nameof(RSA)} cryptography\n");
            // Encrypting data using public key
            stopwatch.Restart();
            encryptedBuffer = rsa.Encrypt(WordBytes, RSAEncryptionPadding.OaepSHA1);
            stopwatch.Stop();
            Console.Write("Encrypted: ");
            WriteBytes(encryptedBuffer);
            Console.WriteLine();
        }

        private static void WriteBytes(byte[] buffer, bool asHex = true)
        {
            if (asHex)
            {
                foreach (var @byte in buffer)
                {
                    ConsoleHelper.WriteHightlight(string.Format("{0:X}", @byte));
                }
            }
            else
                ConsoleHelper.WriteHightlight(Encoding.UTF8.GetString(buffer));
            Console.Write($" [{buffer.Length} bytes in {stopwatch.ElapsedTicks} ticks]");
        }
    }
}
