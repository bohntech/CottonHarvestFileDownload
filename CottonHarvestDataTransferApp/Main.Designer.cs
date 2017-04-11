namespace CottonHarvestDataTransferApp
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.applicationTabControl = new System.Windows.Forms.TabControl();
            this.tabActivitySummary = new System.Windows.Forms.TabPage();
            this.activitySummaryControl = new CottonHarvestDataTransferApp.UserControls.ActivitySummary();
            this.tabManagePartners = new System.Windows.Forms.TabPage();
            this.managePartnersControl = new CottonHarvestDataTransferApp.UserControls.ManagePartners();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.downloadSettingsControl = new CottonHarvestDataTransferApp.UserControls.DownloadSettings();
            this.tabConnectionSettings = new System.Windows.Forms.TabPage();
            this.connectionSettingsControl = new CottonHarvestDataTransferApp.UserControls.ConnectionSettings();
            this.notifyIconControl = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationTabControl.SuspendLayout();
            this.tabActivitySummary.SuspendLayout();
            this.tabManagePartners.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabConnectionSettings.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // applicationTabControl
            // 
            this.applicationTabControl.Controls.Add(this.tabActivitySummary);
            this.applicationTabControl.Controls.Add(this.tabManagePartners);
            this.applicationTabControl.Controls.Add(this.tabSettings);
            this.applicationTabControl.Controls.Add(this.tabConnectionSettings);
            this.applicationTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applicationTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applicationTabControl.Location = new System.Drawing.Point(0, 0);
            this.applicationTabControl.Name = "applicationTabControl";
            this.applicationTabControl.Padding = new System.Drawing.Point(10, 10);
            this.applicationTabControl.SelectedIndex = 0;
            this.applicationTabControl.Size = new System.Drawing.Size(784, 642);
            this.applicationTabControl.TabIndex = 0;
            this.applicationTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.applicationTabControl_Selected);
            // 
            // tabActivitySummary
            // 
            this.tabActivitySummary.Controls.Add(this.activitySummaryControl);
            this.tabActivitySummary.Location = new System.Drawing.Point(4, 39);
            this.tabActivitySummary.Name = "tabActivitySummary";
            this.tabActivitySummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabActivitySummary.Size = new System.Drawing.Size(776, 599);
            this.tabActivitySummary.TabIndex = 0;
            this.tabActivitySummary.Text = "Activity Summary";
            this.tabActivitySummary.UseVisualStyleBackColor = true;
            // 
            // activitySummaryControl
            // 
            this.activitySummaryControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.activitySummaryControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activitySummaryControl.Location = new System.Drawing.Point(1, 1);
            this.activitySummaryControl.Margin = new System.Windows.Forms.Padding(4);
            this.activitySummaryControl.Name = "activitySummaryControl";
            this.activitySummaryControl.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.activitySummaryControl.Size = new System.Drawing.Size(775, 598);
            this.activitySummaryControl.TabIndex = 0;
            this.activitySummaryControl.ManageConnectionClicked += new System.EventHandler(this.activitySummaryControl_ManageConnectionClicked);
            this.activitySummaryControl.NewFilesDownloaded += new System.EventHandler(this.activitySummaryControl_NewFilesDownloaded);            
            // 
            // tabManagePartners
            // 
            this.tabManagePartners.Controls.Add(this.managePartnersControl);
            this.tabManagePartners.Location = new System.Drawing.Point(4, 39);
            this.tabManagePartners.Name = "tabManagePartners";
            this.tabManagePartners.Size = new System.Drawing.Size(776, 599);
            this.tabManagePartners.TabIndex = 3;
            this.tabManagePartners.Text = "Manage Partners";
            this.tabManagePartners.UseVisualStyleBackColor = true;
            // 
            // managePartnersControl
            // 
            this.managePartnersControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managePartnersControl.Location = new System.Drawing.Point(1, 3);
            this.managePartnersControl.Margin = new System.Windows.Forms.Padding(4);
            this.managePartnersControl.Name = "managePartnersControl";
            this.managePartnersControl.Padding = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.managePartnersControl.Size = new System.Drawing.Size(775, 599);
            this.managePartnersControl.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.downloadSettingsControl);
            this.tabSettings.Location = new System.Drawing.Point(4, 39);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(776, 599);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Download Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // downloadSettingsControl
            // 
            this.downloadSettingsControl.AutoSaveChanges = false;
            this.downloadSettingsControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.downloadSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.downloadSettingsControl.Margin = new System.Windows.Forms.Padding(4, 10, 4, 4);
            this.downloadSettingsControl.Name = "downloadSettingsControl";
            this.downloadSettingsControl.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.downloadSettingsControl.SelectedDownloadFolder = "";
            this.downloadSettingsControl.Size = new System.Drawing.Size(770, 372);
            this.downloadSettingsControl.TabIndex = 0;
            // 
            // tabConnectionSettings
            // 
            this.tabConnectionSettings.Controls.Add(this.connectionSettingsControl);
            this.tabConnectionSettings.Location = new System.Drawing.Point(4, 39);
            this.tabConnectionSettings.Name = "tabConnectionSettings";
            this.tabConnectionSettings.Size = new System.Drawing.Size(776, 599);
            this.tabConnectionSettings.TabIndex = 2;
            this.tabConnectionSettings.Text = "Connection Settings";
            this.tabConnectionSettings.UseVisualStyleBackColor = true;
            // 
            // connectionSettingsControl
            // 
            this.connectionSettingsControl.AutoSize = true;
            this.connectionSettingsControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectionSettingsControl.ExitButtonText = "Cancel";
            this.connectionSettingsControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.connectionSettingsControl.Margin = new System.Windows.Forms.Padding(4);
            this.connectionSettingsControl.Name = "connectionSettingsControl";
            this.connectionSettingsControl.ShowExitButton = true;
            this.connectionSettingsControl.Size = new System.Drawing.Size(776, 299);
            this.connectionSettingsControl.TabIndex = 0;
            this.connectionSettingsControl.ConnectionComplete += new System.EventHandler(this.connectionSettingsControl_ConnectionComplete);
            this.connectionSettingsControl.ExitClicked += new System.EventHandler(this.connectionSettingsControl_ExitClicked);
            // 
            // notifyIconControl
            // 
            this.notifyIconControl.ContextMenuStrip = this.contextMenu;
            this.notifyIconControl.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconControl.Icon")));
            this.notifyIconControl.Text = "Cotton Harvest File Download Utility";
            this.notifyIconControl.Visible = true;
            this.notifyIconControl.BalloonTipClicked += new System.EventHandler(this.notifyIconControl_BalloonTipClicked);
            this.notifyIconControl.DoubleClick += new System.EventHandler(this.notifyIconControl_DoubleClick);
            this.notifyIconControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconControl_MouseClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenu.Size = new System.Drawing.Size(104, 48);
            this.contextMenu.Text = "Menu";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 642);
            this.Controls.Add(this.applicationTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cotton Harvest File Download Utility";
            this.Activated += new System.EventHandler(this.Main_Activated);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.applicationTabControl.ResumeLayout(false);
            this.tabActivitySummary.ResumeLayout(false);
            this.tabManagePartners.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabConnectionSettings.ResumeLayout(false);
            this.tabConnectionSettings.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl applicationTabControl;
        private System.Windows.Forms.TabPage tabActivitySummary;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabConnectionSettings;
        private System.Windows.Forms.TabPage tabManagePartners;
        private UserControls.DownloadSettings downloadSettingsControl;
        private UserControls.ConnectionSettings connectionSettingsControl;
        private UserControls.ManagePartners managePartnersControl;
        private UserControls.ActivitySummary activitySummaryControl;
        private System.Windows.Forms.NotifyIcon notifyIconControl;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    }
}

