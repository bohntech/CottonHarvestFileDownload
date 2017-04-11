using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonHarvestDataTransferApp.Storage
{
    public class DataProvider : IDisposable
    {
        AppDBEntities context = null;

        public DataSourceRepository DataSources { get; private set; }
        public OrganizationRepository Organizations { get; private set; }
        public RecentFileRepository RecentFiles { get; private set; }
        public SettingsRepository Settings { get; private set; }

        public DataProvider()
        {
            context = new AppDBEntities();
            DataSources = new DataSourceRepository(context);
            Organizations = new OrganizationRepository(context);
            RecentFiles = new RecentFileRepository(context);
            Settings = new SettingsRepository(context);
        }

        public void Dispose()
        {
            context.Dispose();
        }
            
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
