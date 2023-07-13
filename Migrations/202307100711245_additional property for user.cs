namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionalpropertyforuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaccination", "DateCreate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Vaccination", "CreatedBy", c => c.String());
            AddColumn("dbo.Vaccination", "UserID", c => c.Int());
            CreateIndex("dbo.Vaccination", "UserID");
            AddForeignKey("dbo.Vaccination", "UserID", "dbo.User", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vaccination", "UserID", "dbo.User");
            DropIndex("dbo.Vaccination", new[] { "UserID" });
            DropColumn("dbo.Vaccination", "UserID");
            DropColumn("dbo.Vaccination", "CreatedBy");
            DropColumn("dbo.Vaccination", "DateCreate");
        }
    }
}
