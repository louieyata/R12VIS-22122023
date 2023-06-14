namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addadditionaltbl : DbMigration
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
                "dbo.PriorityGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            DropIndex("dbo.Vaccination", new[] { "Province_province_id" });
            DropIndex("dbo.Vaccination", new[] { "CityMunicipality_city_municipality_id" });
            DropIndex("dbo.Vaccination", new[] { "Barangay_barangay_id" });
            DropIndex("dbo.Vaccination", new[] { "AdverseID" });
            DropIndex("dbo.Vaccination", new[] { "DoseID" });
            DropIndex("dbo.Vaccination", new[] { "VaccineID" });
            DropIndex("dbo.Vaccination", new[] { "DeferralID" });
            DropIndex("dbo.Vaccination", new[] { "PersonID" });
            DropIndex("dbo.Vaccination", new[] { "PriorityGroupID" });
            DropTable("dbo.Vaccination");
            DropTable("dbo.PriorityGroup");
            DropTable("dbo.Dose");
            DropTable("dbo.Deferral");
            DropTable("dbo.Adverse");
        }
    }
}
