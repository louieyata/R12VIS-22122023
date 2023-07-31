namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransferAddressColumnsFromVaccinationToPersonTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaccination", "Barangay_barangay_id", "dbo.Barangay");
            DropForeignKey("dbo.Vaccination", "CityMunicipality_city_municipality_id", "dbo.CityMunicipality");
            DropForeignKey("dbo.Vaccination", "Province_province_id", "dbo.Province");
            DropIndex("dbo.Vaccination", new[] { "Barangay_barangay_id" });
            DropIndex("dbo.Vaccination", new[] { "CityMunicipality_city_municipality_id" });
            DropIndex("dbo.Vaccination", new[] { "Province_province_id" });
            AddColumn("dbo.Person", "ProvinceID", c => c.Int(nullable: false));
            AddColumn("dbo.Person", "CityMunicipalityID", c => c.Int(nullable: false));
            AddColumn("dbo.Person", "BarangayID", c => c.Int(nullable: false));
            AddColumn("dbo.Person", "Barangay_barangay_id", c => c.Int());
            AddColumn("dbo.Person", "CityMunicipality_city_municipality_id", c => c.Int());
            AddColumn("dbo.Person", "Province_province_id", c => c.Int());
            CreateIndex("dbo.Person", "Barangay_barangay_id");
            CreateIndex("dbo.Person", "CityMunicipality_city_municipality_id");
            CreateIndex("dbo.Person", "Province_province_id");
            AddForeignKey("dbo.Person", "Barangay_barangay_id", "dbo.Barangay", "barangay_id");
            AddForeignKey("dbo.Person", "CityMunicipality_city_municipality_id", "dbo.CityMunicipality", "city_municipality_id");
            AddForeignKey("dbo.Person", "Province_province_id", "dbo.Province", "province_id");
            DropColumn("dbo.Vaccination", "ProvinceID");
            DropColumn("dbo.Vaccination", "CityMunicipalityID");
            DropColumn("dbo.Vaccination", "BarangayID");
            DropColumn("dbo.Vaccination", "Barangay_barangay_id");
            DropColumn("dbo.Vaccination", "CityMunicipality_city_municipality_id");
            DropColumn("dbo.Vaccination", "Province_province_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vaccination", "Province_province_id", c => c.Int());
            AddColumn("dbo.Vaccination", "CityMunicipality_city_municipality_id", c => c.Int());
            AddColumn("dbo.Vaccination", "Barangay_barangay_id", c => c.Int());
            AddColumn("dbo.Vaccination", "BarangayID", c => c.Int(nullable: false));
            AddColumn("dbo.Vaccination", "CityMunicipalityID", c => c.Int(nullable: false));
            AddColumn("dbo.Vaccination", "ProvinceID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Person", "Province_province_id", "dbo.Province");
            DropForeignKey("dbo.Person", "CityMunicipality_city_municipality_id", "dbo.CityMunicipality");
            DropForeignKey("dbo.Person", "Barangay_barangay_id", "dbo.Barangay");
            DropIndex("dbo.Person", new[] { "Province_province_id" });
            DropIndex("dbo.Person", new[] { "CityMunicipality_city_municipality_id" });
            DropIndex("dbo.Person", new[] { "Barangay_barangay_id" });
            DropColumn("dbo.Person", "Province_province_id");
            DropColumn("dbo.Person", "CityMunicipality_city_municipality_id");
            DropColumn("dbo.Person", "Barangay_barangay_id");
            DropColumn("dbo.Person", "BarangayID");
            DropColumn("dbo.Person", "CityMunicipalityID");
            DropColumn("dbo.Person", "ProvinceID");
            CreateIndex("dbo.Vaccination", "Province_province_id");
            CreateIndex("dbo.Vaccination", "CityMunicipality_city_municipality_id");
            CreateIndex("dbo.Vaccination", "Barangay_barangay_id");
            AddForeignKey("dbo.Vaccination", "Province_province_id", "dbo.Province", "province_id");
            AddForeignKey("dbo.Vaccination", "CityMunicipality_city_municipality_id", "dbo.CityMunicipality", "city_municipality_id");
            AddForeignKey("dbo.Vaccination", "Barangay_barangay_id", "dbo.Barangay", "barangay_id");
        }
    }
}
