namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTempTableAddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderTemp", "ChangeDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderTemp", "ChangeDescription");
        }
    }
}
