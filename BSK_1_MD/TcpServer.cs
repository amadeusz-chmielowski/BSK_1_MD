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

namespace BSK_1_MD
{
    class TcpServer
    {
        private string message = "[Server] {0}";
        Regex preTextRegex = new Regex(".*File (.*), size (.*) being send" + Environment.NewLine + ".*");
        private Int32 port;
        private Logger logger;
        private Socket listener;
        private IPEndPoint localEndPoint;
        private FileToSave fileToSave = null;
        private Cipher cipher;
        public string NotSecurePasswd { get; set;}
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
            ip = Helper.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();

        }

        public void Setcipher()
        {
            cipher = new Cipher(ref logger);
            SecureString secureString = new SecureString();
            string pass = "1234";
            if (NotSecurePasswd == null)
            {
            }
            else
            {
                pass = NotSecurePasswd;
            }
            logger.addToLogger(string.Format(message, "Setting password of RSA keys to " + pass));
            for (int i = 0; i < pass.Length; i++)
            {
                secureString.AppendChar(pass[i]);
            }
            cipher.Passwd = secureString;
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
            logger.addToLogger(string.Format(message, string.Format("Connected to : {0}", listener.RemoteEndPoint.ToString())));

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
