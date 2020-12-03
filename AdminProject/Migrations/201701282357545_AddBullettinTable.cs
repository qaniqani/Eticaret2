namespace AdminProject.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddBullettinTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bulletin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bulletin");
        }
    }
}
