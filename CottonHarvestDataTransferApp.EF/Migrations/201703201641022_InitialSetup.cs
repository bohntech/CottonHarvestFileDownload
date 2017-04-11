namespace CottonHarvestDataTransferApp.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataSources",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FilesETag = c.String(),
                        FilesETagDate = c.DateTime(nullable: false),
                        RemoteID = c.String(),
                        MyLinkedOrgId = c.String(),
                        RequestingOrgID = c.String(),
                        PartnerLink = c.String(),
                        PermissionGranted = c.Boolean(nullable: false),
                        SharedFiles = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        DataSourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataSources", t => t.DataSourceId, cascadeDelete: true)
                .Index(t => t.DataSourceId);
            
            CreateTable(
                "dbo.OutputFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SourceFileId = c.Int(nullable: false),
                        Filename = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SourceFiles", t => t.SourceFileId, cascadeDelete: true)
                .Index(t => t.SourceFileId);
            
            CreateTable(
                "dbo.SourceFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        SourceFilename = c.String(),
                        FileIdentifer = c.String(),
                        FirstLineText = c.String(),
                        FirstDownload = c.DateTime(nullable: false),
                        LastDownload = c.DateTime(nullable: false),
                        LineCount = c.Int(nullable: false),
                        BatchNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Key = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OutputFiles", "SourceFileId", "dbo.SourceFiles");
            DropForeignKey("dbo.SourceFiles", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "DataSourceId", "dbo.DataSources");
            DropIndex("dbo.SourceFiles", new[] { "OrganizationId" });
            DropIndex("dbo.OutputFiles", new[] { "SourceFileId" });
            DropIndex("dbo.Organizations", new[] { "DataSourceId" });
            DropTable("dbo.Settings");
            DropTable("dbo.SourceFiles");
            DropTable("dbo.OutputFiles");
            DropTable("dbo.Organizations");
            DropTable("dbo.DataSources");
        }
    }
}
