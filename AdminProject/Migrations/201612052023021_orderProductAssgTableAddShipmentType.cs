namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderProductAssgTableAddShipmentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProductAssg", "CauseOfRefund", c => c.String());
            AddColumn("dbo.OrderProductAssg", "ShipmentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProductAssg", "ShipmentType");
            DropColumn("dbo.OrderProductAssg", "CauseOfRefund");
        }
    }
}
