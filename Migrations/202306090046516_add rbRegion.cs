namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrbRegion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CityMunicipality", "CityMunicipalityCodeExcel", c => c.String());
            AddColumn("dbo.Province", "RegionID", c => c.Int());
            AddColumn("dbo.Province", "province_code_excel", c => c.String());
            CreateIndex("dbo.Province", "RegionID");
            AddForeignKey("dbo.Province", "RegionID", "dbo.Region", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Province", "RegionID", "dbo.Region");
            DropIndex("dbo.Province", new[] { "RegionID" });
            DropColumn("dbo.Province", "province_code_excel");
            DropColumn("dbo.Province", "RegionID");
            DropColumn("dbo.CityMunicipality", "CityMunicipalityCodeExcel");
            DropTable("dbo.Region");
        }
    }
}
