namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MTS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequests", "requestDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceRequests", "requestDate");
        }
    }
}
