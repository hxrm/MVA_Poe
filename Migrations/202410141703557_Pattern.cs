namespace MVA_Poe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pattern : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryFrequencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Frequency = c.Int(nullable: false),
                        PatternFrequencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PatternFrequencies", t => t.PatternFrequencyId, cascadeDelete: true)
                .Index(t => t.PatternFrequencyId);
            
            CreateTable(
                "dbo.PatternFrequencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.DateFrequencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Frequency = c.Int(nullable: false),
                        PatternFrequencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PatternFrequencies", t => t.PatternFrequencyId, cascadeDelete: true)
                .Index(t => t.PatternFrequencyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatternFrequencies", "userId", "dbo.Users");
            DropForeignKey("dbo.DateFrequencies", "PatternFrequencyId", "dbo.PatternFrequencies");
            DropForeignKey("dbo.CategoryFrequencies", "PatternFrequencyId", "dbo.PatternFrequencies");
            DropIndex("dbo.DateFrequencies", new[] { "PatternFrequencyId" });
            DropIndex("dbo.PatternFrequencies", new[] { "userId" });
            DropIndex("dbo.CategoryFrequencies", new[] { "PatternFrequencyId" });
            DropTable("dbo.DateFrequencies");
            DropTable("dbo.PatternFrequencies");
            DropTable("dbo.CategoryFrequencies");
        }
    }
}
