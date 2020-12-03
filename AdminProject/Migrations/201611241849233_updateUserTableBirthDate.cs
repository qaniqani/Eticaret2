namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserTableBirthDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "BirthDate", c => c.Boolean(nullable: false));
        }
    }
}
