namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Graph : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceRequestDependencies",
                c => new
                    {
                        ServiceRequestId = c.Int(nullable: false),
                        DependencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceRequestId, t.DependencyId })
                .ForeignKey("dbo.ServiceRequests", t => t.ServiceRequestId)
                .ForeignKey("dbo.ServiceRequests", t => t.DependencyId)
                .Index(t => t.ServiceRequestId)
                .Index(t => t.DependencyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequestDependencies", "DependencyId", "dbo.ServiceRequests");
            DropForeignKey("dbo.ServiceRequestDependencies", "ServiceRequestId", "dbo.ServiceRequests");
            DropIndex("dbo.ServiceRequestDependencies", new[] { "DependencyId" });
            DropIndex("dbo.ServiceRequestDependencies", new[] { "ServiceRequestId" });
            DropTable("dbo.ServiceRequestDependencies");
        }
    }
}
