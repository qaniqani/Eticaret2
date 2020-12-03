namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileTableAddCategoryId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "Picture", c => c.String());
            AddColumn("dbo.File", "CategoryId", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Language", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Language", "UrlTag", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Language", "UrlTag", c => c.String());
            AlterColumn("dbo.Language", "Name", c => c.String());
            DropColumn("dbo.File", "CategoryId");
            DropColumn("dbo.Category", "Picture");
        }
    }
}
