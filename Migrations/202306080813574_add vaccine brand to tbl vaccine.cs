namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvaccinebrandtotblvaccine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaccine", "VaccineBrand", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vaccine", "VaccineBrand");
        }
    }
}
