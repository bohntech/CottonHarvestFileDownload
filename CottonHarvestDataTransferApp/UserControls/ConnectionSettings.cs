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
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using CottonHarvestDataTransferApp.Helpers;

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
        private string _connectionsUrl = "";

        
        private string authResponseFilename;

        private Process openProcess = null;
        #endregion

        #region private methods
        
        private void btnConnectionsSettingsLinkAccount_Click(object sender, EventArgs e)
        {
            pnlConnectionSettingsVerifyCode.Visible = true;
            pnlConnectionSettingsLinkAccount.Visible = false;
            pnlConnectionSettingsConnected.Visible = false;
            pnlNoNetwork.Visible = false; 
        }
        
        private void btnGetVerifierCode_Click(object sender, EventArgs e)
        {
           
        }

        private void ClearAuthResponseText()
        {           
            //delete any existing auth response file
            if (System.IO.File.Exists(authResponseFilename))
            {
                System.IO.File.Delete(authResponseFilename);
            }
        }

        private async void btnSubmitVerificationCode_Click(object sender, EventArgs e)
        {
            ClearAuthResponseText();
                       
            Dictionary<string, object> oAuthMetadata = await GetOAuthMetadata(AppConfig.JohnDeereWellKnownUrl);
            string authEndpoint = oAuthMetadata["authorization_endpoint"].ToString();
                  
            string redirectUrl = authEndpoint + "?response_type=code&scope=" + System.Web.HttpUtility.UrlEncode(AppConfig.Scopes) +
                                 "&client_id=" + System.Web.HttpUtility.UrlEncode(AppConfig.AppId) +
                                 "&state=" + System.Web.HttpUtility.UrlEncode("test state") +
                                 "&redirect_uri=cottonutil%3A%2F%2Flocalhost%2Fcallback";

            fileSystemWatcher.EnableRaisingEvents = true;
            
            //launch web browser to well known url
            ProcessStartInfo sInfo = new ProcessStartInfo(redirectUrl);
            //sInfo.Arguments = " --new-window";
            openProcess = Process.Start(sInfo);

            pnlConnectionSettingsConnected.Visible = false;
            pnlConnectionSettingsLinkAccount.Visible = false;
            pnlConnectionSettingsVerifyCode.Visible = false;
            pnlNoNetwork.Visible = false;
            pnlLoading.Visible = true;
            lblLoadingMessage.Text = "Attempting to connect to your MyJohnDeere account";
           
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

        private static async Task<Dictionary<string, object>> GetOAuthMetadata(string WellKnown)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(WellKnown);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var oAuthMetadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
            return oAuthMetadata;
        }

        private async Task showPermissionButtonIfNeeded()
        {
            try
            {
                _connectionsUrl = await NeedsOrganizationAccess();
                if (!string.IsNullOrWhiteSpace(_connectionsUrl))
                {
                    btnConnections.Visible = true;
                    lblPermissions.Visible = true;
                }
                else
                {
                    btnConnections.Visible = false;
                    lblPermissions.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);                
            }
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
                lblPermissions.Visible = false;
                btnConnections.Visible = false;

                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    _currentUserId = dp.Settings.GetAsString(SettingKeyType.LastUserID);
                }

                remoteDataRepository.Initialize(AppConfig.JohnDeereApiUrl, AppConfig.JohnDeereWellKnownUrl, RemoteProviderType.OAuthProvider,
                                        DataHelper.SaveNewToken, DataHelper.IsTokenExpired,
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

                await Task.Run(async () =>
                {

                    System.Threading.Thread.Sleep(1000);

                //check network state
                if (!NetworkHelper.HasInternetConnection("https://google.com"))
                    {
                        connected = false;
                        noNetwork = true;
                    }
                    else
                    {
                        //we have a network connection so verify we can connect to John Deere
                        using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                        {
                            var accessToken = dp.Settings.GetAsString(SettingKeyType.JDAccessToken);
                            var refreshToken = dp.Settings.GetAsString(SettingKeyType.JDRefreshToken);
                            var deereCredentialDate = dp.Settings.GetLastJohnDeereLoginDateTime();
                            var accessTokenExpires = dp.Settings.GetAccessTokenExpiresUTC();

                            //if we have an access token and refresh token and we last logged in within the last 360 days 
                            //we should have at a minimum a valid refresh token
                            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken) && (deereCredentialDate.HasValue && deereCredentialDate.Value.AddDays(360) >= DateTime.UtcNow))
                            {
                                try
                                {
                                    remoteDataRepository.SetAuthData(accessToken, refreshToken);
                                    _currentUserId = await remoteDataRepository.TestConnection();
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

                    if (connected)
                    {
                        await showPermissionButtonIfNeeded();
                    }
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


            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\CottonHarvestData";

            authResponseFilename = dir.TrimEnd('\\') + "\\AuthResponse.txt";                        
            
            fileSystemWatcher.Path = dir;
            fileSystemWatcher.EnableRaisingEvents = false;
        }

        private string GetBase64EncodedClientCredentials()
        {
            byte[] credentialBytes = Encoding.UTF8.GetBytes($"{AppConfig.AppId}:{AppConfig.AppSecret}");
            return Convert.ToBase64String(credentialBytes);
        }

        private async Task ExchangeCodeForToken(string code)
        {
            //now what do we do with the code - we need to exchange it for tokens
            Dictionary<string, object> oAuthMetadata = await GetOAuthMetadata(AppConfig.JohnDeereWellKnownUrl);
            string tokenEndpoint = oAuthMetadata["token_endpoint"].ToString();

            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("grant_type", "authorization_code");
            queryParameters.Add("code", code);
            queryParameters.Add("redirect_uri", "cottonutil://localhost/callback");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("authorization", $"Basic {GetBase64EncodedClientCredentials()}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            {
                Content = new FormUrlEncodedContent(queryParameters)
            };

            HttpResponseMessage response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            Token accessToken = JsonConvert.DeserializeObject<Token>(responseContent);

            //need to save access token info
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDAccessToken, accessToken.access_token);
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDAccessTokenExpires, DateTime.UtcNow.AddSeconds(accessToken.expires_in).ToString());
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDRefreshToken, accessToken.refresh_token);
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDCredentialDateTime, DateTime.UtcNow.ToString());
                dp.SaveChanges();
            }

            string organizationAccessUrl = await NeedsOrganizationAccess();

            //if we received an organizationsAccessUrl we need to open a browser for user to give permission
            //once completed browser will redirect to the call back
            if (organizationAccessUrl != null)
            {
                ClearAuthResponseText();
                ProcessStartInfo sInfo = new ProcessStartInfo(organizationAccessUrl);
                Process.Start(sInfo);
            }
            else
            {
                if (openProcess != null && !openProcess.HasExited)
                {
                    openProcess.CloseMainWindow();
                    if (this.ParentForm != null)
                        this.ParentForm.Focus();
                }

                verifyConnection();
            }
        }

        private async void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                string code = "";
                if (System.IO.File.Exists(e.FullPath))
                {
                    System.Threading.Thread.Sleep(1000);

                    string contents = System.IO.File.ReadAllText(e.FullPath);

                    if (contents.IndexOf("?code=") > 0)
                    {
                        try
                        {                            
                            //cottonutil://localhost/callback?code=rK1SnYGj9E4quJMGVI4TBetXOxnW-hGftBI10PL6aPE&state=test+state
                            code = contents.Replace("cottonutil://localhost/callback?code=", "");
                            code = code.Substring(0, code.IndexOf("&state"));
                            await ExchangeCodeForToken(code);
                        }
                        catch (Exception exc)
                        {
                            Logger.Log(exc);
                            verifyConnection();
                        }

                        this.ParentForm.BringToFront();
                    }
                    else
                    {
                        //handle return from partner access
                        fileSystemWatcher.EnableRaisingEvents = false;
                        if (openProcess != null && !openProcess.HasExited)
                        {
                            openProcess.CloseMainWindow();

                            if (this.ParentForm != null)
                                this.ParentForm.Focus();
                        }

                        verifyConnection();

                        await showPermissionButtonIfNeeded();

                        this.ParentForm.BringToFront();
                    }


                }
            }
            catch (Exception outerExc)
            {
                Logger.Log(outerExc);
                MessageBox.Show("An error occurred completing authentication.");
            }
        }

        private async void verifyConnection()
        {
            string accessToken = "";
            string refreshToken = "";
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                accessToken = dp.Settings.GetAsString(SettingKeyType.JDAccessToken);
                refreshToken = dp.Settings.GetAsString(SettingKeyType.JDRefreshToken);
                remoteDataRepository.SetAuthData(accessToken, refreshToken);
            }
            string newUserID = "";

            lblLoadingMessage.Text = "Verifying connection";
            await Task.Run(async () =>
            {
                System.Threading.Thread.Sleep(1000);
                newUserID = await remoteDataRepository.TestConnection();
            });

            pnlLoading.Visible = false;

            if (!string.IsNullOrEmpty(newUserID))
            {
                if (newUserID != _currentUserId && !_firstRun)  //switched accounts
                {
                    if (MessageBox.Show("You have switched to a different account.  This will reset all application data.  Are you sure you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        pnlConnectionSettingsConnected.Visible = true;
                        pnlConnectionSettingsLinkAccount.Visible = false;
                        pnlConnectionSettingsVerifyCode.Visible = false;
                        _currentUserId = newUserID;
                        using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                        {
                            //remove all organizations saved
                            dp.Organizations.DeleteAll();
                            //save updated token and secret
                            dp.Settings.UpsertSettingWithKey(SettingKeyType.JDAccessToken, accessToken);
                            dp.Settings.UpsertSettingWithKey(SettingKeyType.JDRefreshToken, refreshToken);
                            dp.Settings.UpsertSettingWithKey(SettingKeyType.LastUserID, _currentUserId);
                            dp.SaveChanges();
                        }                      


                        this.OnConnectionComplete(new EventArgs());
                    }
                    else
                    {                     
                        //reset auth token back to original
                        remoteDataRepository.SetAuthData(accessToken, refreshToken);
                        pnlConnectionSettingsConnected.Visible = true;
                        pnlConnectionSettingsLinkAccount.Visible = false;
                        pnlConnectionSettingsVerifyCode.Visible = false;
                        this.OnConnectionComplete(new EventArgs());
                    }
                }
                else
                {
                    //handle same user just save new secret and token                            
                    _currentUserId = newUserID;

                    using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                    {
                        dp.Settings.UpdateSettingWithKey(SettingKeyType.JDAccessToken, accessToken);
                        dp.Settings.UpdateSettingWithKey(SettingKeyType.JDRefreshToken, refreshToken);
                        dp.Settings.UpsertSettingWithKey(SettingKeyType.LastUserID, _currentUserId);
                        dp.SaveChanges();
                    }
                    
                    pnlConnectionSettingsConnected.Visible = true;
                    pnlConnectionSettingsLinkAccount.Visible = false;
                    pnlConnectionSettingsVerifyCode.Visible = false;
                    this.OnConnectionComplete(new EventArgs());
                }
                remoteDataRepository.EmptyAllCacheItems();
            }
            else
            {
                MessageBox.Show("Unable to establish connection. Verify you have a network connection.");
                pnlConnectionSettingsVerifyCode.Visible = true;
            }


            pnlLoading.Visible = false;
        }

        /// This code is from John Deere sample
        /// <summary>Check to see if the 'connections' rel is present for any organization.
        /// If the rel is present it means the oauth application has not completed it's
        /// access to an organization and must redirect the user to the uri provided
        /// in the link.</summary>
        /// <returns>A redirect uri if the <code>connections</code>
        /// connections rel is present or <null> if no redirect is
        /// required to finish the setup.</returns>
        private async Task<string> NeedsOrganizationAccess()
        {
            var response = await SecuredApiGetRequest(AppConfig.JohnDeereApiUrl + "/organizations");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var dynorg = JsonConvert.DeserializeObject<dynamic>(responseContent);

            foreach (var organization in dynorg.values)
            {
                foreach (var link in organization.links)
                {
                    string rel = link.rel;
                    if (rel == "connections")
                    {
                        string connectionsLink = link.uri;
                        return connectionsLink + "?redirect_uri=cottonutil%3A%2F%2Flocalhost%2Fcallback";
                    }
                }
            }
            return null;
        }

        private async Task<HttpResponseMessage> SecuredApiGetRequest(string url)
        {
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                var client = new HttpClient();
                var tokenSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.JDAccessToken);                                    

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenSetting.Value}");
                return await client.GetAsync(url);
            }
        }

        private async void btnStartOver_Click(object sender, EventArgs e)
        {
            pnlLoading.Visible = false;
            await LoadDataAsync();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnConnections_Click(object sender, EventArgs e)
        {            
            ClearAuthResponseText();
            fileSystemWatcher.EnableRaisingEvents = true;
            ProcessStartInfo sInfo = new ProcessStartInfo(_connectionsUrl);
            //sInfo.Arguments = " --new-window";
            openProcess = Process.Start(sInfo);
        }
    }
}
