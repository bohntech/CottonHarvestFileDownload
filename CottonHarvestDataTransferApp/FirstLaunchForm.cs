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
using JdAPI;
using JdAPI.Client;
using CottonHarvestDataTransferApp.Data;
using CottonHarvestDataTransferApp.Configuration;
using CottonHarvestDataTransferApp.Remote;

namespace CottonHarvestDataTransferApp
{
    /// <summary>
    /// This form contains a wizard to walk the user through initial setup.
    /// The form will configure API authorization, download folder location,
    /// and download schedule settings.  
    /// </summary>
    public partial class FirstLaunchForm : Form
    {
        #region private properties
        IRemoteDataRepository remoteDataRepository = (IRemoteDataRepository)new JDRemoteDataRepository();
        #endregion

        #region private methods
        /// <summary>
        /// Shows message box confirming application exit and sets dialog result
        /// </summary>
        private void confirmExit()
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }
        #endregion

        #region private events
        private void btnStep2Finish_Click(object sender, EventArgs e)
        {
            if (downloadSettingsControl.SelectedDownloadFolder == string.Empty)
            {
                MessageBox.Show("Please select a download folder to save files into.");
            }
            else
            {
                downloadSettingsControl.SaveCurrentSettings();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private async void FirstLaunchForm_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            await connectionSettingsControl.LoadDataAsync();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            confirmExit();
        }

        private void connectionSettingsControl_ConnectionComplete(object sender, EventArgs e)
        {
            downloadSettingsControl.SelectedDownloadFolder = "";
            pnlStep1.Visible = false;
            pnlStep2.Visible = true;
        }

        private void connectionSettingsControl_ExitClicked(object sender, EventArgs e)
        {
            confirmExit();
        }
        #endregion

        public FirstLaunchForm()
        {
            InitializeComponent();

            string oAuthToken = "";
            string oAuthSecret = "";

            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                var token = dp.Settings.FindSingle(k => k.Key == SettingKeyType.JDAuthTokenKey);
                var secret = dp.Settings.FindSingle(k => k.Key == SettingKeyType.JDAuthSecretKey);

                if (token != null && secret != null && !string.IsNullOrWhiteSpace(token.Value) && !string.IsNullOrWhiteSpace(secret.Value))
                {
                    oAuthSecret = secret.Value.Trim();
                    oAuthToken = token.Value.Trim();
                }
            }

            remoteDataRepository.Initialize(AppConfig.JohnDeereApiUrl, RemoteProviderType.OAuthProvider, AppConfig.AppId, AppConfig.AppSecret, oAuthToken, oAuthSecret);
        }

        private void connectionSettingsControl_Load(object sender, EventArgs e)
        {

        }
    }
}
