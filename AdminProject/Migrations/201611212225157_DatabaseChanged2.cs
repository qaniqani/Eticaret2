namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseChanged2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProductAssg", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "OtherDetail", c => c.String());
            AddColumn("dbo.Order", "UpdateUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Product", "Url", c => c.String());
            AddColumn("dbo.User", "LastLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UpdateDate");
            DropColumn("dbo.User", "LastLoginDate");
            DropColumn("dbo.Product", "Url");
            DropColumn("dbo.Order", "UpdateUserId");
            DropColumn("dbo.Order", "OtherDetail");
            DropColumn("dbo.OrderProductAssg", "OrderId");
            DropColumn("dbo.Invoice", "Status");
            DropColumn("dbo.Comment", "ProductId");
        }
    }
}
