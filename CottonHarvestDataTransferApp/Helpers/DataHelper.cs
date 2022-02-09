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
using CottonHarvestDataTransferApp.Classes;
using CottonHarvestDataTransferApp.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Net.NetworkInformation;
using CottonHarvestDataTransferApp.Logging;

namespace CottonHarvestDataTransferApp.Helpers
{
    public enum ImportStatus { REMOTE_FETCH_ERROR = 0, NO_PARTNERS = 1, INVITATION_ERROR = 2, SAVE_ERROR=4, SUCCESS=5 }

    public class ImportPartnerResult
    {
        public ImportStatus Status { get; set; }
        public string Message { get; set; }
    }

    public static class DataHelper
    {

        public static bool IsTokenExpired()
        {
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                var expiresAt = dp.Settings.GetAccessTokenExpiresUTC();
                expiresAt = expiresAt.AddMinutes(-10);

                return DateTime.UtcNow > expiresAt;
            }
        }

        public static void SaveNewToken(Token token)
        {
            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDAccessToken, token.access_token);
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDAccessTokenExpires, DateTime.UtcNow.AddSeconds(token.expires_in).ToString());
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDRefreshToken, token.refresh_token);
                dp.Settings.UpsertSettingWithKey(SettingKeyType.JDCredentialDateTime, DateTime.UtcNow.ToString());
                dp.SaveChanges();
            }
        }


        /// <summary>
        /// This helper method handles the import of partners.
        /// </summary>
        /// <param name="results">List of partners retrieved from remote data source</param>
        /// <param name="displayMessage">Error message if any</param>
        /// <param name="remoteDataRepository">repository instance to access the remote datasource</param>
        /// <param name="addNew"></param>
        /// <returns></returns>
        public static async Task<ImportPartnerResult> ImportPartners(List<Partner> results, IRemoteDataRepository remoteDataRepository, bool addNew)
        {
            results = null;
            ImportPartnerResult result = new ImportPartnerResult();
            try
            {
                using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
                {
                    var orgIds = dp.Organizations.GetAll().Select(o => o.RemoteID).ToArray();

                    //passing null for eTags results in only total file count being fetched
                    //instead of downloading files when filing the partner object tree
                    results = await remoteDataRepository.FetchAllPartners(null, (addNew) ? null : orgIds);
                }
            }
            catch (Exception fetchExc)
            {
                result.Message = "An error occurred fetching partner data.";
                Logger.Log("MESSAGE", result.Message);
                Logger.Log(fetchExc);
                result.Status = ImportStatus.REMOTE_FETCH_ERROR;
                return result;
            }

            if (results == null || results.Count() == 0)
            {
                result.Message = "No partners found.";
                result.Status = ImportStatus.REMOTE_FETCH_ERROR;
                return result;
            }

            using (IUnitOfWorkDataProvider dp = AppStorage.GetUnitOfWorkDataProvider())
            {
                try
                {
                    foreach (var p in dp.Organizations.GetAll())
                    {
                        if (string.IsNullOrEmpty(p.RemoteID) && !string.IsNullOrEmpty(p.PartnerLink))
                        {
                            p.RemoteID = await remoteDataRepository.GetRemoteIDFromPartnerLink(p.PartnerLink);
                            dp.Organizations.Update(p);
                        }
                    }
                    dp.SaveChanges();
                }
                catch (Exception exc)
                {                    
                    result.Message = "An error occurred checking status of pending partner invitations.";
                    Logger.Log("MESSAGE", result.Message);
                    Logger.Log(exc);
                    result.Status = ImportStatus.REMOTE_FETCH_ERROR;
                    return result;
                }

                try
                {
                    foreach (var p in results)
                    {
                        Organization existingOrg = null;

                        var matches = dp.Organizations.FindMatching(x => x.RemoteID == p.Id);

                        if (matches.Count() > 0)
                        {
                            existingOrg = matches.ToArray()[0];
                        }

                        if (existingOrg == null)  //try to find a matching partnership link
                        {
                            existingOrg = dp.Organizations.FindSingle(x => x.PartnerLink == p.PartnershipLink);
                        }

                        if (existingOrg == null) //only import new partners
                        {
                            if (addNew)
                            {
                                Organization newOrg = new Organization();
                                newOrg.Name = p.Name;
                                newOrg.Created = DateTime.Now;
                                newOrg.DataSourceId = 1;
                                newOrg.RemoteID = p.Id;
                                newOrg.MyLinkedOrgId = p.MyLinkedOrgId;
                                newOrg.SharedFiles = p.SharedFileCount;
                                newOrg.PermissionGranted = p.PermissionGranted;
                                newOrg.FilesETag = "";
                                newOrg.FilesETagDate = DateTime.Now;
                                dp.Organizations.Add(newOrg);
                            }
                        }
                        else
                        {
                            existingOrg.RemoteID = p.Id;
                            existingOrg.MyLinkedOrgId = p.MyLinkedOrgId;
                            existingOrg.SharedFiles = p.SharedFileCount;
                            existingOrg.PermissionGranted = p.PermissionGranted;
                            dp.Organizations.Update(existingOrg);
                        }
                    }

                    dp.SaveChanges();
                }
                catch (Exception exc)
                {
                    result.Message = "An error occurred saving partners. " + exc.Message;
                    Logger.Log("MESSAGE", result.Message);
                    Logger.Log(exc);
                    result.Status = ImportStatus.SAVE_ERROR;
                    return result;
                }
            }
            result.Status = ImportStatus.SUCCESS;
            result.Message = "";
            return result;
        }
    }
}