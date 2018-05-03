namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoguserGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "UserGuid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "UserGuid");
        }
    }
}
