namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreProfileFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Address", c => c.String());
            AddColumn("dbo.UserProfiles", "Postcode", c => c.String());
            AddColumn("dbo.UserProfiles", "Country", c => c.String());
            AddColumn("dbo.UserProfiles", "Birthday", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Birthday");
            DropColumn("dbo.UserProfiles", "Country");
            DropColumn("dbo.UserProfiles", "Postcode");
            DropColumn("dbo.UserProfiles", "Address");
        }
    }
}
