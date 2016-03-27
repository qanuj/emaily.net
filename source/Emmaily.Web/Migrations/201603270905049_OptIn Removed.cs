namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptInRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Lists", "IsOptIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lists", "IsOptIn", c => c.Boolean(nullable: false));
        }
    }
}
