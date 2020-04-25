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
        private byte[] bytes;
        private UInt32 appendedSize;
        private UInt32 sizeToAppend;

        public uint SizeToAppend { get => sizeToAppend; set => sizeToAppend = value; }

        public FileToSave(string name, UInt32 size)
        {
            this.name = name;
            this.size = size;
            this.bytes = new byte[size];
            this.appendedSize = 0;
            this.sizeToAppend = this.size;
        }

        public void SaveFile(string path)
        {
            File.WriteAllBytes(path + this.name, this.bytes);
        }

        public void AppendBytes( byte[] bytes, UInt32 size)
        {
            for(int i =0; i < size; i++)
            {
                this.bytes.Append(bytes[i]);
            }
            SizeToAppend -= size;
        }

    }
}
