using System;

namespace CryptoAndHash
{
    class Program
    {
        static void Main(string[] args)
        {
            // Hashes provide a single way to transform data, which can be used in password, file checksum...
            Hash.Run();
            Console.WriteLine();
            // Keyed hashes provide a single way to transform data using a single secret key, which can be used to verify authenticity and data integrity. In this case, the generated hash (also called MAC) must be supplied with the original message, it is not useful to hide the message content
            KeyedHash.Run();
            Console.WriteLine();
            // Symmetric crypto provide two ways to transform data using a single secret key, which can be used to encrypt and decrypt data, providing a safe way between two parts to communicate. This is not safe because the key must be shared, if it's captured, the whole process is compromised.
            SymmetricCrypto.Run();
            // Asymmetric crypto [fill it here]
            
        }
    }
}
