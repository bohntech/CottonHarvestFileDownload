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
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using JdAPI;
using JdAPI.Client;
using CottonHarvestDataTransferApp.Data;
using IWshRuntimeLibrary;

namespace CottonHarvestDataTransferApp.UserControls
{
    /// <summary>
    /// This user control is used to provide the user interface for
    /// managing download settings.   It us used in both the first
    /// launch screens and the download settings tab of the main
    /// application.
    /// </summary>
    public partial class DownloadSettings : UserControl
    {

        #region constants
        private const string NO_SELECTED_FOLDER_TEXT = "-- None selected --";        
        #endregion

        #region private properties
        private string _selectedDownloadFolder = string.Empty;
        private bool _autoSaveChanges = false;
        private bool loading = false;
        #endregion

        #region private methods
        /// <summary>
        /// Sets visibility of time interval options based on selected download interval type
        /// </summary>
        private void SetPanelVisibility()
        {
            if (cboDownloadType.SelectedIndex == 0)
            {
                pnlDailyOptions.Visible = true;
                pnlHourly.Visible = false;
            }
            else
            {
                pnlHourly.Visible = true;
                pnlDailyOptions.Visible = false;
            }
        }
        #endregion

        #region private event handlers
        private void cboDownloadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPanelVisibility();

            if (_autoSaveChanges)
                SaveCurrentSettings();
        }

        private void btnSettingsChangeFolder_Click(object sender, EventArgs e)
        {
            //if a folder has already be set default the selection dialog to
            //the currently selected folder.  
            if (!string.IsNullOrEmpty(lblSettingsDownloadLocation.Text) &&
                lblSettingsDownloadLocation.Text.Trim() != NO_SELECTED_FOLDER_TEXT)
            {
                selectFolderDialog.SelectedPath = SelectedDownloadFolder;
            }

            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                lblSettingsDownloadLocation.Text = selectFolderDialog.SelectedPath;
                SelectedDownloadFolder = selectFolderDialog.SelectedPath;

                if (_autoSaveChanges)
                    SaveCurrentSettings();
            }
        }

        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            if (_autoSaveChanges)
                SaveCurrentSettings();
        }

        private void hourPicker_ValueChanged(object sender, EventArgs e)
        {
            if (_autoSaveChanges)
                SaveCurrentSettings();
        }

        private void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && _autoSaveChanges)  //no need to trigger save/delete when value set from load method
            {
                SaveCurrentSettings();
            }
        }
        #endregion

        #region public properties
        /// <summary>
        /// If true changes to download settings are saved instantly when the value is change
        /// </summary>
        public bool AutoSaveChanges
        {
            get
            {
                return _autoSaveChanges;
            }
            set
            {
                _autoSaveChanges = value;
            }
        }

        /// <summary>
        /// The folder that is configured as the download location
        /// </summary>
        public string SelectedDownloadFolder
        {
            get
            {
                return _selectedDownloadFolder;
            }
            set
            {
                _selectedDownloadFolder = value;

                if (_selectedDownloadFolder == string.Empty)
                {
                    lblSettingsDownloadLocation.Text =  NO_SELECTED_FOLDER_TEXT;
                }
                else
                {
                    lblSettingsDownloadLocation.Text = value;
                }
            }
        }

        /// <summary>
        /// The type of download schedule hourly or daily
        /// </summary>
        public DownloadIntervalType IntervalType
        {
            get
            {
                return (DownloadIntervalType)cboDownloadType.SelectedIndex;
            }
        }

        /// <summary>
        /// The time for daily downloads if interval type is daily
        /// </summary>
        public DateTime DailyDownloadTime
        {
            get
            {
                return timePicker.Value;
            }
        }

        /// <summary>
        /// The hourly download interval if interval type is hourly
        /// </summary>
        public int HourlyDownloadInterval
        {
            get
            {
                return Convert.ToInt32(hourPicker.Value);
            }
        }
       
        #endregion

        private void createShortcut()
        {
            var wshCSClass = new WshShellClass();
            IWshRuntimeLibrary.IWshShortcut appShortcut;
            appShortcut = (IWshRuntimeLibrary.IWshShortcut)wshCSClass.CreateShortcut(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonHarvestFileDownloadUtility.lnk"));
            appShortcut.TargetPath = Application.ExecutablePath;
            appShortcut.Description = "Start Cotton Harvest File Download Utility";
            appShortcut.IconLocation = Application.StartupPath + @"\Resources\favicon.ico";
            appShortcut.Arguments = " -minimized";
            appShortcut.WindowStyle = 7;
            appShortcut.Save();            
        }

        #region public methods
        /// <summary>
        /// Saves the currently selected settings to the database
        /// </summary>
        public void SaveCurrentSettings()
        {
            //save settings
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                dp.Settings.UpdateSettingWithKey(SettingKeyType.DownloadFolderKey, SelectedDownloadFolder);
                dp.Settings.UpdateSettingWithKey(SettingKeyType.DownloadFrequencyTypeKey, ((int)IntervalType).ToString());

                dp.Settings.UpdateSettingWithKey(SettingKeyType.HourlyDownloadTimeKey, HourlyDownloadInterval.ToString());
                dp.Settings.UpdateSettingWithKey(SettingKeyType.DailyDownloadTimeKey, DailyDownloadTime.ToString());

                dp.SaveChanges();
            }

            //make sure checkbox value is saved            
            var shortcutFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonHarvestFileDownloadUtility.lnk");
            if (chkStartup.Checked)
            {
                if (!System.IO.File.Exists(shortcutFile))
                {
                    createShortcut();
                }
            }
            else
            {
                if (System.IO.File.Exists(shortcutFile))
                {
                    System.IO.File.Delete(shortcutFile);
                }
            }
        }

        /// <summary>
        /// Loads settings data and initializes UI
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            loading = true;
            Setting downloadFolderSetting = null;
            Setting downloadFrequencyTypeSetting = null;
            Setting hourlyDownloadTimeSetting = null;
            Setting dailyDownloadTimeSetting = null;
                        

            chkStartup.Checked = false;

            var shortcutFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonHarvestFileDownloadUtility.lnk");

            chkStartup.Checked = System.IO.File.Exists(shortcutFile);
                    

            await Task.Run(() =>
            {
                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    downloadFolderSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.DownloadFolderKey);
                    downloadFrequencyTypeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.DownloadFrequencyTypeKey);
                    hourlyDownloadTimeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.HourlyDownloadTimeKey);
                    dailyDownloadTimeSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.DailyDownloadTimeKey);
                }
            });

            if (downloadFolderSetting != null)
                SelectedDownloadFolder = downloadFolderSetting.Value;

            if (downloadFrequencyTypeSetting != null)
                cboDownloadType.SelectedIndex = int.Parse(downloadFrequencyTypeSetting.Value);

            if (hourlyDownloadTimeSetting != null)
                hourPicker.Value = decimal.Parse(hourlyDownloadTimeSetting.Value);

            if (dailyDownloadTimeSetting != null)
                timePicker.Value = DateTime.Parse(dailyDownloadTimeSetting.Value);

            SetPanelVisibility();
            loading = false;
        }
        #endregion
        
        public DownloadSettings()
        {
            InitializeComponent();

            //load download settings
            cboDownloadType.SelectedIndex = 0;
            pnlDailyOptions.Visible = true;
            pnlHourly.Visible = false;
        }

        
    }
}
