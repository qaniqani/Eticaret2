namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFieldTypeOrderNr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "OrderNr", c => c.String());
            AlterColumn("dbo.OrderTemp", "OrderNr", c => c.String());
            AlterColumn("dbo.User", "BirthDate2", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "BirthDate2", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderTemp", "OrderNr", c => c.Int(nullable: false));
            AlterColumn("dbo.Order", "OrderNr", c => c.Int(nullable: false));
        }
    }
}
