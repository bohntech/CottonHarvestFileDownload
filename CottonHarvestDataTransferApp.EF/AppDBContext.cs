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
using CottonHarvestDataTransferApp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using CottonHarvestDataTransferApp.EF.Migrations;

namespace CottonHarvestDataTransferApp.EF
{
    /// <summary>
    /// Entity Framework Code first Datacontext for data access
    /// </summary>
    public class AppDBContext : DbContext
    {
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<SourceFile> SourceFiles { get; set; }
        public DbSet<CottonHarvestDataTransferApp.Data.Setting> Settings { get; set; }
        public DbSet<OutputFile>  OutputFiles { get; set; }

        public AppDBContext() : base()
        {            
            InitializeDatabase();
        }
      
        /// <summary>
        /// Initialize entity keys and schema requirements
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSource>()
                .HasKey<int>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Organization>()
                .HasKey<int>(k => k.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SourceFile>()
                .HasKey<int>(k => k.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<OutputFile>()
               .HasKey<int>(k => k.Id)
               .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Setting>()
                .HasKey<int>(k => k.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Creates database and initialized default settings if does not already exist
        /// </summary>
        protected virtual void InitializeDatabase()
        {
            try
            {
                Database.SetInitializer<AppDBContext>(new FirstRunInitializer());
                if (!this.Database.Exists())
                {
                    this.Database.Initialize(false);
                }
            }
            catch(Exception exc)
            {
                int i = 0;
            }
        }
    }
}
