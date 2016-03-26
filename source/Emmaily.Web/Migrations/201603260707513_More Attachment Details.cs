namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreAttachmentDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Name", c => c.String());
            AddColumn("dbo.Attachments", "ContentType", c => c.String());
            AddColumn("dbo.Attachments", "Size", c => c.Long(nullable: false));
            DropColumn("dbo.Attachments", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attachments", "FileName", c => c.String());
            DropColumn("dbo.Attachments", "Size");
            DropColumn("dbo.Attachments", "ContentType");
            DropColumn("dbo.Attachments", "Name");
        }
    }
}
