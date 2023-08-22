namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfieldsperson : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Person", new[] { "Barangay_barangay_id" });
            DropIndex("dbo.Person", new[] { "CityMunicipality_city_municipality_id" });
            DropIndex("dbo.Person", new[] { "Province_province_id" });
            DropColumn("dbo.Person", "BarangayID");
            DropColumn("dbo.Person", "CityMunicipalityID");
            DropColumn("dbo.Person", "ProvinceID");
            RenameColumn(table: "dbo.Person", name: "Barangay_barangay_id", newName: "BarangayID");
            RenameColumn(table: "dbo.Person", name: "CityMunicipality_city_municipality_id", newName: "CityMunicipalityID");
            RenameColumn(table: "dbo.Person", name: "Province_province_id", newName: "ProvinceID");
            AlterColumn("dbo.Person", "ProvinceID", c => c.Int());
            AlterColumn("dbo.Person", "CityMunicipalityID", c => c.Int());
            AlterColumn("dbo.Person", "BarangayID", c => c.Int());
            CreateIndex("dbo.Person", "ProvinceID");
            CreateIndex("dbo.Person", "CityMunicipalityID");
            CreateIndex("dbo.Person", "BarangayID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Person", new[] { "BarangayID" });
            DropIndex("dbo.Person", new[] { "CityMunicipalityID" });
            DropIndex("dbo.Person", new[] { "ProvinceID" });
            AlterColumn("dbo.Person", "BarangayID", c => c.Int(nullable: false));
            AlterColumn("dbo.Person", "CityMunicipalityID", c => c.Int(nullable: false));
            AlterColumn("dbo.Person", "ProvinceID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Person", name: "ProvinceID", newName: "Province_province_id");
            RenameColumn(table: "dbo.Person", name: "CityMunicipalityID", newName: "CityMunicipality_city_municipality_id");
            RenameColumn(table: "dbo.Person", name: "BarangayID", newName: "Barangay_barangay_id");
            AddColumn("dbo.Person", "ProvinceID", c => c.Int(nullable: false));
            AddColumn("dbo.Person", "CityMunicipalityID", c => c.Int(nullable: false));
            AddColumn("dbo.Person", "BarangayID", c => c.Int(nullable: false));
            CreateIndex("dbo.Person", "Province_province_id");
            CreateIndex("dbo.Person", "CityMunicipality_city_municipality_id");
            CreateIndex("dbo.Person", "Barangay_barangay_id");
        }
    }
}
