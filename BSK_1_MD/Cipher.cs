using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BSK_1_MD
{
    class Cipher
    {
        private Logger logger;
        private readonly string message = "[Cipher] {0}";
        public CipherMode CipherMode
        {
            get
            {
                return CipherMode;
            }
            set
            {
                CipherMode = value;
            }
        }
        public SecureString Passwd { get; set; }
        private AesSettings aesSettings;
        private RSA my_keys;
        private RSA other_keys;
        private EncryptedRSA encryptedKeys;

        private Aes aes;

        [Serializable]
        private struct AesSettings : ISerializable
        {
            public int BlockSize { get; set; }
            public int FeedBackSize { get; set; }
            public int KeySize { get; set; }
            public CipherMode CipherMode { get; set; }
            public PaddingMode PaddingMode { get; set; }
            public byte[] SessionKey { get; set; }
            public byte[] IV { get; set; }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("BlockSize", BlockSize, typeof(int));
                info.AddValue("FeedBackSize", FeedBackSize, typeof(int));
                info.AddValue("KeySize", KeySize, typeof(int));
                info.AddValue("CipherMode", CipherMode, typeof(CipherMode));
                info.AddValue("PaddingMode", PaddingMode, typeof(PaddingMode));
                info.AddValue("SessionKey", SessionKey, typeof(byte[]));
                info.AddValue("IV", IV, typeof(byte[]));
            }

            public AesSettings(SerializationInfo info, StreamingContext context)
            {
                BlockSize = (int)info.GetValue("BlockSize", typeof(int));
                FeedBackSize = (int)info.GetValue("FeedBackSize", typeof(int));
                KeySize = (int)info.GetValue("KeySize", typeof(int));
                CipherMode = (CipherMode)info.GetValue("CipherMode", typeof(CipherMode));
                PaddingMode = (PaddingMode)info.GetValue("PaddingMode", typeof(PaddingMode));
                SessionKey = (byte[])info.GetValue("SessionKey", typeof(byte[]));
                IV = (byte[])info.GetValue("IV", typeof(byte[]));
            }
        }

        private struct RSA
        {
            public RSAParameters PrvKey { get; set; }
            public RSAParameters PubKey { get; set; }
            public string PrvKey_s { get; set; }
            public string PubKey_s { get; set; }
        }

        private struct EncryptedRSA
        {
            public byte[] PrvKey { get; set; }
            public byte[] PubKey { get; set; }
        }

        public Cipher(ref Logger logger)
        {
            this.logger = logger;
            aes = Aes.Create();
            aesSettings = new AesSettings();
            DefaultAesSettings();
            my_keys = new RSA();
            encryptedKeys = new EncryptedRSA();

        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static string ExportPrivateKey(RSACryptoServiceProvider csp)
        {
            StringWriter outputStream = new StringWriter();
            if (csp.PublicOnly) throw new ArgumentException("CSP does not contain a private key", "csp");
            var parameters = csp.ExportParameters(true);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                    EncodeIntegerBigEndian(innerWriter, parameters.D);
                    EncodeIntegerBigEndian(innerWriter, parameters.P);
                    EncodeIntegerBigEndian(innerWriter, parameters.Q);
                    EncodeIntegerBigEndian(innerWriter, parameters.DP);
                    EncodeIntegerBigEndian(innerWriter, parameters.DQ);
                    EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write("-----BEGIN RSA PRIVATE KEY-----\n");
                // Output as Base64 with lines chopped at 64 characters
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write("-----END RSA PRIVATE KEY-----");
            }

            return outputStream.ToString();
        }

        private static string ExportPublicKey(RSACryptoServiceProvider csp)
        {
            StringWriter outputStream = new StringWriter();
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write("-----BEGIN PUBLIC KEY-----\n");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write("-----END PUBLIC KEY-----");
            }

            return outputStream.ToString();
        }

        private void DefaultAesSettings()
        {
            aesSettings.BlockSize = 128;
            aesSettings.FeedBackSize = 128;
            aesSettings.KeySize = 256;
            aesSettings.CipherMode = CipherMode.ECB;
            aesSettings.PaddingMode = PaddingMode.None;
            aesSettings.IV = aes.IV;
            aesSettings.SessionKey = aes.Key;
            SetAesSettings();
        }

        private RSA GenerateRsaKeys()
        {
            RSA rsa = new RSA();
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            rsa.PrvKey = csp.ExportParameters(true);
            rsa.PubKey = csp.ExportParameters(false);
            rsa.PrvKey_s = ExportPrivateKey(csp);
            rsa.PubKey_s = ExportPublicKey(csp);
            return rsa;
        }

        private void EncryptRsaKeys()
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] shaPasswd = hash.ComputeHash(Encoding.UTF8.GetBytes(Passwd.ToString()));
                AesManaged aesManaged = new AesManaged();
                byte[] iv = new byte[16];
                Array.Copy(shaPasswd, iv, shaPasswd.Length / 2);
                ICryptoTransform crypto = aesManaged.CreateEncryptor(shaPasswd, iv);
                MemoryStream memoryStream = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {

                        sw.Write(my_keys.PrvKey_s);
                    }
                    encryptedKeys.PrvKey = memoryStream.ToArray();
                }
                memoryStream = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(my_keys.PubKey_s);
                    }
                    encryptedKeys.PubKey = memoryStream.ToArray();
                }
            }
        }

        private static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private void SaveRsaEncryptedKeys()
        {
            string rsaPubPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            rsaPubPath += "/rsa/.rsaPub/rsaPub";
            rsaPubPath = NormalizePath(rsaPubPath);
            string rsaPrivPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            rsaPrivPath += "/rsa/.rsaPriv/rsaPriv";
            rsaPrivPath = NormalizePath(rsaPrivPath);

            Directory.CreateDirectory(Path.GetDirectoryName(rsaPubPath));
            Directory.CreateDirectory(Path.GetDirectoryName(rsaPrivPath));

            using (FileStream fs = File.Create(rsaPubPath))
            {
                
                fs.Write(encryptedKeys.PubKey, 0, encryptedKeys.PubKey.Length);
            }
            using (FileStream fs = File.Create(rsaPrivPath))
            {
                fs.Write(encryptedKeys.PrvKey, 0, encryptedKeys.PrvKey.Length);
            }

        }

        public void GenerateAndSaveEncryptedRsaKeys()
        {
            my_keys = GenerateRsaKeys();
            EncryptRsaKeys();
            SaveRsaEncryptedKeys();

        }

        public string SavePublicRsaKey()
        {
            string path = @"c:\temp\";
            string fileName = path + "public_key.rsa";
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Dir exist");
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // This text is added only once to the file.
            if (!File.Exists(fileName))
            {
                if (my_keys.PubKey_s.Length > 0)
                {
                    File.WriteAllText(fileName, my_keys.PubKey_s);
                    return fileName;
                }
            }
            else
            {
                File.Delete(fileName);
                if (my_keys.PubKey_s.Length > 0)
                {
                    File.WriteAllText(fileName, my_keys.PubKey_s);
                    return fileName;
                }
            }
            return fileName;

        }

        private void EncryptAesSettings()
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateSessionKey()
        {
            using (SHA256 hash = SHA256.Create())
            {
                long key = System.DateTime.Now.ToBinary();
                aesSettings.SessionKey = hash.ComputeHash(BitConverter.GetBytes(key));
            }
            return aesSettings.SessionKey;
        }

        private void SetAesSettings()
        {
            aes.BlockSize = aesSettings.BlockSize;
            aes.FeedbackSize = aesSettings.FeedBackSize;
            aes.IV = aesSettings.IV;
            aes.Key = aesSettings.SessionKey;
            aes.KeySize = aesSettings.KeySize;
            aes.Mode = aesSettings.CipherMode;
            aes.Padding = aesSettings.PaddingMode;
        }

        private byte[] SerializeAesSettings()
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, aesSettings);
                    byte[] serializedAesSettings = memoryStream.ToArray();
                    return serializedAesSettings;
                }

            }
            catch (Exception ex)
            {
                logger.addToLogger(string.Format(message, ex.Message));
                return null;
            }
        }

        private AesSettings DeserializeAesSettings(byte[] data)
        {
            AesSettings aesSettings_ = new AesSettings();
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Read(data, 0, data.Length);
                aesSettings_ = (AesSettings)formatter.Deserialize(memoryStream);
            }
            return aesSettings_;
        }

        private void DecryptWithRsaPrivKey(byte[] data)
        {
            //decrypt data
            byte[] decryptedData = null;
            this.aesSettings = DeserializeAesSettings(decryptedData);
        }

        public void UpdateAes()
        {
            SetAesSettings();
        }
    }
}
