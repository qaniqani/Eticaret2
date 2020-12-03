namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productOrderAssgTableChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProductAssg", "KdvOdd", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProductAssg", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "IsKdv", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderProductAssg", "DiscountOdd", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProductAssg", "KdvAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "SubTotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "OrginalTotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "CargoAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderProductAssg", "OtherDetail", c => c.String());
            AddColumn("dbo.Product", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderProductAssg", "MeasureId");
            DropColumn("dbo.OrderProductAssg", "IsMeasure");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderProductAssg", "IsMeasure", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderProductAssg", "MeasureId", c => c.Int(nullable: false));
            DropColumn("dbo.Product", "PurchasePrice");
            DropColumn("dbo.OrderProductAssg", "OtherDetail");
            DropColumn("dbo.OrderProductAssg", "CargoAmount");
            DropColumn("dbo.OrderProductAssg", "OrginalTotalAmount");
            DropColumn("dbo.OrderProductAssg", "DiscountAmount");
            DropColumn("dbo.OrderProductAssg", "SubTotalAmount");
            DropColumn("dbo.OrderProductAssg", "TotalAmount");
            DropColumn("dbo.OrderProductAssg", "KdvAmount");
            DropColumn("dbo.OrderProductAssg", "DiscountOdd");
            DropColumn("dbo.OrderProductAssg", "IsKdv");
            DropColumn("dbo.OrderProductAssg", "PurchasePrice");
            DropColumn("dbo.OrderProductAssg", "Price");
            DropColumn("dbo.OrderProductAssg", "KdvOdd");
        }
    }
}
