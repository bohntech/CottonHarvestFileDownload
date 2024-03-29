﻿/****************************************************************************
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
using CottonHarvestDataTransferApp.Remote.Data;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.Remote
{
    public delegate void TokenRefreshHandler(Token token);

    public delegate bool TokenExpiredCheckHandler();

    /// <summary>
    /// Interface for interacting with remote oAuth dataprovider
    /// </summary>
    public interface IRemoteDataRepository
    {

        void SetTokenRefreshHandler(TokenRefreshHandler handler);

        void SetTokenExpiredHandler(TokenExpiredCheckHandler handler);

        void Initialize(string endpoint, string wellknown, RemoteProviderType providerType, 
            TokenRefreshHandler refreshHandler, TokenExpiredCheckHandler expireHandler, 
            string clientAppToken="", string clientAppSecret="", string clientAuthToken="", string clientAuthSecret="" );

        //string GetManualAuthUrl();

        //bool ExchangeVerifierForCredentials(string verifier, ref string authToken, ref string authSecret);

        void SetAuthData(string accessToken, string refreshToken);

        Task<string> TestConnection();

        Task<List<Partner>> FetchAllPartners(List<OrgFileETag> fileETags, string[] restrictToOrgIds);

        Task<string> RequestPartnerPermission(string email, string myOrgId);

        Task<string> GetRemoteIDFromPartnerLink(string link);

        Task<List<OrgFileETag>> DownloadOrganizationFiles(List<OrgFileETag> fileETags, string filePath, string[] orgIds, string[] downloadedFileIds, Action<FileDownloadedResult> callback, Action<FileDownloadProgress> progressCallback, Action<FileDownloadedResult> errorCallback);

        Task<List<UserOrganization>> GetMyOrganizations();

        void EmptyAllCacheItems();
    }
}
