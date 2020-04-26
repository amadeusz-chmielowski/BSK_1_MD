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

namespace BSK_1_MD
{

    class TcpClients
    {
        private string message = "[Client] {0}";
        private string ip;
        private Int32 port;
        private Logger logger;

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

        public TcpClients(string ip, Int32 port, ref Logger logger)
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

        public void SendMessage(string message)
        {
            var msg = ConvertToBytes(message);
            logger.addToLogger(string.Format(message, "Sending message" + message));
            Send(msg, "text");
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
    class TcpServer
    {
        private string message = "[Server] {0}";
        Regex preTextRegex = new Regex(".*File (.*), size (.*) being send" + Environment.NewLine + ".*");
        private Int32 port;
        private Logger logger;
        private Socket listener;
        private IPEndPoint localEndPoint;
        private FileToSave fileToSave = null;
        private string defaultSavePath = "./";
        private enum messageType
        {
            Text,
            File
        };

        private bool savingFile = false;


        IPAddress ip;
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public bool ServerStarted { get => serverStarted; set => serverStarted = value; }
        public string DefaultSavePath { get => defaultSavePath; set => defaultSavePath = value; }

        private bool serverStarted = false;

        public TcpServer(Int32 port, ref Logger logger)
        {
            this.port = port;
            this.logger = logger;
            if (listener == null)
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        }

        public void StartServer()
        {
            serverStarted = true;
            if (localEndPoint == null)
            {
                localEndPoint = new IPEndPoint(ip, port);
            }
            listener.Bind(localEndPoint);
            listener.Listen(100);

            manualResetEvent.Reset();
            logger.addToLogger(string.Format(message, "Waiting for connection..."));
            var result = listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            manualResetEvent.WaitOne();
            logger.addToLogger(string.Format(message, "Connected"));

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var listener_ = (Socket)ar.AsyncState;
            listener = listener_.EndAccept(ar);
            if (listener.Connected)
            {
                manualResetEvent.Set();
            }
        }

        private messageType TextOrFileDetermination(byte[] bytes, ref FileToSave fileToSave)
        {
            var message = Encoding.UTF8.GetString(bytes);
            if (preTextRegex.IsMatch(message))
            {
                var match = preTextRegex.Match(message);
                var fileName = match.Groups[1].Value;
                var fileSize = Convert.ToUInt32(match.Groups[2].Value);
                fileToSave = new FileToSave(fileName, fileSize, ref logger);
                UpdateSavePath(this.defaultSavePath);
                fileToSave.OpenFile();
                return messageType.File;
            }
            else
            {
                return messageType.Text;
            }
        }

        private void Reciver(byte[] bytes, int bytesRecivedSize)
        {
            if (savingFile)
            {
                if (fileToSave.SizeToAppend < Convert.ToUInt32(bytesRecivedSize))
                {
                    byte[] vs = new byte[256];
                    int newSize = (bytesRecivedSize - Convert.ToInt32(fileToSave.SizeToAppend));
                    Array.Copy(bytes, fileToSave.SizeToAppend, vs, 0, newSize);
                    fileToSave.AppendBytes(bytes, fileToSave.SizeToAppend);
                    fileToSave.SaveFile();
                    savingFile = false;
                    Reciver(vs, newSize);
                }
                else
                {
                    fileToSave.AppendBytes(bytes, Convert.ToUInt32(bytesRecivedSize));
                    savingFile = true;
                }

            }
            else
            {
                switch (TextOrFileDetermination(bytes, ref fileToSave))
                {
                    case messageType.Text:
                        {
                            var text = Encoding.UTF8.GetString(bytes);
                            logger.addToLogger(string.Format(message, "Message: " + text));
                            break;
                        }
                    case messageType.File:
                        {
                            var text = Encoding.UTF8.GetString(bytes);
                            logger.addToLogger(string.Format(message, "Message: " + text));
                            savingFile = true;
                            break;
                        }
                }

            }
        }

        public void Recive()
        {
            byte[] bytes = new byte[Convert.ToUInt32(ConfigurationManager.AppSettings.Get("FrameSize"))];
            if (listener.Available > 0)
            {
                int bytesRecivedSize;
                bytesRecivedSize = listener.Receive(bytes, socketFlags: SocketFlags.None);
                Reciver(bytes, bytesRecivedSize);
            }

        }

        private void UpdateSavePath(string path)
        {
            try
            {
                if (this.fileToSave != null)
                {
                    this.fileToSave.PathToSave = path;
                }
            }
            catch (Exception ex)
            {
                logger.addToLogger(string.Format(message, "Error: " + ex.Message));
            }
        }
    }
}
