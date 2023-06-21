namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
