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

namespace JdAPI.Client
{
    public class PartnershipWorkflow
    {
        #region private properties
        private Dictionary<String, Link> apiCataloglinks;
        private Dictionary<string, ETag> _organizationFileETags = null;
        #endregion

        #region private methods
        private static T Deserialise<T>(Stream stream)
        {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)deserializer.ReadObject(stream);
            return result;
        }

        private Hammock.RestClient getRestClient()
        {
            Hammock.Authentication.OAuth.OAuthCredentials credentials = OAuthWorkFlow.createOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.TOKEN.token,
                ApiCredentials.TOKEN.secret, null, null);

            Hammock.RestClient client = new Hammock.RestClient()
            {
                Authority = "",
                Credentials = credentials,
                Timeout = new TimeSpan(0, 0, 30)
            };
            return client;

        }

        private Organization getOrganization(Link orgLink, bool allowFetchFromCache)
        {
            Organization org = null;

            if (allowFetchFromCache)
            {
                org = (Organization) CacheManager.GetCacheItem(orgLink.uri);
            }

            if (org == null)
            {
                Hammock.RestClient client = getRestClient();
                Hammock.RestRequest request = new Hammock.RestRequest()
                {
                    Path = orgLink.uri
                };
                request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");

                Logger.Log("RESOURCE", "getOrganization");

                using (Hammock.RestResponse response = client.Request(request))
                {
                    org = PartnershipWorkflow.Deserialise<Organization>(response.ContentStream);                 
                    Logger.Log(response);
                    CacheManager.AddCacheItem(orgLink.uri, org, 5);
                }
            }
            
            return org;
        }

        private Resource getPartnership(string partnerLink)
        {
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = partnerLink
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            Logger.Log("RESOURCE", "getPartnership");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);
                Resource org = PartnershipWorkflow.Deserialise<Resource>(response.ContentStream);
                return org;
            }
        }

        private List<Partnership> getPartnerShipsList(string uri)
        {
            List<Partnership> partnerShips = (List<Partnership>) CacheManager.GetCacheItem("partnership_list");

            if (partnerShips == null || partnerShips.Count == 0)
            {
                partnerShips = getList<Partnership>(uri);
                CacheManager.AddCacheItem("partnership_list", partnerShips, 5);
            }

            return partnerShips;
        }

        private List<Permission> getPermissions(string link)
        {
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = link
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            Logger.Log("RESOURCE", "getPermissions");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);
                Permissions p = PartnershipWorkflow.Deserialise<Permissions>(response.ContentStream);
                return p.permissions;
            }
        }

        public Resource getResource(string resourceLink)
        {
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = resourceLink
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            Logger.Log("RESOURCE", "getResource");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Resource r = PartnershipWorkflow.Deserialise<Resource>(response.ContentStream);
                Logger.Log(response);
                return r;
            }
        }

        public Resource getResourceFromCache(string resourceLink)
        {
            Resource r = (Resource) CacheManager.GetCacheItem(resourceLink);

            if (r == null)
            {
                Hammock.RestClient client = getRestClient();
                Hammock.RestRequest request = new Hammock.RestRequest()
                {
                    Path = resourceLink
                };
                request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
                Logger.Log("RESOURCE", "getResourceFromCache");
                using (Hammock.RestResponse response = client.Request(request))
                {
                    r = PartnershipWorkflow.Deserialise<Resource>(response.ContentStream);
                    Logger.Log(response);
                    CacheManager.AddCacheItem(resourceLink, r, 5);
                }
            }
            return r;
        }

        public string getOrgIdForCompletedPartnership(string resourceLink)
        {
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = resourceLink
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            Logger.Log("RESOURCE", "getOrgIdForCompletePartnership");

            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);
                //Resource r = Download.Deserialise<Resource>(response.ContentStream);

                if (response.StatusCode == System.Net.HttpStatusCode.Moved) //request is completed
                {
                    string link = response.Headers["Location"];
                    Resource p = getPartnership(link);
                    var partnershipLinks = OAuthWorkFlow.linksFrom(p);
                    return this.ExtractTokenFromLink(partnershipLinks["fromPartnership"].uri);
                }
                else
                {
                    return string.Empty;
                }
            }       
        }

        private List<DataContracts.File> getPartnerFileLinks(string uri, string orgId, bool fetchNewFiles)
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
                    files = getListWithETag<DataContracts.File>(uri, ref tag);                    
                    tag.CreatedDate = DateTime.Now;
                    _organizationFileETags[orgId] = tag;
                }
                else
                {
                    tag.Tag = string.Empty;
                    files = getListWithETag<DataContracts.File>(uri, ref tag);

                    if (!string.IsNullOrEmpty(tag.Tag))
                    {
                        tag.CreatedDate = DateTime.Now;
                        _organizationFileETags.Add(orgId, tag);
                    }                    
                }
            }
            else
            {
                getList<DataContracts.File>(uri);
            }            
            
            return files;
        }

        
        private void fillPartnershipTree(Partnership p, bool fetchNewFiles)
        {
            //get fromPartnership            
            Dictionary<String, Link> partnershipLinks = OAuthWorkFlow.linksFrom(p);
            Link fromPartnerShipLink = partnershipLinks["fromPartnership"];
            Link toPartnerShipLink = partnershipLinks["toPartnership"];

            //only try to fetch partner details if not a mailto link
            if (fromPartnerShipLink.uri.ToLower().IndexOf("mailto:") < 0)
            {             

                if (!fromPartnerShipLink.uri.ToLower().StartsWith("mailto:")) {
                    p.FromOrg = getOrganization(fromPartnerShipLink, true);
                }

                if (!toPartnerShipLink.uri.ToLower().StartsWith("mailto:"))
                {
                    p.ToOrg = getOrganization(toPartnerShipLink, true);
                }

                if (partnershipLinks.ContainsKey("contactInvitation"))
                {
                    p.ContactInvite = getResourceFromCache(partnershipLinks["contactInvitation"].uri);
                }
           
                /*if (partnershipLinks.ContainsKey("permissions"))
                {
                    p.Permissions = getPermissions(partnershipLinks["permissions"].uri);
                }*/

                var orgLinks = OAuthWorkFlow.linksFrom(p.FromOrg);
                if (orgLinks.ContainsKey("files"))
                {
                    if (fetchNewFiles) //only fetch files when we are wanting to download
                    {
                        p.SharedFiles = getPartnerFileLinks(orgLinks["files"].uri, p.FromOrg.id, fetchNewFiles);

                        if (p.SharedFiles != null)
                            p.TotalFileCount = p.SharedFiles.Count();
                    }                   
                    else
                    {
                        p.TotalFileCount = getCount<DataContracts.File>(orgLinks["files"].uri);
                    }                    
                }
            }
        }

        private CollectionPage<T> fetchResources<T>(string path) where T : DataContracts.Resource
        {
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = path
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log("RESOURCE", "fetchResources");
                Logger.Log(response);
                CollectionPageDeserializer ds = new CollectionPageDeserializer();

                CollectionPage<T> resources = null;

                if (!string.IsNullOrEmpty(response.Content))
                {
                    resources = ds.deserialize<T>(response.Content);
                }

                return resources;
            }
        }

        private CollectionPage<T> fetchNewResourcesWithETag<T>(string path, ref string tag) where T : DataContracts.Resource
        {
            
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = path
            };
             
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            request.AddHeader("x-deere-signature", string.IsNullOrEmpty(tag) ? " " : tag);
            Logger.Log("RESOURCE", "fetchNewResourcesWithETag");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);
                CollectionPageDeserializer ds = new CollectionPageDeserializer();

                CollectionPage<T> resources = null;

                if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    return resources;
                }
                else if (!string.IsNullOrEmpty(response.Content))
                {
                    tag = response.Headers["x-deere-signature"];
                    resources = ds.deserialize<T>(response.Content);
                }

                return resources;
            }
        }

        private List<TResource> getList<TResource>(string uri) where TResource : DataContracts.Resource
        {
            List<TResource> resources = new List<TResource>();

            string pageUri = uri;
            do
            {
                CollectionPage<TResource> pageCollection = fetchResources<TResource>(pageUri);

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

        private int getCount<TResource>(string uri) where TResource : DataContracts.Resource
        {
            List<TResource> resources = new List<TResource>();

            string pageUri = uri;

            CollectionPage<TResource> pageCollection = fetchResources<TResource>(pageUri);

            if (pageCollection == null)
            {
                return 0;
            }

            return pageCollection.totalSize;
        }

        private List<TResource> getListWithETag<TResource>(string uri, ref ETag tag) where TResource : DataContracts.Resource
        {
            List<TResource> resources = new List<TResource>();
            string pageUri = uri;
            do
            {
                CollectionPage<TResource> pageCollection = fetchNewResourcesWithETag<TResource>(pageUri, ref tag.Tag);

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
                                resources.Add(p);
                            }
                        }
                        else
                        {
                            resources.Add(p);
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
            return resources;
        }

        private string ExtractTokenFromLink(string link)
        {
            int startIndex = link.LastIndexOf("/") + 1;
            return link.Substring(startIndex);
        }

        private string PostPartnershipRequest(string email, string orgLink)
        {
            
            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = apiCataloglinks["partnerships"].uri,
                Method = Hammock.Web.WebMethod.Post
            };
            //request.Proxy = "http://localhost:8888";


            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            request.AddHeader("Content-Type", "application/vnd.deere.axiom.v3+json");

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

            request.AddPostContent(Encoding.UTF8.GetBytes(json));
            Logger.Log("RESOURCE", "PostPartnershipRequest");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return response.Headers["Location"];
                }
                else
                {
                    return "";
                }
            }
        }

        private bool PostPermissionRequest(string permissionsLink)
        {

            Hammock.RestClient client = getRestClient();
            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = permissionsLink,
                Method = Hammock.Web.WebMethod.Post
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            request.AddHeader("Content-Type", "application/vnd.deere.axiom.v3+json");

            string permissionJson =
@"{
   ""permissions"": [
      {
         ""type"": ""productionAgronomicDetailData"",
         ""status"": ""requested""
      }
   ]
}";
            request.AddPostContent(Encoding.UTF8.GetBytes(permissionJson));
            using (Hammock.RestResponse response = client.Request(request))
            {
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
        }
        #endregion

        public void retrieveApiCatalog(string endpoint)
        {

            apiCataloglinks = (Dictionary<String, Link>)CacheManager.GetCacheItem("api_catalog_links");

            if (apiCataloglinks == null)
            {

                Hammock.RestClient client = getRestClient();

                Hammock.RestRequest request = new Hammock.RestRequest()
                {
                    Path = endpoint
                };

                request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
                Logger.Log("RESOURCE", "retrieveAPICatalog");
                using (Hammock.RestResponse response = client.Request(request))
                {
                    Logger.Log(response);
                    ApiCatalog apiCatalog = PartnershipWorkflow.Deserialise<ApiCatalog>(response.ContentStream);
                    apiCataloglinks = OAuthWorkFlow.linksFrom(apiCatalog);
                    CacheManager.AddCacheItem("api_catalog_links", apiCataloglinks, 60 * 6);
                }
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

        public List<Organization> getMyOrganizations()
        {
            User currentUser = getCurrentUser(true);
            return currentUser.Organizations;
        }

        public List<Partnership> getPartnerships(bool fetchNewFiles, string[] restrictToOrgIds)
        {
            List<Partnership> relavantPartnerships = null;

            User currentUser = getCurrentUser(true);

            //find my organization
            //Organization currentOrg = null;

            /*if (currentUser.Organizations != null && currentUser.Organizations.Count() > 0)
            {
                currentOrg = currentUser.Organizations[0];
            }*/

            List<Partnership> partnerships = getPartnerShipsList(apiCataloglinks["partnerships"].uri);

            relavantPartnerships = new List<Partnership>();
            foreach (var p in partnerships)
            {
                var partnershipLinks = OAuthWorkFlow.linksFrom(p);
                Link fromPartnerShipLink = partnershipLinks["fromPartnership"];
                Link toPartnerShipLink = partnershipLinks["toPartnership"];

                if (fromPartnerShipLink.uri.ToLower() != toPartnerShipLink.uri.ToLower() 
                    && currentUser.Organizations.Count(o => toPartnerShipLink.uri.ToLower().IndexOf(o.id.ToLower()) >= 0) >= 0)
                {
                    //if a list of org ids is passed in restrict to then apply filter
                    if (restrictToOrgIds == null || restrictToOrgIds.Count() == 0 || restrictToOrgIds.Any(id => fromPartnerShipLink.uri.IndexOf(id) >= 0))
                    {
                        if (!relavantPartnerships.Any(r => OAuthWorkFlow.linksFrom(r)["fromPartnership"].uri == fromPartnerShipLink.uri))
                        {
                            fillPartnershipTree(p, fetchNewFiles);
                            relavantPartnerships.Add(p);
                        }
                    }
                }
            }
            //CacheManager.AddCacheItem("partnerships", relavantPartnerships, 5);

            return relavantPartnerships;
        }

        public User getCurrentUser(bool fillChildOrgs)
        {
            string cacheKey = "current_user_childorgs_" + fillChildOrgs.ToString();
            User currentUser = (User)CacheManager.GetCacheItem(cacheKey);

            if (currentUser == null)
            {
                Hammock.RestClient client = getRestClient();
                Hammock.RestRequest request = new Hammock.RestRequest()
                {
                    Path = apiCataloglinks["currentUser"].uri
                };
                request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
                Logger.Log("RESOURCE", "getCurrentUser");
                using (Hammock.RestResponse response = client.Request(request))
                {
                    Logger.Log(response);
                    currentUser = PartnershipWorkflow.Deserialise<User>(response.ContentStream);

                    if (fillChildOrgs)
                    {
                        currentUser.Organizations = getList<Organization>(OAuthWorkFlow.linksFrom(currentUser)["organizations"].uri);
                    }
                    CacheManager.AddCacheItem(cacheKey, currentUser, 30);
                }
            }
            return currentUser;         
        }
        
        public string RequestPartnerPermissions(string email, string myRequestingOrgId)
        {
            User currentUser = getCurrentUser(true);

            //find my organization
            Organization currentOrg = null;

            if (currentUser.Organizations != null && currentUser.Organizations.Count() > 0)
            {                
                currentOrg = currentUser.Organizations.SingleOrDefault(o => o.id == myRequestingOrgId);
            }

            var orgLink = OAuthWorkFlow.linksFrom(currentOrg)["self"].uri;
            var partnerLink = PostPartnershipRequest(email, orgLink);            

            if (!String.IsNullOrEmpty(partnerLink))
            {             
                Resource partnership = getPartnership(partnerLink);

                //get contact invitation link
                var links = OAuthWorkFlow.linksFrom(partnership);
                var contactInviteLink = links["contactInvitation"];

                //result.PartnerLink = links["self"].uri;
                //var contactInviteToken = ExtractTokenFromLink(contactInviteLink.uri);
                //result.OrgId = string.Empty;
                //result.ContactInviteLink = contactInviteLink.uri;

                Resource contactInvite = getPartnership(contactInviteLink.uri);

                partnerLink = contactInviteLink.uri;

                var contactLinks = OAuthWorkFlow.linksFrom(contactInvite);
                PostPermissionRequest(contactLinks["permissions"].uri);                
            }

            return partnerLink;        
        }

        public bool downloadFileInPiecesAndComputeMd5(string path, DataContracts.File file)
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
                if (!getChunkFromStartAndRecurse(file, 0, end, file.nativeSize, output))
                {
                    result = false;
                }
            }

            return result;
        }

        
        private bool getChunkFromStartAndRecurse(DataContracts.File file, long start, long chunkSize, long fileSize, Stream output)
        {
            bool result = true;

            if (fileSize <= chunkSize)
            {
                result = createDownloadRequest(file, start, fileSize, output);

                if (result == false)
                {
                    System.Threading.Thread.Sleep(1000);   //pause then retry
                    result = createDownloadRequest(file, start, chunkSize, output);
                }
            }
            else
            {
                result = createDownloadRequest(file, start, chunkSize, output);

                if (result == false)
                {
                    System.Threading.Thread.Sleep(1000);   //pause then retry
                    result = createDownloadRequest(file, start, chunkSize, output);
                }

                if (result)
                {
                    result = getChunkFromStartAndRecurse(file, start + chunkSize, chunkSize, fileSize - chunkSize, output);
                }
            }

            return result;
        }

        private bool createDownloadRequest(DataContracts.File file, long start, long bytesToRead, Stream output)
        {
            Hammock.Authentication.OAuth.OAuthCredentials credentials = OAuthWorkFlow.createOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.TOKEN.token,
            ApiCredentials.TOKEN.secret, null, null);
            Hammock.RestClient client = new Hammock.RestClient()
            {
                Authority = "",
                Credentials = credentials
            };

            var links = OAuthWorkFlow.linksFrom(file);

            Hammock.RestRequest request = new Hammock.RestRequest()
            {
                Path = links["self"].uri,
                Method = Hammock.Web.WebMethod.Get
            };
            request.AddHeader("Accept", "application/zip");
            request.AddParameter("offset", "" + start);
            request.AddParameter("size", "" + bytesToRead);            
            Logger.Log("RESOURCE", "createDownloadRequest");
            using (Hammock.RestResponse response = client.Request(request))
            {
                Logger.Log(response);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (Stream input = response.ContentStream)
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
}
