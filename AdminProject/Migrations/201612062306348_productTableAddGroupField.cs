namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productTableAddGroupField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "GroupType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "GroupType");
        }
    }
}
