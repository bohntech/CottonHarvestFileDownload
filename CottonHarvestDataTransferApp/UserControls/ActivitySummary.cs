/****************************************************************************
The MIT License(MIT)

Copyright(c) 2016 Bohn Technology Solutions, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using CottonHarvestDataTransferApp.Data;
using CottonHarvestDataTransferApp.Remote;
using CottonHarvestDataTransferApp.Configuration;
using CottonHarvestDataTransferApp.Helpers;
using CottonHarvestDataTransferApp.Classes;
using CottonHarvestDataTransferApp.Logging;

namespace CottonHarvestDataTransferApp.UserControls
{
    public partial class ActivitySummary : UserControl
    {
        #region private properties
        private List<OutputFile> recentFiles = new List<OutputFile>();
        
        private bool downloadRunning = false;
        
        IRemoteDataRepository remoteDataRepository = (IRemoteDataRepository)new JDRemoteDataRepository();
                
        private int extractErrors = 0;
        private string downloadFolder = "";
        private int batchNumber = 1;
        #endregion

        #region private methods
        /// <summary>
        /// This method handles updating the UI with file processing progress.
        /// </summary>
        /// <param name="result"></param>
        private void DownloadProgress(FileDownloadProgress result)
        {
            Logger.Log("INFO", string.Format("Processing file {0} of {1}", result.CurrentFile, result.TotalFiles));
            this.Invoke((MethodInvoker)delegate {
                lblStatusValue.Text = string.Format("Processing file {0} of {1}", result.CurrentFile, result.TotalFiles);
            });
        }

        /// <summary>
        /// Invoked when an error occurres downloading a file
        /// </summary>
        /// <param name="result"></param>
        private void FileDownloadError(FileDownloadedResult result)
        {
            extractErrors++;
        }

        /// <summary>
        /// Callback method that is called when a file is successfully downloaded.   This
        /// method extracts the ZIP file and produces the final output file.
        /// </summary>
        /// <param name="result"></param>
        private void FileDownloaded(FileDownloadedResult result)
        {
            try
            {
                if (result.Filename.ToLower().Trim().EndsWith(".zip"))
                {
                    Logger.Log("INFO", string.Format("Extracting {0}", result.Filename));
                    ZipFile.ExtractToDirectory(result.Filename, downloadFolder + result.OrganizationID + "-" + result.FileIdentifier.ToString());

                    System.IO.File.Delete(result.Filename);

                    FileLocator locator = new FileLocator(downloadFolder + result.OrganizationID + "-" + result.FileIdentifier.ToString());

                    var filenames = locator.FindHIDFiles();

                    using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                    {

                        int oldLineCount = 0;
                        Organization org = dp.Organizations.FindSingle(x => x.RemoteID == result.OrganizationID);
                        int currentFileCount = 1;

                        foreach (var filename in filenames)
                        {
                            //instantiate parser which will read file
                            CSVFileParser parser = new CSVFileParser(filename);

                            //if no data in file then no reason to do anything
                            if (!string.IsNullOrEmpty(parser.FirstLineText) && parser.LineCount > 1 && !string.IsNullOrWhiteSpace(parser.MachineID))
                            {
                                //look to see if we have read another file with the same data appearing at the start of the file
                                //new files may get uploaded that are really the same data with additional modules
                                //appended.
                                var files = dp.SourceFiles.FindMatching(x => x.FirstLineText == parser.FirstLineText && org.Id == x.OrganizationId);
                                SourceFile file = null;

                                //get source file that had the highest line count
                                if (files != null && files.Count() > 0)
                                {
                                    file = files.OrderByDescending(x => x.LineCount).ToList()[0];
                                }

                                var shortName = filename.Replace(downloadFolder, "");
                                OutputFile outputFile = new OutputFile();
                                outputFile.Filename = downloadFolder.Replace("\\temp", "") + parser.MachineID.ToUpper().Trim() + "_"
                                + DateTime.Now.ToString("yyyyMMddHHmmss_fff") + ".TXT";
                                outputFile.Created = DateTime.Now;

                                oldLineCount = 0;

                                if (file != null)
                                {
                                    oldLineCount = file.LineCount;
                                }

                                //add a record for this file id - file will not be downloaded in 
                                //subsequent downloads
                                SourceFile newFile = new SourceFile();
                                newFile.BatchNumber = batchNumber;
                                newFile.FileIdentifer = result.FileIdentifier;
                                newFile.SourceFilename = shortName;
                                newFile.FirstDownload = DateTime.Now;
                                newFile.LastDownload = DateTime.Now;
                                newFile.OrganizationId = org.Id;
                                newFile.LineCount = parser.LineCount;
                                newFile.FirstLineText = parser.FirstLineText.ToLower().Trim();

                                if (parser.LineCount > oldLineCount)
                                {
                                    newFile.OutputFiles.Add(outputFile);
                                    parser.WriteFile(outputFile.Filename, oldLineCount);
                                }

                                dp.SourceFiles.Add(newFile);

                                dp.SaveChanges();
                                currentFileCount++;
                            }
                            else
                            {
                                Logger.Log("WARNING", "Empty file downloaded ignoring.");
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                extractErrors++;
            }
        }

        /// <summary>
        /// Initiates the update of local partnerships and then downloads files for
        /// relevant partners
        /// </summary>
        /// <returns></returns>
        private async Task executeDownload()
        {
            DateTime startTime = DateTime.Now;
            if (!downloadRunning)
            {
                Logger.Log("INFO", "Starting download");

                downloadRunning = true;                
                extractErrors = 0;
                //kick off process to download all files
                List<Partner> results = null;
                string message = "";
                ImportPartnerResult importResult = new ImportPartnerResult();

                //first check for new partner relationships
                this.Invoke((MethodInvoker)delegate
                {
                    lblStatusValue.Text = "Getting download information...";
                    lblStatusValue.ForeColor = Color.Green;
                });

                await Task.Run(async () =>
                {
                    try
                    {
                        using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                        {
                            //save last download time at beginning so another download doesn't start if 
                            //process takes longer
                            dp.Settings.UpdateSettingWithKey(SettingKeyType.LastDownload, startTime.ToString());
                            dp.SaveChanges();
                        }

                        //TODO: NEED A WAY TO PROPAGATE MESSAGE BACK TO SCREEN
                        importResult = await DataHelper.ImportPartners(results, remoteDataRepository, false);
                    }
                    catch (Exception exc)
                    {
                        extractErrors++;
                        Logger.Log(exc);
                    }
                });

                
                await Task.Run(async () =>
                {
                    try
                    {
                        using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                        {                          

                            downloadFolder = dp.Settings.GetDownloadFolder().TrimEnd('\\') + "\\";
                            downloadFolder += "temp\\";

                            //clean up previous download attempt
                            if (System.IO.Directory.Exists(downloadFolder))
                            {
                                System.IO.Directory.Delete(downloadFolder, true);
                            }

                            int fileCountOld = dp.OutputFiles.FileCount();
                            batchNumber = dp.SourceFiles.GetNextBatchNumber();
                            var orgIds = dp.Organizations.GetAll().Select(o => o.RemoteID).ToArray();
                            var fileIds = dp.SourceFiles.GetFileIds();

                            List<OrgFileETag> savedETags = new List<OrgFileETag>();
                            var orgs = dp.Organizations.GetAll();
                            foreach (var o in orgs)
                            {
                                savedETags.Add(new OrgFileETag { OrgId = o.RemoteID, CreatedDate = o.FilesETagDate, Tag = o.FilesETag });
                            }

                            Logger.Log("INFO", "Start download all files.");
                            var updatedETags = await remoteDataRepository.DownloadOrganizationFiles(savedETags, downloadFolder, orgIds, fileIds, FileDownloaded, DownloadProgress, FileDownloadError);
                            Logger.Log("INFO", "Finished download all files.");
                            
                            //update organization record with new file eTags
                            foreach (var o in orgs)
                            {
                                var tag = updatedETags.SingleOrDefault(t => t.OrgId == o.RemoteID);
                                if (tag != null)
                                {
                                    o.FilesETag = tag.Tag;
                                    o.FilesETagDate = tag.CreatedDate;
                                }                               
                            }
                            dp.SaveChanges();

                            int fileCountNew = dp.OutputFiles.FileCount();

                            Logger.Log("INFO", "Old file count: " + fileCountOld.ToString());
                            Logger.Log("INFO", "New file count: " + fileCountNew.ToString());

                            if (System.IO.Directory.Exists(downloadFolder))
                            {
                                System.IO.Directory.Delete(downloadFolder, true);
                            }

                            if (fileCountNew > fileCountOld)
                                OnNewFilesDownloaded(new EventArgs());
                        }
                    }
                    catch (Exception exc)
                    {
                        extractErrors++;
                        Logger.Log(exc);
                    }                    
                });

                await LoadRecentFilesAsync();

                this.Invoke((MethodInvoker)delegate
                {
                    lblNextDownloadValue.Text = "--";
                    if (extractErrors > 0)
                    {
                        lblStatusValue.Text = "Completed with errors at " + DateTime.Now.ToString();
                        lblStatusValue.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatusValue.Text = "Completed successfully at " + DateTime.Now.ToString();
                        lblStatusValue.ForeColor = Color.Green;
                    }
                });

                downloadRunning = false;
            }
        }


        /// <summary>
        /// Runs a simple check to verify connectivity to remote API
        /// </summary>
        /// <returns></returns>
        private async Task checkConnection()
        {
            bool connected = false;
            lblStatusValue.Text = "Checking connection...";
            await Task.Run(async () => {
                System.Threading.Thread.Sleep(1500);
                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    //if we have credentials test connection                   
                    connected = ((await remoteDataRepository.TestConnection()) != string.Empty);                    
                }
            });

            this.Invoke((MethodInvoker)delegate
            {
                if (connected)
                {
                    lblStatusValue.Text = "Connected";
                    lblStatusValue.ForeColor = Color.Green;
                }
                else
                {
                    lblStatusValue.Text = "Not Connected";
                    lblStatusValue.ForeColor = Color.Red;
                }
            });
        }

        private void SetDownloadFolderLabel()
        {
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                lblDownloadFolderValue.Text = dp.Settings.GetDownloadFolder().TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Asynchronously loads the recent files grid
        /// </summary>
        /// <returns></returns>
        private async Task LoadRecentFilesAsync()
        {            
            await Task.Run(() =>
            {
                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    recentFiles.Clear();
                    recentFiles = dp.OutputFiles.GetRecentFiles();
                }
            });

            this.Invoke((MethodInvoker)delegate
            {
                dgRecentFiles.Rows.Clear();
                dgRecentFiles.ShowCellToolTips = true;
                foreach (var file in recentFiles)
                {
                    string shortName = ".." + file.Filename.Substring(file.Filename.LastIndexOf("\\"));
                    
                    var row = new DataGridViewRow();                    
                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    DataGridViewLinkCell c2 = new DataGridViewLinkCell();                    

                    c1.Value = file.SourceFile.Organization.Name;
                    c2.Value = shortName;                    
                    c2.ToolTipText = file.Filename;                    

                    row.Cells.Add(c1);
                    row.Cells.Add(c2);

                    dgRecentFiles.Rows.Add(row);              
                    
                }
            });
        }

        #endregion

        #region private event handlers
        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                //downloadSettingsControl.SelectedDownloadFolder = folderDialog.SelectedPath;
                lblDownloadFolderValue.Text = folderDialog.SelectedPath;

                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    dp.Settings.UpdateSettingWithKey(SettingKeyType.DownloadFolderKey, folderDialog.SelectedPath.TrimEnd('\\'));
                    dp.SaveChanges();
                }
            }
        }

        private void btnManageConnection_Click(object sender, EventArgs e)
        {
            this.OnManageConnectionClicked(e);
            //            applicationTabControl.SelectedIndex = 3;
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            string folder = "";
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                folder = dp.Settings.GetDownloadFolder();
            }

            if (!string.IsNullOrEmpty(folder) && System.IO.Directory.Exists(folder))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = folder,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private async void timerControl_Tick(object sender, EventArgs e)
        {
            timerControl.Enabled = false;
            try
            {
                Setting downloadFrequencyTypeSetting = null;
                Setting hourlyDownloadTimeSetting = null;
                Setting dailyDownloadTimeSetting = null;
                Setting lastDownloadSetting = null;

                DateTime? lastDownload = null;
                DateTime? nextDownloadTime = null;

                await Task.Run(async () =>
                {
                    Logger.CleanUp();
                    using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                    {
                        downloadFrequencyTypeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.DownloadFrequencyTypeKey);
                        hourlyDownloadTimeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.HourlyDownloadTimeKey);
                        dailyDownloadTimeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.DailyDownloadTimeKey);
                        lastDownloadSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.LastDownload);

                        if (lastDownloadSetting != null)
                        {
                            lastDownload = DateTime.Parse(lastDownloadSetting.Value);
                        }
                        else
                        {
                            lastDownload = DateTime.Now;
                            dp.Settings.UpdateSettingWithKey(SettingKeyType.LastDownload, lastDownload.ToString());
                        }

                        if (downloadFrequencyTypeSetting != null)
                        {
                            DownloadIntervalType IntervalType = (DownloadIntervalType)int.Parse(downloadFrequencyTypeSetting.Value);

                            if (IntervalType == DownloadIntervalType.HourlyIntervals)
                            {
                                if (hourlyDownloadTimeSetting != null)
                                {
                                    int hours = int.Parse(hourlyDownloadTimeSetting.Value);

                                    nextDownloadTime = lastDownload.Value.AddHours(hours);

                                    if (lastDownload.Value.AddHours(hours) <= DateTime.Now)
                                    {
                                        await executeDownload();
                                    }
                                }
                            }
                            else  //must be time of day download type
                            {
                                if (dailyDownloadTimeSetting != null)
                                {
                                    DateTime dailyDownloadTime = DateTime.Parse(dailyDownloadTimeSetting.Value);
                                    DateTime current = DateTime.Now;

                                    nextDownloadTime = new DateTime(current.Year, current.Month, current.Day, dailyDownloadTime.Hour, dailyDownloadTime.Minute, 0);

                                    if (nextDownloadTime < current)
                                    {
                                        nextDownloadTime = nextDownloadTime.Value.AddDays(1);
                                    }

                                    if (current.Hour == dailyDownloadTime.Hour && current.Minute == dailyDownloadTime.Minute)
                                    {
                                        await executeDownload();
                                    }
                                }
                            }
                        }
                    }
                });

                if (nextDownloadTime.HasValue)
                {
                    lblNextDownloadValue.Text = nextDownloadTime.ToString();
                }
                else
                {
                    lblNextDownloadValue.Text = "--";
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }

            timerControl.Enabled = true;
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Logger.CurrentLogFile))
            {
                System.Diagnostics.Process.Start(Logger.CurrentLogFile);
            }
            else
            {
                MessageBox.Show("Log file does not exist.");
            }
        }

        private void dgRecentFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgRecentFiles.Columns[e.ColumnIndex].GetType() == typeof(DataGridViewLinkColumn))
            {
                System.Diagnostics.Process.Start(dgRecentFiles.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText);
            }
        }

        private async void btnDownloadNow_Click(object sender, EventArgs e)
        {
            await executeDownload();
        }

        private void summaryInformationGroupBox_Enter(object sender, EventArgs e)
        {

        }
        #endregion

        #region custom events
        [Category("Control Events")]
        [Description("Fires connection change button clicked.")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ManageConnectionClicked;             

        protected virtual void OnNewFilesDownloaded(EventArgs e)
        {
            EventHandler handler = this.NewFilesDownloaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [Category("Control Events")]
        [Description("New Files Downloaded")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler NewFilesDownloaded;

        protected virtual void OnManageConnectionClicked(EventArgs e)
        {
            EventHandler handler = this.ManageConnectionClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region public methods
        public void SetStatusMessage(string message, Color c)
        {
            lblStatusValue.Text = message;
            lblStatusValue.ForeColor = c;
        }

        public async Task Initialize(IRemoteDataRepository repo, bool checkConnection)
        {
            remoteDataRepository = repo;
            timerControl.Enabled = true;

            if (checkConnection)
            {
                await this.checkConnection();
            }
            SetDownloadFolderLabel();
            await LoadRecentFilesAsync();
        }
        #endregion

        public ActivitySummary()
        {
            InitializeComponent();
        }

        
    }
}
