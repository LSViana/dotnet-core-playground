﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            $"[{this.GetType().Name}] " + );
            // Creating a stopwatch instance to measure performance
            stopwatch = new Stopwatch();
            // RSA
            UseRSA();
            $"[{this.GetType().Name}] " + );
            // Diffie-Hellman allows the parties to share a symmetric key generated from both the public and private keys
            UseDiffieHellman();
            $"[{this.GetType().Name}] " + );
            // DSA allows one party to verify the integrity of the data sent by the other part through the use of public and private keys
            UseDSA();
        }

        private static void UseDSA()
        {
            ConsoleHelper.WriteHightlight($"# Starting {nameof(DSA)} cryptography");
            $"[{this.GetType().Name}] " + );
            // Creating the sender
            var sender = DSA.Create();
            // Signing the data using the DSA sender
            var senderSignature = sender.SignData(WordBytes, HashAlgorithmName.SHA512);
            Console.Write("Data signature: ");
            WriteBytes(senderSignature);
            $"[{this.GetType().Name}] " + );
            // Creating the receiver
            var receiver = DSA.Create();
            // Verifying the signature using the sender's public key
            receiver.ImportParameters(sender.ExportParameters(false));
            var validSignature = receiver.VerifyData(WordBytes, senderSignature, HashAlgorithmName.SHA512);
            Console.Write("Signature valid: ");
            ConsoleHelper.WriteSuccess(validSignature);
            $"[{this.GetType().Name}] " + );
            //
            ConsoleHelper.WriteHightlight($"# Finishing {nameof(DSA)} cryptography");
            $"[{this.GetType().Name}] " + );
        }

        private static void UseDiffieHellman()
        {
            ConsoleHelper.WriteHightlight($"# Starting {nameof(ECDiffieHellman)} cryptography");
            $"[{this.GetType().Name}] " + );
            // Creating the sender
            var sender = ECDiffieHellman.Create();
            var senderPublicKey = sender.PublicKey;
            // Creating the receiver
            var receiver = ECDiffieHellman.Create();
            var receiverPublicKey = receiver.PublicKey;
            // Generating the shared public key
            var senderSharedKey = sender.DeriveKeyMaterial(receiverPublicKey);
            // Creating cryptography encryptor from the shared key
            var senderRc2 = RC2.Create();
            using (var encryptor = senderRc2.CreateEncryptor(senderSharedKey.Take(senderRc2.KeySize / 8).ToArray(), senderSharedKey.Take(senderRc2.BlockSize / 8).ToArray()))
            {
                // Encrypting using RC2
                using (var wordStream = new MemoryStream(WordBytes))
                {
                    using (var encryptStream = new CryptoStream(wordStream, encryptor, CryptoStreamMode.Read))
                    {
                        using (var encryptedStream = new MemoryStream(64))
                        {
                            // Copying the encrypted result to the Stream
                            stopwatch.Restart();
                            encryptStream.CopyTo(encryptedStream);
                            stopwatch.Stop();
                            //
                            Console.Write("Encrypted: ");
                            WriteBytes(encryptedStream.ToArray());
                            $"[{this.GetType().Name}] " + );
                            // Decrypting using a shared key generated by the receiver
                            var receiverSharedKey = receiver.DeriveKeyMaterial(senderPublicKey);
                            var receiverRc2 = RC2.Create();
                            using (var decryptor = receiverRc2.CreateDecryptor(receiverSharedKey.Take(receiverRc2.KeySize / 8).ToArray(), receiverSharedKey.Take(receiverRc2.BlockSize / 8).ToArray()))
                            {
                                using (var decryptStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                                {
                                    using (var decryptedStream = new MemoryStream(64))
                                    {
                                        // Moving the current position to the start to read the encrypted stream
                                        encryptedStream.Seek(0, SeekOrigin.Begin);
                                        // Decrypting from the encrypted stream
                                        stopwatch.Restart();
                                        decryptStream.CopyTo(decryptedStream);
                                        stopwatch.Stop();
                                        //
                                        Console.Write("Decrypted: ");
                                        WriteBytes(decryptedStream.ToArray(), false);
                                        $"[{this.GetType().Name}] " + );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ConsoleHelper.WriteHightlight($"# Finishing {nameof(ECDiffieHellman)} cryptography");
            $"[{this.GetType().Name}] " + );
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
            EncryptDataRSA(sender, out encryptedBuffer);
            //
            DecryptDataRSA(receiver, encryptedBuffer);
            ConsoleHelper.WriteHightlight($"# Finishing {nameof(RSA)} cryptography");
            $"[{this.GetType().Name}] " + );
        }

        private static void DecryptDataRSA(RSA rsa, byte[] encryptedBuffer)
        {
            stopwatch.Restart();
            var decryptedBuffer = rsa.Decrypt(encryptedBuffer, RSAEncryptionPadding.OaepSHA1);
            stopwatch.Stop();
            Console.Write("Decrypted: ");
            WriteBytes(decryptedBuffer, false);
            $"[{this.GetType().Name}] " + );
        }

        private static void EncryptDataRSA(RSA rsa, out byte[] encryptedBuffer)
        {
            ConsoleHelper.WriteHightlight($"# Starting {nameof(RSA)} cryptography\n");
            // Encrypting data using public key
            stopwatch.Restart();
            encryptedBuffer = rsa.Encrypt(WordBytes, RSAEncryptionPadding.OaepSHA1);
            stopwatch.Stop();
            Console.Write("Encrypted: ");
            WriteBytes(encryptedBuffer);
            $"[{this.GetType().Name}] " + );
        }

        private static void WriteBytes(byte[] buffer, bool asHex = true)
        {
            if (asHex)
            {
                foreach (var @byte in buffer)
                {
                    ConsoleHelper.WriteSuccess(string.Format("{0:X}", @byte));
                }
            }
            else
                ConsoleHelper.WriteSuccess(Encoding.UTF8.GetString(buffer));
            Console.Write($" [{buffer.Length} bytes in {stopwatch.ElapsedTicks} ticks]");
        }
    }
}
