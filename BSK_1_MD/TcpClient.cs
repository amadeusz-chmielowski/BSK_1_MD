using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;

namespace BSK_1_MD
{

    class TcpClient
    {
        private string message = "[Client] {0}";
        private string ip;
        private Int32 port;
        private Logger logger;
        private Cipher cipher;
        public CipherMode cipherMode { get; set; }

        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private Socket socket;
        private EndPoint ipEndPoint;
        private bool connectionAquired = false;
        private int progressValue = 0;
        private bool fileSent = false;


        public bool ConnectionEstablished { get => connectionAquired; }
        public int ProgressValue { get => progressValue; set => progressValue = value; }
        public bool FileSent { get => fileSent; set => fileSent = value; }

        private FileToRead fileToRead = null;

        public TcpClient(string ip, Int32 port, ref Logger logger)
        {
            this.ip = ip;
            this.port = port;
            this.logger = logger;

            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                logger.addToLogger(string.Format(message, "Creating client socket"));
            }
            if (ipEndPoint == null)
            {
                ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            cipher = new Cipher(ref logger);
            //to do 
            //cipher.GenerateAndSaveEncryptedRsaKeys();
        }

        public void CleanAndCloseConnections()
        {
            socket.Close();

        }

        public void UpdateCipher()
        {
            cipher.CipherMode = cipherMode;
            cipher.UpdateAes();
        }

        public void Connect()
        {
            manualResetEvent.Reset();
            logger.addToLogger(string.Format(message, "Establishing connection to " + ip));
            IAsyncResult result = socket.BeginConnect(ipEndPoint, new AsyncCallback(ConnectionAquired), socket);
            manualResetEvent.WaitOne(10000);
            if (socket.Connected)
            {
                logger.addToLogger(string.Format(message, "Connection established"));
                connectionAquired = true;
            }
            else
            {
                try
                {
                    socket.EndConnect(result);
                }
                catch
                {
                    logger.addToLogger(string.Format(message, "Connection timeout"));
                }
            }
        }

        public bool CheckConnectionStatus()
        {
            try
            {
                bool part1 = socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = socket.Available == 0;
                if (part1 && part2)
                {
                    if (ConnectionEstablished)
                    {
                        CleanAndCloseConnections();
                    }
                    return false;
                }
                else
                    return true;
            }
            catch(Exception)
            {
                CleanAndCloseConnections();
                logger.addToLogger(string.Format(message, "Server terminated connection"));
                return false;
            }
        }

        public void Updatevariables(string ip, Int32 port)
        {
            this.ip = ip;
            this.port = port;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        private void ConnectionAquired(IAsyncResult ar)
        {
            try
            {
                socket = (Socket)ar.AsyncState;
                if (socket.Connected)
                {
                    manualResetEvent.Set();
                    socket.EndConnect(ar);
                }
            }
            catch (Exception e)
            {
                logger.addToLogger(string.Format(message, e.Message));
            }
        }

        private byte[] ConvertToBytes(string text)
        {
            byte[] msg;
            switch (ConfigurationManager.AppSettings.Get("encoding"))
            {
                case "UTF8":
                    msg = Encoding.UTF8.GetBytes(text);
                    break;
                case "UTF32":
                    msg = Encoding.UTF32.GetBytes(text);
                    break;
                case "ASCII":
                    msg = Encoding.ASCII.GetBytes(text);
                    break;
                default:
                    msg = Encoding.Default.GetBytes(text);
                    break;
            }
            return msg;
        }

        public void SendMessage(string message_)
        {
            try
            {


                Regex preTextRegex = new Regex(".*File (.*), size (.*) being send" + Environment.NewLine + ".*");
                if (preTextRegex.IsMatch(message_))
                {
                    throw new System.ArgumentException("Text contains forbidden message " + preTextRegex.ToString());
                }
                var msg = ConvertToBytes(message_);
                logger.addToLogger(string.Format(message, "Sending message" + message));
                Send(msg, "text");
            }
            catch (Exception ex)
            {
                logger.addToLogger(string.Format(message, "Error " + ex.Message));
            }
        }

        public void SendFile(string filePath, long size)
        {
            logger.addToLogger(string.Format(message, "Sending file:" + filePath));
            Send(null, "file", file: filePath, size: size);
        }

        private void Send(byte[] bytes = null, string key_ = null, string file = null, long size = 0)
        {
            bool correct_key = false;
            string correct_keys = "";
            try
            {
                foreach (string key in ConfigurationManager.AppSettings.AllKeys)
                {
                    correct_keys += key + ", ";
                    if (key_ == key)
                    {
                        correct_key = true;
                        break;
                    }
                }
                if (!correct_key)
                {
                    throw new System.ArgumentException("Wrong key, correct keys: " + correct_keys, "key");
                }
            }
            catch (Exception ex)
            {
                logger.addToLogger(string.Format(message, ex.Message));
            }
            switch (key_)
            {
                case "text":
                    try
                    {
                        if (bytes == null)
                        {
                            throw new System.ArgumentNullException("Message empty");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.addToLogger(string.Format(message, ex.Message));
                    }
                    int bytesSent = socket.Send(bytes, SocketFlags.None);
                    logger.addToLogger(string.Format(message, "Sent " + bytesSent + " bytes."));
                    break;
                case "file":
                    try
                    {
                        if (file == null || size == 0)
                        {
                            throw new System.ArgumentNullException("File or size is not given", "file, size");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.addToLogger(string.Format(message, ex.Message));
                    }
                    fileToRead = new FileToRead(file, Convert.ToUInt32(size), ref logger);
                    fileToRead.OpenFile();
                    string preText = "File {0}, size {1} being send" + Environment.NewLine;
                    var preBuffer = ConvertToBytes(string.Format(preText, Path.GetFileName(file), size));
                    byte[] preBufferCorrectSize = new byte[Convert.ToUInt32(ConfigurationManager.AppSettings.Get("FrameSize"))];
                    Array.Copy(preBuffer, preBufferCorrectSize, preBuffer.Length);
                    string postText = "File {0} sent";
                    var postBuffer = ConvertToBytes(string.Format(postText, Path.GetFileName(file)));
                    byte[] postBufferCorrectSize = new byte[Convert.ToUInt32(ConfigurationManager.AppSettings.Get("FrameSize"))];
                    Array.Copy(postBuffer, postBufferCorrectSize, postBuffer.Length);
                    socket.Send(preBufferCorrectSize);
                    Thread.Sleep(10);
                    while (fileToRead.SizeToRead > 0)
                    {
                        byte[] bytesToSend = fileToRead.ReadBytes();
                        socket.Send(buffer: bytesToSend, size: Convert.ToInt32(ConfigurationManager.AppSettings.Get("FrameSize")), socketFlags: SocketFlags.None);
                        progressValue = Convert.ToInt32((size - fileToRead.SizeToRead) / Convert.ToDouble(size) * Convert.ToDouble(ConfigurationManager.AppSettings.Get("ProgressBarMax")));
                    }
                    Thread.Sleep(20);
                    socket.Send(postBufferCorrectSize);
                    logger.addToLogger(string.Format(message, "Sent " + file));
                    fileSent = true;
                    break;
            }
        }
    }

}
