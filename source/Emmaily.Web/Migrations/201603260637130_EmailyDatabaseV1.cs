namespace Emaily.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailyDatabaseV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Write = c.Int(),
                        Read = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProfileId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.ProfileId)
                .Index(t => t.ProfileId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserApps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        AppId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apps", t => t.AppId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => new { t.UserId, t.AppId }, unique: true, name: "UserApp");
            
            CreateTable(
                "dbo.Apps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Currency = c.Int(nullable: false),
                        Smtp_IsActive = c.Boolean(nullable: false),
                        Smtp_Host = c.String(),
                        Smtp_Port = c.String(),
                        Smtp_Ssl = c.String(),
                        Smtp_Username = c.String(),
                        Smtp_Password = c.String(),
                        Sender_Name = c.String(),
                        Sender_Email = c.String(),
                        Sender_ReplyTo = c.String(),
                        IsBounceSetup = c.Boolean(nullable: false),
                        IsComplaintSetup = c.Boolean(nullable: false),
                        AppKey = c.String(),
                        TestEmail = c.String(),
                        Logo = c.String(),
                        Quota = c.Int(nullable: false),
                        Used = c.Int(nullable: false),
                        PlanId = c.Int(nullable: false),
                        PromoId = c.Int(),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.Promoes", t => t.PromoId)
                .Index(t => t.PlanId)
                .Index(t => t.PromoId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppId = c.Int(nullable: false),
                        HtmlText = c.String(),
                        PlainText = c.String(),
                        QueryString = c.String(),
                        OwnerId = c.String(),
                        Sender_Name = c.String(),
                        Sender_Email = c.String(),
                        Sender_ReplyTo = c.String(),
                        Price = c.Int(nullable: false),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                        Label = c.String(),
                        Status = c.Int(),
                        IsHtml = c.Boolean(),
                        Recipients = c.Int(),
                        Sents = c.Int(),
                        Clicks = c.Int(),
                        Opens = c.Int(),
                        Errors = c.String(),
                        Started = c.DateTime(),
                        Future = c.DateTime(),
                        TimeZone = c.String(),
                        AutoResponderId = c.Int(),
                        TimeCondition = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        App_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apps", t => t.AppId, cascadeDelete: true)
                .ForeignKey("dbo.AutoResponders", t => t.AutoResponderId, cascadeDelete: true)
                .ForeignKey("dbo.Apps", t => t.App_Id)
                .Index(t => t.AppId)
                .Index(t => t.AutoResponderId)
                .Index(t => t.App_Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Url = c.String(),
                        TemplateId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(),
                        Url = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Clicks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriberId = c.Int(nullable: false),
                        LinkId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: true)
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId, cascadeDelete: true)
                .Index(t => t.SubscriberId)
                .Index(t => t.LinkId);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.String(),
                        Email = c.String(),
                        Token = c.String(),
                        ListId = c.Int(nullable: false),
                        AppId = c.Int(nullable: false),
                        IsUnsubscribed = c.Boolean(nullable: false),
                        IsBounced = c.Boolean(nullable: false),
                        IsSoftBounce = c.Boolean(nullable: false),
                        IsComplaint = c.Boolean(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        LastCampaignId = c.Int(),
                        MessageID = c.String(),
                        LastAutoResponderId = c.Int(),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apps", t => t.AppId, cascadeDelete: true)
                .ForeignKey("dbo.Lists", t => t.ListId, cascadeDelete: true)
                .ForeignKey("dbo.AutoResponders", t => t.LastAutoResponderId)
                .ForeignKey("dbo.Templates", t => t.LastCampaignId)
                .Index(t => t.ListId)
                .Index(t => t.AppId)
                .Index(t => t.LastCampaignId)
                .Index(t => t.LastAutoResponderId);
            
            CreateTable(
                "dbo.AutoResponders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mode = c.Int(nullable: false),
                        ListId = c.Int(nullable: false),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lists", t => t.ListId, cascadeDelete: true)
                .Index(t => t.ListId);
            
            CreateTable(
                "dbo.Queues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(),
                        SubscriberId = c.Int(),
                        QueryString = c.String(),
                        IsSent = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.CampaignId)
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId)
                .Index(t => t.CampaignId)
                .Index(t => t.SubscriberId);
            
            CreateTable(
                "dbo.CampaignResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriberId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Country = c.String(),
                        UserAgent = c.String(),
                        Opened = c.DateTime(),
                        Bounced = c.DateTime(),
                        MarkedSpam = c.DateTime(),
                        IsSoftBounce = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId)
                .Index(t => t.SubscriberId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        AppId = c.Int(nullable: false),
                        OwnerId = c.String(),
                        IsOptIn = c.Boolean(nullable: false),
                        IsUnsubcribeAllList = c.Boolean(nullable: false),
                        PreviousCount = c.Int(nullable: false),
                        IsProcessing = c.Boolean(nullable: false),
                        TotalRecord = c.Int(nullable: false),
                        ConfirmUrl = c.String(),
                        SubscribedUrl = c.String(),
                        UnsubscribedUrl = c.String(),
                        ThankYou_IsActive = c.Boolean(nullable: false),
                        ThankYou_Subject = c.String(),
                        ThankYou_Message = c.String(),
                        GoodBye_IsActive = c.Boolean(nullable: false),
                        GoodBye_Subject = c.String(),
                        GoodBye_Message = c.String(),
                        Confirmation_IsActive = c.Boolean(nullable: false),
                        Confirmation_Subject = c.String(),
                        Confirmation_Message = c.String(),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apps", t => t.AppId)
                .Index(t => t.AppId);
            
            CreateTable(
                "dbo.CampaignLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ListId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.Lists", t => t.ListId)
                .Index(t => t.ListId)
                .Index(t => t.CampaignId);
            
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
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApiKey = c.String(),
                        OwnerId = c.String(),
                        IsOwner = c.Boolean(nullable: false),
                        AppId = c.Int(nullable: false),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apps", t => t.AppId, cascadeDelete: true)
                .Index(t => t.AppId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Icon = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        IsPayGo = c.Boolean(nullable: false),
                        DeliveryFees = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                        Quota = c.Int(nullable: false),
                        AnnualPrice = c.Int(),
                        DiscountedPrice = c.Int(),
                        PromoId = c.Int(),
                        Currency = c.Int(nullable: false),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promoes", t => t.PromoId)
                .Index(t => t.PromoId);
            
            CreateTable(
                "dbo.Promoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        Quota = c.Int(),
                        Discount = c.Int(),
                        IsDiscountPercentage = c.Boolean(nullable: false),
                        DiscountApplicationMonths = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Picture = c.String(),
                        Address = c.String(),
                        Postcode = c.String(),
                        Country = c.String(),
                        Birthday = c.DateTime(),
                        Name = c.String(),
                        Custom = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Deleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserApps", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserApps", "AppId", "dbo.Apps");
            DropForeignKey("dbo.Apps", "PromoId", "dbo.Promoes");
            DropForeignKey("dbo.Plans", "PromoId", "dbo.Promoes");
            DropForeignKey("dbo.Apps", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Clients", "AppId", "dbo.Apps");
            DropForeignKey("dbo.Templates", "App_Id", "dbo.Apps");
            DropForeignKey("dbo.Subscribers", "LastCampaignId", "dbo.Templates");
            DropForeignKey("dbo.Subscribers", "LastAutoResponderId", "dbo.AutoResponders");
            DropForeignKey("dbo.Subscribers", "ListId", "dbo.Lists");
            DropForeignKey("dbo.SubscriberReports", "ListId", "dbo.Lists");
            DropForeignKey("dbo.CampaignLists", "ListId", "dbo.Lists");
            DropForeignKey("dbo.CampaignLists", "CampaignId", "dbo.Templates");
            DropForeignKey("dbo.AutoResponders", "ListId", "dbo.Lists");
            DropForeignKey("dbo.Lists", "AppId", "dbo.Apps");
            DropForeignKey("dbo.CampaignResults", "SubscriberId", "dbo.Subscribers");
            DropForeignKey("dbo.CampaignResults", "CampaignId", "dbo.Templates");
            DropForeignKey("dbo.Queues", "SubscriberId", "dbo.Subscribers");
            DropForeignKey("dbo.Queues", "CampaignId", "dbo.Templates");
            DropForeignKey("dbo.Templates", "AutoResponderId", "dbo.AutoResponders");
            DropForeignKey("dbo.Clicks", "SubscriberId", "dbo.Subscribers");
            DropForeignKey("dbo.Subscribers", "AppId", "dbo.Apps");
            DropForeignKey("dbo.Clicks", "LinkId", "dbo.Links");
            DropForeignKey("dbo.Links", "CampaignId", "dbo.Templates");
            DropForeignKey("dbo.Attachments", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Templates", "AppId", "dbo.Apps");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Plans", new[] { "PromoId" });
            DropIndex("dbo.Clients", new[] { "AppId" });
            DropIndex("dbo.SubscriberReports", new[] { "ListId" });
            DropIndex("dbo.CampaignLists", new[] { "CampaignId" });
            DropIndex("dbo.CampaignLists", new[] { "ListId" });
            DropIndex("dbo.Lists", new[] { "AppId" });
            DropIndex("dbo.CampaignResults", new[] { "CampaignId" });
            DropIndex("dbo.CampaignResults", new[] { "SubscriberId" });
            DropIndex("dbo.Queues", new[] { "SubscriberId" });
            DropIndex("dbo.Queues", new[] { "CampaignId" });
            DropIndex("dbo.AutoResponders", new[] { "ListId" });
            DropIndex("dbo.Subscribers", new[] { "LastAutoResponderId" });
            DropIndex("dbo.Subscribers", new[] { "LastCampaignId" });
            DropIndex("dbo.Subscribers", new[] { "AppId" });
            DropIndex("dbo.Subscribers", new[] { "ListId" });
            DropIndex("dbo.Clicks", new[] { "LinkId" });
            DropIndex("dbo.Clicks", new[] { "SubscriberId" });
            DropIndex("dbo.Links", new[] { "CampaignId" });
            DropIndex("dbo.Attachments", new[] { "TemplateId" });
            DropIndex("dbo.Templates", new[] { "App_Id" });
            DropIndex("dbo.Templates", new[] { "AutoResponderId" });
            DropIndex("dbo.Templates", new[] { "AppId" });
            DropIndex("dbo.Apps", new[] { "PromoId" });
            DropIndex("dbo.Apps", new[] { "PlanId" });
            DropIndex("dbo.UserApps", "UserApp");
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ProfileId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Promoes");
            DropTable("dbo.Plans");
            DropTable("dbo.Clients");
            DropTable("dbo.SubscriberReports");
            DropTable("dbo.CampaignLists");
            DropTable("dbo.Lists");
            DropTable("dbo.CampaignResults");
            DropTable("dbo.Queues");
            DropTable("dbo.AutoResponders");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Clicks");
            DropTable("dbo.Links");
            DropTable("dbo.Attachments");
            DropTable("dbo.Templates");
            DropTable("dbo.Apps");
            DropTable("dbo.UserApps");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
