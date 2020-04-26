using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BSK_1_MD
{
    class FileToSave
    {
        private static Mutex mutex = new Mutex();
        private string name;
        private UInt32 size;
        private UInt32 appendedSize;
        private UInt32 sizeToAppend;
        private Logger logger;
        private string message = "[FileSave] {0}";
        private FileStream fileStream = null;

        public uint SizeToAppend { get => sizeToAppend; set => sizeToAppend = value; }

        public FileToSave(string name, UInt32 size, ref Logger logger)
        {
            this.name = name;
            this.size = size;
            this.appendedSize = 0;
            this.sizeToAppend = this.size;
            this.logger = logger;
        }

        public void OpenFile(string path)
        {
            try
            {
               fileStream = File.OpenWrite(path + this.name);
            }
            catch(Exception ex)
            {
                logger.addToLogger(string.Format(message, "Error: " + ex.ToString()));
            }

        }

        public void SaveFile()
        {
            fileStream.Close();
        }

        public void AppendBytes( byte[] bytes, UInt32 size)
        {
            if(fileStream != null)
            {
                fileStream.Write(bytes, Convert.ToInt32(appendedSize), Convert.ToInt32(size));
            }
            appendedSize += size;
            SizeToAppend -= size;
        }

    }
}
