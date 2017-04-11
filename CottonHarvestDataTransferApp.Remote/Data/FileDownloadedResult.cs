using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonHarvestDataTransferApp.Remote
{
    public class FileDownloadedResult
    {
        public string OrganizationID { get; set; }
        public string Filename { get; set; }
        public string FileIdentifier { get; set; }
    }
}
