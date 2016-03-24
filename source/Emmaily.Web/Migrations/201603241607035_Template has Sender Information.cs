namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplatehasSenderInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "Sender_Name", c => c.String());
            AddColumn("dbo.Templates", "Sender_Email", c => c.String());
            AddColumn("dbo.Templates", "Sender_ReplyTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "Sender_ReplyTo");
            DropColumn("dbo.Templates", "Sender_Email");
            DropColumn("dbo.Templates", "Sender_Name");
        }
    }
}
