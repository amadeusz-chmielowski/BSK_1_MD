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

        public bool ConnectionEstablished { get => connectionAquired;}

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
                logger.addToLogger(string.Format(message, e.ToString()));
            }
        }

        private byte[] ConvertToBytes(string text)
        {
            byte[] msg;
            switch (ConfigurationManager.AppSettings.Get("encoding"))
            {
                case "UTF8":
                    msg = Encoding.UTF8.GetBytes(message);
                    break;
                case "UTF32":
                    msg = Encoding.UTF32.GetBytes(message);
                    break;
                case "ASCII":
                    msg = Encoding.ASCII.GetBytes(message);
                    break;
                default:
                    msg = Encoding.Default.GetBytes(message);
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

        private void Send(byte[] bytes=null, string key_ = null, string file = null, long size = 0)
        {
            bool correct_key = false;
            string correct_keys = "";
            try
            {
                foreach(string key in ConfigurationManager.AppSettings.AllKeys)
                {
                    correct_keys += key + ", ";
                    if( key_ == key)
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
            catch(Exception ex)
            {
                logger.addToLogger(string.Format(message, ex.ToString()));
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
                        logger.addToLogger(string.Format(message, ex.ToString()));
                    }
                    int bytesSent = socket.Send(bytes, SocketFlags.None);
                    logger.addToLogger(string.Format(message, "Sent " + bytesSent + " bytes."));
                    break;
                case "file":
                    try
                    {
                        if(file ==null || size == 0)
                        {
                            throw new System.ArgumentNullException("File or size is not given", "file, size");
                        }
                    }
                    catch( Exception ex)
                    {
                        logger.addToLogger(string.Format(message, ex.ToString()));
                    }
                    string preText = "File {0}, size {1} being send";
                    var preBuffer = ConvertToBytes(string.Format(preText, file, size));
                    string postText = "File {0} sent";
                    var postBuffer = ConvertToBytes(string.Format(postText, file));
                    //toDo if size is big split file
                    socket.SendFile(file, preBuffer, postBuffer, TransmitFileOptions.UseDefaultWorkerThread);
                    logger.addToLogger(string.Format(message, "Sent " + file));
                    break;
            }
        }
    }
    class TcpServer
    {
        private string message = "[Server] {0}";
        private Int32 port;
        private Logger logger;
        private Socket listener;
        private IPEndPoint localEndPoint;


        IPAddress ip;
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public bool ServerStarted { get => serverStarted; set => serverStarted = value; }
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

        public void Recive()
        {
            byte[] bytes = new byte[256];
            if(listener.Available > 0)
            {
                int bytesRecivedSize = listener.Receive(bytes, socketFlags: SocketFlags.None);
                logger.addToLogger(string.Format(message, "Message:" + Encoding.UTF8.GetString(bytes)));
            }
            
        }
    }
}
