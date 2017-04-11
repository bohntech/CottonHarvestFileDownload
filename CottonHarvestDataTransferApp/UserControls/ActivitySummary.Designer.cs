namespace CottonHarvestDataTransferApp.UserControls
{
    partial class ActivitySummary
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timerControl = new System.Windows.Forms.Timer(this.components);
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.summaryInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutInfoPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.btnManageConnection = new System.Windows.Forms.Button();
            this.scheduleTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnDownloadNow = new System.Windows.Forms.Button();
            this.lblNextDownloadLabel = new System.Windows.Forms.Label();
            this.lblNextDownloadValue = new System.Windows.Forms.Label();
            this.downloadTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnChangeFolder = new System.Windows.Forms.Button();
            this.lblDownloadFolderLabel = new System.Windows.Forms.Label();
            this.lblDownloadFolderValue = new System.Windows.Forms.Label();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.lblRecentFilesLabel = new System.Windows.Forms.Label();
            this.dgRecentFiles = new System.Windows.Forms.DataGridView();
            this.PartnerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnViewFiles = new System.Windows.Forms.Button();
            this.summaryInformationGroupBox.SuspendLayout();
            this.tableLayoutInfoPanel.SuspendLayout();
            this.scheduleTableLayout.SuspendLayout();
            this.downloadTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRecentFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // timerControl
            // 
            this.timerControl.Interval = 45000;
            this.timerControl.Tick += new System.EventHandler(this.timerControl_Tick);
            // 
            // summaryInformationGroupBox
            // 
            this.summaryInformationGroupBox.Controls.Add(this.tableLayoutInfoPanel);
            this.summaryInformationGroupBox.Controls.Add(this.scheduleTableLayout);
            this.summaryInformationGroupBox.Controls.Add(this.downloadTableLayout);
            this.summaryInformationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summaryInformationGroupBox.Location = new System.Drawing.Point(3, 5);
            this.summaryInformationGroupBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.summaryInformationGroupBox.Name = "summaryInformationGroupBox";
            this.summaryInformationGroupBox.Size = new System.Drawing.Size(766, 148);
            this.summaryInformationGroupBox.TabIndex = 12;
            this.summaryInformationGroupBox.TabStop = false;
            this.summaryInformationGroupBox.Text = "Information";
            this.summaryInformationGroupBox.Enter += new System.EventHandler(this.summaryInformationGroupBox_Enter);
            // 
            // tableLayoutInfoPanel
            // 
            this.tableLayoutInfoPanel.ColumnCount = 3;
            this.tableLayoutInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.58984F));
            this.tableLayoutInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.41016F));
            this.tableLayoutInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutInfoPanel.Controls.Add(this.lblStatusValue, 0, 0);
            this.tableLayoutInfoPanel.Controls.Add(this.lblStatusLabel, 0, 0);
            this.tableLayoutInfoPanel.Controls.Add(this.btnManageConnection, 2, 0);
            this.tableLayoutInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutInfoPanel.Location = new System.Drawing.Point(3, 101);
            this.tableLayoutInfoPanel.Name = "tableLayoutInfoPanel";
            this.tableLayoutInfoPanel.RowCount = 1;
            this.tableLayoutInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutInfoPanel.Size = new System.Drawing.Size(760, 38);
            this.tableLayoutInfoPanel.TabIndex = 11;
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoEllipsis = true;
            this.lblStatusValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusValue.ForeColor = System.Drawing.Color.Green;
            this.lblStatusValue.Location = new System.Drawing.Point(141, 0);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(398, 38);
            this.lblStatusValue.TabIndex = 6;
            this.lblStatusValue.Text = "Connected";
            this.lblStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusLabel.Location = new System.Drawing.Point(3, 0);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(132, 38);
            this.lblStatusLabel.TabIndex = 5;
            this.lblStatusLabel.Text = "Status:";
            this.lblStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnManageConnection
            // 
            this.btnManageConnection.AutoEllipsis = true;
            this.btnManageConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManageConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageConnection.Location = new System.Drawing.Point(545, 3);
            this.btnManageConnection.Name = "btnManageConnection";
            this.btnManageConnection.Size = new System.Drawing.Size(212, 32);
            this.btnManageConnection.TabIndex = 3;
            this.btnManageConnection.Text = "Manage connection";
            this.btnManageConnection.UseVisualStyleBackColor = true;
            this.btnManageConnection.Click += new System.EventHandler(this.btnManageConnection_Click);
            // 
            // scheduleTableLayout
            // 
            this.scheduleTableLayout.ColumnCount = 3;
            this.scheduleTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.58984F));
            this.scheduleTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.41016F));
            this.scheduleTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.scheduleTableLayout.Controls.Add(this.btnDownloadNow, 2, 0);
            this.scheduleTableLayout.Controls.Add(this.lblNextDownloadLabel, 0, 0);
            this.scheduleTableLayout.Controls.Add(this.lblNextDownloadValue, 1, 0);
            this.scheduleTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.scheduleTableLayout.Location = new System.Drawing.Point(3, 63);
            this.scheduleTableLayout.Name = "scheduleTableLayout";
            this.scheduleTableLayout.RowCount = 1;
            this.scheduleTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.scheduleTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.scheduleTableLayout.Size = new System.Drawing.Size(760, 38);
            this.scheduleTableLayout.TabIndex = 10;
            // 
            // btnDownloadNow
            // 
            this.btnDownloadNow.AutoEllipsis = true;
            this.btnDownloadNow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownloadNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadNow.Location = new System.Drawing.Point(545, 3);
            this.btnDownloadNow.Name = "btnDownloadNow";
            this.btnDownloadNow.Size = new System.Drawing.Size(212, 32);
            this.btnDownloadNow.TabIndex = 3;
            this.btnDownloadNow.Text = "Download now";
            this.btnDownloadNow.UseVisualStyleBackColor = true;
            this.btnDownloadNow.Click += new System.EventHandler(this.btnDownloadNow_Click);
            // 
            // lblNextDownloadLabel
            // 
            this.lblNextDownloadLabel.AutoSize = true;
            this.lblNextDownloadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNextDownloadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextDownloadLabel.Location = new System.Drawing.Point(3, 0);
            this.lblNextDownloadLabel.Name = "lblNextDownloadLabel";
            this.lblNextDownloadLabel.Size = new System.Drawing.Size(132, 38);
            this.lblNextDownloadLabel.TabIndex = 4;
            this.lblNextDownloadLabel.Text = "Next Download:";
            this.lblNextDownloadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNextDownloadValue
            // 
            this.lblNextDownloadValue.AutoEllipsis = true;
            this.lblNextDownloadValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNextDownloadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextDownloadValue.Location = new System.Drawing.Point(141, 0);
            this.lblNextDownloadValue.Name = "lblNextDownloadValue";
            this.lblNextDownloadValue.Size = new System.Drawing.Size(398, 38);
            this.lblNextDownloadValue.TabIndex = 1;
            this.lblNextDownloadValue.Text = "--";
            this.lblNextDownloadValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // downloadTableLayout
            // 
            this.downloadTableLayout.ColumnCount = 3;
            this.downloadTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.58984F));
            this.downloadTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.41016F));
            this.downloadTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.downloadTableLayout.Controls.Add(this.btnChangeFolder, 2, 0);
            this.downloadTableLayout.Controls.Add(this.lblDownloadFolderLabel, 0, 0);
            this.downloadTableLayout.Controls.Add(this.lblDownloadFolderValue, 1, 0);
            this.downloadTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.downloadTableLayout.Location = new System.Drawing.Point(3, 25);
            this.downloadTableLayout.Name = "downloadTableLayout";
            this.downloadTableLayout.RowCount = 1;
            this.downloadTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.downloadTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.downloadTableLayout.Size = new System.Drawing.Size(760, 38);
            this.downloadTableLayout.TabIndex = 9;
            // 
            // btnChangeFolder
            // 
            this.btnChangeFolder.AutoEllipsis = true;
            this.btnChangeFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChangeFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeFolder.Location = new System.Drawing.Point(545, 3);
            this.btnChangeFolder.Name = "btnChangeFolder";
            this.btnChangeFolder.Size = new System.Drawing.Size(212, 32);
            this.btnChangeFolder.TabIndex = 3;
            this.btnChangeFolder.Text = "Change folder";
            this.btnChangeFolder.UseVisualStyleBackColor = true;
            this.btnChangeFolder.Click += new System.EventHandler(this.btnChangeFolder_Click);
            // 
            // lblDownloadFolderLabel
            // 
            this.lblDownloadFolderLabel.AutoSize = true;
            this.lblDownloadFolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDownloadFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadFolderLabel.Location = new System.Drawing.Point(3, 0);
            this.lblDownloadFolderLabel.Name = "lblDownloadFolderLabel";
            this.lblDownloadFolderLabel.Size = new System.Drawing.Size(132, 38);
            this.lblDownloadFolderLabel.TabIndex = 4;
            this.lblDownloadFolderLabel.Text = "Download Folder: ";
            this.lblDownloadFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDownloadFolderValue
            // 
            this.lblDownloadFolderValue.AutoEllipsis = true;
            this.lblDownloadFolderValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDownloadFolderValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadFolderValue.Location = new System.Drawing.Point(141, 0);
            this.lblDownloadFolderValue.Name = "lblDownloadFolderValue";
            this.lblDownloadFolderValue.Size = new System.Drawing.Size(398, 38);
            this.lblDownloadFolderValue.TabIndex = 1;
            this.lblDownloadFolderValue.Text = "--";
            this.lblDownloadFolderValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnViewLog
            // 
            this.btnViewLog.Location = new System.Drawing.Point(669, 162);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(101, 32);
            this.btnViewLog.TabIndex = 16;
            this.btnViewLog.Text = "View Log";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // lblRecentFilesLabel
            // 
            this.lblRecentFilesLabel.AutoSize = true;
            this.lblRecentFilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecentFilesLabel.Location = new System.Drawing.Point(0, 161);
            this.lblRecentFilesLabel.Name = "lblRecentFilesLabel";
            this.lblRecentFilesLabel.Size = new System.Drawing.Size(127, 24);
            this.lblRecentFilesLabel.TabIndex = 15;
            this.lblRecentFilesLabel.Text = "Recent Files";
            // 
            // dgRecentFiles
            // 
            this.dgRecentFiles.AllowUserToAddRows = false;
            this.dgRecentFiles.AllowUserToDeleteRows = false;
            this.dgRecentFiles.AllowUserToResizeRows = false;
            this.dgRecentFiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgRecentFiles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgRecentFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRecentFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartnerColumn,
            this.FileColumn});
            this.dgRecentFiles.Location = new System.Drawing.Point(3, 200);
            this.dgRecentFiles.Margin = new System.Windows.Forms.Padding(15);
            this.dgRecentFiles.MultiSelect = false;
            this.dgRecentFiles.Name = "dgRecentFiles";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgRecentFiles.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgRecentFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRecentFiles.ShowEditingIcon = false;
            this.dgRecentFiles.ShowRowErrors = false;
            this.dgRecentFiles.Size = new System.Drawing.Size(766, 392);
            this.dgRecentFiles.TabIndex = 17;
            this.dgRecentFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgRecentFiles_CellContentClick);
            // 
            // PartnerColumn
            // 
            this.PartnerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PartnerColumn.HeaderText = "Partner";
            this.PartnerColumn.Name = "PartnerColumn";
            this.PartnerColumn.Width = 80;
            // 
            // FileColumn
            // 
            this.FileColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileColumn.HeaderText = "File";
            this.FileColumn.Name = "FileColumn";
            // 
            // btnViewFiles
            // 
            this.btnViewFiles.Location = new System.Drawing.Point(551, 162);
            this.btnViewFiles.Name = "btnViewFiles";
            this.btnViewFiles.Size = new System.Drawing.Size(101, 32);
            this.btnViewFiles.TabIndex = 18;
            this.btnViewFiles.Text = "View Files";
            this.btnViewFiles.UseVisualStyleBackColor = true;
            this.btnViewFiles.Click += new System.EventHandler(this.btnViewFiles_Click);
            // 
            // ActivitySummary
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.btnViewFiles);
            this.Controls.Add(this.dgRecentFiles);
            this.Controls.Add(this.btnViewLog);
            this.Controls.Add(this.lblRecentFilesLabel);
            this.Controls.Add(this.summaryInformationGroupBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ActivitySummary";
            this.Size = new System.Drawing.Size(775, 598);
            this.summaryInformationGroupBox.ResumeLayout(false);
            this.tableLayoutInfoPanel.ResumeLayout(false);
            this.tableLayoutInfoPanel.PerformLayout();
            this.scheduleTableLayout.ResumeLayout(false);
            this.scheduleTableLayout.PerformLayout();
            this.downloadTableLayout.ResumeLayout(false);
            this.downloadTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRecentFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerControl;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.GroupBox summaryInformationGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutInfoPanel;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.Button btnManageConnection;
        private System.Windows.Forms.TableLayoutPanel scheduleTableLayout;
        private System.Windows.Forms.Button btnDownloadNow;
        private System.Windows.Forms.Label lblNextDownloadLabel;
        private System.Windows.Forms.Label lblNextDownloadValue;
        private System.Windows.Forms.TableLayoutPanel downloadTableLayout;
        private System.Windows.Forms.Button btnChangeFolder;
        private System.Windows.Forms.Label lblDownloadFolderLabel;
        private System.Windows.Forms.Label lblDownloadFolderValue;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Label lblRecentFilesLabel;
        private System.Windows.Forms.DataGridView dgRecentFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerColumn;
        private System.Windows.Forms.DataGridViewLinkColumn FileColumn;
        private System.Windows.Forms.Button btnViewFiles;
    }
}
