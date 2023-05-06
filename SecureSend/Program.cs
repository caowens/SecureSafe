using System;
using System.IO;
using System.Security.Cryptography;

class EncryptionTool
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: encryptiontool [encrypt/decrypt] [input file path] [output file path]");
            return;
        }

        string command = args[0];
        string inputFile = args[1];
        string outputFile = args[2];

        byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

        using (var aes = Aes.Create())
        {
            // aes.Key = key;
            // aes.IV = iv;

            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();

            if (command == "encrypt")
            {
                Encrypt(inputFile, outputFile, aes);
            }
            else if (command == "decrypt")
            {
                Decrypt(inputFile, outputFile, aes);
            }
            else
            {
                Console.WriteLine("Invalid command.");
                return;
            }
        }
    }

    static void Encrypt(string inputFile, string outputFile, SymmetricAlgorithm algorithm)
    {
        using (var inputStream = new FileStream(inputFile, FileMode.Open))
        {
            using (var outputStream = new CryptoStream(new FileStream(outputFile, FileMode.Create), algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                inputStream.CopyTo(outputStream);
            }
        }
    }

    static void Decrypt(string inputFile, string outputFile, SymmetricAlgorithm algorithm)
    {
        using (var inputStream = new CryptoStream(new FileStream(inputFile, FileMode.Open), algorithm.CreateDecryptor(), CryptoStreamMode.Read))
        {
            using (var outputStream = new FileStream(outputFile, FileMode.Create))
            {
                inputStream.CopyTo(outputStream);
            }
        }
    }
}
