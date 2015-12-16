namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class StatesReordering : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.State", "OrderIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.State", "OrderIndex");
        }
    }
}
