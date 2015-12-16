namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserPKToGuid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Issue", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issue", "AssigneeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issue", "ReporterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comment", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Comment", new[] { "User_Id" });
            DropIndex("dbo.Issue", new[] { "ReporterId" });
            DropIndex("dbo.Issue", new[] { "AssigneeId" });
            DropIndex("dbo.Issue", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectApplicationUser", new[] { "ApplicationUser_id" });
            DropColumn("dbo.Comment", "AuthorId");
            RenameColumn(table: "dbo.Comment", name: "User_Id", newName: "AuthorId");
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropPrimaryKey("dbo.AspNetUserRoles");
            DropPrimaryKey("dbo.AspNetRoles");
            DropPrimaryKey("dbo.ProjectApplicationUser");
            AddColumn("dbo.Issue", "EntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Comment", "AuthorId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Comment", "AuthorId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Issue", "ReporterId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Issue", "AssigneeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserClaims", "ApplicationUser_Id", c => c.Guid());
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserLogins", "ApplicationUser_Id", c => c.Guid());
            AlterColumn("dbo.Project", "OwnerId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserRoles", "ApplicationUser_Id", c => c.Guid());
            AlterColumn("dbo.AspNetRoles", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ProjectApplicationUser", "ApplicationUser_id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("dbo.AspNetRoles", "Id");
            AddPrimaryKey("dbo.ProjectApplicationUser", new[] { "Project_Id", "Project_CreatedAt", "ApplicationUser_id" });
            CreateIndex("dbo.Comment", "AuthorId");
            CreateIndex("dbo.AspNetUserClaims", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUserLogins", "ApplicationUser_Id");
            CreateIndex("dbo.Project", "OwnerId");
            CreateIndex("dbo.Issue", "ReporterId");
            CreateIndex("dbo.Issue", "AssigneeId");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserRoles", "ApplicationUser_Id");
            CreateIndex("dbo.ProjectApplicationUser", "ApplicationUser_id");
            AddForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issue", "AssigneeId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issue", "ReporterId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comment", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            DropColumn("dbo.Issue", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Issue", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Comment", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issue", "ReporterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issue", "AssigneeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Project", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectApplicationUser", new[] { "ApplicationUser_id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.Issue", new[] { "AssigneeId" });
            DropIndex("dbo.Issue", new[] { "ReporterId" });
            DropIndex("dbo.Project", new[] { "OwnerId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Comment", new[] { "AuthorId" });
            DropPrimaryKey("dbo.ProjectApplicationUser");
            DropPrimaryKey("dbo.AspNetRoles");
            DropPrimaryKey("dbo.AspNetUserRoles");
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropPrimaryKey("dbo.AspNetUsers");
            AlterColumn("dbo.ProjectApplicationUser", "ApplicationUser_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetRoles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Project", "OwnerId", c => c.String());
            AlterColumn("dbo.AspNetUserLogins", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserClaims", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Issue", "AssigneeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Issue", "ReporterId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comment", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comment", "AuthorId", c => c.String());
            DropColumn("dbo.Issue", "EntityId");
            AddPrimaryKey("dbo.ProjectApplicationUser", new[] { "Project_Id", "Project_CreatedAt", "ApplicationUser_id" });
            AddPrimaryKey("dbo.AspNetRoles", "Id");
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            RenameColumn(table: "dbo.Comment", name: "AuthorId", newName: "User_Id");
            AddColumn("dbo.Comment", "AuthorId", c => c.String());
            CreateIndex("dbo.ProjectApplicationUser", "ApplicationUser_id");
            CreateIndex("dbo.AspNetUserRoles", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserLogins", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUserClaims", "ApplicationUser_Id");
            CreateIndex("dbo.Issue", "ApplicationUser_Id");
            CreateIndex("dbo.Issue", "AssigneeId");
            CreateIndex("dbo.Issue", "ReporterId");
            CreateIndex("dbo.Comment", "User_Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.Comment", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issue", "ReporterId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issue", "AssigneeId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProjectApplicationUser", "ApplicationUser_id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issue", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
