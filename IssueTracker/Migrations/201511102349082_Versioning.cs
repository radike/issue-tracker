namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Versioning : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "IssueId", "dbo.Issue");
            DropForeignKey("dbo.Issue", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project");
            DropIndex("dbo.Comment", new[] { "IssueId" });
            DropIndex("dbo.Issue", new[] { "ProjectId" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            DropPrimaryKey("dbo.Comment");
            DropPrimaryKey("dbo.Issue");
            DropPrimaryKey("dbo.Project");
            AddColumn("dbo.Comment", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comment", "IssueCreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Issue", "ProjectCreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Issue", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Project_CreatedAt", c => c.DateTime());
            AddColumn("dbo.Project", "CreatedAt", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.Comment", new[] { "Id", "CreatedAt" });
            AddPrimaryKey("dbo.Issue", new[] { "Id", "CreatedAt" });
            AddPrimaryKey("dbo.Project", new[] { "Id", "CreatedAt" });
            CreateIndex("dbo.Comment", new[] { "IssueId", "IssueCreatedAt" });
            CreateIndex("dbo.Issue", new[] { "ProjectId", "ProjectCreatedAt" });
            CreateIndex("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" });
            AddForeignKey("dbo.Comment", new[] { "IssueId", "IssueCreatedAt" }, "dbo.Issue", new[] { "Id", "CreatedAt" });
            AddForeignKey("dbo.Issue", new[] { "ProjectId", "ProjectCreatedAt" }, "dbo.Project", new[] { "Id", "CreatedAt" });
            AddForeignKey("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" }, "dbo.Project", new[] { "Id", "CreatedAt" });
            DropColumn("dbo.Comment", "DeletedAt");
            DropColumn("dbo.Comment", "CodeNumber");
            DropColumn("dbo.Issue", "DeletedAt");
            DropColumn("dbo.Project", "DeletedAt");
            DropColumn("dbo.Project", "CodeNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "CodeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Issue", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Comment", "CodeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Comment", "DeletedAt", c => c.DateTime());
            DropForeignKey("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" }, "dbo.Project");
            DropForeignKey("dbo.Issue", new[] { "ProjectId", "ProjectCreatedAt" }, "dbo.Project");
            DropForeignKey("dbo.Comment", new[] { "IssueId", "IssueCreatedAt" }, "dbo.Issue");
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" });
            DropIndex("dbo.Issue", new[] { "ProjectId", "ProjectCreatedAt" });
            DropIndex("dbo.Comment", new[] { "IssueId", "IssueCreatedAt" });
            DropPrimaryKey("dbo.Project");
            DropPrimaryKey("dbo.Issue");
            DropPrimaryKey("dbo.Comment");
            DropColumn("dbo.Project", "CreatedAt");
            DropColumn("dbo.AspNetUsers", "Project_CreatedAt");
            DropColumn("dbo.Issue", "CreatedAt");
            DropColumn("dbo.Issue", "ProjectCreatedAt");
            DropColumn("dbo.Comment", "IssueCreatedAt");
            DropColumn("dbo.Comment", "CreatedAt");
            AddPrimaryKey("dbo.Project", "Id");
            AddPrimaryKey("dbo.Issue", "Id");
            AddPrimaryKey("dbo.Comment", "Id");
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            CreateIndex("dbo.Issue", "ProjectId");
            CreateIndex("dbo.Comment", "IssueId");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project", "Id");
            AddForeignKey("dbo.Issue", "ProjectId", "dbo.Project", "Id");
            AddForeignKey("dbo.Comment", "IssueId", "dbo.Issue", "Id");
        }
    }
}
