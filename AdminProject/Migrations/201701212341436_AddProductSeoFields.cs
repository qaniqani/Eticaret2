namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductSeoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "SeoKeyword", c => c.String());
            AddColumn("dbo.Product", "SeoDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "SeoDescription");
            DropColumn("dbo.Product", "SeoKeyword");
        }
    }
}
