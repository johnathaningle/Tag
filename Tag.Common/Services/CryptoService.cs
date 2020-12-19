using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tag.Common.Services
{
    public class CryptoService
    {
        // Todo - Make the keys configurable
        private readonly byte[] Key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private readonly byte[] Vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };
        private const int Salt = 8;

        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private System.Text.UTF8Encoding UTFEncoder;

        public CryptoService()
        {
            //This is our encryption method
            RijndaelManaged rm = new RijndaelManaged();

            //Create an encryptor and a decryptor using our encryption method, key, and vector.
            EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
            DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        //Encrypt some text and return a string suitable for passing in a URL
        public string EncryptString(string TextValue)
        {
            return (TextValue != "") ? Convert.ToBase64String(Encrypt(TextValue)) : "";
        }

        //Encrypt some text and return an encrypted byte array.
        public byte[] Encrypt(string TextValue)
        {
            //Translates our text value into a byte array.
            Byte[] pepper = UTFEncoder.GetBytes(TextValue);
            // add salt
            Byte[] salt = new byte[Salt];
            var crypto = new RNGCryptoServiceProvider();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
            crypto.GetNonZeroBytes(salt);
            //the bytes variable stores the encrypted byte array
            Byte[] bytes = new byte[2 * Salt + pepper.Length];
            //the code below copies specific bytes from the salt/pepper into the encrtypted byte array
            System.Buffer.BlockCopy(salt, 0, bytes, 0, Salt);
            System.Buffer.BlockCopy(pepper, 0, bytes, Salt, pepper.Length);
            crypto.GetNonZeroBytes(salt);
            System.Buffer.BlockCopy(salt, 0, bytes, Salt + pepper.Length, Salt);
            //Used to stream the data in and out of the CryptoStream.
            var memoryStream = new MemoryStream();
            //We will have to write the unencrypted bytes to the stream,
            var cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            //Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            // Clean up.
            cs.Close();
            memoryStream.Close();
            //return the encrypted byte array of the source text (this can be converted back to a string)
            return encrypted;
        }

        //The other side: Decryption methods
        public string DecryptString(string EncryptedString)
        {
            return (EncryptedString != "") ? Decrypt(Convert.FromBase64String(EncryptedString)) : "";
        }

        //Decryption when working with byte arrays
        public string Decrypt(byte[] EncryptedValue)
        {
            //Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
            decryptStream.FlushFinalBlock();

            //Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            // remove salt
            int len = decryptedBytes.Length - 2 * Salt;
            Byte[] pepper = new Byte[len];
            System.Buffer.BlockCopy(decryptedBytes, Salt, pepper, 0, len);
            return UTFEncoder.GetString(pepper);
        }
    }
}