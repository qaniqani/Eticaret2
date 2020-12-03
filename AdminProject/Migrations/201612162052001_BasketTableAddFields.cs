namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasketTableAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Basket", "ProductName", c => c.String());
            AddColumn("dbo.Basket", "ProductUrl", c => c.String());
            AddColumn("dbo.Basket", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Basket", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Basket", "IpAddress");
            DropColumn("dbo.Basket", "Price");
            DropColumn("dbo.Basket", "ProductUrl");
            DropColumn("dbo.Basket", "ProductName");
        }
    }
}
