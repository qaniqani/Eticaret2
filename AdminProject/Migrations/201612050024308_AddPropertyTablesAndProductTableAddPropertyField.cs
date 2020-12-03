namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyTablesAndProductTableAddPropertyField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false, defaultValue: 9999),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PropertyItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyId = c.Int(nullable: false),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false, defaultValue: 9999),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Product", "Properties", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Properties");
            DropTable("dbo.PropertyItem");
            DropTable("dbo.Property");
        }
    }
}
