namespace CottonHarvestDataTransferApp.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CottonHarvestDataTransferApp.EF.AppDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CottonHarvestDataTransferApp.EF.AppDBContext context)
        {
           //run when update-command is run from package manager
        }
    }
}