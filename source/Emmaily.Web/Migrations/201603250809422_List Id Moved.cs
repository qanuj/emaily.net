namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListIdMoved : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriberReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        ListId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lists", t => t.ListId, cascadeDelete: true)
                .Index(t => t.ListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriberReports", "ListId", "dbo.Lists");
            DropIndex("dbo.SubscriberReports", new[] { "ListId" });
            DropTable("dbo.SubscriberReports");
        }
    }
}
