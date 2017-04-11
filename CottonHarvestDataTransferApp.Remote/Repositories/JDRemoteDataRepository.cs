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
        public void Initialize(string endpoint, RemoteProviderType providerType, string clientAppKey = "",
            string clientAppSecret = "", string clientAuthToken = "", string clientAuthSecret = "")
        {
            _endpoint = endpoint;

            ApiCredentials.CLIENT.key = clientAppKey;
            ApiCredentials.CLIENT.secret = clientAppSecret;
            ApiCredentials.TOKEN.token = clientAuthToken;
            ApiCredentials.TOKEN.secret = clientAuthSecret;
        }

        /// <summary>
        /// Sets client specific token and secret
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="authSecret"></param>
        public void SetAuthData(string authToken, string authSecret)
        {
            ApiCredentials.TOKEN.token = authToken;
            ApiCredentials.TOKEN.secret = authSecret;
        }

        /// <summary>
        /// Gets a URL to be opened in a web browser for OOB verification.
        /// </summary>
        /// <returns></returns>
        public string GetManualAuthUrl()
        {
            try
            {
                _of.retrieveApiCatalogToEstablishOAuthProviderDetails(_endpoint);
                _of.getRequestToken();
                return _of.AuthUri;
            }
            catch (Exception exc)
            {
                int i = 0;
                return string.Empty;
            }
        }

        /// <summary>
        /// Exchanges the verifier token for auth token
        /// </summary>
        /// <param name="verifier"></param>
        /// <param name="authToken"></param>
        /// <param name="authSecret"></param>
        /// <returns></returns>
        public bool ExchangeVerifierForCredentials(string verifier, ref string authToken, ref string authSecret)
        {
            _of.SetVerifier(verifier.Trim());

            if (_of.exchangeRequestTokenForAccessToken(ref authToken, ref authSecret))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a test call to the API to see if a request can be
        /// completed without errors to verify the connection.
        /// </summary>
        /// <returns></returns>
        public string TestConnection()
        {
            try
            {
                PartnershipWorkflow workflow = new PartnershipWorkflow();
                workflow.retrieveApiCatalog(_endpoint);
                var user = workflow.getCurrentUser(false);
                //workflow.getUserOrganizations();
                return user.accountName;
            }
            catch (Exception exc)
            {
                return string.Empty;
            }
        }


        public List<Partner> FetchAllPartners(List<OrgFileETag> fileETags, string[] restrictToOrgIds)
        {
            PartnershipWorkflow workflow = new PartnershipWorkflow();
            workflow.retrieveApiCatalog(_endpoint);
            workflow.ClearFilesETags();

            if (fileETags != null)
            {                
                foreach (var t in fileETags)
                {
                    workflow.AddOrgFileETag(t.OrgId, t.Tag, t.CreatedDate);
                }
            }

            bool fetchNewFiles = (fileETags != null);
            var jdPartnerships = workflow.getPartnerships(fetchNewFiles, restrictToOrgIds);
            List<Partner> results = new List<Partner>();
            foreach (var p in jdPartnerships)
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
            return results;
        }
       

        /// <summary>
        /// Initiates partner request using supplied email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string RequestPartnerPermission(string email, string myOrgId)
        {
            PartnershipWorkflow p = new PartnershipWorkflow();
            p.retrieveApiCatalog(_endpoint);
            var pLink = p.RequestPartnerPermissions(email, myOrgId);

            return pLink;
        }

        /// <summary>
        /// Gets the organization id from the supplied link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public string GetRemoteIDFromPartnerLink(string link)
        {
            PartnershipWorkflow up = new PartnershipWorkflow();
            up.retrieveApiCatalog(_endpoint);
            return up.getOrgIdForCompletedPartnership(link);
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
        public List<OrgFileETag> DownloadOrganizationFiles(List<OrgFileETag> fileETags, string filePath, string[] orgIds, string[] downloadedFileIds, Action<FileDownloadedResult> callback, Action<FileDownloadProgress> progressCallback, Action<FileDownloadedResult> errorCallback)
        {
            List<OrgFileETag> newETags = new List<OrgFileETag>();
            List<OrgFileETag> failedETags = new List<OrgFileETag>();
            PartnershipWorkflow p = new PartnershipWorkflow();
            p.retrieveApiCatalog(_endpoint);
            p.ClearFilesETags();

            if (fileETags != null)
            {
                foreach(var t in fileETags)
                {                    
                    p.AddOrgFileETag(t.OrgId, t.Tag, t.CreatedDate);
                }
            }

            bool fetchNewFiles = (fileETags != null);

            var partnerships = p.getPartnerships(fetchNewFiles, orgIds);
            
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

                        if (!p.downloadFileInPiecesAndComputeMd5(partnerFilePath, file))
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

        public List<UserOrganization> GetMyOrganizations()
        {
            PartnershipWorkflow p = new PartnershipWorkflow();            
            p.retrieveApiCatalog(_endpoint);

            List<UserOrganization> userOrgs = new List<UserOrganization>();
            foreach(var org in p.getMyOrganizations())
            {
                userOrgs.Add(new UserOrganization { Name = org.name, RemoteId = org.id });
            }
            return userOrgs;
        }
    }
}
