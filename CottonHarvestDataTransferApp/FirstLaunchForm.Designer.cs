namespace CottonHarvestDataTransferApp
{
    partial class FirstLaunchForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstLaunchForm));
            this.pnlStep1 = new System.Windows.Forms.Panel();
            this.connectionSettingsControl = new CottonHarvestDataTransferApp.UserControls.ConnectionSettings();
            this.lblFirstLaunch_Step1Header = new System.Windows.Forms.Label();
            this.lblFirstLaunch_WelcomeText = new System.Windows.Forms.Label();
            this.lblFirstLaunch_WelcomeHeader = new System.Windows.Forms.Label();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlStep2 = new System.Windows.Forms.Panel();
            this.btnExit2 = new System.Windows.Forms.Button();
            this.downloadSettingsControl = new CottonHarvestDataTransferApp.UserControls.DownloadSettings();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStep2Finish = new System.Windows.Forms.Button();
            this.pnlStep1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.pnlStep2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStep1
            // 
            this.pnlStep1.Controls.Add(this.connectionSettingsControl);
            this.pnlStep1.Controls.Add(this.lblFirstLaunch_Step1Header);
            this.pnlStep1.Controls.Add(this.lblFirstLaunch_WelcomeText);
            this.pnlStep1.Controls.Add(this.lblFirstLaunch_WelcomeHeader);
            this.pnlStep1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStep1.Location = new System.Drawing.Point(0, 0);
            this.pnlStep1.Name = "pnlStep1";
            this.pnlStep1.Size = new System.Drawing.Size(776, 371);
            this.pnlStep1.TabIndex = 0;
            // 
            // connectionSettingsControl
            // 
            this.connectionSettingsControl.AutoSize = true;
            this.connectionSettingsControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectionSettingsControl.ExitButtonText = "Exit";
            this.connectionSettingsControl.FirstRun = true;
            this.connectionSettingsControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionSettingsControl.Location = new System.Drawing.Point(0, 134);
            this.connectionSettingsControl.Margin = new System.Windows.Forms.Padding(4);
            this.connectionSettingsControl.Name = "connectionSettingsControl";
            this.connectionSettingsControl.ShowExitButton = true;
            this.connectionSettingsControl.Size = new System.Drawing.Size(776, 299);
            this.connectionSettingsControl.TabIndex = 3;
            this.connectionSettingsControl.ConnectionComplete += new System.EventHandler(this.connectionSettingsControl_ConnectionComplete);
            this.connectionSettingsControl.ExitClicked += new System.EventHandler(this.connectionSettingsControl_ExitClicked);
            this.connectionSettingsControl.Load += new System.EventHandler(this.connectionSettingsControl_Load);
            // 
            // lblFirstLaunch_Step1Header
            // 
            this.lblFirstLaunch_Step1Header.AutoSize = true;
            this.lblFirstLaunch_Step1Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFirstLaunch_Step1Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstLaunch_Step1Header.Location = new System.Drawing.Point(0, 108);
            this.lblFirstLaunch_Step1Header.Name = "lblFirstLaunch_Step1Header";
            this.lblFirstLaunch_Step1Header.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.lblFirstLaunch_Step1Header.Size = new System.Drawing.Size(473, 26);
            this.lblFirstLaunch_Step1Header.TabIndex = 2;
            this.lblFirstLaunch_Step1Header.Text = "Step 1 - Link your MyJohnDeere account";
            // 
            // lblFirstLaunch_WelcomeText
            // 
            this.lblFirstLaunch_WelcomeText.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFirstLaunch_WelcomeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstLaunch_WelcomeText.Location = new System.Drawing.Point(0, 59);
            this.lblFirstLaunch_WelcomeText.Name = "lblFirstLaunch_WelcomeText";
            this.lblFirstLaunch_WelcomeText.Padding = new System.Windows.Forms.Padding(15, 0, 15, 15);
            this.lblFirstLaunch_WelcomeText.Size = new System.Drawing.Size(776, 49);
            this.lblFirstLaunch_WelcomeText.TabIndex = 1;
            this.lblFirstLaunch_WelcomeText.Text = "In the next few steps you will be guided through establishing your data link and " +
    "setting up automated downloads.";
            // 
            // lblFirstLaunch_WelcomeHeader
            // 
            this.lblFirstLaunch_WelcomeHeader.AutoSize = true;
            this.lblFirstLaunch_WelcomeHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFirstLaunch_WelcomeHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstLaunch_WelcomeHeader.Location = new System.Drawing.Point(0, 0);
            this.lblFirstLaunch_WelcomeHeader.Margin = new System.Windows.Forms.Padding(30);
            this.lblFirstLaunch_WelcomeHeader.Name = "lblFirstLaunch_WelcomeHeader";
            this.lblFirstLaunch_WelcomeHeader.Padding = new System.Windows.Forms.Padding(15);
            this.lblFirstLaunch_WelcomeHeader.Size = new System.Drawing.Size(657, 59);
            this.lblFirstLaunch_WelcomeHeader.TabIndex = 0;
            this.lblFirstLaunch_WelcomeHeader.Text = "Welcome, to the Cotton Harvest File Download Utility";
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.ContainerControl = this;
            // 
            // pnlStep2
            // 
            this.pnlStep2.Controls.Add(this.btnExit2);
            this.pnlStep2.Controls.Add(this.downloadSettingsControl);
            this.pnlStep2.Controls.Add(this.label6);
            this.pnlStep2.Controls.Add(this.btnStep2Finish);
            this.pnlStep2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStep2.Location = new System.Drawing.Point(0, 371);
            this.pnlStep2.Name = "pnlStep2";
            this.pnlStep2.Size = new System.Drawing.Size(776, 415);
            this.pnlStep2.TabIndex = 4;
            this.pnlStep2.Visible = false;
            // 
            // btnExit2
            // 
            this.btnExit2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit2.Location = new System.Drawing.Point(207, 352);
            this.btnExit2.Name = "btnExit2";
            this.btnExit2.Size = new System.Drawing.Size(179, 43);
            this.btnExit2.TabIndex = 7;
            this.btnExit2.Text = "Exit";
            this.btnExit2.UseVisualStyleBackColor = true;
            this.btnExit2.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // downloadSettingsControl
            // 
            this.downloadSettingsControl.AutoSaveChanges = false;
            this.downloadSettingsControl.Location = new System.Drawing.Point(13, 41);
            this.downloadSettingsControl.Name = "downloadSettingsControl";
            this.downloadSettingsControl.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.downloadSettingsControl.SelectedDownloadFolder = "";
            this.downloadSettingsControl.Size = new System.Drawing.Size(751, 296);
            this.downloadSettingsControl.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(10, 10, 15, 15);
            this.label6.Size = new System.Drawing.Size(327, 51);
            this.label6.TabIndex = 5;
            this.label6.Text = "Step 2 - Download Settings";
            // 
            // btnStep2Finish
            // 
            this.btnStep2Finish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStep2Finish.Location = new System.Drawing.Point(18, 352);
            this.btnStep2Finish.Name = "btnStep2Finish";
            this.btnStep2Finish.Size = new System.Drawing.Size(179, 43);
            this.btnStep2Finish.TabIndex = 3;
            this.btnStep2Finish.Text = "Finish";
            this.btnStep2Finish.UseVisualStyleBackColor = true;
            this.btnStep2Finish.Click += new System.EventHandler(this.btnStep2Finish_Click);
            // 
            // FirstLaunchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 528);
            this.Controls.Add(this.pnlStep2);
            this.Controls.Add(this.pnlStep1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FirstLaunchForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initial Setup";
            this.Load += new System.EventHandler(this.FirstLaunchForm_Load);
            this.pnlStep1.ResumeLayout(false);
            this.pnlStep1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            this.pnlStep2.ResumeLayout(false);
            this.pnlStep2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlStep1;
        private System.Windows.Forms.Label lblFirstLaunch_WelcomeText;
        private System.Windows.Forms.Label lblFirstLaunch_WelcomeHeader;
        private System.Windows.Forms.Label lblFirstLaunch_Step1Header;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
        private System.Windows.Forms.Panel pnlStep2;
        private System.Windows.Forms.Button btnStep2Finish;
        private UserControls.DownloadSettings downloadSettingsControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExit2;
        private UserControls.ConnectionSettings connectionSettingsControl;
    }
}