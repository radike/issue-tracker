namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectsInUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project");
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            CreateTable(
                "dbo.ProjectApplicationUser",
                c => new
                    {
                        Project_Id = c.Guid(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Project", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropColumn("dbo.AspNetUsers", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Guid());
            DropForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectApplicationUser", "Project_Id", "dbo.Project");
            DropIndex("dbo.ProjectApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectApplicationUser", new[] { "Project_Id" });
            DropTable("dbo.ProjectApplicationUser");
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Project", "Id");
        }
    }
}
