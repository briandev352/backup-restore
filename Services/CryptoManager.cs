namespace Bring2mind.Backup.Restore.Services
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class CryptoManager
    {
        public static void DecryptFile(string inFile, string outFile, string passPhrase, string salt)
        {
            var iterations = 1000;
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            var derivedBytes = new Rfc2898DeriveBytes(passPhrase, saltBytes, iterations);
            byte[] derivedKey = derivedBytes.GetBytes(32); // 256 bits
            byte[] derivedInitVector = derivedBytes.GetBytes(16); // 128 bits

            if (File.Exists(outFile)) return;

            using (var sourceStream = File.OpenRead(inFile))
            using (var destinationStream = File.Create(outFile))
            using (var aesProvider = new AesCryptoServiceProvider()
            {
                KeySize = 256,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC
            })
            using (var cryptoTransform = aesProvider.CreateDecryptor(derivedKey, derivedInitVector))
            using (var cryptoStream = new CryptoStream(sourceStream, cryptoTransform, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(destinationStream);
            }
        }

    }
}
