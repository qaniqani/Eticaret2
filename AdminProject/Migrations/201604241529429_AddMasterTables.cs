namespace AdminProject.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddMasterTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false, defaultValue: 0),
                        LanguageId = c.Int(nullable: false, defaultValue: 0),
                        CategoryType = c.Int(nullable: false, defaultValue: 0),
                        LanguageTag = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Keyword = c.String(),
                        Title = c.String(),
                        Url = c.String(),
                        SequenceNumber = c.Int(nullable: false, defaultValue: 9999),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedUser = c.Int(nullable: false, defaultValue: 0),
                        Status = c.Int(nullable: false),
                        Hit = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Content",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false, defaultValue: 0),
                        LanguageId = c.Int(nullable: false, defaultValue: 0),
                        LanguageTag = c.String(),
                        ContentType = c.Int(nullable: false),
                        Name = c.String(),
                        Detail = c.String(),
                        Picture = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Keyword = c.String(),
                        Url = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedUser = c.Int(nullable: false),
                        SequenceNumber = c.Int(nullable: false, defaultValue: 9999),
                        Status = c.Int(nullable: false),
                        Hit = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FileUrl = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        UploadUserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Hit = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gallery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false, defaultValue: 0),
                        LanguageId = c.Int(nullable: false, defaultValue: 0),
                        LanguageTag = c.String(),
                        Name = c.String(),
                        Definition = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Keyword = c.String(),
                        Url = c.String(),
                        SequenceNumber = c.Int(nullable: false, defaultValue: 9999),
                        CreateDate = c.DateTime(nullable: false),
                        Hit = c.Int(nullable: false, defaultValue: 0),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GalleryDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GalleryId = c.Int(nullable: false, defaultValue: 0),
                        Name = c.String(),
                        Description = c.String(),
                        PictureUrl = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        SequenceNumber = c.Int(nullable: false, defaultValue: 9999),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UrlTag = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false, defaultValue: 0),
                        Title = c.String(),
                        Description = c.String(),
                        Keyword = c.String(),
                        MailAddress = c.String(),
                        MailPassword = c.String(),
                        Smtp = c.String(),
                        Port = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Slider",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false, defaultValue: 0),
                        IsVideoLink = c.Int(nullable: false),
                        PictureType = c.Int(nullable: false),
                        Name = c.String(),
                        Detail1 = c.String(),
                        Detail2 = c.String(),
                        Detail3 = c.String(),
                        VideoUrl = c.String(),
                        VideoEmbedCode = c.String(),
                        Picture1 = c.String(),
                        Picture2 = c.String(),
                        Picture3 = c.String(),
                        Picture4 = c.String(),
                        Picture5 = c.String(),
                        SequenceNumber = c.Int(nullable: false, defaultValue: 9999),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Slider");
            DropTable("dbo.Settings");
            DropTable("dbo.Language");
            DropTable("dbo.GalleryDetail");
            DropTable("dbo.Gallery");
            DropTable("dbo.File");
            DropTable("dbo.Content");
            DropTable("dbo.Category");
        }
    }
}
