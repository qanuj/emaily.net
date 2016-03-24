namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserApps", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserApps", new[] { "UserId" });
            DropIndex("dbo.UserApps", new[] { "AppId" });
            DropPrimaryKey("dbo.UserApps");
            AddColumn("dbo.UserApps", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.UserApps", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserApps", "Modified", c => c.DateTime());
            AddColumn("dbo.UserApps", "Deleted", c => c.DateTime());
            AlterColumn("dbo.UserApps", "UserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.UserApps", "Id");
            CreateIndex("dbo.UserApps", new[] { "UserId", "AppId" }, unique: true, name: "UserApp");
            AddForeignKey("dbo.UserApps", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserApps", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserApps", "UserApp");
            DropPrimaryKey("dbo.UserApps");
            AlterColumn("dbo.UserApps", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.UserApps", "Deleted");
            DropColumn("dbo.UserApps", "Modified");
            DropColumn("dbo.UserApps", "Created");
            DropColumn("dbo.UserApps", "Id");
            AddPrimaryKey("dbo.UserApps", new[] { "UserId", "AppId" });
            CreateIndex("dbo.UserApps", "AppId");
            CreateIndex("dbo.UserApps", "UserId");
            AddForeignKey("dbo.UserApps", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
