using System.Web.Configuration;
using AdminProject.Infrastructure;
using AdminProject.Models;
using AdminProject.Services;
using AdminProject.Services.Interface;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AdminProject.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AdminProject.App_Start.NinjectWebCommon), "Stop")]

namespace AdminProject.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<RuntimeSettings>().ToMethod<RuntimeSettings>(context =>
            {
                var maxWitdh = WebConfigurationManager.AppSettings["ImageMaxWidth"];
                var maxHeight = WebConfigurationManager.AppSettings["ImageMaxHeight"];

                var productMaxWitdh = WebConfigurationManager.AppSettings["ProductImageMaxWidth"];
                var productMaxHeight = WebConfigurationManager.AppSettings["ProductImageMaxHeight"];

                var productMinWitdh = WebConfigurationManager.AppSettings["ProductImageMinWidth"];
                var productMinHeight = WebConfigurationManager.AppSettings["ProductImageMinHeight"];

                var productImagePath = WebConfigurationManager.AppSettings["ProductImagePath"];

                var contactEmailAddress = WebConfigurationManager.AppSettings["ContactEmailAddress"];
                var emailAddress = WebConfigurationManager.AppSettings["EmailAddress"];
                var emailPassword = WebConfigurationManager.AppSettings["EmailPassword"];
                var port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]);
                var smtp = WebConfigurationManager.AppSettings["Smtp"];
                var domain = WebConfigurationManager.AppSettings["Domain"];

                var setting = new RuntimeSettings
                {
                    ContactEmailAddress = contactEmailAddress,
                    EmailAddress = emailAddress,
                    EmailPassword = emailPassword,
                    Port = port,
                    Smtp = smtp,
                    Domain = domain,

                    ImageMaxHeight = Convert.ToInt32(maxHeight),
                    ImageMaxWidth = Convert.ToInt32(maxWitdh),

                    ProductImagePath = productImagePath,

                    ProductImageMaxWidth = Convert.ToInt32(productMaxWitdh),
                    ProductImageMaxHeight = Convert.ToInt32(productMaxHeight),

                    ProductImageMinWidth = Convert.ToInt32(productMinWitdh),
                    ProductImageMinHeight = Convert.ToInt32(productMinHeight),

                    Language = "tr",
                    LanguageId = 1,
                    PictureExtensionTypes = new[] { ".bmp", ".jpg", ".jpeg", ".png", ".ico", ".gif", ".BMP", ".JPG", ".JPEG", ".PNG", ".ICO", ".GIF" },
                    PictureMimeType = new[] { "image/jpeg", "image/pjpeg", "image/bmp", "image/x-icon", "image/png", "image/gif" },

                    UserExtensionTypes = new[] { ".bmp", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".pdf", ".BMP", ".JPG", ".JPEG", ".PNG", ".DOC", ".DOCX", ".PDF" },
                    UserMimeTypes = new[] { "image/jpeg", "image/pjpeg", "image/bmp", "image/png", "image/gif", "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },

                    FileMimeTypes = new[]
                    {
                        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.openxmlformats-officedocument.wordprocessingml.template",
                        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.openxmlformats-officedocument.spreadsheetml.template",
                        "application/vnd.ms-excel.sheet.macroEnabled.12", "application/vnd.ms-excel.template.macroEnabled.12", "application/vnd.ms-excel.addin.macroEnabled.12",
                        "application/vnd.ms-excel.sheet.binary.macroEnabled.12", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                        "application/vnd.openxmlformats-officedocument.presentationml.slideshow", "application/vnd.openxmlformats-officedocument.presentationml.template", "application/vnd.ms-powerpoint.presentation.macroEnabled.12", "application/x-7z-compressed", "application/pdf", "application/vnd.android.package-archive", "image/vnd.dxf",
                        "model/vnd.dwf", "image/bmp", "text/csv", "image/vnd.dwg", "image/gif", "video/h263", "image/x-icon", "image/jpeg", "application/x-msaccess","application/vnd.ms-xpsdocument",
                        "video/mpeg", "audio/mp4", "video/mp4", "application/mp4", "application/ogg", "audio/ogg", "video/ogg", "audio/webm", "video/webm", "application/vnd.oasis.opendocument.text",
                        "application/vnd.oasis.opendocument.database", "application/vnd.oasis.opendocument.graphics", "application/vnd.oasis.opendocument.text-master", "application/x-font-otf",
                        "image/vnd.adobe.photoshop", "application/rtf", "text/richtext", "text/x-vcard", "application/xml", "application/xslt+xml", "application/x-rar-compressed", "application/zip",
                        "image/png", "video/3gpp", "video/x-msvideo", "application/x-shockwave-flash", "application/onenote", "application/vnd.ms-powerpoint.addin.macroenabled.12",
                        "application/vnd.ms-powerpoint.slideshow.macroenabled.12", "video/x-ms-wm", "audio/x-ms-wma", "audio/mpeg", "audio/webm", "video/webm"
                    },
                    FileExtensionTypes = new[] { ".doc", ".docx", ".xls", ".xlsx", ".rar", ".zip", ".7z", ".bmp", ".jpg", 
                                            ".jpeg", ".ico", ".png", ".3gp", ".avi", ".pdf", ".swf", ".apk", ".dxf", 
                                            ".dwg", ".gif", ".h263", ".mpg", ".mpeg", ".mdb", ".xlam", ".xlsb", ".xltm", 
                                            ".xlsm", ".pptx", ".ppt", ".dotx", ".onetoc", ".ppam", ".pptm", ".ppsm", ".wm", 
                                            ".wma", ".xps", ".mpga", ".mp4", ".mp4a", ".ogv", ".oga", ".webm", ".weba",".odb", 
                                            ".odg", ".odt", ".odm", ".otf", ".psd", ".rtf", ".rtx", ".vcf", ".xml", ".xslt" },
                    //[KARGO]
                    RightToWithdraw = @"
                    Cayma Hakký<br>
                    Tüketici (ALICI), 14 (ondört) gün içinde herhangi bir gerekçe göstermeksizin ve cezai þart ödemeksizin sözleþmeden cayma hakkýna sahiptir. Cayma hakký süresi, hizmet ifasýna iliþkin sözleþmelerde sözleþmenin kurulduðu gün; mal teslimine iliþkin sözleþmelerde ise tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin malý teslim aldýðý gün baþlar. Ancak tüketici, sözleþmenin kurulmasýndan malýn teslimine kadar olan süre içinde de cayma hakkýný kullanabilir. Cayma hakký süresinin belirlenmesinde;<br>
                    1.	Tek sipariþ konusu olup ayrý ayrý teslim edilen mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son malý teslim aldýðý gün,<br>
                    2.	Birden fazla parçadan oluþan mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son parçayý teslim aldýðý gün,<br>
                    3.	Belirli bir süre boyunca malýn düzenli tesliminin yapýldýðý sözleþmelerde, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin ilk malý teslim aldýðý gün esas alýnýr. Cayma bildiriminizi cayma hakký süresi dolmadan www.payidar.com.tr'de yer alan kiþisel üyelik sayfanýzdaki iade ve geri gönderim seçeneði üzerinden gerçekleþtirebilirsiniz. Cayma hakkýnýz kapsamýnda öngörülen taþýyýcý [KARGO] (herhangi bir þirket/ firma) olup, www.payidar.com.tr'de yer alan kiþisel üyelik sayfanýzdaki iade ve geri gönderim seçeneðinde geri gönderime iliþkin detaylar açýklanmýþtýr.<br>
                    Tüketici aþaðýdaki sözleþmelerde cayma hakkýný kullanamaz: <br>
                    1.	Fiyatý finansal piyasalardaki dalgalanmalara baðlý olarak deðiþen ve SATICI veya saðlayýcýnýn kontrolünde olmayan mal veya hizmetlere iliþkin sözleþmeler.<br>
                    2.	Tüketicinin istekleri veya kiþisel ihtiyaçlarý doðrultusunda hazýrlanan mallara iliþkin sözleþmeler.<br>
                    3.	Çabuk bozulabilen veya son kullanma tarihi geçebilecek mallarýn teslimine iliþkin sözleþmeler.<br>
                    4.	Tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olan mallardan; iadesi saðlýk ve hijyen açýsýndan uygun olmayanlarýn teslimine iliþkin sözleþmeler.<br>
                    5.	Tesliminden sonra baþka ürünlerle karýþan ve doðasý gereði ayrýþtýrýlmasý mümkün olmayan mallara iliþkin sözleþmeler.<br>
                    6.	Malýn tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olmasý halinde maddi ortamda sunulan kitap, dijital içerik ve bilgisayar sarf malzemelerine iliþkin sözleþmeler.<br>
                    7.	Abonelik sözleþmesi kapsamýnda saðlananlar dýþýnda, gazete ve dergi gibi süreli yayýnlarýn teslimine iliþkin sözleþmeler.<br>
                    8.	Belirli bir tarihte veya dönemde yapýlmasý gereken, konaklama, eþya taþýma, araba kiralama, yiyecek-içecek tedariki ve eðlence veya dinlenme amacýyla yapýlan boþ zamanýn deðerlendirilmesine iliþkin sözleþmeler.<br>
                    9.	Elektronik ortamda anýnda ifa edilen hizmetler veya tüketiciye anýnda teslim edilen gayri maddi mallara iliþkin sözleþmeler.<br>
                    10.	Cayma hakký süresi sona ermeden önce, tüketicinin onayý ile ifasýna baþlanan hizmetlere iliþkin sözleþmeler.<br>",
                    //[ALICI]
                    //[ADRESI]
                    //[TELEFON]
                    //[EMAIL]
                    //[TABLO]
                    //[TESLIMATADRESI]
                    //[TESLIMEDILECEKKISI]
                    //[FATURAADRESI]
                    //[TARIH] dd.mm.yyyy
                    DistanceSalesContract = @"
                    MESAFELÝ SATIÞ SÖZLEÞMESÝ <br>
                    MADDE 1- TARAFLAR <br><br>

                    1.1. SATICI: <br><br>

                    Ünvaný: Duha Gümüþ Saat/ Payidar.com.tr<br>
                    Adresi: Kayýþdaðý Mh. Akyazý Cd. No: 42/ C Ataþehir/ Ýstanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br>
                    <br>
                    1.2. ALICI: <br>
                    Adý/Soyadý/Ünvaný: [ALICI] <br>
                    Adresi : [ADRESI] <br>
                    Telefon: [TELEFON] <br>
                    Email: [EMAIL] <br><br>

                    MADDE 2- KONU <br>
                    Ýþbu sözleþmenin konusu, ALICI'nýn www.payidar.com.tr internet sitesinden elektronik ortamda sipariþini yaptýðý aþaðýda nitelikleri ve satýþ fiyatý belirtilen ürünün satýþý ve teslimi ile ilgili olarak 6502 sayýlý Tüketicinin Korunmasý Hakkýndaki Kanun hükümleri gereðince taraflarýn hak ve yükümlülüklerinin saptanmasýdýr. <br>


                    MADDE 3- SÖZLEÞME KONUSU ÜRÜN, ÖDEME VE TESLÝMATA ÝLÝÞKÝN BÝLGÝLER <br>
                    3.1- Sözleþme konusu mal veya hizmetin adý, adeti, KDV dahil satýþ fiyatý, ödeme þekli ve temel nitelikleri 
                    <br>
                    [TABLO]
                    <br><br>
                    3.2- Ödeme Þekli: Havale/ EFT ile ödeme <br>
                    3.3- Diðer yandan vadeli satýþlarýn sadece Bankalara ait kredi kartlarý ile yapýlmasý nedeniyle, ALICI, ilgili faiz oranlarýný ve temerrüt faizi ile ilgili bilgileri bankasýndan ayrýca teyit edeceðini, yürürlükte bulunan mevzuat hükümleri gereðince faiz ve temerrüt faizi ile ilgili hükümlerin Banka ve ALICI arasýndaki kredi kartý sözleþmesi kapsamýnda uygulanacaðýný kabul, beyan ve taahhüt eder. <br>

                    Ýade Prosedürü: <br>
                    a) Kredi Kartýna Ýade Prosedürü<br>
                    ALICI’nýn cayma hakkýný kullandýðý durumlarda ya da sipariþe konu olan ürünün çeþitli sebeplerle tedarik edilememesi veya hakem heyeti kararlarý ile ALICI’ya bedel iadesine karar verilen durumlarda, alýþveriþ kredi kartý ile ve taksitli olarak yapýlmýþsa, kredi kartýna iade prosedürü aþaðýda belirtilmiþtir: <br>

                    ALICI ürünü kaç taksit ile aldýysa Banka ALICI’ya geri ödemesini taksitle yapmaktadýr. SATICI bankaya ürün bedelinin tamamýný tek seferde ödedikten sonra, Banka poslarýndan yapýlan taksitli harcamalarýn ALICI’nýn kredi kartýna iadesi durumunda, konuya müdahil taraflarýn maðdur duruma düþmemesi için talep edilen iade tutarlarý, yine taksitli olarak hamil taraf hesaplarýna Banka tarafýndan aktarýlýr. ALICI’nýn satýþ iptaline kadar ödemiþ olduðu taksit tutarlarý, eðer iade tarihi ile kartýn hesap kesim tarihleri çakýþmazsa her ay karta 1 (bir) iade yansýyacak ve ALICI iade öncesinde ödemiþ olduðu taksitleri satýþýn taksitleri bittikten sonra, iade öncesinde ödemiþ olduðu taksitleri sayýsý kadar ay daha alacak ve mevcut borçlarýndan düþmüþ olacaktýr. 
                    <br>
                    Kart ile alýnmýþ mal ve hizmetin iadesi durumunda SATICI, Banka ile yapmýþ olduðu sözleþme gereði ALICI’ya nakit para ile ödeme yapamaz. Üye iþyeri yani SATICI, bir iade iþlemi sözkonusu olduðunda ilgili yazýlým aracýlýðý ile iadesini yapacak olup, Üye iþyeri yani SATICI ilgili tutarý Banka’ya nakden veya mahsuben ödemekle yükümlü olduðundan yukarýda anlatmýþ olduðumuz prosedür gereðince ALICI’ya nakit olarak ödeme yapýlamamaktadýr. Kredi kartýna iade, SATICI’nýn Banka’ya bedeli tek seferde ödemesinden sonra, Banka tarafýndan yukarýdaki prosedür gereðince yapýlacaktýr. 
                    <br>
                    ALICI, bu prosedürü okuduðunu ve kabul ettiðini kabul ve taahhüt eder. <br><br>

                    B) Kapýdan Ödeme ile Havale/EFT Ödeme Seçeneklerinde Ýade Prosedürü <br>

                    Kapýdan ödeme ile havale/EFT ödeme seçeneklerinde iade Tüketiciden banka hesap bilgileri istenerek, Tüketicinin belirttiði hesaba (hesabýn fatura adresindeki kiþinin adýna veya kullanýcý üyenin adýna olmasý þarttýr) havale ve EFT þeklinde yapýlacaktýr. <br>
                    3.4- Teslimat Þekli ve Adresi : <br>
                    Teslimat Adresi : [TESLIMATADRESI] <br>
                    Teslim Edilecek Kiþi: [TESLIMEDILECEKKISI] <br>
                    Fatura Adresi : [FATURAADRESI] <br>

                    Paketleme, kargo ve teslim masraflarý ALICI tarafýndan karþýlanmaktadýr. Kargo ücreti X -TL olup, kargo fiyatý sipariþ toplam tutarýna eklenmektedir. Ürün bedeline dahil deðildir. Teslimat, anlaþmalý kargo þirketi aracýlýðý ile, ALICI'nýn yukarýda belirtilen adresinde elden teslim edilecektir. Teslim anýnda ALICI'nýn adresinde bulunmamasý durumunda dahi Firmamýz edimini tam ve eksiksiz olarak yerine getirmiþ olarak kabul edilecektir. Bu nedenle, ALICI'nýn ürünü geç teslim almasýndan ve/veya hiç teslim almamasýndan kaynaklanan zararlardan ve giderlerden SATICI sorumlu deðildir. SATICI, sözleþme konusu ürünün saðlam, eksiksiz, sipariþte belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullaným kýlavuzlarý ile teslim edilmesinden sorumludur.<br>
                    <br>
                    MADDE 4- CAYMA HAKKI <br>
                    Tüketici (ALICI), 14 (ondört) gün içinde herhangi bir gerekçe göstermeksizin ve cezai þart ödemeksizin sözleþmeden cayma hakkýna sahiptir. Cayma hakký süresi, hizmet ifasýna iliþkin sözleþmelerde sözleþmenin kurulduðu gün; mal teslimine iliþkin sözleþmelerde ise tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin malý teslim aldýðý gün baþlar. Ancak tüketici, sözleþmenin kurulmasýndan malýn teslimine kadar olan süre içinde de cayma hakkýný kullanabilir. Cayma hakký süresinin belirlenmesinde; <br>
                    a) Tek sipariþ konusu olup ayrý ayrý teslim edilen mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son malý teslim aldýðý gün, <br>
                    b) Birden fazla parçadan oluþan mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son parçayý teslim aldýðý gün, <br>
                    c) Belirli bir süre boyunca malýn düzenli tesliminin yapýldýðý sözleþmelerde, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin ilk malý teslim aldýðý gün esas alýnýr. Cayma bildiriminizi cayma hakký süresi dolmadan www.payidar.com.tr‘de yer alan kiþisel üyelik sayfanýzdaki iade seçeneði üzerinden gerçekleþtirebilirsiniz. Cayma hakkýnýz kapsamýnda öngörülen taþýyýcý [KARGO] (herhangi bir þirket/ firma) olup, www.payidar.com.tr‘de yer alan kiþisel üyelik sayfanýzdaki kolay iade seçeneðinde geri gönderime iliþkin detaylar açýklanmýþtýr. <br>
                    Tüketici aþaðýdaki sözleþmelerde cayma hakkýný kullanamaz: <br>
                    a) Fiyatý finansal piyasalardaki dalgalanmalara baðlý olarak deðiþen ve SATICI veya saðlayýcýnýn kontrolünde olmayan mal veya hizmetlere iliþkin sözleþmeler. <br>
                    b) Tüketicinin istekleri veya kiþisel ihtiyaçlarý doðrultusunda hazýrlanan mallara iliþkin sözleþmeler. <br>
                    c) Çabuk bozulabilen veya son kullanma tarihi geçebilecek mallarýn teslimine iliþkin sözleþmeler. <br>
                    ç) Tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olan mallardan; iadesi saðlýk ve hijyen açýsýndan uygun olmayanlarýn teslimine iliþkin sözleþmeler. <br>
                    d) Tesliminden sonra baþka ürünlerle karýþan ve doðasý gereði ayrýþtýrýlmasý mümkün olmayan mallara iliþkin sözleþmeler. <br>
                    e) Malýn tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olmasý halinde maddi ortamda sunulan kitap, dijital içerik ve bilgisayar sarf malzemelerine iliþkin sözleþmeler. <br>
                    f) Abonelik sözleþmesi kapsamýnda saðlananlar dýþýnda, gazete ve dergi gibi süreli yayýnlarýn teslimine iliþkin sözleþmeler. <br>
                    g) Belirli bir tarihte veya dönemde yapýlmasý gereken, konaklama, eþya taþýma, araba kiralama, yiyecek-içecek tedariki ve eðlence veya dinlenme amacýyla yapýlan boþ zamanýn deðerlendirilmesine iliþkin sözleþmeler. <br>
                    ð) Elektronik ortamda anýnda ifa edilen hizmetler veya tüketiciye anýnda teslim edilen gayrimaddi mallara iliþkin sözleþmeler. <br>
                    h) Cayma hakký süresi sona ermeden önce, tüketicinin onayý ile ifasýna baþlanan hizmetlere iliþkin sözleþmeler.<br>
                    <br>

                    MADDE 5- GENEL HÜKÜMLER <br>
                    4.1- ALICI, www.payidar.com.tr internet sitesinde sözleþme konusu ürüne iliþkin ön bilgileri okuyup bilgi sahibi olduðunu ve elektronik ortamda gerekli teyidi verdiðini beyan eder. <br>
                    4.2- Ürün sözleþme tarihinden itibaren en geç 30 gün içerisinde teslim edilecektir. Ürününün teslim edilmesi anýna kadar tüm sorumluluk SATICI’ya aittir. <br>
                    4.3- Sözleþme konusu ürün, ALICI'dan baþka bir kiþi/kuruluþa teslim edilecek ise, teslim edilecek kiþi/kuruluþun teslimatý kabul etmemesinden SATICI sorumlu tutulamaz. <br>
                    4.4- SATICI, sözleþme konusu ürünün saðlam, eksiksiz, sipariþte belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullaným kýlavuzlarý ile teslim edilmesinden sorumludur. <br>
                    4.5- Sözleþme konusu ürünün teslimatý için iþbu sözleþmenin bedelinin ALICI'nýn tercih ettiði ödeme þekli ile ödenmiþ olmasý þarttýr. Herhangi bir nedenle ürün bedeli ödenmez veya banka kayýtlarýnda iptal edilir ise, SATICI ürünün teslimi yükümlülüðünden kurtulmuþ kabul edilir. <br>
                    4.6- Ürünün tesliminden sonra ALICI'ya ait kredi kartýnýn ALICI'nýn kusurundan kaynaklanmayan bir þekilde yetkisiz kiþilerce haksýz veya hukuka aykýrý olarak kullanýlmasý nedeni ile ilgili banka veya finans kuruluþun ürün bedelini SATICI'ya ödememesi halinde, ALICI'nýn kendisine teslim edilmiþ olmasý kaydýyla ürünün SATICI'ya gönderilmesi zorunludur. <br>
                    4.7- Garanti belgesi ile satýlan ürünlerden olan veya olmayan ürünlerin ayýplý (arýzalý, bozuk vb.) halinde veya garanti kapsamýnda ve þartlarý dahilinde arýzalanmasý veya bozulmasý halinde gerekli onarýmýn yetkili servise yaptýrýlmasý için söz konusu ürünler SATICI'ya gönderilebilir.<br>
                    4.8-385 sayýlý vergi usul kanunu genel tebliði uyarýnca iade iþlemlerinin yapýlabilmesi için tarafýnýza göndermiþ olduðumuz iade bölümü bulunan faturada ilgili bölümlerin eksiksiz olarak doldurulmasý ve imzalandýktan sonra tarafýmýza ürün ile birlikte geri gönderilmesi gerekmektedir. <br>

                    MADDE 8- UYUÞMAZLIK VE YETKÝLÝ MAHKEME <br>
                    Ýþbu sözleþmeden doðan uyuþmazlýklarda doðrudan yerleþim yerinizin bulunduðu veya tüketici iþleminin yapýldýðý yerdeki Tüketici Sorunlarý Hakem Heyeti veya Tüketici Mahkemesi yetkilidir. <br><br>

                    Sipariþin gerçekleþmesi durumunda ALICI iþbu sözleþmenin tüm koþullarýný kabul etmiþ sayýlýr. 
                    <br><br>
                    SATICI: Duha Gümüþ Saat/ Payidar.com.tr
                    <br><br>
                    ALICI: [ALICI]
                    <br><br>
                    Tarih: [TARIH]",

                    //[ALICI]
                    //[ADRESI]
                    //[TELEFON]
                    //[EMAIL]
                    //[TABLO]
                    //[KARGOUCRETI]
                    //[ODEMESEKLI] kredi karti, havale
                    //[TARIH] dd.mm.yyyy
                    PreliminaryInformationForm = @"
                    SATICI: <br>
                    Ünvaný: Payidar.com.tr <br>
                    Adresi: Kayýþdaðý Mh. Akyazý Cd. No:42/ C Ataþehir/ Ýstanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br><br>


                    ÖN BÝLGÝLENDÝRME FORMU <br>

                    1) Sözleþme konusu mal veya hizmetin adý, adeti, KDV dahil satýþ fiyatý ve temel nitelikleri <br>

                    [TABLO]
                    <br><br>

                    2) Paketleme, kargo ve teslim masraflarý ALICI tarafýndan karþýlanmaktadýr. Kargo ücreti [KARGOUCRETI] -TL olup, kargo fiyatý sipariþ toplam tutarýna eklenmektedir. Ürün bedeline dahil deðildir. Teslimat, anlaþmalý kargo þirketi aracýlýðý ile ALICI'nýn yukarýda belirtilen adresinde elden teslim edilecektir. Teslim anýnda ALICI'nýn adresinde bulunmamasý durumunda dahi Firmamýz edimini tam ve eksiksiz olarak yerine getirmiþ olarak kabul edilecektir. Bu nedenle, ALICI'nýn ürünü geç teslim almasýndan ve/veya hiç teslim almamasýndan kaynaklanan zararlardan ve giderlerden SATICI sorumlu deðildir. SATICI, sözleþme konusu ürünün saðlam, eksiksiz, sipariþte belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullaným kýlavuzlarý ile teslim edilmesinden sorumludur.
                    <br>
                    3) Ürün sözleþme tarihinden itibaren en geç 30 gün içerisinde teslim edilecektir. Ürününün teslim edilmesi anýna kadar tüm sorumluluk SATICI’ya aittir. <br>

                    4) Tüketici (ALICI), 14 (ondört) gün içinde herhangi bir gerekçe göstermeksizin ve cezai þart ödemeksizin sözleþmeden cayma hakkýna sahiptir. Cayma hakký süresi, hizmet ifasýna iliþkin sözleþmelerde sözleþmenin kurulduðu gün; mal teslimine iliþkin sözleþmelerde ise tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin malý teslim aldýðý gün baþlar. Ancak tüketici, sözleþmenin kurulmasýndan malýn teslimine kadar olan süre içinde de cayma hakkýný kullanabilir. Cayma hakký süresinin belirlenmesinde; <br>
                    a) Tek sipariþ konusu olup ayrý ayrý teslim edilen mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son malý teslim aldýðý gün, <br>
                    b) Birden fazla parçadan oluþan mallarda, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin son parçayý teslim aldýðý gün, <br>
                    c) Belirli bir süre boyunca malýn düzenli tesliminin yapýldýðý sözleþmelerde, tüketicinin veya tüketici tarafýndan belirlenen üçüncü kiþinin ilk malý teslim aldýðý gün esas alýnýr. Cayma bildiriminizi cayma hakký süresi dolmadan www.payidar.com.tr‘de yer alan kiþisel üyelik sayfanýzdaki kolay iade seçeneði üzerinden gerçekleþtirebilirsiniz. Cayma hakkýnýz kapsamýnda öngörülen taþýyýcý [KARGO] (veya herhangi bir þirket/ firma) olup, www.payidar.com.tr‘de yer alan kiþisel üyelik sayfanýzdaki kolay iade seçeneðinde geri gönderime iliþkin detaylar açýklanmýþtýr. <br>
                    Tüketici aþaðýdaki sözleþmelerde cayma hakkýný kullanamaz: <br>
                    a) Fiyatý finansal piyasalardaki dalgalanmalara baðlý olarak deðiþen ve SATICI veya saðlayýcýnýn kontrolünde olmayan mal veya hizmetlere iliþkin sözleþmeler. <br>
                    b) Tüketicinin istekleri veya kiþisel ihtiyaçlarý doðrultusunda hazýrlanan mallara iliþkin sözleþmeler. <br>
                    c) Çabuk bozulabilen veya son kullanma tarihi geçebilecek mallarýn teslimine iliþkin sözleþmeler. <br>
                    ç) Tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olan mallardan; iadesi saðlýk ve hijyen açýsýndan uygun olmayanlarýn teslimine iliþkin sözleþmeler. <br>
                    d) Tesliminden sonra baþka ürünlerle karýþan ve doðasý gereði ayrýþtýrýlmasý mümkün olmayan mallara iliþkin sözleþmeler. <br>
                    e) Malýn tesliminden sonra ambalaj, bant, mühür, paket gibi koruyucu unsurlarý açýlmýþ olmasý halinde maddi ortamda sunulan kitap, dijital içerik ve bilgisayar sarf malzemelerine iliþkin sözleþmeler. <br>
                    f) Abonelik sözleþmesi kapsamýnda saðlananlar dýþýnda, gazete ve dergi gibi süreli yayýnlarýn teslimine iliþkin sözleþmeler. <br>
                    g) Belirli bir tarihte veya dönemde yapýlmasý gereken, konaklama, eþya taþýma, araba kiralama, yiyecek-içecek tedariki ve eðlence veya dinlenme amacýyla yapýlan boþ zamanýn deðerlendirilmesine iliþkin sözleþmeler. <br>
                    ð) Elektronik ortamda anýnda ifa edilen hizmetler veya tüketiciye anýnda teslim edilen gayrimaddi mallara iliþkin sözleþmeler. <br>
                    h) Cayma hakký süresi sona ermeden önce, tüketicinin onayý ile ifasýna baþlanan hizmetlere iliþkin sözleþmeler. <br>
                    5) Tüketicinin herhangi bir dijital içerik satýn almasý halinde dijital içeriklerin iþlevselliðini etkileyecek teknik koruma önlemleri ve SATICI’nýn bildiði ya da makul olarak bilmesinin beklendiði, dijital içeriðin hangi donaným ya da yazýlýmla birlikte çalýþabileceðine iliþkin bilgiler satýn alýnan ürünün www.payidar.com.tr‘de satýþa sunulduðu sayfadaki tanýtým içeriðinde yer almaktadýr. <br>


                    6) Tüketicilerin þikayet ve itirazlarý: Sipariþinize ve/veya sipariþinize konu ürüne ve/veya þipariþinizle ilgili herhangi bir konuda þikayetinizin olmasý halinde þikayetlerinizi yukarýda belirtilen iletiþim bilgileri veya www.payidar.com.tr internet sitesinde belirtilen iletiþim bilgileri vasýtasýyla SATICI’ya iletebilirsiniz. Ýletmiþ olduðunuz þikâyet baþvurularýnýz derhal kayýtlara alýnacak, yetkili birimler tarafýndan deðerlendirilerek çözümlenmeye çalýþýlacak ve en kýsa sürede size geri dönüþ saðlanacaktýr. Ayrýca, þikâyet baþvurularýnýzý doðrudan yerleþim yerinizin bulunduðu veya tüketici iþleminin yapýldýðý yerdeki Tüketici Sorunlarý Hakem Heyetine veya Tüketici Mahkemesine yapabilirsiniz.<br>
                    <br>

                    SATICI: <br>
                    Ünvaný: Duha Gümüþ Saat/ Payidar.com.tr<br>
                    Adresi: Kayýþdaðý Mh. Akyazý Cd. No:42/ C Ataþehir/ Ýstanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br><br>

                    ALICI: <br>
                    Adý/ Soyadý/ Ünvaný: [ALICI] <br>
                    Adresi: [ADRESI] <br>
                    Telefon: [TELEFON] <br>
                    Email: [EMAIL] <br>
                    Tarih : [TARIH] <br>"
                };
                return setting;
            }).InSingletonScope();

            var iyzipayConfig = new IyzipayConfig
            {
                ApiKey = WebConfigurationManager.AppSettings["iyzico.apiKey"],
                SecretKey = WebConfigurationManager.AppSettings["iyzico.secretKey"],
                BaseUrl = WebConfigurationManager.AppSettings["iyzico.baseUrl"],
                CallbackUrl = WebConfigurationManager.AppSettings["iyzico.callbackUrl"]
            };

            kernel.Bind<IyzipayConfig>().ToConstant(iyzipayConfig);

            kernel.Bind<IAddressService>().To<AddressService>().InSingletonScope();
            kernel.Bind<IBankService>().To<BankService>().InSingletonScope();
            kernel.Bind<IBasketService>().To<BasketService>().InSingletonScope();
            kernel.Bind<IBrandService>().To<BrandService>().InSingletonScope();
            kernel.Bind<ICampaignService>().To<CampaignService>().InSingletonScope();
            kernel.Bind<ICargoService>().To<CargoService>().InSingletonScope();
            kernel.Bind<ICityService>().To<CityService>().InSingletonScope();
            kernel.Bind<ICommentService>().To<CommentService>().InSingletonScope();
            kernel.Bind<ICountryService>().To<CountryService>().InSingletonScope();
            kernel.Bind<IInvoiceService>().To<InvoiceService>().InSingletonScope();
            kernel.Bind<IMeasureService>().To<MeasureService>().InSingletonScope();
            kernel.Bind<IOrderService>().To<OrderService>().InSingletonScope();
            kernel.Bind<IPictureService>().To<PictureService>().InSingletonScope();
            kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            kernel.Bind<IRegionService>().To<RegionService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            kernel.Bind<ICategoryService>().To<CategoryService>().InSingletonScope();
            kernel.Bind<ISliderService>().To<SliderService>().InSingletonScope();
            kernel.Bind<IPropertyService>().To<PropertyService>().InSingletonScope();
            kernel.Bind<ISettingService>().To<SettingService>().InSingletonScope();
            kernel.Bind<IEmailService>().To<EmailService>().InSingletonScope();
            kernel.Bind<IFavoriteService>().To<FavoriteService>().InSingletonScope();
            kernel.Bind<IContentService>().To<ContentService>().InSingletonScope();
            kernel.Bind<IBulletinService>().To<BulletinService>().InSingletonScope();

            kernel.Bind<IIyzipayService>().To<IyzipayService>().InSingletonScope();

            kernel.Bind<IRssService>().To<RssService>().InSingletonScope();

            kernel.Bind<AdminDbContext>().ToMethod<AdminDbContext>(context =>
            {
                var dbContext = new AdminDbContext("AdminDbContext");
                return dbContext;
            }).InRequestScope();
        }
    }
}
