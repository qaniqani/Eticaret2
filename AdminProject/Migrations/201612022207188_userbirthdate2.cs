namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userbirthdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "BirthDate2", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "BirthDate2");
        }
    }
}
