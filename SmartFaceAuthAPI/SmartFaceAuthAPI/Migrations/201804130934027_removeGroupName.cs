namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeGroupName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Groups", "GroupName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "GroupName", c => c.String());
        }
    }
}
