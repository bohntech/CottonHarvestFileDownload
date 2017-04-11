using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.Storage
{
    public class SettingsRepository : GenericRepository<Setting>
    {
        public SettingsRepository(AppDBEntities context) : base(context)
        {

        }

        public string GetDownloadFolder()
        {
            var setting = _context.Settings.SingleOrDefault(s => s.Key == "DownloadFolder");

            return (setting != null) ? setting.Value.Trim() : string.Empty;
        }

        public void UpdateSettingWithKey(string key, string value)
        {
            var setting = _context.Settings.SingleOrDefault(s => s.Key == key);

            if (setting != null)
            {
                setting.Value = value;
            }
        }


    }
}
