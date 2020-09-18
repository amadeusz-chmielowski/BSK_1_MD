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
using System.Timers;
using System.Windows.Forms;

namespace BSK_1_MD
{
    class TcpServer
    {
        private string message = "[Server] {0}";
        Regex preTextRegex = new Regex(".*File (.*), size (.*) being send" + Environment.NewLine + ".*");
        Regex preTextRegex2 = new Regex(".*Session key, size (.*) being send" + Environment.NewLine + ".*");
        private Int32 port;
        private Logger logger;
        private Socket listener;
        private IPEndPoint localEndPoint;
        private FileToSave fileToSave = null;
        private bool useEncryption = false;
        public struct RecivedKey
        {
            public byte[] keyToRecive;
            public UInt32 keySize;
            public bool keyRecived;

        }
        public RecivedKey recivedKey;
        public Cipher cipher = null;
        public string clientIp;
        public string NotSecurePasswd { get; set; }
        private string defaultSavePath = "./";
        private static System.Timers.Timer timer;
        private bool connectionStatus = false;
        private int connectionResetValue = 0;
        private enum messageType
        {
            Text,
            File,
            Key
        };

        private bool savingFile = false;
        private bool recivingKey = false;


        IPAddress ip;
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public bool ServerStarted { get => serverStarted; set => serverStarted = value; }
        public bool ServerConnectedToClient { get => serverConnectedToClient; set => serverConnectedToClient = value; }
        public string DefaultSavePath { get => defaultSavePath; set => defaultSavePath = value; }
        public bool ConnectionStatus { get => connectionStatus; set => connectionStatus = value; }
        public int ConnectionResetValue { get => connectionResetValue; set => connectionResetValue = value; }

        private bool serverStarted = false;
        private bool serverConnectedToClient = false;

        public TcpServer(Int32 port, ref Logger logger)
        {
            this.port = port;
            this.logger = logger;
            if (listener == null)
            {
                LingerOption lingerOption = new LingerOption(false, 0);
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            ip = Helper.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();

        }

        public void SetTimer()
        {
            timer = new System.Timers.Timer(100000);

            timer.Elapsed += OnTimedEvet;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void RestartTimer()
        {
            if (timer != null)
            {
                connectionResetValue = 0;
                if (!timer.Enabled)
                    timer.Start();
            }
        }

        private void OnTimedEvet(object sender, ElapsedEventArgs e)
        {
            if (listener.Available > 0)
            {
                ConnectionResetValue = 0;
            }
            else
            {
                if (ConnectionResetValue >= 10)
                {
                    ConnectionResetValue = 0;
                    ConnectionStatus = false;
                }
                else
                {
                    ConnectionResetValue++;
                    logger.addToLogger(string.Format(message, "Client not sending data, waiting periods left: " + (10 - connectionResetValue)));
                }
            }
        }

        public void StopTimer()
        {
            timer.Stop();
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

        public void CloseAllSocets()
        {
            try
            {

            }
            finally
            {
                listener.Close();
                listener.Dispose();
                listener = null;
            }
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
            serverConnectedToClient = true;
            clientIp = listener.RemoteEndPoint.ToString().Split(':')[0];
            ConnectionStatus = serverConnectedToClient;
            SetTimer();

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
            else if (preTextRegex2.IsMatch(message))
            {
                var match = preTextRegex2.Match(message);
                var keySize = match.Groups[1].Value;
                recivedKey = new RecivedKey();
                recivedKey.keySize = Convert.ToUInt32(keySize);
                recivedKey.keyToRecive = new byte[recivedKey.keySize];
                recivedKey.keyRecived = false;
                return messageType.Key;
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
            else if (recivingKey)
            {
                if (recivedKey.keySize <= Convert.ToUInt32(bytesRecivedSize))
                {
                    Array.Copy(bytes, recivedKey.keyToRecive, bytesRecivedSize);
                    recivedKey.keyRecived = true;
                    recivingKey = false;
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
                    case messageType.Key:
                        {
                            var text = Encoding.UTF8.GetString(bytes);
                            logger.addToLogger(string.Format(message, "Message: " + text));
                            recivingKey = true;
                            break;
                        }
                }

            }
        }

        public bool CheckConnectionStatus()
        {
            return connectionStatus;
        }

        public void Recive()
        {
            byte[] bytes = new byte[Convert.ToUInt32(ConfigurationManager.AppSettings.Get("FrameSize"))];
            if (listener.Available > 0)
            {
                ConnectionResetValue = 0;
                int bytesRecivedSize;
                bytesRecivedSize = listener.Receive(bytes, socketFlags: SocketFlags.None);
                if (useEncryption)
                {
                    byte[] decryptedBytes = cipher.DecryptData(bytes);
                    Reciver(decryptedBytes, bytesRecivedSize);
                }
                else
                {
                    Reciver(bytes, bytesRecivedSize);
                }
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

        public string GenerateRsaKeys()
        {
            string returnPath = "";
            if (cipher != null)
            {
                cipher.GenerateAndSaveEncryptedRsaKeys();
                returnPath = cipher.SavePublicRsaKey();
            }

            return returnPath;
        }

        public bool DeletePublicRsaKey(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        public void UseEncryption(bool yesNo)
        {
            useEncryption = yesNo;
        }
    }
}
