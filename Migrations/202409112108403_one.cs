namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileSize = c.Double(nullable: false),
                        FileContent = c.Binary(),
                        reportID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reports", t => t.reportID, cascadeDelete: true)
                .Index(t => t.reportID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        reportID = c.Int(nullable: false, identity: true),
                        reportName = c.String(),
                        reportDesc = c.String(),
                        reportDate = c.DateTime(nullable: false),
                        reportLoc = c.String(),
                        reportCat = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.reportID)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        uName = c.String(),
                        pWord = c.String(),
                        fName = c.String(),
                        lName = c.String(),
                        email = c.String(),
                        ID = c.String(),
                        address = c.String(),
                        langPref = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "userId", "dbo.Users");
            DropForeignKey("dbo.Attachments", "reportID", "dbo.Reports");
            DropIndex("dbo.Reports", new[] { "userId" });
            DropIndex("dbo.Attachments", new[] { "reportID" });
            DropTable("dbo.Users");
            DropTable("dbo.Reports");
            DropTable("dbo.Attachments");
        }
    }
}
