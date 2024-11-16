namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceRequests",
                c => new
                    {
                        requestId = c.Int(nullable: false, identity: true),
                        reportId = c.Int(nullable: false),
                        requestStat = c.Int(nullable: false),
                        requestPri = c.Int(nullable: false),
                        employeeId = c.Int(nullable: false),
                        requestUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.requestId)
                .ForeignKey("dbo.Reports", t => t.reportId, cascadeDelete: true)
                .Index(t => t.reportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequests", "reportId", "dbo.Reports");
            DropIndex("dbo.ServiceRequests", new[] { "reportId" });
            DropTable("dbo.ServiceRequests");
        }
    }
}
