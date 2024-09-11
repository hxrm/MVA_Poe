namespace WPFModernVerticalMenu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "fName", c => c.String());
            AddColumn("dbo.Users", "lName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "lName");
            DropColumn("dbo.Users", "fName");
        }
    }
}
