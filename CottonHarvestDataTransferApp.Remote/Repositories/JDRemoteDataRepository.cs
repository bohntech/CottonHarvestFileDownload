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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JdAPI.Client;
using CottonHarvestDataTransferApp.Remote.Data;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.Remote
{
    /// <summary>
    /// Concrete implementation of IRemoteDataRepository for
    /// authenticating and retreiving information from the 
    /// John Deere API.  Uses modified version of OAuthWorkFlow
    /// from open source John Deere sample library.
    /// </summary>
    public class JDRemoteDataRepository : IRemoteDataRepository
    {
        protected OAuthWorkFlow _of = new OAuthWorkFlow();

        protected string _endpoint = "";

        protected string _wellKnown = "";

        protected TokenRefreshHandler _refreshHandler = null;

        protected TokenExpiredCheckHandler _expireCheckHandler = null;

        protected bool IsTokenExpired()
        {
            return true;

            /*if (_expireCheckHandler != null)
            {
                return _expireCheckHandler();
            }
            else
            {
                return false;
            }*/
        }

        private async Task<Dictionary<string, object>> GetOAuthMetadata()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(_wellKnown);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var oAuthMetadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
            return oAuthMetadata;
        }

        private string GetBase64EncodedClientCredentials()
        {
            byte[] credentialBytes = Encoding.UTF8.GetBytes($"{ApiCredentials.APP_ID}:{ApiCredentials.APP_KEY}");
            return Convert.ToBase64String(credentialBytes);
        }

        protected async Task DoTokenCheck()
        {

            if (IsTokenExpired())
            {
                Dictionary<string, object> oAuthMetadata = await GetOAuthMetadata();
                string tokenEndpoint = oAuthMetadata["token_endpoint"].ToString();

                var queryParameters = new Dictionary<string, string>();
                queryParameters.Add("grant_type", "refresh_token");
                queryParameters.Add("refresh_token", ApiCredentials.REFRESH_TOKEN);
                queryParameters.Add("redirect_uri", "cottonutil://localhost/callback");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("authorization", $"Basic {GetBase64EncodedClientCredentials()}");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
                {
                    Content = new FormUrlEncodedContent(queryParameters)
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<Token>(responseContent);

                    ApiCredentials.ACCESS_TOKEN = token.access_token;
                    ApiCredentials.REFRESH_TOKEN = token.refresh_token;

                    if (_refreshHandler != null)
                    {
                        _refreshHandler(token);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes connection information for the repository.  Should
        /// always be called before any other methods.
        /// </summary>
        /// <param name="endpoint">URL of the API endpoint</param>
        /// <param name="providerType">Type of provider only oAuth implemented</param>
        /// <param name="clientAppKey">App token</param>
        /// <param name="clientAppSecret">App secret</param>
        /// <param name="clientAuthToken">May be blank, but if blank auth token should be retrieved and set using SetAuthData before other calls are made.</param>
        /// <param name="clientAuthSecret">May be blank, but if blank auth secret should be retrieved and set using SetAuthData before other calls are made.</param>
        public void Initialize(string endpoint, string wellknown, RemoteProviderType providerType, TokenRefreshHandler refreshHandler, TokenExpiredCheckHandler expireHandler, string clientAppKey = "",
            string clientAppSecret = "", string accessToken = "", string refreshToken = "")
        {
            _endpoint = endpoint;
            _wellKnown = wellknown;

            _refreshHandler = refreshHandler;
            _expireCheckHandler = expireHandler;

            ApiCredentials.APP_ID = clientAppKey;
            ApiCredentials.APP_KEY = clientAppSecret;
            ApiCredentials.ACCESS_TOKEN = accessToken;
            ApiCredentials.REFRESH_TOKEN = refreshToken;
        }

        public void SetTokenRefreshHandler(TokenRefreshHandler handler)
        {
            _refreshHandler = handler;
        }

        public void SetTokenExpiredHandler(TokenExpiredCheckHandler handler)
        {
            _expireCheckHandler = handler;
        }



        /// <summary>
        /// Sets client specific token and secret
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="authSecret"></param>
        public void SetAuthData(string accessToken, string refreshToken)
        {
            ApiCredentials.ACCESS_TOKEN = accessToken;
            ApiCredentials.REFRESH_TOKEN = refreshToken;
        }
               

        /// <summary>
        /// Makes a test call to the API to see if a request can be
        /// completed without errors to verify the connection.
        /// </summary>
        /// <returns></returns>
        public async Task<string> TestConnection()
        {
            try
            {
                await DoTokenCheck();
                PartnershipWorkflow workflow = new PartnershipWorkflow();
                await workflow.retrieveApiCatalog(_endpoint);
                var user = await workflow.getCurrentUser(false);
                //workflow.getUserOrganizations();
                return user.accountName;
            }
            catch (Exception exc)
            {
                return string.Empty;
            }
        }


        public async Task<List<Partner>> FetchAllPartners(List<OrgFileETag> fileETags, string[] restrictToOrgIds)
        {
            await DoTokenCheck();

            PartnershipWorkflow workflow = new PartnershipWorkflow();
            await workflow.retrieveApiCatalog(_endpoint);
            workflow.ClearFilesETags();

            if (fileETags != null)
            {                
                foreach (var t in fileETags)
                {
                    workflow.AddOrgFileETag(t.OrgId, t.Tag, t.CreatedDate);
                }
            }

            bool fetchNewFiles = (fileETags != null);
            var jdPartnerships = await workflow.getPartnerships(fetchNewFiles, restrictToOrgIds);
            List<Partner> results = new List<Partner>();
            foreach (var p in jdPartnerships)
            {
                if (!string.IsNullOrEmpty(p.FromOrg.id))
                {
                    Partner dataPartner = new Partner();
                    dataPartner.Id = p.FromOrg.id;
                    dataPartner.Name = p.FromOrg.name;
                    dataPartner.MyLinkedOrgId = p.ToOrg.id;
                    //dataPartner.PermissionGranted = (p.Permissions != null) ? p.Permissions.Any(x => x.status == "approved" && x.type == "productionAgronomicDetailData") : false;
                    dataPartner.SharedFileCount = p.TotalFileCount; // (p.SharedFiles != null) ? p.SharedFiles.Count(x => x.archived.ToLower() == "false") : 0;
                    dataPartner.PartnershipLink = p.links.Single(l => l.rel == "self").uri;

                    string fileETag = "";
                    DateTime tagTime = DateTime.Now;
                    workflow.GetOrgETagData(p.FromOrg.id, ref fileETag, ref tagTime);
                    dataPartner.FileETag = fileETag;
                    dataPartner.FilesETagCreatedDate = tagTime;
                    results.Add(dataPartner);
                }
            }
            return results;
        }
       

        /// <summary>
        /// Initiates partner request using supplied email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> RequestPartnerPermission(string email, string myOrgId)
        {
            await DoTokenCheck();

            PartnershipWorkflow p = new PartnershipWorkflow();
            await p.retrieveApiCatalog(_endpoint);
            var pLink = await p.RequestPartnerPermissions(email, myOrgId);
            return pLink;
        }

        /// <summary>
        /// Gets the organization id from the supplied link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<string> GetRemoteIDFromPartnerLink(string link)
        {
            await DoTokenCheck(); 
            PartnershipWorkflow up = new PartnershipWorkflow();
            await up.retrieveApiCatalog(_endpoint);
            return await up.getOrgIdForCompletedPartnership(link);
        }

        /// <summary>
        /// Clears all cached data
        /// </summary>
        public void EmptyAllCacheItems()
        {
            JdAPI.CacheManager.Empty();
        }

        /// <summary>
        /// Downloads files for organizations specified in orgIds
        /// </summary>
        /// <param name="fileETags">eTags from previous download</param>
        /// <param name="filePath"></param>
        /// <param name="orgIds">Only files with an organization id in orgIds will be downloaded</param>
        /// <param name="downloadedFileIds">List of file ids that have previously been processed</param>
        /// <param name="callback">Callback executed when a file has been downloaded</param>
        /// <param name="progressCallback">Executed to update progress</param>
        /// <param name="errorCallback">Executed when a download error occurs</param>
        /// <returns></returns>
        public async Task<List<OrgFileETag>> DownloadOrganizationFiles(List<OrgFileETag> fileETags, string filePath, string[] orgIds, string[] downloadedFileIds, Action<FileDownloadedResult> callback, Action<FileDownloadProgress> progressCallback, Action<FileDownloadedResult> errorCallback)
        {
            await DoTokenCheck(); 
            List<OrgFileETag> newETags = new List<OrgFileETag>();
            List<OrgFileETag> failedETags = new List<OrgFileETag>();
            PartnershipWorkflow p = new PartnershipWorkflow();
            await p.retrieveApiCatalog(_endpoint);
            p.ClearFilesETags();

            if (fileETags != null)
            {
                foreach(var t in fileETags)
                {                    
                    p.AddOrgFileETag(t.OrgId, t.Tag, t.CreatedDate);
                }
            }

            bool fetchNewFiles = (fileETags != null);

            var partnerships = await p.getPartnerships(fetchNewFiles, orgIds);
            
            FileDownloadProgress progressResult = new FileDownloadProgress();
            progressResult.CurrentFile = 1;
            progressResult.TotalFiles = 0;
            foreach (var partner in partnerships)
            {
                if (orgIds.Contains(partner.FromOrg.id) && partner.SharedFiles != null)                {

                    progressResult.TotalFiles += partner.SharedFiles.Count(f => !downloadedFileIds.Contains(f.id) && f.type != null && f.type.ToLower() == "hic" && f.archived.ToLower() == "false");
                }
            }

            progressCallback(progressResult);
            
            foreach (var partner in partnerships)
            {
                string fileETag = "";
                DateTime tagTime = DateTime.Now;
                p.GetOrgETagData(partner.FromOrg.id, ref fileETag, ref tagTime);

                newETags.Add(new OrgFileETag { CreatedDate = tagTime, OrgId = partner.FromOrg.id, Tag = fileETag });

                if (orgIds.Contains(partner.FromOrg.id) && partner.SharedFiles != null)
                {
                    foreach(var file in partner.SharedFiles.Where(f => !downloadedFileIds.Contains(f.id) && f.type != null && f.type.ToLower() == "hic" && f.archived.ToLower() == "false"))
                    {
                        string partnerFilePath = filePath.TrimEnd('\\') + "\\" + partner.FromOrg.id + "-" + file.id + "-source\\";

                        progressCallback(progressResult);

                        if (!(await p.downloadFileInPiecesAndComputeMd5(partnerFilePath, file)))
                        {
                            var originalETag = fileETags.SingleOrDefault(f => f.OrgId == partner.FromOrg.id);

                            //file download failed so add the eTag to the failed list
                            if (originalETag != null)
                            {
                                failedETags.Add(originalETag);
                            }

                            errorCallback(new FileDownloadedResult { Filename = partnerFilePath + file.name, OrganizationID = partner.FromOrg.id, FileIdentifier = file.id });
                        }
                        else
                        {
                            callback(new FileDownloadedResult { Filename = partnerFilePath + file.name, OrganizationID = partner.FromOrg.id, FileIdentifier = file.id });
                        }
                        
                        progressResult.CurrentFile++;
                    }
                }
            }

            //if download failed for a certain org, reset the eTag back to the original one to ensure
            //the download gets retried next time
            if (failedETags.Count > 0)
            {
                foreach(var failedTag in failedETags)
                {
                    foreach(var newTag in newETags)
                    {
                        if (newTag.OrgId == failedTag.OrgId)
                        {
                            newTag.Tag = failedTag.Tag;
                        }
                    }
                }
            }

            return newETags;
        }

        public async Task<List<UserOrganization>> GetMyOrganizations()
        {
            await DoTokenCheck(); 
            PartnershipWorkflow p = new PartnershipWorkflow();            
            await p.retrieveApiCatalog(_endpoint);

            List<UserOrganization> userOrgs = new List<UserOrganization>();
            foreach(var org in await p.getMyOrganizations())
            {
                userOrgs.Add(new UserOrganization { Name = org.name, RemoteId = org.id });
            }
            return userOrgs;
        }
    }
}
