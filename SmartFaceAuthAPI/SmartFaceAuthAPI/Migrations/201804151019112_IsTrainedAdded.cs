namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsTrainedAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "IsTrained", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "IsTrained");
        }
    }
}
