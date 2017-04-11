namespace CottonHarvestDataTransferApp.UserControls
{
    partial class ManagePartners
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagePartners));
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.lblLoadingMessage = new System.Windows.Forms.Label();
            this.pictureLoading = new System.Windows.Forms.PictureBox();
            this.dgPartners = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoteID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrganizationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharedFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSourceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlButtonBar = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnImportPartners = new System.Windows.Forms.Button();
            this.btnDeletePartner = new System.Windows.Forms.Button();
            this.btnAddPartner = new System.Windows.Forms.Button();
            this.pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPartners)).BeginInit();
            this.pnlButtonBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLoading
            // 
            this.pnlLoading.Controls.Add(this.lblLoadingMessage);
            this.pnlLoading.Controls.Add(this.pictureLoading);
            this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLoading.Location = new System.Drawing.Point(0, 67);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(775, 150);
            this.pnlLoading.TabIndex = 13;
            // 
            // lblLoadingMessage
            // 
            this.lblLoadingMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLoadingMessage.Location = new System.Drawing.Point(0, 122);
            this.lblLoadingMessage.Name = "lblLoadingMessage";
            this.lblLoadingMessage.Size = new System.Drawing.Size(775, 28);
            this.lblLoadingMessage.TabIndex = 1;
            this.lblLoadingMessage.Text = "Loading";
            this.lblLoadingMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureLoading
            // 
            this.pictureLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureLoading.Image = ((System.Drawing.Image)(resources.GetObject("pictureLoading.Image")));
            this.pictureLoading.Location = new System.Drawing.Point(0, 0);
            this.pictureLoading.Name = "pictureLoading";
            this.pictureLoading.Size = new System.Drawing.Size(775, 150);
            this.pictureLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureLoading.TabIndex = 0;
            this.pictureLoading.TabStop = false;
            // 
            // dgPartners
            // 
            this.dgPartners.AllowUserToAddRows = false;
            this.dgPartners.AllowUserToDeleteRows = false;
            this.dgPartners.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgPartners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPartners.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.RemoteID,
            this.OrganizationColumn,
            this.EmailColumn,
            this.Status,
            this.SharedFiles,
            this.DataSourceColumn});
            this.dgPartners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPartners.Location = new System.Drawing.Point(0, 67);
            this.dgPartners.MultiSelect = false;
            this.dgPartners.Name = "dgPartners";
            this.dgPartners.ReadOnly = true;
            this.dgPartners.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPartners.Size = new System.Drawing.Size(775, 532);
            this.dgPartners.TabIndex = 12;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // RemoteID
            // 
            this.RemoteID.HeaderText = "RemoteID";
            this.RemoteID.Name = "RemoteID";
            this.RemoteID.ReadOnly = true;
            this.RemoteID.Visible = false;
            // 
            // OrganizationColumn
            // 
            this.OrganizationColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OrganizationColumn.HeaderText = "Organization";
            this.OrganizationColumn.Name = "OrganizationColumn";
            this.OrganizationColumn.ReadOnly = true;
            this.OrganizationColumn.Width = 114;
            // 
            // EmailColumn
            // 
            this.EmailColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EmailColumn.HeaderText = "Email";
            this.EmailColumn.Name = "EmailColumn";
            this.EmailColumn.ReadOnly = true;
            this.EmailColumn.Width = 67;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Visible = false;
            // 
            // SharedFiles
            // 
            this.SharedFiles.HeaderText = "Shared Files";
            this.SharedFiles.Name = "SharedFiles";
            this.SharedFiles.ReadOnly = true;
            // 
            // DataSourceColumn
            // 
            this.DataSourceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataSourceColumn.HeaderText = "Source";
            this.DataSourceColumn.Name = "DataSourceColumn";
            this.DataSourceColumn.ReadOnly = true;
            // 
            // pnlButtonBar
            // 
            this.pnlButtonBar.Controls.Add(this.btnEdit);
            this.pnlButtonBar.Controls.Add(this.btnImportPartners);
            this.pnlButtonBar.Controls.Add(this.btnDeletePartner);
            this.pnlButtonBar.Controls.Add(this.btnAddPartner);
            this.pnlButtonBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtonBar.Location = new System.Drawing.Point(0, 0);
            this.pnlButtonBar.Name = "pnlButtonBar";
            this.pnlButtonBar.Size = new System.Drawing.Size(775, 67);
            this.pnlButtonBar.TabIndex = 11;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(144, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(166, 36);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit  Selected Partner";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnImportPartners
            // 
            this.btnImportPartners.Location = new System.Drawing.Point(461, 15);
            this.btnImportPartners.Name = "btnImportPartners";
            this.btnImportPartners.Size = new System.Drawing.Size(138, 36);
            this.btnImportPartners.TabIndex = 2;
            this.btnImportPartners.Text = "Import Partners";
            this.btnImportPartners.UseVisualStyleBackColor = true;
            this.btnImportPartners.Click += new System.EventHandler(this.btnImportPartners_Click);
            // 
            // btnDeletePartner
            // 
            this.btnDeletePartner.Location = new System.Drawing.Point(317, 15);
            this.btnDeletePartner.Name = "btnDeletePartner";
            this.btnDeletePartner.Size = new System.Drawing.Size(138, 36);
            this.btnDeletePartner.TabIndex = 1;
            this.btnDeletePartner.Text = "Delete Partner";
            this.btnDeletePartner.UseVisualStyleBackColor = true;
            this.btnDeletePartner.Click += new System.EventHandler(this.btnDeletePartner_Click);
            // 
            // btnAddPartner
            // 
            this.btnAddPartner.Location = new System.Drawing.Point(0, 15);
            this.btnAddPartner.Name = "btnAddPartner";
            this.btnAddPartner.Size = new System.Drawing.Size(138, 36);
            this.btnAddPartner.TabIndex = 0;
            this.btnAddPartner.Text = "Add Partner";
            this.btnAddPartner.UseVisualStyleBackColor = true;
            this.btnAddPartner.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ManagePartners
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.dgPartners);
            this.Controls.Add(this.pnlButtonBar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManagePartners";
            this.Size = new System.Drawing.Size(775, 599);
            this.pnlLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPartners)).EndInit();
            this.pnlButtonBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label lblLoadingMessage;
        private System.Windows.Forms.PictureBox pictureLoading;
        private System.Windows.Forms.DataGridView dgPartners;
        private System.Windows.Forms.Panel pnlButtonBar;
        private System.Windows.Forms.Button btnImportPartners;
        private System.Windows.Forms.Button btnDeletePartner;
        private System.Windows.Forms.Button btnAddPartner;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemoteID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrganizationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharedFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataSourceColumn;
    }
}
