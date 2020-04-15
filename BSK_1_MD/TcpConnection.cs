using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int reciveDataSize = 10;
            var result = listener.BeginAccept(null, reciveDataSize, new AsyncCallback(AcceptCallback), listener);
            manualResetEvent.WaitOne();
            logger.addToLogger(string.Format(message, "Connected"));

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            listener = (Socket)ar.AsyncState;
            byte[] Buffer;
            int bytesTransferred;
            Socket handler = listener.EndAccept(out Buffer, out bytesTransferred, ar);
            string stringTransferred = Encoding.ASCII.GetString(Buffer, 0, bytesTransferred);

            Console.WriteLine(stringTransferred);
            Console.WriteLine("Size of data transferred is {0}", bytesTransferred);

            // Create the state object for the asynchronous receive.
        }
    }
}
