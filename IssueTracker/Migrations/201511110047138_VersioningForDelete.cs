namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class VersioningForDelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Issue", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Active");
            DropColumn("dbo.Issue", "Active");
            DropColumn("dbo.Comment", "Active");
        }
    }
}
