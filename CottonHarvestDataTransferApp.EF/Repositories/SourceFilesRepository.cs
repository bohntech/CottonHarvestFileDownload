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
    public class SourceFilesRepository : GenericRepository<SourceFile>, ISourceFilesRepository
    {
        public SourceFilesRepository(AppDBContext context) : base(context)
        {

        }

        public int GetNextBatchNumber()
        {
            int result = 0;
            if (_context.SourceFiles.Count() > 0)
            {
                result = _context.SourceFiles.Max(x => x.BatchNumber);
            }
            return result + 1;
        }

        public int GetLastBatchNumber()
        {
            int result = 0;
            if (_context.SourceFiles.Count() > 0)
            {
                result = _context.SourceFiles.Max(x => x.BatchNumber);
            }
            return result;
        }

        public string[] GetFileIds()
        {
            return _context.SourceFiles.Select(x => x.FileIdentifer).ToArray();
        }

        public List<SourceFile> GetRecentFiles()
        {
            int lastBatch = GetLastBatchNumber();

            return _context.SourceFiles.Include("Organization").Where(d => d.BatchNumber == lastBatch).OrderByDescending(d => d.LastDownload).ToList();
        }
    }
}
