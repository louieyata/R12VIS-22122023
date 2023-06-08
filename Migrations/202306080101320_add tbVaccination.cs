namespace R12VIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtbVaccination : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EthnicGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndigenousMember = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UniquePersonID = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        ContactNumber = c.String(),
                        GuardianName = c.String(),
                        isMale = c.Boolean(nullable: false),
                        isPWD = c.Boolean(nullable: false),
                        EthnicGroupID = c.Int(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EthnicGroup", t => t.EthnicGroupID)
                .Index(t => t.EthnicGroupID);
            
            CreateTable(
                "dbo.Vaccine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VaccineManufacturer = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "EthnicGroupID", "dbo.EthnicGroup");
            DropIndex("dbo.Person", new[] { "EthnicGroupID" });
            DropTable("dbo.Vaccine");
            DropTable("dbo.Person");
            DropTable("dbo.EthnicGroup");
        }
    }
}
