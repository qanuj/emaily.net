namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promomaynotbeneeded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plans", "PromoId", "dbo.Promoes");
            DropIndex("dbo.Plans", new[] { "PromoId" });
            AlterColumn("dbo.Plans", "PromoId", c => c.Int());
            CreateIndex("dbo.Plans", "PromoId");
            AddForeignKey("dbo.Plans", "PromoId", "dbo.Promoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plans", "PromoId", "dbo.Promoes");
            DropIndex("dbo.Plans", new[] { "PromoId" });
            AlterColumn("dbo.Plans", "PromoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Plans", "PromoId");
            AddForeignKey("dbo.Plans", "PromoId", "dbo.Promoes", "Id", cascadeDelete: true);
        }
    }
}
