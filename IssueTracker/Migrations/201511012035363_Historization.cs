namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Historization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "AuthorId", c => c.String());
            AddColumn("dbo.Comment", "DeletedAt", c => c.DateTime(nullable: true));
            AddColumn("dbo.Comment", "CodeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Issue", "DeletedAt", c => c.DateTime(nullable: true));
            AddColumn("dbo.Issue", "CodeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "Code", c => c.String(nullable: false));
            AddColumn("dbo.Project", "DeletedAt", c => c.DateTime(nullable: true));
            AddColumn("dbo.Project", "CodeNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "CodeNumber");
            DropColumn("dbo.Project", "DeletedAt");
            DropColumn("dbo.Project", "Code");
            DropColumn("dbo.Issue", "CodeNumber");
            DropColumn("dbo.Issue", "DeletedAt");
            DropColumn("dbo.Comment", "CodeNumber");
            DropColumn("dbo.Comment", "DeletedAt");
            DropColumn("dbo.Comment", "AuthorId");
        }
    }
}
