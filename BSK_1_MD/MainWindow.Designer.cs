namespace BSK_1_MD
{
    partial class BSK
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTabControl = new MaterialSkin.Controls.MaterialTabControl();
            this.clientTabPage = new System.Windows.Forms.TabPage();
            this.connectToServerButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.connectionStatusTextBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.materialTabSelector2 = new MaterialSkin.Controls.MaterialTabSelector();
            this.clientTabConrol = new MaterialSkin.Controls.MaterialTabControl();
            this.clientFileTabPage = new System.Windows.Forms.TabPage();
            this.sendFileButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.fileSelectButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.clientTextTabPage = new System.Windows.Forms.TabPage();
            this.textToSendTextBox = new System.Windows.Forms.TextBox();
            this.sendTextButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.serverTabPage = new System.Windows.Forms.TabPage();
            this.setPasswordButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.serverStartButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.materialTabSelector3 = new MaterialSkin.Controls.MaterialTabSelector();
            this.settingsTabControl = new MaterialSkin.Controls.MaterialTabControl();
            this.settingsClientTabPage = new System.Windows.Forms.TabPage();
            this.portBox = new System.Windows.Forms.TextBox();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portLabel = new MaterialSkin.Controls.MaterialLabel();
            this.ipLabel = new MaterialSkin.Controls.MaterialLabel();
            this.settingsServerTabPage = new System.Windows.Forms.TabPage();
            this.savePathLabel = new System.Windows.Forms.TextBox();
            this.pathSelectorButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.serverPortBox = new System.Windows.Forms.TextBox();
            this.serverPortLabel = new MaterialSkin.Controls.MaterialLabel();
            this.settingsGeneralTabPage = new System.Windows.Forms.TabPage();
            this.cipherCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.consoleTabPage = new System.Windows.Forms.TabPage();
            this.consoleOutputTextBox = new System.Windows.Forms.TextBox();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loggerWorker = new System.ComponentModel.BackgroundWorker();
            this.startServerWorker = new System.ComponentModel.BackgroundWorker();
            this.copyConsoleWorker = new System.ComponentModel.BackgroundWorker();
            this.messageReciverWorker = new System.ComponentModel.BackgroundWorker();
            this.connectionWorker = new System.ComponentModel.BackgroundWorker();
            this.sendFileWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBarWorker = new System.ComponentModel.BackgroundWorker();
            this.checkServerStatus = new System.ComponentModel.BackgroundWorker();
            this.fullyConnectedCheckerWorker = new System.ComponentModel.BackgroundWorker();
            this.updateReciverSettingsWorker = new System.ComponentModel.BackgroundWorker();
            this.mainTabControl.SuspendLayout();
            this.clientTabPage.SuspendLayout();
            this.clientTabConrol.SuspendLayout();
            this.clientFileTabPage.SuspendLayout();
            this.clientTextTabPage.SuspendLayout();
            this.serverTabPage.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.settingsClientTabPage.SuspendLayout();
            this.settingsServerTabPage.SuspendLayout();
            this.settingsGeneralTabPage.SuspendLayout();
            this.consoleTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.clientTabPage);
            this.mainTabControl.Controls.Add(this.serverTabPage);
            this.mainTabControl.Controls.Add(this.settingsTabPage);
            this.mainTabControl.Controls.Add(this.consoleTabPage);
            this.mainTabControl.Depth = 0;
            this.mainTabControl.Location = new System.Drawing.Point(0, 109);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabControl.MouseState = MaterialSkin.MouseState.HOVER;
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(700, 309);
            this.mainTabControl.TabIndex = 0;
            // 
            // clientTabPage
            // 
            this.clientTabPage.Controls.Add(this.connectToServerButton);
            this.clientTabPage.Controls.Add(this.connectionStatusTextBox);
            this.clientTabPage.Controls.Add(this.progressBar1);
            this.clientTabPage.Controls.Add(this.materialTabSelector2);
            this.clientTabPage.Controls.Add(this.clientTabConrol);
            this.clientTabPage.Location = new System.Drawing.Point(4, 24);
            this.clientTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.clientTabPage.Name = "clientTabPage";
            this.clientTabPage.Size = new System.Drawing.Size(692, 281);
            this.clientTabPage.TabIndex = 0;
            this.clientTabPage.Text = "Client";
            this.clientTabPage.UseVisualStyleBackColor = true;
            // 
            // connectToServerButton
            // 
            this.connectToServerButton.Depth = 0;
            this.connectToServerButton.Font = new System.Drawing.Font("Consolas", 12F);
            this.connectToServerButton.Location = new System.Drawing.Point(7, 44);
            this.connectToServerButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.connectToServerButton.Name = "connectToServerButton";
            this.connectToServerButton.Primary = true;
            this.connectToServerButton.Size = new System.Drawing.Size(88, 94);
            this.connectToServerButton.TabIndex = 5;
            this.connectToServerButton.Text = "Connect";
            this.connectToServerButton.UseVisualStyleBackColor = true;
            this.connectToServerButton.Click += new System.EventHandler(this.connectToServerButton_Click);
            // 
            // connectionStatusTextBox
            // 
            this.connectionStatusTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this.connectionStatusTextBox.Location = new System.Drawing.Point(7, 158);
            this.connectionStatusTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.connectionStatusTextBox.MaxLength = 50;
            this.connectionStatusTextBox.Multiline = true;
            this.connectionStatusTextBox.Name = "connectionStatusTextBox";
            this.connectionStatusTextBox.ReadOnly = true;
            this.connectionStatusTextBox.Size = new System.Drawing.Size(88, 94);
            this.connectionStatusTextBox.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 259);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(692, 22);
            this.progressBar1.TabIndex = 2;
            // 
            // materialTabSelector2
            // 
            this.materialTabSelector2.BaseTabControl = this.clientTabConrol;
            this.materialTabSelector2.Depth = 0;
            this.materialTabSelector2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTabSelector2.Location = new System.Drawing.Point(0, 0);
            this.materialTabSelector2.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector2.Name = "materialTabSelector2";
            this.materialTabSelector2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.materialTabSelector2.Size = new System.Drawing.Size(700, 30);
            this.materialTabSelector2.TabIndex = 1;
            this.materialTabSelector2.Text = "materialTabSelector2";
            // 
            // clientTabConrol
            // 
            this.clientTabConrol.Controls.Add(this.clientFileTabPage);
            this.clientTabConrol.Controls.Add(this.clientTextTabPage);
            this.clientTabConrol.Depth = 0;
            this.clientTabConrol.Location = new System.Drawing.Point(118, 36);
            this.clientTabConrol.MouseState = MaterialSkin.MouseState.HOVER;
            this.clientTabConrol.Name = "clientTabConrol";
            this.clientTabConrol.SelectedIndex = 0;
            this.clientTabConrol.Size = new System.Drawing.Size(582, 219);
            this.clientTabConrol.TabIndex = 0;
            // 
            // clientFileTabPage
            // 
            this.clientFileTabPage.Controls.Add(this.sendFileButton);
            this.clientFileTabPage.Controls.Add(this.fileSelectButton);
            this.clientFileTabPage.Location = new System.Drawing.Point(4, 24);
            this.clientFileTabPage.Name = "clientFileTabPage";
            this.clientFileTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.clientFileTabPage.Size = new System.Drawing.Size(574, 191);
            this.clientFileTabPage.TabIndex = 0;
            this.clientFileTabPage.Text = "File";
            this.clientFileTabPage.UseVisualStyleBackColor = true;
            // 
            // sendFileButton
            // 
            this.sendFileButton.Depth = 0;
            this.sendFileButton.Enabled = false;
            this.sendFileButton.Font = new System.Drawing.Font("Consolas", 10F);
            this.sendFileButton.Location = new System.Drawing.Point(494, 112);
            this.sendFileButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.sendFileButton.Name = "sendFileButton";
            this.sendFileButton.Primary = true;
            this.sendFileButton.Size = new System.Drawing.Size(70, 75);
            this.sendFileButton.TabIndex = 2;
            this.sendFileButton.Text = "Send";
            this.sendFileButton.UseVisualStyleBackColor = true;
            this.sendFileButton.Click += new System.EventHandler(this.sendFileButton_Click);
            // 
            // fileSelectButton
            // 
            this.fileSelectButton.Depth = 0;
            this.fileSelectButton.Font = new System.Drawing.Font("Consolas", 10F);
            this.fileSelectButton.Location = new System.Drawing.Point(494, 6);
            this.fileSelectButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.fileSelectButton.Name = "fileSelectButton";
            this.fileSelectButton.Primary = true;
            this.fileSelectButton.Size = new System.Drawing.Size(70, 75);
            this.fileSelectButton.TabIndex = 1;
            this.fileSelectButton.Text = "File Select";
            this.fileSelectButton.UseVisualStyleBackColor = true;
            this.fileSelectButton.Click += new System.EventHandler(this.fileSelectButton_Click);
            // 
            // clientTextTabPage
            // 
            this.clientTextTabPage.Controls.Add(this.textToSendTextBox);
            this.clientTextTabPage.Controls.Add(this.sendTextButton);
            this.clientTextTabPage.Location = new System.Drawing.Point(4, 24);
            this.clientTextTabPage.Name = "clientTextTabPage";
            this.clientTextTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.clientTextTabPage.Size = new System.Drawing.Size(574, 191);
            this.clientTextTabPage.TabIndex = 1;
            this.clientTextTabPage.Text = "Text";
            this.clientTextTabPage.UseVisualStyleBackColor = true;
            // 
            // textToSendTextBox
            // 
            this.textToSendTextBox.Font = new System.Drawing.Font("Consolas", 12F);
            this.textToSendTextBox.Location = new System.Drawing.Point(6, 7);
            this.textToSendTextBox.Multiline = true;
            this.textToSendTextBox.Name = "textToSendTextBox";
            this.textToSendTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textToSendTextBox.Size = new System.Drawing.Size(484, 180);
            this.textToSendTextBox.TabIndex = 4;
            // 
            // sendTextButton
            // 
            this.sendTextButton.Depth = 0;
            this.sendTextButton.Font = new System.Drawing.Font("Consolas", 10F);
            this.sendTextButton.Location = new System.Drawing.Point(494, 112);
            this.sendTextButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.sendTextButton.Name = "sendTextButton";
            this.sendTextButton.Primary = true;
            this.sendTextButton.Size = new System.Drawing.Size(70, 75);
            this.sendTextButton.TabIndex = 3;
            this.sendTextButton.Text = "Send";
            this.sendTextButton.UseVisualStyleBackColor = true;
            this.sendTextButton.Click += new System.EventHandler(this.sendTextButton_Click);
            // 
            // serverTabPage
            // 
            this.serverTabPage.Controls.Add(this.setPasswordButton);
            this.serverTabPage.Controls.Add(this.passwordTextBox);
            this.serverTabPage.Controls.Add(this.passwordLabel);
            this.serverTabPage.Controls.Add(this.serverStartButton);
            this.serverTabPage.Location = new System.Drawing.Point(4, 24);
            this.serverTabPage.Name = "serverTabPage";
            this.serverTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.serverTabPage.Size = new System.Drawing.Size(692, 281);
            this.serverTabPage.TabIndex = 1;
            this.serverTabPage.Text = "Server";
            this.serverTabPage.UseVisualStyleBackColor = true;
            // 
            // setPasswordButton
            // 
            this.setPasswordButton.Depth = 0;
            this.setPasswordButton.Font = new System.Drawing.Font("Consolas", 12F);
            this.setPasswordButton.Location = new System.Drawing.Point(355, 32);
            this.setPasswordButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.setPasswordButton.Name = "setPasswordButton";
            this.setPasswordButton.Primary = true;
            this.setPasswordButton.Size = new System.Drawing.Size(164, 23);
            this.setPasswordButton.TabIndex = 3;
            this.setPasswordButton.Text = "SET";
            this.setPasswordButton.UseVisualStyleBackColor = true;
            this.setPasswordButton.Click += new System.EventHandler(this.setPasswordButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(118, 32);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(218, 23);
            this.passwordTextBox.TabIndex = 2;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(19, 32);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(72, 17);
            this.passwordLabel.TabIndex = 1;
            this.passwordLabel.Text = "Password";
            // 
            // serverStartButton
            // 
            this.serverStartButton.Depth = 0;
            this.serverStartButton.Font = new System.Drawing.Font("Consolas", 12F);
            this.serverStartButton.Location = new System.Drawing.Point(618, 202);
            this.serverStartButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.serverStartButton.Name = "serverStartButton";
            this.serverStartButton.Primary = true;
            this.serverStartButton.Size = new System.Drawing.Size(70, 75);
            this.serverStartButton.TabIndex = 0;
            this.serverStartButton.Text = "Server Start";
            this.serverStartButton.UseVisualStyleBackColor = true;
            this.serverStartButton.Click += new System.EventHandler(this.serverStartButton_Click);
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.materialTabSelector3);
            this.settingsTabPage.Controls.Add(this.settingsTabControl);
            this.settingsTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTabPage.Size = new System.Drawing.Size(692, 281);
            this.settingsTabPage.TabIndex = 2;
            this.settingsTabPage.Text = "Settings";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // materialTabSelector3
            // 
            this.materialTabSelector3.BaseTabControl = this.settingsTabControl;
            this.materialTabSelector3.Depth = 0;
            this.materialTabSelector3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTabSelector3.Location = new System.Drawing.Point(0, 0);
            this.materialTabSelector3.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector3.Name = "materialTabSelector3";
            this.materialTabSelector3.Size = new System.Drawing.Size(700, 38);
            this.materialTabSelector3.TabIndex = 1;
            this.materialTabSelector3.Text = "materialTabSelector3";
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.settingsClientTabPage);
            this.settingsTabControl.Controls.Add(this.settingsServerTabPage);
            this.settingsTabControl.Controls.Add(this.settingsGeneralTabPage);
            this.settingsTabControl.Depth = 0;
            this.settingsTabControl.Location = new System.Drawing.Point(3, 47);
            this.settingsTabControl.MouseState = MaterialSkin.MouseState.HOVER;
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(694, 235);
            this.settingsTabControl.TabIndex = 0;
            // 
            // settingsClientTabPage
            // 
            this.settingsClientTabPage.Controls.Add(this.portBox);
            this.settingsClientTabPage.Controls.Add(this.ipBox);
            this.settingsClientTabPage.Controls.Add(this.portLabel);
            this.settingsClientTabPage.Controls.Add(this.ipLabel);
            this.settingsClientTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsClientTabPage.Name = "settingsClientTabPage";
            this.settingsClientTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsClientTabPage.Size = new System.Drawing.Size(686, 207);
            this.settingsClientTabPage.TabIndex = 0;
            this.settingsClientTabPage.Text = "Client";
            this.settingsClientTabPage.UseVisualStyleBackColor = true;
            // 
            // portBox
            // 
            this.portBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBox.Location = new System.Drawing.Point(86, 59);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(110, 31);
            this.portBox.TabIndex = 3;
            this.portBox.Text = "810";
            // 
            // ipBox
            // 
            this.ipBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipBox.Location = new System.Drawing.Point(86, 22);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(110, 31);
            this.ipBox.TabIndex = 2;
            this.ipBox.Text = "127.0.0.1";
            // 
            // portLabel
            // 
            this.portLabel.Depth = 0;
            this.portLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.portLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.portLabel.Location = new System.Drawing.Point(20, 62);
            this.portLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(88, 28);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Port:";
            // 
            // ipLabel
            // 
            this.ipLabel.Depth = 0;
            this.ipLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.ipLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ipLabel.Location = new System.Drawing.Point(21, 25);
            this.ipLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(88, 28);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "IP:";
            // 
            // settingsServerTabPage
            // 
            this.settingsServerTabPage.Controls.Add(this.savePathLabel);
            this.settingsServerTabPage.Controls.Add(this.pathSelectorButton);
            this.settingsServerTabPage.Controls.Add(this.serverPortBox);
            this.settingsServerTabPage.Controls.Add(this.serverPortLabel);
            this.settingsServerTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsServerTabPage.Name = "settingsServerTabPage";
            this.settingsServerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsServerTabPage.Size = new System.Drawing.Size(686, 207);
            this.settingsServerTabPage.TabIndex = 1;
            this.settingsServerTabPage.Text = "Server";
            this.settingsServerTabPage.UseVisualStyleBackColor = true;
            // 
            // savePathLabel
            // 
            this.savePathLabel.Font = new System.Drawing.Font("Consolas", 10F);
            this.savePathLabel.Location = new System.Drawing.Point(215, 22);
            this.savePathLabel.Margin = new System.Windows.Forms.Padding(0);
            this.savePathLabel.MaxLength = 50;
            this.savePathLabel.Multiline = true;
            this.savePathLabel.Name = "savePathLabel";
            this.savePathLabel.ReadOnly = true;
            this.savePathLabel.Size = new System.Drawing.Size(465, 147);
            this.savePathLabel.TabIndex = 8;
            // 
            // pathSelectorButton
            // 
            this.pathSelectorButton.Depth = 0;
            this.pathSelectorButton.Font = new System.Drawing.Font("Consolas", 12F);
            this.pathSelectorButton.Location = new System.Drawing.Point(25, 75);
            this.pathSelectorButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.pathSelectorButton.Name = "pathSelectorButton";
            this.pathSelectorButton.Primary = true;
            this.pathSelectorButton.Size = new System.Drawing.Size(88, 94);
            this.pathSelectorButton.TabIndex = 7;
            this.pathSelectorButton.Text = "Path";
            this.pathSelectorButton.UseVisualStyleBackColor = true;
            this.pathSelectorButton.Click += new System.EventHandler(this.pathSelectorButton_Click);
            // 
            // serverPortBox
            // 
            this.serverPortBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverPortBox.Location = new System.Drawing.Point(89, 22);
            this.serverPortBox.Name = "serverPortBox";
            this.serverPortBox.Size = new System.Drawing.Size(110, 31);
            this.serverPortBox.TabIndex = 5;
            this.serverPortBox.Text = "810";
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.Depth = 0;
            this.serverPortLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.serverPortLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.serverPortLabel.Location = new System.Drawing.Point(21, 25);
            this.serverPortLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(88, 28);
            this.serverPortLabel.TabIndex = 4;
            this.serverPortLabel.Text = "Port:";
            // 
            // settingsGeneralTabPage
            // 
            this.settingsGeneralTabPage.Controls.Add(this.cipherCheckedListBox);
            this.settingsGeneralTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsGeneralTabPage.Name = "settingsGeneralTabPage";
            this.settingsGeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsGeneralTabPage.Size = new System.Drawing.Size(686, 207);
            this.settingsGeneralTabPage.TabIndex = 2;
            this.settingsGeneralTabPage.Text = "General";
            this.settingsGeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // cipherCheckedListBox
            // 
            this.cipherCheckedListBox.CheckOnClick = true;
            this.cipherCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cipherCheckedListBox.Font = new System.Drawing.Font("Consolas", 12F);
            this.cipherCheckedListBox.FormattingEnabled = true;
            this.cipherCheckedListBox.Location = new System.Drawing.Point(3, 3);
            this.cipherCheckedListBox.Name = "cipherCheckedListBox";
            this.cipherCheckedListBox.Size = new System.Drawing.Size(680, 201);
            this.cipherCheckedListBox.TabIndex = 0;
            this.cipherCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cipherCheckedListBox_ItemCheck);
            // 
            // consoleTabPage
            // 
            this.consoleTabPage.Controls.Add(this.consoleOutputTextBox);
            this.consoleTabPage.Location = new System.Drawing.Point(4, 24);
            this.consoleTabPage.Name = "consoleTabPage";
            this.consoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.consoleTabPage.Size = new System.Drawing.Size(692, 281);
            this.consoleTabPage.TabIndex = 3;
            this.consoleTabPage.Text = "Console";
            this.consoleTabPage.UseVisualStyleBackColor = true;
            // 
            // consoleOutputTextBox
            // 
            this.consoleOutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleOutputTextBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.consoleOutputTextBox.Location = new System.Drawing.Point(3, 3);
            this.consoleOutputTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.consoleOutputTextBox.MaxLength = 100000;
            this.consoleOutputTextBox.Multiline = true;
            this.consoleOutputTextBox.Name = "consoleOutputTextBox";
            this.consoleOutputTextBox.ReadOnly = true;
            this.consoleOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleOutputTextBox.Size = new System.Drawing.Size(686, 275);
            this.consoleOutputTextBox.TabIndex = 0;
            this.consoleOutputTextBox.TextChanged += new System.EventHandler(this.consoleOutputTextBox_TextChanged);
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.mainTabControl;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Consolas", 12F);
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 60);
            this.materialTabSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(700, 49);
            this.materialTabSelector1.TabIndex = 1;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // selectFileDialog
            // 
            this.selectFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.selectFileDialog_FileOk);
            // 
            // loggerWorker
            // 
            this.loggerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loggerWorker_DoWork);
            // 
            // startServerWorker
            // 
            this.startServerWorker.WorkerSupportsCancellation = true;
            this.startServerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.startServerWorker_DoWork);
            // 
            // copyConsoleWorker
            // 
            this.copyConsoleWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.copyConsoleWorker_DoWork);
            // 
            // messageReciverWorker
            // 
            this.messageReciverWorker.WorkerSupportsCancellation = true;
            this.messageReciverWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.messageReciverWorker_DoWork);
            // 
            // connectionWorker
            // 
            this.connectionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.connectionWorker_DoWork);
            // 
            // sendFileWorker
            // 
            this.sendFileWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.sendFileWorker_DoWork);
            // 
            // progressBarWorker
            // 
            this.progressBarWorker.WorkerReportsProgress = true;
            this.progressBarWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.progressBarWorker_DoWork);
            // 
            // checkServerStatus
            // 
            this.checkServerStatus.WorkerSupportsCancellation = true;
            this.checkServerStatus.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkServerStatus_DoWork);
            this.checkServerStatus.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.checkServerStatus_RunWorkerCompleted);
            // 
            // fullyConnectedCheckerWorker
            // 
            this.fullyConnectedCheckerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.fullyConnectedCheckerWorker_DoWork);
            // 
            // updateReciverSettingsWorker
            // 
            this.updateReciverSettingsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateReciverSettingsWorker_DoWork);
            // 
            // BSK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 422);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.mainTabControl);
            this.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BSK";
            this.mainTabControl.ResumeLayout(false);
            this.clientTabPage.ResumeLayout(false);
            this.clientTabPage.PerformLayout();
            this.clientTabConrol.ResumeLayout(false);
            this.clientFileTabPage.ResumeLayout(false);
            this.clientTextTabPage.ResumeLayout(false);
            this.clientTextTabPage.PerformLayout();
            this.serverTabPage.ResumeLayout(false);
            this.serverTabPage.PerformLayout();
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabControl.ResumeLayout(false);
            this.settingsClientTabPage.ResumeLayout(false);
            this.settingsClientTabPage.PerformLayout();
            this.settingsServerTabPage.ResumeLayout(false);
            this.settingsServerTabPage.PerformLayout();
            this.settingsGeneralTabPage.ResumeLayout(false);
            this.consoleTabPage.ResumeLayout(false);
            this.consoleTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl mainTabControl;
        private System.Windows.Forms.TabPage clientTabPage;
        private System.Windows.Forms.TabPage serverTabPage;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.TabPage consoleTabPage;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector2;
        private MaterialSkin.Controls.MaterialTabControl clientTabConrol;
        private System.Windows.Forms.TabPage clientFileTabPage;
        private System.Windows.Forms.TabPage clientTextTabPage;
        private System.Windows.Forms.TextBox consoleOutputTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector3;
        private MaterialSkin.Controls.MaterialTabControl settingsTabControl;
        private System.Windows.Forms.TabPage settingsClientTabPage;
        private System.Windows.Forms.TabPage settingsServerTabPage;
        private System.Windows.Forms.TabPage settingsGeneralTabPage;
        private System.Windows.Forms.TextBox ipBox;
        private MaterialSkin.Controls.MaterialLabel portLabel;
        private MaterialSkin.Controls.MaterialLabel ipLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox serverPortBox;
        private MaterialSkin.Controls.MaterialLabel serverPortLabel;
        private System.Windows.Forms.CheckedListBox cipherCheckedListBox;
        private System.Windows.Forms.TextBox connectionStatusTextBox;
        private MaterialSkin.Controls.MaterialRaisedButton serverStartButton;
        private MaterialSkin.Controls.MaterialRaisedButton connectToServerButton;
        private MaterialSkin.Controls.MaterialRaisedButton sendFileButton;
        private MaterialSkin.Controls.MaterialRaisedButton fileSelectButton;
        private MaterialSkin.Controls.MaterialRaisedButton sendTextButton;
        private System.Windows.Forms.TextBox textToSendTextBox;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.ComponentModel.BackgroundWorker loggerWorker;
        private System.ComponentModel.BackgroundWorker startServerWorker;
        private System.ComponentModel.BackgroundWorker copyConsoleWorker;
        private System.ComponentModel.BackgroundWorker messageReciverWorker;
        private MaterialSkin.Controls.MaterialRaisedButton pathSelectorButton;
        private System.Windows.Forms.TextBox savePathLabel;
        private System.ComponentModel.BackgroundWorker connectionWorker;
        private System.ComponentModel.BackgroundWorker sendFileWorker;
        private System.ComponentModel.BackgroundWorker progressBarWorker;
        private MaterialSkin.Controls.MaterialRaisedButton setPasswordButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.ComponentModel.BackgroundWorker checkServerStatus;
        private System.ComponentModel.BackgroundWorker fullyConnectedCheckerWorker;
        private System.ComponentModel.BackgroundWorker updateReciverSettingsWorker;
    }
}

