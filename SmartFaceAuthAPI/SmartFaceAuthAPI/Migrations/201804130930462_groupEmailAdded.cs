namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupEmailAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Email");
        }
    }
}
