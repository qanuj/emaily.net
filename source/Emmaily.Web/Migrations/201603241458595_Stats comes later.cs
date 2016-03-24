namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statscomeslater : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "Recipients", c => c.Int());
            AlterColumn("dbo.Campaigns", "Sents", c => c.Int());
            AlterColumn("dbo.Campaigns", "Clicks", c => c.Int());
            AlterColumn("dbo.Campaigns", "Opens", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "Opens", c => c.Int(nullable: false));
            AlterColumn("dbo.Campaigns", "Clicks", c => c.Int(nullable: false));
            AlterColumn("dbo.Campaigns", "Sents", c => c.Int(nullable: false));
            AlterColumn("dbo.Campaigns", "Recipients", c => c.Int(nullable: false));
        }
    }
}
