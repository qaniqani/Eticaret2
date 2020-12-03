namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegionTableAddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Region", "CityName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Region", "CityName");
        }
    }
}
