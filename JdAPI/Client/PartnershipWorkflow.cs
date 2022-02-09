using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JdAPI.DataContracts;
using System.IO;
using System.Runtime.Serialization.Json;
using Hammock.Authentication.OAuth;
using System.Compat.Web;
using JdAPI.Client.Rest;
using Newtonsoft.Json;
using CottonHarvestDataTransferApp.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Configuration;

namespace JdAPI.Client
{
    public class PartnershipWorkflow
    {
        #region private properties
        private Dictionary<String, Link> apiCataloglinks;
        private Dictionary<string, ETag> _organizationFileETags = null;
        private string baseUrl = "";
        #endregion

        #region private methods
        private static T Deserialise<T>(Stream stream)
        {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)deserializer.ReadObject(stream);
            return result;
        }

        private static async Task<T> DeserialiseAsync<T>(HttpContent content)
        {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)deserializer.ReadObject(await content.ReadAsStreamAsync());
            return result;
        }
        

        private HttpClient getHttpClient()
        {
            var client = new HttpClient();
            //var tokenSetting = dp.Settings.FindSingle(x => x.Key == SettingKeyType.JDAccessToken);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetBase64EncodedClientCredentials()}");

            return client;
        }
               
        private string GetBase64EncodedClientCredentials()
        {
            byte[] credentialBytes = Encoding.UTF8.GetBytes($"{ApiCredentials.APP_ID}:{ApiCredentials.APP_KEY}");
            return Convert.ToBase64String(credentialBytes);
        }

        private async Task<Organization> getOrganization(Link orgLink, bool allowFetchFromCache)
        {
            Organization org = null;

            if (allowFetchFromCache)
            {
                org = (Organization) CacheManager.GetCacheItem(orgLink.uri);
            }

            if (org == null)
            {
                Logger.Log("RESOURCE", "getOrganization");
                var response = await SecuredApiGetRequest(orgLink.uri);
                Logger.Log(response);
                org = await PartnershipWorkflow.DeserialiseAsync<Organization>(response.Content);
                CacheManager.AddCacheItem(orgLink.uri, org, 5);               
            }
            
            return org;
        }

        private async Task<Resource> getPartnership(string partnerLink)
        {                       
            var response = await SecuredApiGetRequest(partnerLink);
            Logger.Log(response);
            Resource org = await PartnershipWorkflow.DeserialiseAsync<Resource>(response.Content);
            return org;
        }

        private async Task<List<Partnership>> getPartnerShipsList(string uri)
        {
            List<Partnership> partnerShips = (List<Partnership>) CacheManager.GetCacheItem("partnership_list");

            if (partnerShips == null || partnerShips.Count == 0)
            {
                partnerShips = await getList<Partnership>(uri);
                CacheManager.AddCacheItem("partnership_list", partnerShips, 5);
            }

            return partnerShips;
        }

        private async Task<List<Permission>> getPermissions(string link)
        {            
            Logger.Log("RESOURCE", "getPermissions");
            var response = await SecuredApiGetRequest(link);            
            Logger.Log(response);
            Permissions p = await PartnershipWorkflow.DeserialiseAsync<Permissions>(response.Content);
            return p.permissions;
        }

        public async Task<Resource> getResource(string resourceLink)
        {            
            Logger.Log("RESOURCE", "getResource");
            var response = await SecuredApiGetRequest(resourceLink);
            Logger.Log(response);            
            Resource r = await PartnershipWorkflow.DeserialiseAsync<Resource>(response.Content);            
            return r;
        }

        public async Task<Resource> getResourceFromCache(string resourceLink)
        {
            Resource r = (Resource) CacheManager.GetCacheItem(resourceLink);

            if (r == null)
            {
                Logger.Log("RESOURCE", "getResourceFromCache");

                var response = await SecuredApiGetRequest(resourceLink);
                r = await PartnershipWorkflow.DeserialiseAsync<Resource>(response.Content);
                Logger.Log(response);
                CacheManager.AddCacheItem(resourceLink, r, 5);
            }
            return r;
        }

        public async Task<string> getOrgIdForCompletedPartnership(string resourceLink)
        {

            Logger.Log("RESOURCE", "getOrgIdForCompletePartnership");

            Resource p = await getPartnership(resourceLink);
            var partnershipLinks = OAuthWorkFlow.linksFrom(p);


            string link =  this.ExtractTokenFromLink(partnershipLinks["fromPartnership"].uri);

            if (link.ToLower().IndexOf("mailto:") >= 0) //partnership is incomplete
            {
                return string.Empty;
            }
            else
            {
                return link;
            }           

        }

        private async Task<List<DataContracts.File>> getPartnerFileLinks(string uri, string orgId, bool fetchNewFiles)
        {
            orgId = orgId.ToLower();
            List<DataContracts.File> files = null;
            ETag tag = new ETag();

            if (fetchNewFiles)
            {
                if (_organizationFileETags == null)
                {
                    _organizationFileETags = new Dictionary<string, ETag>();
                }

                if (_organizationFileETags.ContainsKey(orgId))
                {
                    tag = _organizationFileETags[orgId];                    
                    var result = await getListWithETag<DataContracts.File>(uri, tag);
                    files = result.Resources;
                    tag.CreatedDate = DateTime.Now;
                    tag.Tag = result.Tag;
                    _organizationFileETags[orgId] = tag;
                }
                else
                {
                    tag.Tag = string.Empty;
                    var result = await getListWithETag<DataContracts.File>(uri, tag);
                    files = result.Resources;
                    tag.Tag = result.Tag;
                    if (!string.IsNullOrEmpty(tag.Tag))
                    {
                        tag.CreatedDate = DateTime.Now;
                        _organizationFileETags.Add(orgId, tag);
                    }                    
                }
            }
            else
            {
                await getList<DataContracts.File>(uri);
            }            
            
            return files;
        }

        
        private async Task fillPartnershipTree(Partnership p, bool fetchNewFiles)
        {
            //get fromPartnership            
            Dictionary<String, Link> partnershipLinks = OAuthWorkFlow.linksFrom(p);
            Link fromPartnerShipLink = partnershipLinks["fromPartnership"];
            Link toPartnerShipLink = partnershipLinks["toPartnership"];

            //only try to fetch partner details if not a mailto link
            if (fromPartnerShipLink.uri.ToLower().IndexOf("mailto:") < 0)
            {             

                if (!fromPartnerShipLink.uri.ToLower().StartsWith("mailto:")) {
                    p.FromOrg = await getOrganization(fromPartnerShipLink, true);
                }

                if (!toPartnerShipLink.uri.ToLower().StartsWith("mailto:"))
                {
                    p.ToOrg = await getOrganization(toPartnerShipLink, true);
                }

                if (partnershipLinks.ContainsKey("contactInvitation"))
                {
                    p.ContactInvite = await getResourceFromCache(partnershipLinks["contactInvitation"].uri);
                }

             

                if (p.FromOrg != null && !string.IsNullOrEmpty(p.FromOrg.id)) //ADDED MB
                {
                    var orgLinks = OAuthWorkFlow.linksFrom(p.FromOrg);
                    if (orgLinks.ContainsKey("self"))
                    {
                    string filesLink =  orgLinks["self"].uri.TrimEnd("/".ToCharArray()) + "/files";

                    if (fetchNewFiles) //only fetch files when we are wanting to download
                    {
                        p.SharedFiles = await getPartnerFileLinks(filesLink, p.FromOrg.id, fetchNewFiles);

                        if (p.SharedFiles != null)
                            p.TotalFileCount = p.SharedFiles.Count();
                    }
                    else
                    {
                        p.TotalFileCount = await getCount<DataContracts.File>(filesLink);
                    }
                    }
                }
            }
        }

        private async Task<CollectionPage<T>> fetchResources<T>(string path) where T : DataContracts.Resource
        {
            var response = await SecuredApiGetRequest(path);

            Logger.Log("RESOURCE", "fetchResources");
            Logger.Log(response);
            CollectionPageDeserializer ds = new CollectionPageDeserializer();

            CollectionPage<T> resources = null;

            string content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(content))
            {
                resources = ds.deserialize<T>(content);
            }

            return resources;

        }

        private async Task<TagCollection<T>> fetchNewResourcesWithETag<T>(string path, string tag) where T : DataContracts.Resource
        {
          
            Logger.Log("RESOURCE", "fetchNewResourcesWithETag");

            var response = await SecuredApiGetRequest(path, true, tag);

            Logger.Log(response);
            CollectionPageDeserializer ds = new CollectionPageDeserializer();

            //CollectionPage<T> resources = null;

            TagCollection<T> tagCollection = new TagCollection<T>();
            tagCollection.Tag = tag;
            tagCollection.CollectionPage = null;

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return tagCollection;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return tagCollection;
            }
            else if (!string.IsNullOrEmpty(content))
            {
                tagCollection.Tag = response.Headers.GetValues("x-deere-signature").FirstOrDefault();
                tagCollection.CollectionPage = ds.deserialize<T>(content);
            }

            return tagCollection;
        }

        private async Task<List<TResource>> getList<TResource>(string uri) where TResource : DataContracts.Resource
        {
            List<TResource> resources = new List<TResource>();

            string pageUri = uri;
            do
            {
                CollectionPage<TResource> pageCollection = await fetchResources<TResource>(pageUri);

                if (pageCollection == null)
                {
                    break;
                }

                if (pageCollection.totalSize > 0)
                {
                    foreach (var p in pageCollection.page)
                    {
                        resources.Add(p);
                    }

                    if (pageCollection.nextPage != null)
                    {
                        pageUri = pageCollection.nextPage.AbsoluteUri;
                    }
                    else
                    {
                        pageUri = string.Empty;
                    }
                }
                else
                {
                    //no results so break out of loop
                    break;
                }
            } while (!string.IsNullOrEmpty(pageUri));
            return resources;
        }

        private async Task<int> getCount<TResource>(string uri) where TResource : DataContracts.Resource
        {
            List<TResource> resources = new List<TResource>();

            string pageUri = uri;

            CollectionPage<TResource> pageCollection = await fetchResources<TResource>(pageUri);

            if (pageCollection == null)
            {
                return 0;
            }

            return pageCollection.totalSize;
        }

        private async Task<TagResource<TResource>> getListWithETag<TResource>(string uri, ETag tag) where TResource : DataContracts.Resource
        {
            var tagResource = new TagResource<TResource>();
            tagResource.Tag = tag.Tag;

            tagResource.Resources = new List<TResource>();
            string pageUri = uri;
            do
            {
                var tagCollection = await fetchNewResourcesWithETag<TResource>(pageUri, tag.Tag);
                CollectionPage<TResource> pageCollection = tagCollection.CollectionPage;
                tagResource.Tag = tagCollection.Tag;

                if (pageCollection == null)
                {
                    break;
                }

                if (pageCollection.totalSize > 0)
                {
                    foreach (var p in pageCollection.page)
                    {
                        if (p.GetType() == typeof(DataContracts.File))
                        {
                            var f = (DataContracts.File) ((object) p);
                            if (f.aType.ToLower() == "file")
                            {
                                tagResource.Resources.Add(p);
                            }
                        }
                        else
                        {
                            tagResource.Resources.Add(p);
                        }
                    }

                    if (pageCollection.nextPage != null)
                    {
                        pageUri = pageCollection.nextPage.AbsoluteUri;
                    }
                    else
                    {
                        pageUri = string.Empty;
                    }
                }
                else
                {
                    //no results so break out of loop
                    break;
                }
            } while (!string.IsNullOrEmpty(pageUri));
            return tagResource;
        }

        private string ExtractTokenFromLink(string link)
        {
            int startIndex = link.LastIndexOf("/") + 1;
            return link.Substring(startIndex);
        }

        private async Task<string> PostPartnershipRequest(string email, string orgLink)
        {                        
            string json =
@"{
   ""links"": [
      {
         ""rel"": ""toPartnership"",
         ""uri"": ""mailto:{Email}""
      },
      {
         ""rel"": ""fromPartnership"",
         ""uri"": ""{PartnerUri}""
      }
   ]
}";
            json = json.Replace("{Email}", email);
            json = json.Replace("{PartnerUri}", orgLink);
            var response = await SecuredApiJsonPostRequest(apiCataloglinks["partnerships"].uri, Encoding.UTF8.GetBytes(json));

            Logger.Log("RESOURCE", "PostPartnershipRequest");

            Logger.Log(response);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return response.Headers.GetValues("Location").FirstOrDefault();
            }
            else
            {
                return "";
            }
        }

        private async Task<bool> PostPermissionRequest(string permissionsLink)
        {

            string permissionJson =
@"{
   ""permissions"": [     
      {
         ""type"": ""assetManage"",
         ""status"": ""requested""
      },
      {
         ""type"": ""viewOnListAndMap"",
         ""status"": ""requested""
      },
      {
         ""type"": ""viewPeopleAndPreferences"",
         ""status"": ""requested""
      },
      {
         ""type"": ""makeCSCConnection"",
         ""status"": ""requested""
      }
   ]
}";            
            
            var response = await SecuredApiJsonPostRequest(permissionsLink, Encoding.UTF8.GetBytes(permissionJson));
            Logger.Log(response);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private async Task<HttpResponseMessage> SecuredApiGetRequest(string url, bool includeXDeereSig = false, string tag = "")
        {

            var client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            //client.DefaultRequestHeaders.Add("Accept", "application/vnd.deere.axiom.v3+json");

            if (includeXDeereSig)
            {
                client.DefaultRequestHeaders.Add("x-deere-signature", string.IsNullOrEmpty(tag) ? " " : tag);
            }

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiCredentials.ACCESS_TOKEN}");
            var response = await client.GetAsync(url);
            return response;
        }

        private async Task<HttpResponseMessage> SecuredApiFileGetRequest(string url)
        {

            var client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiCredentials.ACCESS_TOKEN}");
            var response = await client.GetAsync(url);
            return response;
        }

        private async Task<HttpResponseMessage> SecuredApiJsonPostRequest(string url, byte[] bytes)
        {

            var client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.deere.axiom.v3+json"));
            
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiCredentials.ACCESS_TOKEN}");
                        
            var content = new ByteArrayContent(bytes, 0, bytes.Length);
            content.Headers.Add("Content-Type", "application/vnd.deere.axiom.v3+json");
            var response = await client.PostAsync(url, content);
            return response;
        }

        #endregion

        public async Task retrieveApiCatalog(string endpoint)
        {

            apiCataloglinks = (Dictionary<String, Link>)CacheManager.GetCacheItem("api_catalog_links");

            if (apiCataloglinks == null)
            {             

                var response = await SecuredApiGetRequest(endpoint);
                ApiCatalog apiCatalog = await PartnershipWorkflow.DeserialiseAsync<ApiCatalog>(response.Content);
                apiCataloglinks = OAuthWorkFlow.linksFrom(apiCatalog);
                CacheManager.AddCacheItem("api_catalog_links", apiCataloglinks, 60 * 6);
               
            }
        }

        public void ClearFilesETags()
        {
            if (_organizationFileETags != null) _organizationFileETags.Clear();

            _organizationFileETags = new Dictionary<string, ETag>();
        }

        /// <summary>
        /// Used to build a list of file eTags for all organizations
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="eTag"></param>
        /// <param name="createdTime"></param>
        public void AddOrgFileETag(string orgId, string eTag, DateTime createdTime)
        {
            try
            {
                if (_organizationFileETags == null) _organizationFileETags = new Dictionary<string, ETag>();

                _organizationFileETags.Add(orgId, new ETag { Tag = eTag, CreatedDate = createdTime });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw new Exception("Etag already added to dictionary");
            }
        }

        /// <summary>
        /// Gets the files eTag for a specific organization
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="eTag"></param>
        /// <param name="createdTime"></param>
        public void GetOrgETagData(string orgId, ref string eTag, ref DateTime createdTime)
        {


            if (_organizationFileETags.ContainsKey(orgId))
            {
                eTag = _organizationFileETags[orgId].Tag;
                createdTime = _organizationFileETags[orgId].CreatedDate;
            }
            else
            {
                eTag = string.Empty;
            }
        }

        public async Task<List<Organization>> getMyOrganizations()
        {
            User currentUser = await getCurrentUser(true);
            return currentUser.Organizations;
        }

        public async Task<List<Partnership>> getPartnerships(bool fetchNewFiles, string[] restrictToOrgIds)
        {
            List<Partnership> relavantPartnerships = null;

            User currentUser = await getCurrentUser(true);

            //find my organization
            //Organization currentOrg = null;

            /*if (currentUser.Organizations != null && currentUser.Organizations.Count() > 0)
            {
                currentOrg = currentUser.Organizations[0];
            }*/

            List<Partnership> partnerships = await getPartnerShipsList(apiCataloglinks["partnerships"].uri);

            relavantPartnerships = new List<Partnership>();
            foreach (var p in partnerships)
            {
                var partnershipLinks = OAuthWorkFlow.linksFrom(p);
                Link fromPartnerShipLink = partnershipLinks["fromPartnership"];
                Link toPartnerShipLink = partnershipLinks["toPartnership"];

                if (fromPartnerShipLink.uri.ToLower() != toPartnerShipLink.uri.ToLower()
                    && currentUser.Organizations.Count(o => toPartnerShipLink.uri.ToLower().IndexOf(o.id.ToLower()) >= 0) > 0)
                {
                    //if a list of org ids is passed in restrict to then apply filter
                    if (restrictToOrgIds == null || restrictToOrgIds.Count() == 0 || restrictToOrgIds.Any(id => fromPartnerShipLink.uri.IndexOf(id) >= 0))
                    {
                        if (!relavantPartnerships.Any(r => OAuthWorkFlow.linksFrom(r)["fromPartnership"].uri == fromPartnerShipLink.uri))
                        {
                            await fillPartnershipTree(p, fetchNewFiles);

                            if (p.FromOrg != null && !string.IsNullOrEmpty(p.FromOrg.id))
                            {
                                relavantPartnerships.Add(p);
                            }
                        }
                    }
                }

            }
            //CacheManager.AddCacheItem("partnerships", relavantPartnerships, 5);

            return relavantPartnerships;
        }

        public async Task<User> getCurrentUser(bool fillChildOrgs)
        {
            string cacheKey = "current_user_childorgs_" + fillChildOrgs.ToString();
            User currentUser = (User)CacheManager.GetCacheItem(cacheKey);

            if (currentUser == null)
            {              
                var response = await SecuredApiGetRequest(apiCataloglinks["currentUser"].uri);
                currentUser = await PartnershipWorkflow.DeserialiseAsync<User>(response.Content);
                if (fillChildOrgs)
                {
                    currentUser.Organizations = await getList<Organization>(OAuthWorkFlow.linksFrom(currentUser)["organizations"].uri);
                }
                CacheManager.AddCacheItem(cacheKey, currentUser, 30);
            }
            return currentUser;         
        }
        
        public async Task<string> RequestPartnerPermissions(string email, string myRequestingOrgId)
        {
            User currentUser = await getCurrentUser(true);

            //find my organization
            Organization currentOrg = null;

            if (currentUser.Organizations != null && currentUser.Organizations.Count() > 0)
            {                
                currentOrg = currentUser.Organizations.SingleOrDefault(o => o.id == myRequestingOrgId);
            }

            var orgLink = OAuthWorkFlow.linksFrom(currentOrg)["self"].uri;
            var partnerLink = await PostPartnershipRequest(email, orgLink);            

            if (!String.IsNullOrEmpty(partnerLink))
            {             
                Resource partnership = await getPartnership(partnerLink);

                //get contact invitation link
                var links = OAuthWorkFlow.linksFrom(partnership);
                var contactInviteLink = links["contactInvitation"];

              
                Resource contactInvite = await getPartnership(contactInviteLink.uri);

                partnerLink = contactInviteLink.uri;

                var contactLinks = OAuthWorkFlow.linksFrom(contactInvite);
                await PostPermissionRequest(contactLinks["permissions"].uri);                
            }

            return partnerLink;        
        }

        public async Task<bool> downloadFileInPiecesAndComputeMd5(string path, DataContracts.File file)
        {
            //Max file size for download is 16 MB
            long chunkSize = 16 * 1024 * 1024;
            long end = file.nativeSize <= chunkSize ? file.nativeSize : chunkSize;

            bool result = true;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(path + file.name))
            {
                System.IO.File.Create(path + file.name).Dispose();
            }
            using (Stream output = System.IO.File.OpenWrite(path + file.name))
            {
                if (!(await getChunkFromStartAndRecurse(file, 0, end, file.nativeSize, output)))
                {
                    result = false;
                }
            }

            return result;
        }

        
        private async Task<bool> getChunkFromStartAndRecurse(DataContracts.File file, long start, long chunkSize, long fileSize, Stream output)
        {
            bool result = true;

            if (fileSize <= chunkSize)
            {
                result = await createDownloadRequest(file, start, fileSize, output);

                if (result == false)
                {
                    System.Threading.Thread.Sleep(1000);   //pause then retry
                    result = await createDownloadRequest(file, start, chunkSize, output);
                }
            }
            else
            {
                result = await createDownloadRequest(file, start, chunkSize, output);

                if (result == false)
                {
                    System.Threading.Thread.Sleep(1000);   //pause then retry
                    result = await createDownloadRequest(file, start, chunkSize, output);
                }

                if (result)
                {
                    result = await getChunkFromStartAndRecurse(file, start + chunkSize, chunkSize, fileSize - chunkSize, output);
                }
            }

            return result;
        }

        private async Task<bool> createDownloadRequest(DataContracts.File file, long start, long bytesToRead, Stream output)
        {         
            var links = OAuthWorkFlow.linksFrom(file);            

            var url = links["self"].uri;
            url = url.TrimEnd("?&/".ToCharArray());
            url = url + "?offset=" + start.ToString() + "&size=" + bytesToRead.ToString();

            var response = await SecuredApiFileGetRequest(url);

                 
            Logger.Log("RESOURCE", "createDownloadRequest");

            Logger.Log(response);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (Stream input = await response.Content.ReadAsStreamAsync())
                {
                    input.CopyTo(output);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
