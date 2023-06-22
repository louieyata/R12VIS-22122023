namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vaccination", "ProvinceID", "dbo.Province");
            DropIndex("dbo.Vaccination", new[] { "ProvinceID" });
            AddColumn("dbo.Vaccination", "Province_province_id", c => c.Int());
            AlterColumn("dbo.Person", "BirthDate", c => c.DateTime());
            CreateIndex("dbo.Vaccination", "Province_province_id");
            AddForeignKey("dbo.Vaccination", "Province_province_id", "dbo.Province", "province_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaccination", "Province_province_id", "dbo.Province");
            DropIndex("dbo.Vaccination", new[] { "Province_province_id" });
            AlterColumn("dbo.Person", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Vaccination", "Province_province_id");
            CreateIndex("dbo.Vaccination", "ProvinceID");
            AddForeignKey("dbo.Vaccination", "ProvinceID", "dbo.Province", "province_id", cascadeDelete: true);
        }
    }
}
