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
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.EF
{
    public class SettingsRepository : GenericRepository<Setting>, ISettingsRepository
    {
        public SettingsRepository(AppDBContext context) : base(context)
        {

        }

        /// <summary>
        /// Retreives current download folder setting
        /// </summary>
        /// <returns></returns>
        public string GetDownloadFolder()
        {
            var setting = _context.Settings.SingleOrDefault(s => s.Key == SettingKeyType.DownloadFolderKey);

            return (setting != null) ? setting.Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Updates setting with new value
        /// </summary>
        /// <param name="key">The key indicating the setting to update</param>
        /// <param name="value">The value to save for the setting</param>
        public void UpdateSettingWithKey(SettingKeyType key, string value)
        {
            var setting = _context.Settings.SingleOrDefault(s => s.Key == key);

            if (setting != null)
            {
                setting.Value = value;
            }           
        }


    }
}
