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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonHarvestDataTransferApp.Data;
using CottonHarvestDataTransferApp.Remote;
using CottonHarvestDataTransferApp.Configuration;
using CottonHarvestDataTransferApp.Classes;
using System.Threading;
using System.Diagnostics;
using System.Net.NetworkInformation;
using CottonHarvestDataTransferApp.Logging;

namespace CottonHarvestDataTransferApp.UserControls
{
    /// <summary>
    /// This user control provides the user interface for establishing a 
    /// OAuth connection to the John Deere API.  It is used in both the
    /// initial setup screen and the connections tab.
    /// </summary>
    public partial class ConnectionSettings : UserControl
    {
        #region private properties
        IRemoteDataRepository remoteDataRepository = (IRemoteDataRepository)new JDRemoteDataRepository();
        private bool _showExitButton = false;
        private bool _firstRun = false;
        private string _exitButtonText = "Exit";
        private bool _loading = false;
        private string _currentUserId = "";
        #endregion

        #region private methods
        /// <summary>
        /// This method validates the verification code is not empty
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            var valid = true;
            if (string.IsNullOrEmpty(tbVerificationCode.Text))
            {
                valid = false;
                formErrorProvider.SetError(tbVerificationCode, "Verifier code is required.");
            }

            return valid;
        }
        #endregion

