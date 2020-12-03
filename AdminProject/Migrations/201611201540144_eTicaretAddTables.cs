namespace AdminProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eTicaretAddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AddressSaveName = c.String(),
                        NameSurname = c.String(),
                        AddressDetail = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        Phone = c.String(),
                        Gsm = c.String(),
                        TcNr = c.String(),
                        AddressNote = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExchangeType = c.String(),
                        Branch = c.String(),
                        BranchCode = c.String(),
                        AccountNo = c.String(),
                        Iban = c.String(),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Basket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Unit = c.Int(nullable: false),
                        OtherDetail = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cargo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Logo = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPayDoor = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryProductAssg",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Detail = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        InvoiceSaveName = c.String(),
                        InvoiceType = c.Int(nullable: false),
                        NameSurname = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Gsm = c.String(),
                        TaxNr = c.String(),
                        TaxOffice = c.String(),
                        IsEInvoice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeasureDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeasureId = c.Int(nullable: false),
                        Size = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Measure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProductAssg",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Unit = c.Int(nullable: false),
                        MeasureId = c.Int(nullable: false),
                        IsMeasure = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentOrderId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        OrderNr = c.Int(nullable: false),
                        CargoId = c.Int(nullable: false),
                        CargoNr = c.String(),
                        Description = c.String(),
                        PayType = c.Int(nullable: false),
                        OrderType = c.Int(nullable: false),
                        OrderNote = c.String(),
                        CauseOfRefund = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Picture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MinPicture = c.String(),
                        BigPicture = c.String(),
                        IsShowcase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductMeasureAssg",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MeasureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StockType = c.Int(nullable: false),
                        ProductType = c.Int(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountOdd = c.String(),
                        KdvOdd = c.String(),
                        IsKdv = c.String(),
                        StockNr = c.String(),
                        CreateDate = c.String(),
                        UpdateDate = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Name = c.String(),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                        BirthDate = c.Boolean(nullable: false),
                        Gender = c.String(),
                        Phone = c.String(),
                        Gsm = c.String(),
                        TcNr = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        Address = c.String(),
                        Password = c.String(),
                        ActivationCode = c.String(),
                        BannedMessage = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.Region");
            DropTable("dbo.Product");
            DropTable("dbo.ProductMeasureAssg");
            DropTable("dbo.Picture");
            DropTable("dbo.Order");
            DropTable("dbo.OrderProductAssg");
            DropTable("dbo.Measure");
            DropTable("dbo.MeasureDetail");
            DropTable("dbo.Invoice");
            DropTable("dbo.Country");
            DropTable("dbo.Comment");
            DropTable("dbo.City");
            DropTable("dbo.CategoryProductAssg");
            DropTable("dbo.Cargo");
            DropTable("dbo.Brand");
            DropTable("dbo.Basket");
            DropTable("dbo.Bank");
            DropTable("dbo.Address");
        }
    }
}
