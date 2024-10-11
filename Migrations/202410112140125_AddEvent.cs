namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        EventName = c.String(nullable: false, maxLength: 100),
                        EventDesc = c.String(maxLength: 500),
                        EventDate = c.DateTime(nullable: false),
                        EventLoc = c.String(maxLength: 200),
                        EventCat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Events");
        }
    }
}
