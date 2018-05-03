namespace SmartFaceAuthAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idontknow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        GroupId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PersonId = c.Guid(nullable: false),
                        PersonImage_Id = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageDatas", t => t.PersonImage_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.PersonImage_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.People", "PersonImage_Id", "dbo.ImageDatas");
            DropIndex("dbo.People", new[] { "Group_Id" });
            DropIndex("dbo.People", new[] { "PersonImage_Id" });
            DropTable("dbo.People");
            DropTable("dbo.Groups");
        }
    }
}
