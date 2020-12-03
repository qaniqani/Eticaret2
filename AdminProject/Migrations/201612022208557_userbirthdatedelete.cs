namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userbirthdatedelete : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
