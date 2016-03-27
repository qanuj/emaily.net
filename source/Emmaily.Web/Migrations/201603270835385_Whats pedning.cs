namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Whatspedning : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Lists", "ConfirmUrl");
            DropColumn("dbo.Lists", "SubscribedUrl");
            DropColumn("dbo.Lists", "UnsubscribedUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lists", "UnsubscribedUrl", c => c.String());
            AddColumn("dbo.Lists", "SubscribedUrl", c => c.String());
            AddColumn("dbo.Lists", "ConfirmUrl", c => c.String());
        }
    }
}
