namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolePermissionsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Write", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Read", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Read");
            DropColumn("dbo.AspNetRoles", "Write");
        }
    }
}
