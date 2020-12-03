namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cityTableAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.City", "PlateNo", c => c.String());
            AddColumn("dbo.City", "PhoneCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.City", "PhoneCode");
            DropColumn("dbo.City", "PlateNo");
        }
    }
}
