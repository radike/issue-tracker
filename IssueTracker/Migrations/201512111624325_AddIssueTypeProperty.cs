namespace IssueTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIssueTypeProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issue", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issue", "Type");
        }
    }
}
