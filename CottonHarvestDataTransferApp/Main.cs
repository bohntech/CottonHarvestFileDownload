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
using CottonHarvestDataTransferApp.Logging;
using CottonHarvestDataTransferApp.Classes;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CottonHarvestDataTransferApp
{    

    public partial class Main : Form
    {
        #region private properties                

        IRemoteDataRepository remoteDataRepository = (IRemoteDataRepository)new JDRemoteDataRepository();
        private string accessToken = "";
        private string refreshToken = "";
        private bool _loading = false;
        FirstLaunchForm setupForm = null;

        #endregion

        #region external user32.dll imports
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);
        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void connectionSettingsControl_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region private methods
        /// <summary>
        /// This method brings the appropriate window to the foreground
        /// </summary>
        public void focusApplication()
        {
            if (setupForm != null && setupForm.Visible)
            {               
                this.WindowState = FormWindowState.Normal;
                SetForegroundWindow(this.Handle);
                setupForm.WindowState = FormWindowState.Normal;
                SetForegroundWindow(setupForm.Handle);
            }
            else
            {               
                Show();
                this.WindowState = FormWindowState.Normal;
                SetForegroundWindow(this.Handle);
            }
        }

        private void initRemoteRepo()
        {
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                accessToken = dp.Settings.GetAsString(SettingKeyType.JDAccessToken);
                refreshToken = dp.Settings.GetAsString(SettingKeyType.JDRefreshToken);
            }
            remoteDataRepository.Initialize(AppConfig.JohnDeereApiUrl, AppConfig.JohnDeereWellKnownUrl, RemoteProviderType.OAuthProvider, 
                DataHelper.SaveNewToken, DataHelper.IsTokenExpired,                
                AppConfig.AppId, AppConfig.AppSecret, accessToken, refreshToken);
        }
        #endregion

        #region protected overrides
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ClassStyle = 0x200;
                return parms;
            }
        }
        #endregion

        #region private events
        private void activitySummaryControl_ManageConnectionClicked(object sender, EventArgs e)
        {
            applicationTabControl.SelectedIndex = 3;
        }

        /// <summary>
        /// Handles changing tabs and initializing the new tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void applicationTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabSettings)
            {
                await downloadSettingsControl.LoadDataAsync();
            }
            else if (e.TabPage == tabConnectionSettings)
            {
                await connectionSettingsControl.LoadDataAsync();
            }
            else if (e.TabPage == tabManagePartners)
            {
                managePartnersControl.SetRemoteDataRepository(remoteDataRepository);
                await managePartnersControl.LoadDataAsync();
            }
            else if (e.TabPage == tabActivitySummary)
            {                
                await activitySummaryControl.Initialize(remoteDataRepository, false);
            }            
        }
                
        private async void connectionSettingsControl_ExitClicked(object sender, EventArgs e)
        {
            await connectionSettingsControl.LoadDataAsync();
        }              

        /// <summary>
        /// When window state is set to minimized hide window since it
        /// will remain visible in the system tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }


        private void notifyIconControl_DoubleClick(object sender, EventArgs e)
        {
            focusApplication();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            focusApplication();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setupForm != null && setupForm.Visible)
            {
                setupForm.Close();                
            }
            this.Close();
        }

        private void activitySummaryControl_NewFilesDownloaded(object sender, EventArgs e)
        {
            notifyIconControl.ShowBalloonTip(5000, "Cotton Harvest File Download", "New data files have been downloaded.", ToolTipIcon.Info);
        }

        private void notifyIconControl_BalloonTipClicked(object sender, EventArgs e)
        {
            focusApplication();
        }

        private void Main_Activated(object sender, EventArgs e)
        {            
            if (!_loading)
                focusApplication();
        }        
                
        private async void Main_Load(object sender, EventArgs e)
        {
            _loading = true;
            try
            {
                string arguments = "Application loading with arguments: ";
                foreach (var s in Environment.GetCommandLineArgs())
                {
                    arguments += s + " ";
                }
                Logger.Log("INFO", arguments);
                Logger.Log("INFO", "Main_Load");

                this.WindowState = FormWindowState.Minimized;
             
                initRemoteRepo();

                downloadSettingsControl.AutoSaveChanges = true;

                string downloadFolderSetting = "";

                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    downloadFolderSetting = dp.Settings.GetDownloadFolder();
                }

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(downloadFolderSetting))
                {
                    Logger.Log("INFO", "Launching setup form");
                    setupForm = new FirstLaunchForm();                    
                    focusApplication();
                    if (setupForm.ShowDialog() == DialogResult.OK)
                    {
                        activitySummaryControl.SetStatusMessage("Connected", Color.Green);
                        await activitySummaryControl.Initialize(remoteDataRepository, false);                        
                        setupForm = null;
                        focusApplication();
                    }
                    else
                    {                        
                        Application.Exit();
                    }
                }
                else
                {
                    await activitySummaryControl.Initialize(remoteDataRepository, true);
                    var args = Environment.GetCommandLineArgs();
                    //if started from startup then minimize
                    if (args.Length >= 2 && args[1] == "-minimized")
                    {
                        Logger.Log("INFO", "Starting minimized");
                        this.WindowState = FormWindowState.Minimized;
                        this.Hide();
                    }
                    else
                    {
                        Logger.Log("INFO", "Starting normal");
                        this.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);                
                Application.Exit();
            }
            _loading = false;
        }

        private async void connectionSettingsControl_ConnectionComplete(object sender, EventArgs e)
        {
            //refresh remote API connection after account has been changed or connected
            remoteDataRepository.EmptyAllCacheItems();
            initRemoteRepo();
            await activitySummaryControl.Initialize(remoteDataRepository, false);
        }        

        private void notifyIconControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                focusApplication();
            }
        }
        #endregion

        public Main()
        {
            InitializeComponent();   
        }
        
    }
}
