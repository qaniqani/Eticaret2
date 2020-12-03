namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryTableAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "SingleHit", c => c.Int(nullable: false));
            AddColumn("dbo.Country", "TwoLetterCode", c => c.String());
            AddColumn("dbo.Country", "ThreeLetterCode", c => c.String());
            AddColumn("dbo.Country", "PhoneCode", c => c.String());
            AddColumn("dbo.Product", "SingleHit", c => c.Int(nullable: false));
            AddColumn("dbo.Product", "PluralHit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "PluralHit");
            DropColumn("dbo.Product", "SingleHit");
            DropColumn("dbo.Country", "PhoneCode");
            DropColumn("dbo.Country", "ThreeLetterCode");
            DropColumn("dbo.Country", "TwoLetterCode");
            DropColumn("dbo.Category", "SingleHit");
        }
    }
}
