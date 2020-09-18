using MaterialSkin.Controls;
using MaterialSkin.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Configuration;
using System.Threading;

namespace BSK_1_MD
{
    public partial class BSK : MaterialForm
    {
        private string[] cipherBlocks = new[] { "ECB", "OBC", "CFB", "OFB" };
        private string tcpCipherMode;
        private Logger logger;
        private TcpClient tcpClient;
        private string tcpPasswd;
        private TcpServer tcpServer;
        private IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        private IPAddress ipAddress = null;
        private string fileName = null;
        private long fileSize = 0;
        private bool fileOk = false;
        private bool sendingFile = false;
        private string pathToSave = "";
        private bool restartServer = false;
        bool reciveData = true;
        private string clientIp = null;
        private Int32 clientPort = 0;
        private struct Role
        {
            public bool serverMainRole;
            public bool clientMainRole;
        }
        Role role = new Role();


        public BSK()
        {
            InitializeComponent();
            cipherCheckedListBox.Items.AddRange(cipherBlocks);
            cipherCheckedListBox.SetItemChecked(0, true);
            logger = new Logger();
            loggerWorker.RunWorkerAsync();
            copyConsoleWorker.RunWorkerAsync();
            ipAddress = Helper.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();
            savePathLabel.Text = "Ip: " + ipAddress + Environment.NewLine +
                "Path to save files: " + System.IO.Directory.GetCurrentDirectory();
            this.pathToSave = System.IO.Directory.GetCurrentDirectory();
            progressBarWorker.WorkerSupportsCancellation = true;
            progressBar1.Maximum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ProgressBarMax"));
            serverStartButton.Enabled = false;
            
            role.clientMainRole = false;
            role.serverMainRole = false;
        }

