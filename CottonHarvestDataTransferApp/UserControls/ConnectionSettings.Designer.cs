namespace CottonHarvestDataTransferApp.UserControls
{
    partial class ConnectionSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionSettings));
            this.pnlConnectionSettingsConnected = new System.Windows.Forms.Panel();
            this.btnConnectionSettingsLinkDifferentAccount = new System.Windows.Forms.Button();
            this.lblApplicationLinkedText = new System.Windows.Forms.Label();
            this.pnlConnectionSettingsVerifyCode = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSubmitVerificationCode = new System.Windows.Forms.Button();
            this.pnlConnectionSettingsLinkAccount = new System.Windows.Forms.Panel();
            this.btnConnectionsSettingsLinkAccount = new System.Windows.Forms.Button();
            this.lblConnectionInstructionsText = new System.Windows.Forms.Label();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlNoNetwork = new System.Windows.Forms.Panel();
            this.btnRetry = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.btnStartOver = new System.Windows.Forms.Button();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.pictureLoading = new System.Windows.Forms.PictureBox();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.btnConnections = new System.Windows.Forms.Button();
            this.lblPermissions = new System.Windows.Forms.Label();
            this.pnlConnectionSettingsConnected.SuspendLayout();
            this.pnlConnectionSettingsVerifyCode.SuspendLayout();
            this.pnlConnectionSettingsLinkAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.pnlNoNetwork.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlConnectionSettingsConnected
            // 
            this.pnlConnectionSettingsConnected.Controls.Add(this.lblPermissions);
            this.pnlConnectionSettingsConnected.Controls.Add(this.btnConnections);
            this.pnlConnectionSettingsConnected.Controls.Add(this.btnConnectionSettingsLinkDifferentAccount);
            this.pnlConnectionSettingsConnected.Controls.Add(this.lblApplicationLinkedText);
            this.pnlConnectionSettingsConnected.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnectionSettingsConnected.Location = new System.Drawing.Point(0, 307);
            this.pnlConnectionSettingsConnected.Margin = new System.Windows.Forms.Padding(4);
            this.pnlConnectionSettingsConnected.Name = "pnlConnectionSettingsConnected";
            this.pnlConnectionSettingsConnected.Size = new System.Drawing.Size(775, 128);
            this.pnlConnectionSettingsConnected.TabIndex = 7;
            this.pnlConnectionSettingsConnected.Visible = false;
            // 
            // btnConnectionSettingsLinkDifferentAccount
            // 
            this.btnConnectionSettingsLinkDifferentAccount.Location = new System.Drawing.Point(19, 69);
            this.btnConnectionSettingsLinkDifferentAccount.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectionSettingsLinkDifferentAccount.Name = "btnConnectionSettingsLinkDifferentAccount";
            this.btnConnectionSettingsLinkDifferentAccount.Size = new System.Drawing.Size(197, 43);
            this.btnConnectionSettingsLinkDifferentAccount.TabIndex = 4;
            this.btnConnectionSettingsLinkDifferentAccount.Text = "Change connection";
            this.btnConnectionSettingsLinkDifferentAccount.UseVisualStyleBackColor = true;
            this.btnConnectionSettingsLinkDifferentAccount.Click += new System.EventHandler(this.btnConnectionSettingsLinkDifferentAccount_Click);
            // 
            // lblApplicationLinkedText
            // 
            this.lblApplicationLinkedText.AutoSize = true;
            this.lblApplicationLinkedText.Location = new System.Drawing.Point(16, 22);
            this.lblApplicationLinkedText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApplicationLinkedText.Name = "lblApplicationLinkedText";
            this.lblApplicationLinkedText.Size = new System.Drawing.Size(369, 17);
            this.lblApplicationLinkedText.TabIndex = 4;
            this.lblApplicationLinkedText.Text = "Application is currently linked to a MyJohnDeere account.";
            // 
            // pnlConnectionSettingsVerifyCode
            // 
            this.pnlConnectionSettingsVerifyCode.Controls.Add(this.btnExit);
            this.pnlConnectionSettingsVerifyCode.Controls.Add(this.label4);
            this.pnlConnectionSettingsVerifyCode.Controls.Add(this.btnSubmitVerificationCode);
            this.pnlConnectionSettingsVerifyCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnectionSettingsVerifyCode.Location = new System.Drawing.Point(0, 149);
            this.pnlConnectionSettingsVerifyCode.Margin = new System.Windows.Forms.Padding(4);
            this.pnlConnectionSettingsVerifyCode.Name = "pnlConnectionSettingsVerifyCode";
            this.pnlConnectionSettingsVerifyCode.Size = new System.Drawing.Size(775, 158);
            this.pnlConnectionSettingsVerifyCode.TabIndex = 6;
            this.pnlConnectionSettingsVerifyCode.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.CausesValidation = false;
            this.btnExit.Location = new System.Drawing.Point(230, 85);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(197, 43);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 19);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label4.Size = new System.Drawing.Size(776, 63);
            this.label4.TabIndex = 6;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // btnSubmitVerificationCode
            // 
            this.btnSubmitVerificationCode.Location = new System.Drawing.Point(16, 85);
            this.btnSubmitVerificationCode.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmitVerificationCode.Name = "btnSubmitVerificationCode";
            this.btnSubmitVerificationCode.Size = new System.Drawing.Size(197, 43);
            this.btnSubmitVerificationCode.TabIndex = 4;
            this.btnSubmitVerificationCode.Text = "Sign In";
            this.btnSubmitVerificationCode.UseVisualStyleBackColor = true;
            this.btnSubmitVerificationCode.Click += new System.EventHandler(this.btnSubmitVerificationCode_Click);
            // 
            // pnlConnectionSettingsLinkAccount
            // 
            this.pnlConnectionSettingsLinkAccount.Controls.Add(this.btnConnectionsSettingsLinkAccount);
            this.pnlConnectionSettingsLinkAccount.Controls.Add(this.lblConnectionInstructionsText);
            this.pnlConnectionSettingsLinkAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnectionSettingsLinkAccount.Location = new System.Drawing.Point(0, 0);
            this.pnlConnectionSettingsLinkAccount.Margin = new System.Windows.Forms.Padding(4);
            this.pnlConnectionSettingsLinkAccount.Name = "pnlConnectionSettingsLinkAccount";
            this.pnlConnectionSettingsLinkAccount.Size = new System.Drawing.Size(775, 149);
            this.pnlConnectionSettingsLinkAccount.TabIndex = 5;
            // 
            // btnConnectionsSettingsLinkAccount
            // 
            this.btnConnectionsSettingsLinkAccount.Location = new System.Drawing.Point(20, 72);
            this.btnConnectionsSettingsLinkAccount.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectionsSettingsLinkAccount.Name = "btnConnectionsSettingsLinkAccount";
            this.btnConnectionsSettingsLinkAccount.Size = new System.Drawing.Size(197, 43);
            this.btnConnectionsSettingsLinkAccount.TabIndex = 3;
            this.btnConnectionsSettingsLinkAccount.Text = "Link Account";
            this.btnConnectionsSettingsLinkAccount.UseVisualStyleBackColor = true;
            this.btnConnectionsSettingsLinkAccount.Click += new System.EventHandler(this.btnConnectionsSettingsLinkAccount_Click);
            // 
            // lblConnectionInstructionsText
            // 
            this.lblConnectionInstructionsText.Location = new System.Drawing.Point(16, 20);
            this.lblConnectionInstructionsText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionInstructionsText.Name = "lblConnectionInstructionsText";
            this.lblConnectionInstructionsText.Size = new System.Drawing.Size(737, 94);
            this.lblConnectionInstructionsText.TabIndex = 2;
            this.lblConnectionInstructionsText.Text = "This application is not currently linked to a MyJohnDeere account.   Click the bu" +
    "tton below to link  an account.";
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.formErrorProvider.ContainerControl = this;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // pnlNoNetwork
            // 
            this.pnlNoNetwork.Controls.Add(this.btnRetry);
            this.pnlNoNetwork.Controls.Add(this.label1);
            this.pnlNoNetwork.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNoNetwork.Location = new System.Drawing.Point(0, 435);
            this.pnlNoNetwork.Name = "pnlNoNetwork";
            this.pnlNoNetwork.Size = new System.Drawing.Size(775, 106);
            this.pnlNoNetwork.TabIndex = 8;
            this.pnlNoNetwork.Visible = false;
            // 
            // btnRetry
            // 
            this.btnRetry.Location = new System.Drawing.Point(20, 44);
            this.btnRetry.Margin = new System.Windows.Forms.Padding(4);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(197, 43);
            this.btnRetry.TabIndex = 5;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(737, 94);
            this.label1.TabIndex = 4;
            this.label1.Text = "Network not available.  Please check your internet connection and click retry bel" +
    "ow.";
            // 
            // pnlLoading
            // 
            this.pnlLoading.Controls.Add(this.btnStartOver);
            this.pnlLoading.Controls.Add(this.lblLoadingMessage);
            this.pnlLoading.Controls.Add(this.pictureLoading);
            this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLoading.Location = new System.Drawing.Point(0, 541);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(775, 189);
            this.pnlLoading.TabIndex = 9;
            this.pnlLoading.Visible = false;
            // 
            // btnStartOver
            // 
            this.btnStartOver.Location = new System.Drawing.Point(290, 12);
            this.btnStartOver.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartOver.Name = "btnStartOver";
            this.btnStartOver.Size = new System.Drawing.Size(197, 33);
            this.btnStartOver.TabIndex = 10;
            this.btnStartOver.Text = "Retry Login";
            this.btnStartOver.UseVisualStyleBackColor = true;
            this.btnStartOver.Click += new System.EventHandler(this.btnStartOver_Click);
            // 
            // lblLoadingMessage
            // 
            this.lblLoadingMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLoadingMessage.Location = new System.Drawing.Point(0, 161);
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            this.lblLoadingMessage.Size = new System.Drawing.Size(775, 28);
            this.lblLoadingMessage.TabIndex = 1;
            this.lblLoadingMessage.Text = "Loading";
            this.lblLoadingMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureLoading
            // 
            this.pictureLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureLoading.Image = ((System.Drawing.Image)(resources.GetObject("pictureLoading.Image")));
            this.pictureLoading.InitialImage = null;
            this.pictureLoading.Location = new System.Drawing.Point(0, 0);
            this.pictureLoading.Name = "pictureLoading";
            this.pictureLoading.Size = new System.Drawing.Size(775, 189);
            this.pictureLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureLoading.TabIndex = 0;
            this.pictureLoading.TabStop = false;
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "AuthResponse.txt";
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Created);
            // 
            // btnConnections
            // 
            this.btnConnections.Location = new System.Drawing.Point(230, 69);
            this.btnConnections.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnections.Name = "btnConnections";
            this.btnConnections.Size = new System.Drawing.Size(216, 43);
            this.btnConnections.TabIndex = 5;
            this.btnConnections.Text = "Set Permissions";
            this.btnConnections.UseVisualStyleBackColor = true;
            this.btnConnections.Click += new System.EventHandler(this.btnConnections_Click);
            // 
            // lblPermissions
            // 
            this.lblPermissions.AutoSize = true;
            this.lblPermissions.ForeColor = System.Drawing.Color.Red;
            this.lblPermissions.Location = new System.Drawing.Point(15, 43);
            this.lblPermissions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPermissions.Name = "lblPermissions";
            this.lblPermissions.Size = new System.Drawing.Size(723, 17);
            this.lblPermissions.TabIndex = 6;
            this.lblPermissions.Text = "Access has not been granted to one or more partners.   Please click the set permission" +
    "s button to review access.";
            this.lblPermissions.Click += new System.EventHandler(this.label2_Click);
            // 
            // ConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.pnlNoNetwork);
            this.Controls.Add(this.pnlConnectionSettingsConnected);
            this.Controls.Add(this.pnlConnectionSettingsVerifyCode);
            this.Controls.Add(this.pnlConnectionSettingsLinkAccount);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConnectionSettings";
            this.Size = new System.Drawing.Size(775, 746);
            this.pnlConnectionSettingsConnected.ResumeLayout(false);
            this.pnlConnectionSettingsConnected.PerformLayout();
            this.pnlConnectionSettingsVerifyCode.ResumeLayout(false);
            this.pnlConnectionSettingsLinkAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.pnlNoNetwork.ResumeLayout(false);
            this.pnlLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlConnectionSettingsConnected;
        private System.Windows.Forms.Button btnConnectionSettingsLinkDifferentAccount;
        private System.Windows.Forms.Label lblApplicationLinkedText;
        private System.Windows.Forms.Panel pnlConnectionSettingsVerifyCode;
        private System.Windows.Forms.Button btnSubmitVerificationCode;
        private System.Windows.Forms.Panel pnlConnectionSettingsLinkAccount;
        private System.Windows.Forms.Button btnConnectionsSettingsLinkAccount;
        private System.Windows.Forms.Label lblConnectionInstructionsText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel pnlNoNetwork;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label lblLoadingMessage;
        private System.Windows.Forms.PictureBox pictureLoading;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.Button btnStartOver;
        private System.Windows.Forms.Button btnConnections;
        private System.Windows.Forms.Label lblPermissions;
    }
}
