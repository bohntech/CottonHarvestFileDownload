using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonHarvestDataTransferApp.Data
{
    public class OutputFile
    {
        public int Id { get; set; }
        public int SourceFileId { get; set; }
        public SourceFile SourceFile { get; set; }
        public string Filename { get; set; }                
        public DateTime Created { get; set; }        
    }
}
