namespace AdminProject.Models
{
    public enum CommentTypes
    {
        Deactive = 0,
        Active = 1,
        New = 2,
        Deleted = 3
    }

    public enum InoviceTypes
    {
        Personal = 0,
        Corporate = 1
    }

    public enum OrderTypes
    {
        All = -1,
        Pending = 0,            //beklemede
        Canceled = 1,           //iptal edildi
        CanceledReversal = 2,   //iptal surecinde
        Chargeback = 3,         //kargodan geri dondu
        Chargebacking = 4,      //kargodan geri donuyor
        Complete = 5,           //tamamlandi
        Denied = 6,             //reddedildi
        Expired = 7,            //suresi doldu
        Failed = 8,             //basarisiz oldu
        Processed = 9,          //islemde
        Processing = 10,        //isleme alindi
        Refunded = 11,          //geri gonderildi
        Reversed = 12,          //kullanici geri gonderdi
        Shipped = 13,           //gonderildi
        Voided = 14,            //gecersiz
        AwaitingProduct = 15,   //urun beklemede
        ProcurementProcess = 16,//tedarik surecinde
        Days23 = 17,            //2-3 gun icerisinde
        MissingSent = 18        //eksik gonderildi
    }

    //public enum ShipmentTypes
    //{
    //    All = -1,
    //    ExpectedPayment = 0, //ödeme bekleniyor
    //    CompletePayment = 1, //ödeme tamamlandı (alındı)
    //    CanceledRefundedPayment = 2, //ödeme iade sürecinde
    //    RefundedPayment = 3, //ödeme iade edildi
    //    FailedPayment = 4, //ödeme başarısız oldu
    //    VoidedPayment = 5, //ödeme geçersiz
    //}

    public enum StockTypes
    {
        All = -1,
        InStock = 0, //stokta
        OutStock = 1, //stokta yok
        Days23 = 2, //2-3 gun icerisinde
        ProcurementProcess = 3, //tedarik surecinde
        PreOrder = 4 //On siparis
    }

    public enum PayTypes
    {
        All = -1,
        Remittance = 0, //havale
        CreditCard = 1, //kredi karti
        PayDoor = 2,
        PayDoorCreditCard = 3,
        PayU = 4,
        Iyzico = 5,
        PayPal = 6
    }

    public enum UserTypes
    {
        Deleted = 3,
        Banned = 2,
        Active = 1,
        Deactive = 0
    }

    public enum ProductGroupTypes
    {
        Normal = 0,
        WeChose = 1,
        News = 2,
        Liked = 3,
        Favorites = 4
    }

    public enum ProductTypes
    {
        All = -1,
        VerySold = 5, //cok satiliyor
        Special = 4, //ozel
        Ending = 3, //tukeniyor
        Discount = 2, //indirimde
        Normal = 1,
        New = 0 //yeni
    }

    public enum ProductOrderTypes
    {
        Normal,
        PriceDesc,
        PriceAsc,
        Popularity
    }



    public enum StatusTypes
    {
        Freeze = 2,
        Active = 1,
        Deactive = 0
    }

    public enum CategoryTypes
    {
        Product = 3,
        Static = 2,
        Parent = 1,
        Master = 0,
        All
    }

    public enum ContentTypes
    {
        Content = 0,
        GalleryDetail = 1
    }

    public enum PictureTypes
    {
        Slider = 0,
        LightBox = 1,
        Showcase = 2,
        ContentSlider = 3
    }

    public enum VideoTypes
    {
        IsNotVideo = 0,
        IsVideo = 1,
        IsEmbedCode = 2
    }

    public enum UserActivationTypes
    {
        ActivationCodeNotFound,
        ActivationSuccess
    }
}