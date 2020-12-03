namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTableAddShipmentField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        OrderNr = c.Int(nullable: false),
                        CargoId = c.Int(nullable: false),
                        CargoNr = c.String(),
                        Description = c.String(),
                        PayType = c.Int(nullable: false),
                        OrderType = c.Int(nullable: false),
                        ShipmentType = c.Int(nullable: false),
                        OrderNote = c.String(),
                        OtherDetail = c.String(),
                        CauseOfRefund = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdateUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Order", "ShipmentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "ShipmentType");
            DropTable("dbo.OrderTemp");
        }
    }
}
