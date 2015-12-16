namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectOwner : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectApplicationUser", "Project_Id", "dbo.Project");
            DropForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectApplicationUser", new[] { "Project_Id" });
            DropIndex("dbo.ProjectApplicationUser", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Guid());
            AddColumn("dbo.Project", "OwnerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Project", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            CreateIndex("dbo.Project", "OwnerId");
            CreateIndex("dbo.Project", "ApplicationUser_Id");
            AddForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project", "Id");
            AddForeignKey("dbo.Project", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ProjectApplicationUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectApplicationUser",
                c => new
                    {
                        Project_Id = c.Guid(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.Project", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project");
            DropForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Project", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Project", new[] { "OwnerId" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            DropColumn("dbo.Project", "ApplicationUser_Id");
            DropColumn("dbo.Project", "OwnerId");
            DropColumn("dbo.AspNetUsers", "Project_Id");
            CreateIndex("dbo.ProjectApplicationUser", "ApplicationUser_Id");
            CreateIndex("dbo.ProjectApplicationUser", "Project_Id");
            AddForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectApplicationUser", "Project_Id", "dbo.Project", "Id", cascadeDelete: true);
        }
    }
}
