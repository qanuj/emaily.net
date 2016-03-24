namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Templateshavemorefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "HtmlText", c => c.String());
            AddColumn("dbo.Templates", "PlainText", c => c.String());
            AddColumn("dbo.Templates", "QueryString", c => c.String());
            DropColumn("dbo.Templates", "Html");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Templates", "Html", c => c.String());
            DropColumn("dbo.Templates", "QueryString");
            DropColumn("dbo.Templates", "PlainText");
            DropColumn("dbo.Templates", "HtmlText");
        }
    }
}
