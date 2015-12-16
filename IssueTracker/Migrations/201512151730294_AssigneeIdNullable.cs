namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AssigneeIdNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Issue", new[] { "AssigneeId" });
            AlterColumn("dbo.Issue", "AssigneeId", c => c.Guid());
            CreateIndex("dbo.Issue", "AssigneeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Issue", new[] { "AssigneeId" });
            AlterColumn("dbo.Issue", "AssigneeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Issue", "AssigneeId");
        }
    }
}
