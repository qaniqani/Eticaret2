namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productTableAddBrandId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "BrandId", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "BrandId");
        }
    }
}
