using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace BSK_1_MD
{
    class TcpClients
    {
        #region Private declarations
        private string ip;
        private Int32 port;
        private Logger logger;
        NetworkStream ns;
        public bool ConnectedToServer { get; set; } = false;
        TcpClient tcpClient;
        #endregion
        #region Private methods
        #endregion
        #region Public declarations
        #endregion
        #region Public methods
        public TcpClients(string ip, Int32 port, ref Logger logger)
        {
            this.ip = ip;
            this.port = port;
            this.logger = logger;
        }

        public void Connect()
        {
            try
            {
                logger.addToLogger("[Client] Trying to connect to host: " + ip + " at port: " + port);
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient(ip, port);
                    bool status = tcpClient.Connected;
                    ConnectedToServer = status;
                    logger.addToLogger("[Client] Connection status: " + ConnectedToServer);
                    if (ConnectedToServer)
                    {
                        ns = tcpClient.GetStream();
                    }
                }
            }
            catch (Exception e)
            {
                logger.addToLogger("[Client] " + e.ToString());
            }
        }
        public void SendMessage(string message)
        {
            try
            {
                //var client = new TcpClient(hostName, portNum);
                if (tcpClient != null && ns != null)
                {
                    
                    try
                    {
                           byte[] message_ = Encoding.ASCII.GetBytes(message);
                           ns.Write(message_, 0, message_.Length);
                    }
                    catch (Exception e)
                    {
                        logger.addToLogger("[Client] " + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                logger.addToLogger("[Client] " + e.ToString());
            }
        }
        public void CloseClient()
        {
            if (tcpClient != null)
            {
                if(ns!= null)
                {
                    ns.Close();
                }
                tcpClient.Close();
            }
        }
        #endregion

    }
    class TcpServer
    {
        #region Private declarations
        private Int32 listeningPort = 810;
        private Logger logger;
        private TcpClient tcpClient;
        public bool StopServer { get; set; } = false;
        public bool ServerStarted { get; set; } = false;
        #endregion
        #region Private methods
        #endregion
        #region Public declarations
        #endregion
        #region Public methods
        public TcpServer(Int32 port, ref Logger logger)
        {
            this.listeningPort = port;
            this.logger = logger;
        }

        public void StartServer()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, listeningPort);
                listener.Start();
                ServerStarted = true;
                while (!StopServer)
                {
                    logger.addToLogger("[Server] Waiting for connection...");
                    tcpClient = listener.AcceptTcpClient();
                    if (tcpClient.Connected)
                    {
                        logger.addToLogger("[Server] Connected");
                        StopServer = true;
                    }
                }
                if (StopServer)
                {
                    listener.Stop();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ReciveMessage()
        {
            if(tcpClient != null)
            {
                try
                {
                    if (tcpClient.Connected)
                    {
                        NetworkStream ns = tcpClient.GetStream();

                        byte[] bytes = new byte[1024];
                        int bytesRead = ns.Read(bytes, 0, bytes.Length);
                        if(bytesRead != 0)
                        {
                            logger.addToLogger("[Server] Message: " + Encoding.ASCII.GetString(bytes, 0, bytesRead));
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.addToLogger("[Server] " + e.ToString());
                }
            }
        }
        public void SendFile()
        {

        }
        public void CloseClient()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
        #endregion

    }
}