        #region private event handlers
        private void tbVerificationCode_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !ValidateForm();
        }

        private void btnConnectionsSettingsLinkAccount_Click(object sender, EventArgs e)
        {
            pnlConnectionSettingsVerifyCode.Visible = true;
            pnlConnectionSettingsLinkAccount.Visible = false;
            pnlConnectionSettingsConnected.Visible = false;
            pnlNoNetwork.Visible = false; 
        }
        
        private void btnGetVerifierCode_Click(object sender, EventArgs e)
        {
            string manualUrl = remoteDataRepository.GetManualAuthUrl();

            if (!string.IsNullOrWhiteSpace(manualUrl))
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(manualUrl);
                Process.Start(sInfo);
            }
            else
            {
                MessageBox.Show("Unable to retrieve authorization link.  Verify you are connected to the internet and try again.");
            }
        }

        private async void btnSubmitVerificationCode_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {   
                string oAuthToken = "";
                string oAuthTokenSecret = "";
                string newUserID = "";
                bool result = false;

                string originalToken = "";
                string originalSecret = "";

                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    var tokenSetting = dp.Settings.FindSingle(s => s.Key == SettingKeyType.JDAuthTokenKey);
                    var secretSetting = dp.Settings.FindSingle(s => s.Key == SettingKeyType.JDAuthSecretKey);

                    if (tokenSetting != null) originalToken = tokenSetting.Value;
                    if (secretSetting != null) originalSecret = secretSetting.Value;
                }

                pnlConnectionSettingsConnected.Visible = false;
                pnlConnectionSettingsLinkAccount.Visible = false;
                pnlConnectionSettingsVerifyCode.Visible = false;
                pnlNoNetwork.Visible = false;
                pnlLoading.Visible = true;
                lblLoadingMessage.Text = "Attempting to connect to your MyJohnDeere account";
                
                await Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(1000);
                    result = remoteDataRepository.ExchangeVerifierForCredentials(tbVerificationCode.Text, ref oAuthToken, ref oAuthTokenSecret);
                });

                if (result)
                {
                    remoteDataRepository.SetAuthData(oAuthToken, oAuthTokenSecret);                                        

                    lblLoadingMessage.Text = "Verifying connection";
                    await Task.Run(() => {
                        System.Threading.Thread.Sleep(1000);
                        newUserID = remoteDataRepository.TestConnection();
                    });

                    pnlLoading.Visible = false;

                    if (!string.IsNullOrEmpty(newUserID))
                    {                        
                        if (newUserID != _currentUserId && !_firstRun)  //switched accounts
                        {
                            if (MessageBox.Show("You have switched to a different account.  This will reset all application data.  Are you sure you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {                             
                                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                                {
                                    //remove all organizations saved
                                    dp.Organizations.DeleteAll();
                                    //save updated token and secret
                                    dp.Settings.UpdateSettingWithKey(SettingKeyType.JDAuthTokenKey, oAuthToken);
                                    dp.Settings.UpdateSettingWithKey(SettingKeyType.JDAuthSecretKey, oAuthTokenSecret);
                                    dp.SaveChanges();
                                }

                                pnlConnectionSettingsConnected.Visible = true;
                                pnlConnectionSettingsLinkAccount.Visible = false;
                                pnlConnectionSettingsVerifyCode.Visible = false;
                                _currentUserId = newUserID;
                                this.OnConnectionComplete(e);
                            }
                            else
                            {
                                //go back to initial screen state
                                oAuthToken = originalToken;
                                oAuthTokenSecret = originalSecret;

                                //reset auth token back to original
                                remoteDataRepository.SetAuthData(oAuthToken, oAuthTokenSecret);

                                pnlConnectionSettingsConnected.Visible = true;
                                pnlConnectionSettingsLinkAccount.Visible = false;
                                pnlConnectionSettingsVerifyCode.Visible = false;
                                this.OnConnectionComplete(e);
                            }
                        }
                        else
                        {
                            //handle same user just save new secret and token                            
                            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                            {
                                dp.Settings.UpdateSettingWithKey(SettingKeyType.JDAuthTokenKey, oAuthToken);
                                dp.Settings.UpdateSettingWithKey(SettingKeyType.JDAuthSecretKey, oAuthTokenSecret);
                                dp.SaveChanges();
                            }
                            _currentUserId = newUserID;
                            pnlConnectionSettingsConnected.Visible = true;
                            pnlConnectionSettingsLinkAccount.Visible = false;
                            pnlConnectionSettingsVerifyCode.Visible = false;
                            this.OnConnectionComplete(e);
                        }
                        remoteDataRepository.EmptyAllCacheItems();          
                    }
                    else
                    {
                        MessageBox.Show("Unable to establish connection. Verify you have a network connection.");
                        pnlConnectionSettingsVerifyCode.Visible = true;
                    }

                }
                else
                {
                    MessageBox.Show("Unable to establish connection.  Please check that you entered your verify token correctly.", "Error", MessageBoxButtons.OK);
                    pnlConnectionSettingsVerifyCode.Visible = true;
                }
                pnlLoading.Visible = false;
            }
        }

        private void btnConnectionSettingsLinkDifferentAccount_Click(object sender, EventArgs e)
        {
            pnlConnectionSettingsVerifyCode.Visible = true;
            pnlConnectionSettingsConnected.Visible = false;
            pnlConnectionSettingsLinkAccount.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.OnExitClicked(e);
        }

        private async void btnRetry_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }
        #endregion

        #region public properties
        [Category("Connection Events")]
        [Description("Fires when connection step complete.")]
        public event EventHandler ConnectionComplete;

        [Category("Connection Events")]
        [Description("Fires when exit clicked.")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ExitClicked;

        
        [Description("Shows exit button"), Category("Connection Settings"), DefaultValue(false)]
        public bool ShowExitButton
        {
            get
            {
                return _showExitButton;
            }
            set
            {
                _showExitButton = value;
                btnExit.Visible = _showExitButton;
            }
        }                

        [Description("Shows form in initial setup mode.."), Category("Connection Settings"), DefaultValue(false)]
        public bool FirstRun { get { return _firstRun; } set { _firstRun = value; } }

        [Description("Exit button text"), Category("Connection Settings"), DefaultValue(false)]
        public string ExitButtonText { get { return _exitButtonText; } set { _exitButtonText = value; btnExit.Text = _exitButtonText; } }
        #endregion

        #region custom event handlers
        protected virtual void OnConnectionComplete(EventArgs e)
        {
            EventHandler handler = this.ConnectionComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnExitClicked(EventArgs e)
        {
            EventHandler handler = this.ExitClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Checks for a connection and initializes UI for connections
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            if (!_loading || _firstRun)
            {
                _loading = true;

                remoteDataRepository.Initialize(AppConfig.JohnDeereApiUrl, RemoteProviderType.OAuthProvider,
                                        AppConfig.AppId, AppConfig.AppSecret, string.Empty, string.Empty);

                btnExit.Visible = _showExitButton;
                if (_firstRun)
                {
                    pnlConnectionSettingsConnected.Visible = false;
                    pnlConnectionSettingsLinkAccount.Visible = false;
                    pnlConnectionSettingsVerifyCode.Visible = true;
                    pnlNoNetwork.Visible = false;
                    return;
                }

                pnlConnectionSettingsConnected.Visible = false;
                pnlConnectionSettingsLinkAccount.Visible = false;
                pnlConnectionSettingsVerifyCode.Visible = false;
                pnlNoNetwork.Visible = false;
                pnlLoading.Visible = true;
                lblLoadingMessage.Text = "Checking for connection";
                bool connected = false;
                bool noNetwork = false;

                await Task.Run(() =>
                {

                    System.Threading.Thread.Sleep(1000);

                //check network state
                if (!NetworkHelper.HasInternetConnection(AppConfig.JohnDeereApiUrl))
                    {
                        connected = false;
                        noNetwork = true;
                    }
                    else
                    {
                    //we have a network connection so verify we can connect to John Deere
                    using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                        {
                            var tokenSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.JDAuthTokenKey);
                            var secretSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.JDAuthSecretKey);
                            if (tokenSetting != null && secretSetting != null)
                            {
                                try
                                {
                                    remoteDataRepository.SetAuthData(tokenSetting.Value, secretSetting.Value);
                                    _currentUserId = remoteDataRepository.TestConnection();
                                    if (_currentUserId != string.Empty)
                                    {
                                        connected = true;
                                    }
                                }
                                catch (Exception exc)
                                {
                                    Logger.Log(exc);
                                    connected = false;
                                }
                            }
                        }
                    }
                });

                pnlLoading.Visible = false;
                if (noNetwork == false)
                {
                    pnlConnectionSettingsConnected.Visible = connected;
                    pnlConnectionSettingsLinkAccount.Visible = !connected;
                    pnlConnectionSettingsVerifyCode.Visible = false;
                    pnlNoNetwork.Visible = false;
                }
                else
                {
                    pnlConnectionSettingsConnected.Visible = false;
                    pnlConnectionSettingsLinkAccount.Visible = false;
                    pnlConnectionSettingsVerifyCode.Visible = false;
                    pnlNoNetwork.Visible = true;
                }

                _loading = false;
            }
        }
        #endregion

        public ConnectionSettings()
        {   
            InitializeComponent();
        }        
    }
}