        private void fileSelectButton_Click(object sender, EventArgs e)
        {
            selectFileDialog.ShowDialog();
            try
            {
                string fileName = selectFileDialog.FileName;
                this.fileName = fileName;
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Length > UInt32.MaxValue - 1)
                {
                    throw new System.ApplicationException("File to big > 4GB");
                }
                double fileSize = fileInfo.Length / Math.Pow(10, 6);
                this.fileSize = fileInfo.Length;
                logger.addToLogger("[WFA] " + "File: " + fileName + " Size: " + fileSize + " MB");
                sendFileButton.Enabled = true;
            }
            catch (Exception error)
            {
                logger.addToLogger("[WFA] " + error.Message);
            }
        }
        private void cipherCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && cipherCheckedListBox.CheckedItems.Count > 0)
            {
                cipherCheckedListBox.ItemCheck -= cipherCheckedListBox_ItemCheck;
                cipherCheckedListBox.SetItemChecked(cipherCheckedListBox.CheckedIndices[0], false);
                cipherCheckedListBox.ItemCheck += cipherCheckedListBox_ItemCheck;
                tcpCipherMode = cipherCheckedListBox.SelectedItem.ToString();
                if (tcpClient != null)
                {
                    //update tcp cipher modes
                    switch (tcpCipherMode)
                    {
                        case "ECB":
                            tcpClient.cipherMode = System.Security.Cryptography.CipherMode.ECB;
                            break;
                        case "OBC":
                            tcpClient.cipherMode = System.Security.Cryptography.CipherMode.CBC;
                            break;
                        case "CFB":
                            tcpClient.cipherMode = System.Security.Cryptography.CipherMode.CFB;
                            break;
                        case "OFB":
                            tcpClient.cipherMode = System.Security.Cryptography.CipherMode.OFB;
                            break;

                    }
                    tcpClient.UpdateCipher();

                }
            }
        }
        private void startConnections(bool startNewConnection = false)
        {
            string ip = ipBox.Text;
            if(clientIp == null)
            {
                clientIp = ip;
            }
            if (Helper.ValidateIpV4Address(ip))
            {
                ChangeVisibilityConnectButton(false);
                Int32 port = Convert.ToInt32(portBox.Text);
                if(clientPort == 0)
                {
                    clientPort = port;
                }
                if (!startNewConnection)
                {
                    if (tcpClient == null)
                    {
                        tcpClient = new TcpClient(clientIp, clientPort, ref logger);
                    }
                    tcpClient.Updatevariables(clientIp, clientPort);
                    tcpClient.Connect();
                    if (!tcpClient.ConnectionEstablished)
                    {
                        ChangeVisibilityConnectButton(true);
                    }
                    else 
                    {
                        if (role.clientMainRole)
                        {
                            serverStartButton.Enabled = false;
                            StartServer();
                        }
                    }
                }
                else
                {
                    tcpClient = new TcpClient(clientIp, clientPort, ref logger);
                    tcpClient.Updatevariables(clientIp, clientPort);
                    tcpClient.Connect();
                    if (!tcpClient.ConnectionEstablished)
                    {
                        ChangeVisibilityConnectButton(true);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please provide correct IP address", "Error", MessageBoxButtons.OK);
            }
        }

        #region Logger worker methods
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.consoleOutputTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (consoleOutputTextBox.TextLength > consoleOutputTextBox.MaxLength - 20)
                {
                    ResetText(text);
                }
                else
                {
                    consoleOutputTextBox.AppendText("\r\n" + text);
                }
            }
        }

        delegate void ResetTextCallback(string text);

        private void ResetText(string text)
        {
            if (this.consoleOutputTextBox.InvokeRequired)
            {
                ResetTextCallback r = new ResetTextCallback(ResetText);
                this.Invoke(r, new object[] { text });
            }
            else
            {
                this.consoleOutputTextBox.ResetText();
                consoleOutputTextBox.AppendText("\r\n" + text);
            }
        }

        private void loggerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                var text = logger.popOfLogger();
                if (text != "")
                {
                    SetText(text);
                }
            }
        }
        #endregion

        private void connectToServerButton_Click(object sender, EventArgs e)
        {
            connectionWorker.RunWorkerAsync();
        }

        private void serverStartButton_Click(object sender, EventArgs e)
        {
            this.role.clientMainRole = false;
            this.role.serverMainRole = true;
            connectToServerButton.Enabled = false;
            StartServer();
        }

        private void StartServer()
        {

            Int32 port = Convert.ToInt32(serverPortBox.Text);
            tcpServer = new TcpServer(port, ref logger);
            tcpServer.DefaultSavePath = this.pathToSave;
            tcpServer.NotSecurePasswd = tcpPasswd;
            tcpServer.Setcipher();
            serverStartButton.Enabled = false;
            startServerWorker.RunWorkerAsync(argument: tcpServer);
            messageReciverWorker.RunWorkerAsync();
            checkServerStatus.RunWorkerAsync();
        }

        private void startServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var server = (TcpServer)e.Argument;
            if (!server.ServerStarted)
            {
                server.StartServer();
                while (true)
                {
                    if (server.ServerConnectedToClient && role.serverMainRole)
                    {
                        clientIp = server.clientIp;
                        clientPort = Convert.ToInt32(serverPortBox.Text);
                        startConnections();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        delegate void StatusTextCallback();

        private void StatusText()
        {
            if (this.consoleOutputTextBox.InvokeRequired)
            {
                StatusTextCallback r = new StatusTextCallback(StatusText);
                this.Invoke(r, new object[] { });
            }
            else
            {
                string text = "";
                if (tcpClient == null)
                {
                    text = "Not connected";
                }
                else
                {
                    text = tcpClient.CheckConnectionStatus() ? "Connected" : "Not connected";
                }
                connectionStatusTextBox.Text = text;
            }
        }

        private void copyConsoleWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                StatusText();
                if (tcpClient != null)
                {
                    if (!tcpClient.CheckConnectionStatus())
                    {
                        ChangeVisibilityConnectButton(true);
                        tcpClient = null;
                    }
                }
                Thread.Sleep(20);
            }
        }

        private void sendTextButton_Click(object sender, EventArgs e)
        {
            if (tcpClient != null)
            {
                if (tcpClient.ConnectionEstablished)
                {
                    var text = textToSendTextBox.Text;
                    if (text.Length != 0)
                    {
                        if (!this.sendingFile)
                        {
                            tcpClient.SendMessage(text);
                        }
                    }
                }
            }
        }

        private void messageReciverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (reciveData)
            {
                if (tcpServer != null)
                {
                    if (tcpServer.ServerStarted && tcpServer.ServerConnectedToClient)
                    {
                        if (tcpServer.CheckConnectionStatus())
                        {
                            tcpServer.Recive();
                        }
                        else
                        {
                            reciveData = false;
                            restartServer = true;
                        }
                    }
                }
            }
        }

        private void RestartServer()
        {
            Thread.Sleep(24);
            tcpServer.StopTimer();
            startServerWorker.CancelAsync();
            messageReciverWorker.CancelAsync();
            checkServerStatus.CancelAsync();

            var result = MessageBox.Show("Client disconnected" + System.Environment.NewLine + "Restart server", "Info", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                tcpServer.ConnectionResetValue = 0;
                tcpServer.CloseAllSocets();
                tcpServer = null;
                StartServer();
            }
            else
            {
                tcpServer.ConnectionStatus = true;
                reciveData = true;
                tcpServer.RestartTimer();
                startServerWorker.RunWorkerAsync(argument: tcpServer);
                messageReciverWorker.RunWorkerAsync();
                checkServerStatus.RunWorkerAsync();
            }
        }

        private void consoleOutputTextBox_TextChanged(object sender, EventArgs e)
        {
            consoleOutputTextBox.Focus();
            consoleOutputTextBox.Select(consoleOutputTextBox.Text.Length, 0);
        }

        private void pathSelectorButton_Click(object sender, EventArgs e)
        {
            using (var fldrDlg = new FolderBrowserDialog())
            {
                //fldrDlg.Filter = "Png Files (*.png)|*.png";
                //fldrDlg.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|CSV Files (*.csv)|*.csv"

                if (fldrDlg.ShowDialog() == DialogResult.OK)
                {
                    //fldrDlg.SelectedPath -- your result
                    Console.WriteLine(fldrDlg.SelectedPath);
                    this.pathToSave = fldrDlg.SelectedPath;
                    savePathLabel.Text = "Ip: " + ipAddress + Environment.NewLine +
                "Path to save files: " + fldrDlg.SelectedPath;
                    if (tcpServer != null)
                    {
                        tcpServer.DefaultSavePath = this.pathToSave;
                    }
                }
            }
        }

        delegate void SetTextToConnectButton(string text);
        delegate void ChangeVisibilityToConnectButton(bool value);

        private void SetTextConnectButton(string text)
        {
            if (connectToServerButton.InvokeRequired)
            {
                SetTextToConnectButton setTextToConnectButton = new SetTextToConnectButton(SetTextConnectButton);
                this.Invoke(setTextToConnectButton, new object[] { text });
            }
            else
            {
                connectToServerButton.Text = text;
            }
        }

        private void ChangeVisibilityConnectButton(bool value)
        {
            if (connectToServerButton.InvokeRequired)
            {
                ChangeVisibilityToConnectButton changeVisibilityToConnectButton = new ChangeVisibilityToConnectButton(ChangeVisibilityConnectButton);
                this.Invoke(changeVisibilityToConnectButton, new object[] { value });
            }
            else
            {
                connectToServerButton.Enabled = value;
            }
        }

        private void connectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.role.clientMainRole = true;
            this.role.serverMainRole = false;
            serverPortBox.Text = portBox.Text;
            startConnections();
        }

        private void sendFileButton_Click(object sender, EventArgs e)
        {
            ChangeProgress(0);
            sendFileWorker.RunWorkerAsync();

        }

        private void sendFileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (tcpClient != null)
            {
                if (tcpClient.ConnectionEstablished)
                {
                    if (this.fileOk)
                    {
                        this.sendingFile = true;
                        this.sendTextButton.Enabled = false;
                        tcpClient.FileSent = false;
                        progressBarWorker.RunWorkerAsync();
                        tcpClient.SendFile(this.fileName, this.fileSize);
                        this.fileOk = false;
                        this.sendingFile = false;
                        this.sendTextButton.Enabled = true;

                    }
                }
            }
        }

        delegate void ProgressHandle(int value);

        private void ChangeProgress(int value)
        {
            if (connectToServerButton.InvokeRequired)
            {
                ProgressHandle progressHandle = new ProgressHandle(ChangeProgress);
                this.Invoke(progressHandle, new object[] { value });
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private void progressBarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!tcpClient.FileSent)
            {
                var progresValue = tcpClient.ProgressValue;
                if (progresValue > 0)
                {
                    ChangeProgress(progresValue);
                }
            }
        }

        private void selectFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            this.fileOk = true;
        }

        private void setPasswordButton_Click(object sender, EventArgs e)
        {
            tcpPasswd = passwordTextBox.Text;
            if (tcpPasswd == "" || tcpPasswd == null)
            {
                MessageBox.Show("Wprowadz inne haslo", "Password Error", MessageBoxButtons.OK);
            }
            else
            {
                //przekazac haslo wyżej do serwera
                serverStartButton.Enabled = true;
                setPasswordButton.Enabled = false;
                passwordTextBox.ReadOnly = true;
            }
        }

        private void checkServerStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (restartServer)
                {
                    restartServer = false;
                    break;
                }
            }
        }

        private void checkServerStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RestartServer();
        }
    }
}
