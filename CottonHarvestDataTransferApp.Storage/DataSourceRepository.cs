using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.Storage
{
    public class DataSourceRepository : GenericRepository<DataSource>
    {
        public DataSourceRepository(AppDBEntities context) : base(context)
        {

        }
    }
}
