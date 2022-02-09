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
using System.Data.Entity;

namespace CottonHarvestDataTransferApp.EF
{
    /// <summary>
    /// This class is an initializer to populate the database with initial 
    /// settings when first created.
    /// </summary>
    public class FirstRunInitializer : CreateDatabaseIfNotExists<AppDBContext>
    {
        protected override void Seed(AppDBContext context)
        {
            context.Settings.Add(new Data.Setting { Id = 1, Key = Data.SettingKeyType.DownloadFolderKey, Value = "" });
            context.Settings.Add(new Data.Setting { Id = 2, Key = Data.SettingKeyType.DownloadFrequencyTypeKey, Value = "1" });
            context.Settings.Add(new Data.Setting { Id = 3, Key = Data.SettingKeyType.DailyDownloadTimeKey, Value = "01/01/2016 7:00AM" });
            context.Settings.Add(new Data.Setting { Id = 4, Key = Data.SettingKeyType.HourlyDownloadTimeKey, Value = "1" });
            //context.Settings.Add(new Data.Setting { Id = 5, Key = Data.SettingKeyType.JDAuthTokenKey, Value = "" });
            //context.Settings.Add(new Data.Setting { Id = 6, Key = Data.SettingKeyType.JDAuthSecretKey, Value = "" });
            context.Settings.Add(new Data.Setting { Id = 7, Key = Data.SettingKeyType.LastDownload, Value = DateTime.Now.ToString() });
            context.Settings.Add(new Data.Setting { Id = 8, Key = Data.SettingKeyType.JDAccessToken, Value = "" });
            context.Settings.Add(new Data.Setting { Id = 9, Key = Data.SettingKeyType.JDRefreshToken, Value = "" });
            context.Settings.Add(new Data.Setting { Id = 10, Key = Data.SettingKeyType.JDAccessTokenExpires, Value = "12/01/1980 12:00AM" });
            context.Settings.Add(new Data.Setting { Id = 11, Key = Data.SettingKeyType.JDCredentialDateTime, Value = "" });
            context.Settings.Add(new Data.Setting { Id = 12, Key = Data.SettingKeyType.LastUserID, Value = "" });

            context.DataSources.Add(new Data.DataSource { Id = 1, Name = "MyJohnDeere" });
        }
    }
}
