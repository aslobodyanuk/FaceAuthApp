namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(),
                        Message = c.String(),
                        AuthImage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageDatas", t => t.AuthImage_Id)
                .Index(t => t.AuthImage_Id);
            
            CreateTable(
                "dbo.ImageDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "AuthImage_Id", "dbo.ImageDatas");
            DropIndex("dbo.Logs", new[] { "AuthImage_Id" });
            DropTable("dbo.ImageDatas");
            DropTable("dbo.Logs");
        }
    }
}
