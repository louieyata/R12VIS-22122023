namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adverse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Event = c.String(),
                        Condition = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        CityMunicipalityCodeExcel = c.String(),
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
                        RegionID = c.Int(),
                        province_code = c.String(),
                        province_code_excel = c.String(),
                    })
                .PrimaryKey(t => t.province_id)
                .ForeignKey("dbo.Region", t => t.RegionID)
                .Index(t => t.RegionID);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Deferral",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dose",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VaccineDose = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EthnicGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndigenousMember = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UniquePersonID = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        ContactNumber = c.String(),
                        GuardianName = c.String(),
                        isMale = c.Boolean(nullable: false),
                        isPWD = c.Boolean(nullable: false),
                        EthnicGroupID = c.Int(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EthnicGroup", t => t.EthnicGroupID)
                .Index(t => t.EthnicGroupID);
            
            CreateTable(
                "dbo.PriorityGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.Vaccination",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PriorityGroupID = c.Int(nullable: false),
                        PersonID = c.Int(nullable: false),
                        ProvinceID = c.Int(nullable: false),
                        CityMunicipalityID = c.Int(nullable: false),
                        BarangayID = c.Int(nullable: false),
                        DeferralID = c.Int(),
                        VaccinationDate = c.DateTime(nullable: false),
                        VaccineID = c.Int(nullable: false),
                        BatchNumber = c.String(),
                        LotNumber = c.String(),
                        BakunaCenterCBCRID = c.String(),
                        VaccinatorName = c.String(),
                        DoseID = c.Int(nullable: false),
                        AdverseID = c.Int(),
                        Barangay_barangay_id = c.Int(),
                        CityMunicipality_city_municipality_id = c.Int(),
                        Province_province_id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Adverse", t => t.AdverseID)
                .ForeignKey("dbo.Barangay", t => t.Barangay_barangay_id)
                .ForeignKey("dbo.CityMunicipality", t => t.CityMunicipality_city_municipality_id)
                .ForeignKey("dbo.Deferral", t => t.DeferralID)
                .ForeignKey("dbo.Dose", t => t.DoseID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .ForeignKey("dbo.PriorityGroup", t => t.PriorityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Province", t => t.Province_province_id)
                .ForeignKey("dbo.Vaccine", t => t.VaccineID, cascadeDelete: true)
                .Index(t => t.PriorityGroupID)
                .Index(t => t.PersonID)
                .Index(t => t.DeferralID)
                .Index(t => t.VaccineID)
                .Index(t => t.DoseID)
                .Index(t => t.AdverseID)
                .Index(t => t.Barangay_barangay_id)
                .Index(t => t.CityMunicipality_city_municipality_id)
                .Index(t => t.Province_province_id);
            
            CreateTable(
                "dbo.Vaccine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VaccineManufacturer = c.String(),
                        VaccineBrand = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaccination", "VaccineID", "dbo.Vaccine");
            DropForeignKey("dbo.Vaccination", "Province_province_id", "dbo.Province");
            DropForeignKey("dbo.Vaccination", "PriorityGroupID", "dbo.PriorityGroup");
            DropForeignKey("dbo.Vaccination", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Vaccination", "DoseID", "dbo.Dose");
            DropForeignKey("dbo.Vaccination", "DeferralID", "dbo.Deferral");
            DropForeignKey("dbo.Vaccination", "CityMunicipality_city_municipality_id", "dbo.CityMunicipality");
            DropForeignKey("dbo.Vaccination", "Barangay_barangay_id", "dbo.Barangay");
            DropForeignKey("dbo.Vaccination", "AdverseID", "dbo.Adverse");
            DropForeignKey("dbo.User", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Person", "EthnicGroupID", "dbo.EthnicGroup");
            DropForeignKey("dbo.Barangay", "city_municipality_id", "dbo.CityMunicipality");
            DropForeignKey("dbo.CityMunicipality", "province_id", "dbo.Province");
            DropForeignKey("dbo.Province", "RegionID", "dbo.Region");
            DropIndex("dbo.Vaccination", new[] { "Province_province_id" });
            DropIndex("dbo.Vaccination", new[] { "CityMunicipality_city_municipality_id" });
            DropIndex("dbo.Vaccination", new[] { "Barangay_barangay_id" });
            DropIndex("dbo.Vaccination", new[] { "AdverseID" });
            DropIndex("dbo.Vaccination", new[] { "DoseID" });
            DropIndex("dbo.Vaccination", new[] { "VaccineID" });
            DropIndex("dbo.Vaccination", new[] { "DeferralID" });
            DropIndex("dbo.Vaccination", new[] { "PersonID" });
            DropIndex("dbo.Vaccination", new[] { "PriorityGroupID" });
            DropIndex("dbo.User", new[] { "RoleID" });
            DropIndex("dbo.Person", new[] { "EthnicGroupID" });
            DropIndex("dbo.Province", new[] { "RegionID" });
            DropIndex("dbo.CityMunicipality", new[] { "province_id" });
            DropIndex("dbo.Barangay", new[] { "city_municipality_id" });
            DropTable("dbo.Vaccine");
            DropTable("dbo.Vaccination");
            DropTable("dbo.User");
            DropTable("dbo.Role");
            DropTable("dbo.PriorityGroup");
            DropTable("dbo.Person");
            DropTable("dbo.EthnicGroup");
            DropTable("dbo.Dose");
            DropTable("dbo.Deferral");
            DropTable("dbo.Region");
            DropTable("dbo.Province");
            DropTable("dbo.CityMunicipality");
            DropTable("dbo.Barangay");
            DropTable("dbo.Adverse");
        }
    }
}
