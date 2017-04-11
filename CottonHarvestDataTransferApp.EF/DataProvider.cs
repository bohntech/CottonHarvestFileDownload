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
    /// <summary>
    /// Entity Framework / MS SQL Local db specific implementation
    /// of a dataprovider for storage
    /// </summary>
    public class DataProvider : IUnitOfWorkDataProvider, IDisposable
    {
        AppDBContext context = null;

        public IDataSourcesRepository DataSources { get; private set; }
        public IOrganizationsRepository Organizations { get; private set; }
        public ISourceFilesRepository SourceFiles { get; private set; }
        public ISettingsRepository Settings { get; private set; }
        public IOutputFilesRepository OutputFiles { get; private set; }

        public DataProvider()
        {
            context = new AppDBContext();
            DataSources = (IDataSourcesRepository) new DataSourcesRepository(context);
            Organizations = (IOrganizationsRepository) new OrganizationsRepository(context);
            SourceFiles = (ISourceFilesRepository) new SourceFilesRepository(context);
            OutputFiles = (IOutputFilesRepository)new OutputFilesRepository(context);
            Settings = (ISettingsRepository) new SettingsRepository(context);
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
