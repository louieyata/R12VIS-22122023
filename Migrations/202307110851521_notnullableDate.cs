namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notnullableDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vaccination", "VaccinationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vaccination", "VaccinationDate", c => c.DateTime(nullable: false));
        }
    }
}
