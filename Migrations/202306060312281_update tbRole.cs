namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetbRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Role", "Access", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Role", "Access");
        }
    }
}
