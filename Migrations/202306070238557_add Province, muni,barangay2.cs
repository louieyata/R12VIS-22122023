namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProvincemunibarangay2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Barangay",
                c => new
                    {
                        barangay_id = c.Int(nullable: false, identity: true),
                        city_municipality_id = c.Int(nullable: false),
                        barangay_name = c.String(),
                        province_code = c.String(),
                        city_municipality_code = c.String(),
                        barangay_code = c.String(),
                    })
                .PrimaryKey(t => t.barangay_id)
                .ForeignKey("dbo.CityMunicipality", t => t.city_municipality_id, cascadeDelete: true)
                .Index(t => t.city_municipality_id);
            
            CreateTable(
                "dbo.CityMunicipality",
                c => new
                    {
                        city_municipality_id = c.Int(nullable: false, identity: true),
                        province_id = c.Int(nullable: false),
                        CityMunicipalityName = c.String(),
                        ProvinceCode = c.String(),
                        CityMunicipalityCode = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.city_municipality_id)
                .ForeignKey("dbo.Province", t => t.province_id, cascadeDelete: true)
                .Index(t => t.province_id);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        province_id = c.Int(nullable: false, identity: true),
                        province_name = c.String(),
                        province_code = c.String(),
                    })
                .PrimaryKey(t => t.province_id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Access = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        isActive = c.Boolean(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Barangay", "city_municipality_id", "dbo.CityMunicipality");
            DropForeignKey("dbo.CityMunicipality", "province_id", "dbo.Province");
            DropIndex("dbo.User", new[] { "RoleID" });
            DropIndex("dbo.CityMunicipality", new[] { "province_id" });
            DropIndex("dbo.Barangay", new[] { "city_municipality_id" });
            DropTable("dbo.User");
            DropTable("dbo.Role");
            DropTable("dbo.Province");
            DropTable("dbo.CityMunicipality");
            DropTable("dbo.Barangay");
        }
    }
}
