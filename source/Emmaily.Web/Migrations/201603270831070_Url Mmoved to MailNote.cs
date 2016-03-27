namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlMmovedtoMailNote : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Lists", "ConfirmUrl", "Confirmation_Url");
            RenameColumn("dbo.Lists", "SubscribedUrl", "ThankYou_Url");
            RenameColumn("dbo.Lists", "UnsubscribedUrl", "GoodBye_Url");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Lists", "Confirmation_Url", "ConfirmUrl");
            RenameColumn("dbo.Lists", "ThankYou_Url", "SubscribedUrl");
            RenameColumn("dbo.Lists", "GoodBye_Url", "UnsubscribedUrl"); 
        }
    }
}
