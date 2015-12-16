namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddIssueResolveDateProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Issue", "ResolvedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Issue", "ResolvedAt", c => c.DateTime(nullable: false));
        }
    }
}
