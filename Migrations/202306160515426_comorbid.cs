namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comorbid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaccination", "Comorbidity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vaccination", "Comorbidity");
        }
    }
}
