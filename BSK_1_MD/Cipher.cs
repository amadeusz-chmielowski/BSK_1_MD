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
                this.OnCipherModeValueChanged();
            }
        }
        public SecureString Passwd { get; set; }
        private AesSettings aesSettings;

        protected virtual void OnCipherModeValueChanged()
        {
            throw new NotImplementedException();
        }

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

        public Cipher(ref Logger logger)
        {
            this.logger = logger;
            aes = Aes.Create();
            aesSettings = new AesSettings();
        }


        public string GenerateRsaKeys()
        {
            throw new NotImplementedException();
        }

        private bool EncryptRsaKeys()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void SetAesSettings()
        {
            aes.BlockSize = aesSettings.BlockSize;
            aes.FeedbackSize = aesSettings.FeedBackSize;
            aes.IV = aesSettings.IV;
            aes.Key = aesSettings.SessionKey;
            aes.KeySize= aesSettings.KeySize;
            aes.Mode= aesSettings.CipherMode;
            aes.Padding= aesSettings.PaddingMode;
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

        private void DeserializeAesSettings(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();
            using ( MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Read(data, 0, data.Length);
                this.aesSettings = (AesSettings)formatter.Deserialize(memoryStream);
            }

        }
    }
}
