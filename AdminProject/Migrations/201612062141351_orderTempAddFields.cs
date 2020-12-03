namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderTempAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderTemp", "KdvAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderTemp", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderTemp", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderTemp", "IsCampaignApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderTemp", "IpAddress", c => c.String());
            AddColumn("dbo.OrderTemp", "CreateUserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderTemp", "CreateUserId");
            DropColumn("dbo.OrderTemp", "IpAddress");
            DropColumn("dbo.OrderTemp", "IsCampaignApplied");
            DropColumn("dbo.OrderTemp", "DiscountAmount");
            DropColumn("dbo.OrderTemp", "TotalAmount");
            DropColumn("dbo.OrderTemp", "KdvAmount");
        }
    }
}
