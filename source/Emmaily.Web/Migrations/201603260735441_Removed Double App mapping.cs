namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDoubleAppmapping : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Templates", "App_Id", "dbo.Apps");
            DropIndex("dbo.Templates", new[] { "App_Id" });
            DropColumn("dbo.Templates", "App_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Templates", "App_Id", c => c.Int());
            CreateIndex("dbo.Templates", "App_Id");
            AddForeignKey("dbo.Templates", "App_Id", "dbo.Apps", "Id");
        }
    }
}
