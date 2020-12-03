namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kazimChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProductAssg", "OrderType", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "IpAddress", c => c.String());
            AddColumn("dbo.Order", "CreateUserId", c => c.Int(nullable: false));
            //AddColumn("dbo.OrderProductAssg", "DiscountOdd", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderProductAssg", "ShipmentType");
            DropColumn("dbo.Order", "ShipmentType");
            DropColumn("dbo.OrderTemp", "ShipmentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderTemp", "ShipmentType", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "ShipmentType", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProductAssg", "ShipmentType", c => c.Int(nullable: false));
            DropColumn("dbo.OrderProductAssg", "DiscountOdd");
            DropColumn("dbo.Order", "CreateUserId");
            DropColumn("dbo.Order", "IpAddress");
            DropColumn("dbo.OrderProductAssg", "OrderType");
        }
    }
}
