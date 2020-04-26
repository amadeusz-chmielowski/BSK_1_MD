using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BSK_1_MD
{
    class FileToRead
    {
        private static Mutex mutex = new Mutex();
        private string name;
        private UInt32 fileSize;
        private UInt32 readSize;
        private UInt32 sizeToRead;
        private Logger logger;
        private string message = "[FileRead] {0}";
        private FileStream fileStream = null;

        public uint SizeToRead { get => sizeToRead; set => sizeToRead = value; }

        public FileToRead(string name, UInt32 fileSize, ref Logger logger)
        {
            this.name = name;
            this.fileSize = fileSize;
            this.readSize = 0;
            this.sizeToRead = this.fileSize;
            this.logger = logger;
        }

        public void OpenFile()
        {
            try
            {
                fileStream = File.OpenRead(this.name);
            }
            catch (Exception ex)
            {
                logger.addToLogger(string.Format(message, "Error: " + ex.ToString()));
            }

        }

        public void StopReading()
        {
            fileStream.Close();
        }

        public byte[] ReadBytes()
        {
            if (fileStream != null)
            {
                byte[] readedbytes = new byte[Convert.ToUInt32(ConfigurationManager.AppSettings.Get("FrameSize"))];
                int sizeRead = fileStream.Read(readedbytes, Convert.ToInt32(readSize), readedbytes.Length);
                readSize += Convert.ToUInt32(sizeRead);
                sizeToRead -= Convert.ToUInt32(sizeRead);
                return readedbytes;
            }
            else
            {
                return null;
            }

        }
    }
}
