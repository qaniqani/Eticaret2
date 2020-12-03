namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasketTableAddSessionIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Basket", "SessionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Basket", "SessionId");
        }
    }
}
