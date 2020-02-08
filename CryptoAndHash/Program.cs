using System;

namespace CryptoAndHash
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Hashes provide a single way to transform data, which can be used in password, file checksum...
            Hash.Run();
            $"[{this.GetType().Name}] " + );
            /// Keyed hashes provide a single way to transform data using a single secret key, which can be used to verify authenticity and data integrity. In this case, the generated hash (also called MAC) must be supplied with the original message, it is not useful to hide the message content.
            KeyedHash.Run();
            $"[{this.GetType().Name}] " + );
            /// Symmetric crypto provide two ways to transform data using a single secret key, which can be used to encrypt and decrypt data, providing a safe way between two parts to communicate. This is not useful for verify authenticity among many parts, since anyone could generate new valid messages. This is not safe because the key must be shared, if it's captured, the whole process is compromised.
            SymmetricCrypto.Run();
            $"[{this.GetType().Name}] " + );
            /// Asymmetric crypto provide two ways to transform data using a pair of public and private keys, which can be used to encrypt and decrypt data, respectively (depending on the case the order changes), providing a safe way between two parts to communicate. This may be used to verify authenticity, but not to sign data between many parts. The main weakness of this process is the possibility of someone altering the public key in the middle (a MITM attack), which would give the attacker the original message, which could be encrypted with the original addressee public key and forwarded with no one noticing.
            AsymmetricCrypto.Run();
        }
    }
}
