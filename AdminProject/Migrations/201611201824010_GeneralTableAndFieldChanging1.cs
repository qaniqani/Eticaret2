namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneralTableAndFieldChanging1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignType = c.Int(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Detail = c.String(),
                        DiscountOdd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmountCriterion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Basket", "CargoId", c => c.Int(nullable: false));
            AddColumn("dbo.Cargo", "DefaultCargo", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Product", "DiscountOdd", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Product", "KdvOdd", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Product", "IsKdv", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Product", "StockNr", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Product", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Product", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "UpdateDate", c => c.String());
            AlterColumn("dbo.Product", "CreateDate", c => c.String());
            AlterColumn("dbo.Product", "StockNr", c => c.String());
            AlterColumn("dbo.Product", "IsKdv", c => c.String());
            AlterColumn("dbo.Product", "KdvOdd", c => c.String());
            AlterColumn("dbo.Product", "DiscountOdd", c => c.String());
            DropColumn("dbo.Cargo", "DefaultCargo");
            DropColumn("dbo.Basket", "CargoId");
            DropTable("dbo.Campaign");
        }
    }
}
