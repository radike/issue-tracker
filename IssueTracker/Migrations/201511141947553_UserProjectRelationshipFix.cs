namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserProjectRelationshipFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" }, "dbo.Project");
            DropForeignKey("dbo.Project", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" });
            DropIndex("dbo.Project", new[] { "OwnerId" });
            DropIndex("dbo.Project", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.ProjectApplicationUser",
                c => new
                    {
                        Project_Id = c.Guid(nullable: false),
                        Project_CreatedAt = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Project_CreatedAt, t.ApplicationUser_Id })
                .ForeignKey("dbo.Project", t => new { t.Project_Id, t.Project_CreatedAt }, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => new { t.Project_Id, t.Project_CreatedAt })
                .Index(t => t.ApplicationUser_Id);
            
            AlterColumn("dbo.Project", "OwnerId", c => c.String());
            DropColumn("dbo.AspNetUsers", "Project_Id");
            DropColumn("dbo.AspNetUsers", "Project_CreatedAt");
            DropColumn("dbo.Project", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Project_CreatedAt", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Guid());
            DropForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectApplicationUser", new[] { "Project_Id", "Project_CreatedAt" }, "dbo.Project");
            DropIndex("dbo.ProjectApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectApplicationUser", new[] { "Project_Id", "Project_CreatedAt" });
            AlterColumn("dbo.Project", "OwnerId", c => c.String(maxLength: 128));
            DropTable("dbo.ProjectApplicationUser");
            CreateIndex("dbo.Project", "ApplicationUser_Id");
            CreateIndex("dbo.Project", "OwnerId");
            CreateIndex("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" });
            AddForeignKey("dbo.Project", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", new[] { "Project_Id", "Project_CreatedAt" }, "dbo.Project", new[] { "Id", "CreatedAt" });
            AddForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers", "Id");
        }
    }
}
