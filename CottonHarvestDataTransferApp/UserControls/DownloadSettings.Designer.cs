namespace CottonHarvestDataTransferApp.UserControls
{
    partial class DownloadSettings
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSettingsDownloadLocation = new System.Windows.Forms.Label();
            this.btnSettingsChangeFolder = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlHourly = new System.Windows.Forms.Panel();
            this.flowLayoutPanelHourlyOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.hourPicker = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlDailyOptions = new System.Windows.Forms.Panel();
            this.flowLayoutDownloadDailyOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.pnlTypeOptions = new System.Windows.Forms.Panel();
            this.flowLayoutDownloadFrequency = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDownloadType = new System.Windows.Forms.ComboBox();
            this.selectFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.chkStartup = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlHourly.SuspendLayout();
            this.flowLayoutPanelHourlyOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hourPicker)).BeginInit();
            this.pnlDailyOptions.SuspendLayout();
            this.flowLayoutDownloadDailyOptions.SuspendLayout();
            this.pnlTypeOptions.SuspendLayout();
            this.flowLayoutDownloadFrequency.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Controls.Add(this.btnSettingsChangeFolder);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 142);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(766, 122);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Download location";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSettingsDownloadLocation, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(760, 40);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "Current download folder:";
            // 
            // lblSettingsDownloadLocation
            // 
            this.lblSettingsDownloadLocation.AutoEllipsis = true;
            this.lblSettingsDownloadLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSettingsDownloadLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingsDownloadLocation.Location = new System.Drawing.Point(172, 10);
            this.lblSettingsDownloadLocation.Name = "lblSettingsDownloadLocation";
            this.lblSettingsDownloadLocation.Size = new System.Drawing.Size(585, 17);
            this.lblSettingsDownloadLocation.TabIndex = 11;
            this.lblSettingsDownloadLocation.Text = "-- None Selected --";
            // 
            // btnSettingsChangeFolder
            // 
            this.btnSettingsChangeFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettingsChangeFolder.Location = new System.Drawing.Point(7, 69);
            this.btnSettingsChangeFolder.Name = "btnSettingsChangeFolder";
            this.btnSettingsChangeFolder.Size = new System.Drawing.Size(120, 39);
            this.btnSettingsChangeFolder.TabIndex = 8;
            this.btnSettingsChangeFolder.Text = "Select Folder";
            this.btnSettingsChangeFolder.UseVisualStyleBackColor = true;
            this.btnSettingsChangeFolder.Click += new System.EventHandler(this.btnSettingsChangeFolder_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlHourly);
            this.groupBox2.Controls.Add(this.pnlDailyOptions);
            this.groupBox2.Controls.Add(this.pnlTypeOptions);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(766, 132);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Download Frequency";
            // 
            // pnlHourly
            // 
            this.pnlHourly.Controls.Add(this.flowLayoutPanelHourlyOptions);
            this.pnlHourly.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHourly.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHourly.Location = new System.Drawing.Point(3, 128);
            this.pnlHourly.Name = "pnlHourly";
            this.pnlHourly.Size = new System.Drawing.Size(760, 50);
            this.pnlHourly.TabIndex = 2;
            this.pnlHourly.Visible = false;
            // 
            // flowLayoutPanelHourlyOptions
            // 
            this.flowLayoutPanelHourlyOptions.Controls.Add(this.label7);
            this.flowLayoutPanelHourlyOptions.Controls.Add(this.hourPicker);
            this.flowLayoutPanelHourlyOptions.Controls.Add(this.label9);
            this.flowLayoutPanelHourlyOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelHourlyOptions.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelHourlyOptions.Name = "flowLayoutPanelHourlyOptions";
            this.flowLayoutPanelHourlyOptions.Size = new System.Drawing.Size(760, 36);
            this.flowLayoutPanelHourlyOptions.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "Download files every ";
            // 
            // hourPicker
            // 
            this.hourPicker.Location = new System.Drawing.Point(151, 3);
            this.hourPicker.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.hourPicker.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hourPicker.Name = "hourPicker";
            this.hourPicker.Size = new System.Drawing.Size(120, 23);
            this.hourPicker.TabIndex = 4;
            this.hourPicker.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hourPicker.ValueChanged += new System.EventHandler(this.hourPicker_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 5);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "hour(s)";
            // 
            // pnlDailyOptions
            // 
            this.pnlDailyOptions.Controls.Add(this.flowLayoutDownloadDailyOptions);
            this.pnlDailyOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDailyOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlDailyOptions.Location = new System.Drawing.Point(3, 78);
            this.pnlDailyOptions.Name = "pnlDailyOptions";
            this.pnlDailyOptions.Size = new System.Drawing.Size(760, 50);
            this.pnlDailyOptions.TabIndex = 1;
            this.pnlDailyOptions.Visible = false;
            // 
            // flowLayoutDownloadDailyOptions
            // 
            this.flowLayoutDownloadDailyOptions.Controls.Add(this.label6);
            this.flowLayoutDownloadDailyOptions.Controls.Add(this.timePicker);
            this.flowLayoutDownloadDailyOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutDownloadDailyOptions.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutDownloadDailyOptions.Name = "flowLayoutDownloadDailyOptions";
            this.flowLayoutDownloadDailyOptions.Size = new System.Drawing.Size(760, 44);
            this.flowLayoutDownloadDailyOptions.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Download files daily at";
            // 
            // timePicker
            // 
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(157, 3);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(114, 23);
            this.timePicker.TabIndex = 4;
            this.timePicker.Value = new System.DateTime(2016, 11, 2, 8, 0, 0, 0);
            this.timePicker.ValueChanged += new System.EventHandler(this.timePicker_ValueChanged);
            // 
            // pnlTypeOptions
            // 
            this.pnlTypeOptions.Controls.Add(this.flowLayoutDownloadFrequency);
            this.pnlTypeOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTypeOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTypeOptions.Location = new System.Drawing.Point(3, 25);
            this.pnlTypeOptions.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.pnlTypeOptions.Name = "pnlTypeOptions";
            this.pnlTypeOptions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlTypeOptions.Size = new System.Drawing.Size(760, 53);
            this.pnlTypeOptions.TabIndex = 0;
            // 
            // flowLayoutDownloadFrequency
            // 
            this.flowLayoutDownloadFrequency.Controls.Add(this.label4);
            this.flowLayoutDownloadFrequency.Controls.Add(this.cboDownloadType);
            this.flowLayoutDownloadFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutDownloadFrequency.Location = new System.Drawing.Point(0, 10);
            this.flowLayoutDownloadFrequency.Name = "flowLayoutDownloadFrequency";
            this.flowLayoutDownloadFrequency.Size = new System.Drawing.Size(760, 43);
            this.flowLayoutDownloadFrequency.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Files should be downloaded";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDownloadType
            // 
            this.cboDownloadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDownloadType.FormattingEnabled = true;
            this.cboDownloadType.Items.AddRange(new object[] {
            "at specific time each day",
            "at hourly intervals"});
            this.cboDownloadType.Location = new System.Drawing.Point(192, 3);
            this.cboDownloadType.Name = "cboDownloadType";
            this.cboDownloadType.Size = new System.Drawing.Size(259, 24);
            this.cboDownloadType.TabIndex = 3;
            this.cboDownloadType.SelectedIndexChanged += new System.EventHandler(this.cboDownloadType_SelectedIndexChanged);
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 132);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(766, 10);
            this.pnlSpacer.TabIndex = 5;
            // 
            // chkStartup
            // 
            this.chkStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartup.Location = new System.Drawing.Point(9, 274);
            this.chkStartup.Name = "chkStartup";
            this.chkStartup.Size = new System.Drawing.Size(330, 26);
            this.chkStartup.TabIndex = 6;
            this.chkStartup.Text = "Automatically start application on system startup";
            this.chkStartup.UseVisualStyleBackColor = true;
            this.chkStartup.CheckedChanged += new System.EventHandler(this.chkStartup_CheckedChanged);
            // 
            // DownloadSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkStartup);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pnlSpacer);
            this.Controls.Add(this.groupBox2);
            this.Name = "DownloadSettings";
            this.Size = new System.Drawing.Size(766, 318);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.pnlHourly.ResumeLayout(false);
            this.flowLayoutPanelHourlyOptions.ResumeLayout(false);
            this.flowLayoutPanelHourlyOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hourPicker)).EndInit();
            this.pnlDailyOptions.ResumeLayout(false);
            this.flowLayoutDownloadDailyOptions.ResumeLayout(false);
            this.flowLayoutDownloadDailyOptions.PerformLayout();
            this.pnlTypeOptions.ResumeLayout(false);
            this.flowLayoutDownloadFrequency.ResumeLayout(false);
            this.flowLayoutDownloadFrequency.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSettingsChangeFolder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel pnlHourly;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown hourPicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlDailyOptions;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlTypeOptions;
        private System.Windows.Forms.FolderBrowserDialog selectFolderDialog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutDownloadDailyOptions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutDownloadFrequency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboDownloadType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSettingsDownloadLocation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHourlyOptions;
        private System.Windows.Forms.Panel pnlSpacer;
        private System.Windows.Forms.CheckBox chkStartup;
    }
}
