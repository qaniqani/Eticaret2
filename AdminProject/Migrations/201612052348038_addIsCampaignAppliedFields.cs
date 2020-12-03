namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsCampaignAppliedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProductAssg", "IsCampaignApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "KdvAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "IsCampaignApplied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "IsCampaignApplied");
            DropColumn("dbo.Order", "DiscountAmount");
            DropColumn("dbo.Order", "TotalAmount");
            DropColumn("dbo.Order", "KdvAmount");
            DropColumn("dbo.OrderProductAssg", "IsCampaignApplied");
        }
    }
}
