﻿using MaterialSkin.Controls;
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

namespace BSK_1_MD
{
    public partial class BSK : MaterialForm
    {
        private string[] cipherBlocks = new[] { "ECB", "OBC", "CFB", "OFB" };
        private Logger logger;
        private TcpClients tcpClient;
        private TcpServer tcpServer;
        private IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        private IPAddress ipAddress = null;
        public BSK()
        {
            InitializeComponent();
            cipherCheckedListBox.Items.AddRange(cipherBlocks);
            cipherCheckedListBox.SetItemChecked(0, true);
            logger = new Logger();
            loggerWorker.RunWorkerAsync();
            copyConsoleWorker.RunWorkerAsync();
            ipAddress = ipHostInfo.AddressList[1];
            savePathLabel.Text = "Ip: " + ipAddress + Environment.NewLine +
                "Path to save files: " + System.IO.Directory.GetCurrentDirectory();
        }

        private void fileSelectButton_Click(object sender, EventArgs e)
        {
            selectFileDialog.ShowDialog();
            try
            {
                string fileName = selectFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(fileName);
                double fileSize = fileInfo.Length / Math.Pow(10, 6);
                logger.addToLogger("[WFA] " + "File:\n " + fileName + "\nSize:\n" + fileSize + " MB");
            }
            catch (Exception error)
            {
                logger.addToLogger("[WFA] " + error.ToString());
            }
        }
        private void cipherCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && cipherCheckedListBox.CheckedItems.Count > 0)
            {
                cipherCheckedListBox.ItemCheck -= cipherCheckedListBox_ItemCheck;
                cipherCheckedListBox.SetItemChecked(cipherCheckedListBox.CheckedIndices[0], false);
                cipherCheckedListBox.ItemCheck += cipherCheckedListBox_ItemCheck;
            }
        }
        private void startConnections()
        {
            string ip = ipBox.Text;
            Int32 port = Convert.ToInt32(portBox.Text);
            tcpClient = new TcpClients(ip, port, ref logger);
            tcpClient.Connect();
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
            //To do
            // verify ip, port 
            startConnections();
        }

        private void serverStartButton_Click(object sender, EventArgs e)
        {
            Int32 port = Convert.ToInt32(serverPortBox.Text);
            tcpServer = new TcpServer(port, ref logger);
            serverStartButton.Enabled = false;
            startServerWorker.RunWorkerAsync(argument: tcpServer);
            messageReciverWorker.RunWorkerAsync();
        }

        private void startServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var server = (TcpServer)e.Argument;
            if (!server.ServerStarted)
            {
                server.StartServer();
            }
        }

        delegate void StatusTextCallback();

        private void StatusText()
        {
            if (this.consoleOutputTextBox.InvokeRequired)
            {
                StatusTextCallback r = new StatusTextCallback(StatusText);
                this.Invoke(r, new object[] {});
            }
            else
            {
                string text = tcpClient.ConnectedToServer ? "Connected" : "Not connected";
                connectionStatusTextBox.Text = text;
            }
        }
        private void copyConsoleWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
               if(tcpClient != null)
                {
                    StatusText();
                    if (tcpClient.ConnectedToServer)
                    {
                        break;
                    }
                }
            }
        }

        private void sendTextButton_Click(object sender, EventArgs e)
        {
            if (tcpClient.ConnectedToServer)
            {
                var text = textToSendTextBox.Text;
                tcpClient.SendMessage(text);
            }
        }

        private void messageReciverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (tcpServer.ServerStarted)
                {
                    tcpServer.ReciveMessage();
                    System.Threading.Thread.Sleep(50);
                }
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
                    savePathLabel.Text = "Ip: " + ipAddress + Environment.NewLine +
                "Path to save files: " + fldrDlg.SelectedPath;
                }
            }
        }
    }
}
