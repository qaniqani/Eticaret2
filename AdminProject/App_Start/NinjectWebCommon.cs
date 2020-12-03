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
                    Cayma Hakk�<br>
                    T�ketici (ALICI), 14 (ond�rt) g�n i�inde herhangi bir gerek�e g�stermeksizin ve cezai �art �demeksizin s�zle�meden cayma hakk�na sahiptir. Cayma hakk� s�resi, hizmet ifas�na ili�kin s�zle�melerde s�zle�menin kuruldu�u g�n; mal teslimine ili�kin s�zle�melerde ise t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin mal� teslim ald��� g�n ba�lar. Ancak t�ketici, s�zle�menin kurulmas�ndan mal�n teslimine kadar olan s�re i�inde de cayma hakk�n� kullanabilir. Cayma hakk� s�resinin belirlenmesinde;<br>
                    1.	Tek sipari� konusu olup ayr� ayr� teslim edilen mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son mal� teslim ald��� g�n,<br>
                    2.	Birden fazla par�adan olu�an mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son par�ay� teslim ald��� g�n,<br>
                    3.	Belirli bir s�re boyunca mal�n d�zenli tesliminin yap�ld��� s�zle�melerde, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin ilk mal� teslim ald��� g�n esas al�n�r. Cayma bildiriminizi cayma hakk� s�resi dolmadan www.payidar.com.tr'de yer alan ki�isel �yelik sayfan�zdaki iade ve geri g�nderim se�ene�i �zerinden ger�ekle�tirebilirsiniz. Cayma hakk�n�z kapsam�nda �ng�r�len ta��y�c� [KARGO] (herhangi bir �irket/ firma) olup, www.payidar.com.tr'de yer alan ki�isel �yelik sayfan�zdaki iade ve geri g�nderim se�ene�inde geri g�nderime ili�kin detaylar a��klanm��t�r.<br>
                    T�ketici a�a��daki s�zle�melerde cayma hakk�n� kullanamaz: <br>
                    1.	Fiyat� finansal piyasalardaki dalgalanmalara ba�l� olarak de�i�en ve SATICI veya sa�lay�c�n�n kontrol�nde olmayan mal veya hizmetlere ili�kin s�zle�meler.<br>
                    2.	T�keticinin istekleri veya ki�isel ihtiya�lar� do�rultusunda haz�rlanan mallara ili�kin s�zle�meler.<br>
                    3.	�abuk bozulabilen veya son kullanma tarihi ge�ebilecek mallar�n teslimine ili�kin s�zle�meler.<br>
                    4.	Tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olan mallardan; iadesi sa�l�k ve hijyen a��s�ndan uygun olmayanlar�n teslimine ili�kin s�zle�meler.<br>
                    5.	Tesliminden sonra ba�ka �r�nlerle kar��an ve do�as� gere�i ayr��t�r�lmas� m�mk�n olmayan mallara ili�kin s�zle�meler.<br>
                    6.	Mal�n tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olmas� halinde maddi ortamda sunulan kitap, dijital i�erik ve bilgisayar sarf malzemelerine ili�kin s�zle�meler.<br>
                    7.	Abonelik s�zle�mesi kapsam�nda sa�lananlar d���nda, gazete ve dergi gibi s�reli yay�nlar�n teslimine ili�kin s�zle�meler.<br>
                    8.	Belirli bir tarihte veya d�nemde yap�lmas� gereken, konaklama, e�ya ta��ma, araba kiralama, yiyecek-i�ecek tedariki ve e�lence veya dinlenme amac�yla yap�lan bo� zaman�n de�erlendirilmesine ili�kin s�zle�meler.<br>
                    9.	Elektronik ortamda an�nda ifa edilen hizmetler veya t�keticiye an�nda teslim edilen gayri maddi mallara ili�kin s�zle�meler.<br>
                    10.	Cayma hakk� s�resi sona ermeden �nce, t�keticinin onay� ile ifas�na ba�lanan hizmetlere ili�kin s�zle�meler.<br>",
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
                    MESAFEL� SATI� S�ZLE�MES� <br>
                    MADDE 1- TARAFLAR <br><br>

                    1.1. SATICI: <br><br>

                    �nvan�: Duha G�m�� Saat/ Payidar.com.tr<br>
                    Adresi: Kay��da�� Mh. Akyaz� Cd. No: 42/ C Ata�ehir/ �stanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br>
                    <br>
                    1.2. ALICI: <br>
                    Ad�/Soyad�/�nvan�: [ALICI] <br>
                    Adresi : [ADRESI] <br>
                    Telefon: [TELEFON] <br>
                    Email: [EMAIL] <br><br>

                    MADDE 2- KONU <br>
                    ��bu s�zle�menin konusu, ALICI'n�n www.payidar.com.tr internet sitesinden elektronik ortamda sipari�ini yapt��� a�a��da nitelikleri ve sat�� fiyat� belirtilen �r�n�n sat��� ve teslimi ile ilgili olarak 6502 say�l� T�keticinin Korunmas� Hakk�ndaki Kanun h�k�mleri gere�ince taraflar�n hak ve y�k�ml�l�klerinin saptanmas�d�r. <br>


                    MADDE 3- S�ZLE�ME KONUSU �R�N, �DEME VE TESL�MATA �L��K�N B�LG�LER <br>
                    3.1- S�zle�me konusu mal veya hizmetin ad�, adeti, KDV dahil sat�� fiyat�, �deme �ekli ve temel nitelikleri 
                    <br>
                    [TABLO]
                    <br><br>
                    3.2- �deme �ekli: Havale/ EFT ile �deme <br>
                    3.3- Di�er yandan vadeli sat��lar�n sadece Bankalara ait kredi kartlar� ile yap�lmas� nedeniyle, ALICI, ilgili faiz oranlar�n� ve temerr�t faizi ile ilgili bilgileri bankas�ndan ayr�ca teyit edece�ini, y�r�rl�kte bulunan mevzuat h�k�mleri gere�ince faiz ve temerr�t faizi ile ilgili h�k�mlerin Banka ve ALICI aras�ndaki kredi kart� s�zle�mesi kapsam�nda uygulanaca��n� kabul, beyan ve taahh�t eder. <br>

                    �ade Prosed�r�: <br>
                    a) Kredi Kart�na �ade Prosed�r�<br>
                    ALICI�n�n cayma hakk�n� kulland��� durumlarda ya da sipari�e konu olan �r�n�n �e�itli sebeplerle tedarik edilememesi veya hakem heyeti kararlar� ile ALICI�ya bedel iadesine karar verilen durumlarda, al��veri� kredi kart� ile ve taksitli olarak yap�lm��sa, kredi kart�na iade prosed�r� a�a��da belirtilmi�tir: <br>

                    ALICI �r�n� ka� taksit ile ald�ysa Banka ALICI�ya geri �demesini taksitle yapmaktad�r. SATICI bankaya �r�n bedelinin tamam�n� tek seferde �dedikten sonra, Banka poslar�ndan yap�lan taksitli harcamalar�n ALICI�n�n kredi kart�na iadesi durumunda, konuya m�dahil taraflar�n ma�dur duruma d��memesi i�in talep edilen iade tutarlar�, yine taksitli olarak hamil taraf hesaplar�na Banka taraf�ndan aktar�l�r. ALICI�n�n sat�� iptaline kadar �demi� oldu�u taksit tutarlar�, e�er iade tarihi ile kart�n hesap kesim tarihleri �ak��mazsa her ay karta 1 (bir) iade yans�yacak ve ALICI iade �ncesinde �demi� oldu�u taksitleri sat���n taksitleri bittikten sonra, iade �ncesinde �demi� oldu�u taksitleri say�s� kadar ay daha alacak ve mevcut bor�lar�ndan d��m�� olacakt�r. 
                    <br>
                    Kart ile al�nm�� mal ve hizmetin iadesi durumunda SATICI, Banka ile yapm�� oldu�u s�zle�me gere�i ALICI�ya nakit para ile �deme yapamaz. �ye i�yeri yani SATICI, bir iade i�lemi s�zkonusu oldu�unda ilgili yaz�l�m arac�l��� ile iadesini yapacak olup, �ye i�yeri yani SATICI ilgili tutar� Banka�ya nakden veya mahsuben �demekle y�k�ml� oldu�undan yukar�da anlatm�� oldu�umuz prosed�r gere�ince ALICI�ya nakit olarak �deme yap�lamamaktad�r. Kredi kart�na iade, SATICI�n�n Banka�ya bedeli tek seferde �demesinden sonra, Banka taraf�ndan yukar�daki prosed�r gere�ince yap�lacakt�r. 
                    <br>
                    ALICI, bu prosed�r� okudu�unu ve kabul etti�ini kabul ve taahh�t eder. <br><br>

                    B) Kap�dan �deme ile Havale/EFT �deme Se�eneklerinde �ade Prosed�r� <br>

                    Kap�dan �deme ile havale/EFT �deme se�eneklerinde iade T�keticiden banka hesap bilgileri istenerek, T�keticinin belirtti�i hesaba (hesab�n fatura adresindeki ki�inin ad�na veya kullan�c� �yenin ad�na olmas� �artt�r) havale ve EFT �eklinde yap�lacakt�r. <br>
                    3.4- Teslimat �ekli ve Adresi : <br>
                    Teslimat Adresi : [TESLIMATADRESI] <br>
                    Teslim Edilecek Ki�i: [TESLIMEDILECEKKISI] <br>
                    Fatura Adresi : [FATURAADRESI] <br>

                    Paketleme, kargo ve teslim masraflar� ALICI taraf�ndan kar��lanmaktad�r. Kargo �creti X -TL olup, kargo fiyat� sipari� toplam tutar�na eklenmektedir. �r�n bedeline dahil de�ildir. Teslimat, anla�mal� kargo �irketi arac�l��� ile, ALICI'n�n yukar�da belirtilen adresinde elden teslim edilecektir. Teslim an�nda ALICI'n�n adresinde bulunmamas� durumunda dahi Firmam�z edimini tam ve eksiksiz olarak yerine getirmi� olarak kabul edilecektir. Bu nedenle, ALICI'n�n �r�n� ge� teslim almas�ndan ve/veya hi� teslim almamas�ndan kaynaklanan zararlardan ve giderlerden SATICI sorumlu de�ildir. SATICI, s�zle�me konusu �r�n�n sa�lam, eksiksiz, sipari�te belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullan�m k�lavuzlar� ile teslim edilmesinden sorumludur.<br>
                    <br>
                    MADDE 4- CAYMA HAKKI <br>
                    T�ketici (ALICI), 14 (ond�rt) g�n i�inde herhangi bir gerek�e g�stermeksizin ve cezai �art �demeksizin s�zle�meden cayma hakk�na sahiptir. Cayma hakk� s�resi, hizmet ifas�na ili�kin s�zle�melerde s�zle�menin kuruldu�u g�n; mal teslimine ili�kin s�zle�melerde ise t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin mal� teslim ald��� g�n ba�lar. Ancak t�ketici, s�zle�menin kurulmas�ndan mal�n teslimine kadar olan s�re i�inde de cayma hakk�n� kullanabilir. Cayma hakk� s�resinin belirlenmesinde; <br>
                    a) Tek sipari� konusu olup ayr� ayr� teslim edilen mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son mal� teslim ald��� g�n, <br>
                    b) Birden fazla par�adan olu�an mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son par�ay� teslim ald��� g�n, <br>
                    c) Belirli bir s�re boyunca mal�n d�zenli tesliminin yap�ld��� s�zle�melerde, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin ilk mal� teslim ald��� g�n esas al�n�r. Cayma bildiriminizi cayma hakk� s�resi dolmadan www.payidar.com.tr�de yer alan ki�isel �yelik sayfan�zdaki iade se�ene�i �zerinden ger�ekle�tirebilirsiniz. Cayma hakk�n�z kapsam�nda �ng�r�len ta��y�c� [KARGO] (herhangi bir �irket/ firma) olup, www.payidar.com.tr�de yer alan ki�isel �yelik sayfan�zdaki kolay iade se�ene�inde geri g�nderime ili�kin detaylar a��klanm��t�r. <br>
                    T�ketici a�a��daki s�zle�melerde cayma hakk�n� kullanamaz: <br>
                    a) Fiyat� finansal piyasalardaki dalgalanmalara ba�l� olarak de�i�en ve SATICI veya sa�lay�c�n�n kontrol�nde olmayan mal veya hizmetlere ili�kin s�zle�meler. <br>
                    b) T�keticinin istekleri veya ki�isel ihtiya�lar� do�rultusunda haz�rlanan mallara ili�kin s�zle�meler. <br>
                    c) �abuk bozulabilen veya son kullanma tarihi ge�ebilecek mallar�n teslimine ili�kin s�zle�meler. <br>
                    �) Tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olan mallardan; iadesi sa�l�k ve hijyen a��s�ndan uygun olmayanlar�n teslimine ili�kin s�zle�meler. <br>
                    d) Tesliminden sonra ba�ka �r�nlerle kar��an ve do�as� gere�i ayr��t�r�lmas� m�mk�n olmayan mallara ili�kin s�zle�meler. <br>
                    e) Mal�n tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olmas� halinde maddi ortamda sunulan kitap, dijital i�erik ve bilgisayar sarf malzemelerine ili�kin s�zle�meler. <br>
                    f) Abonelik s�zle�mesi kapsam�nda sa�lananlar d���nda, gazete ve dergi gibi s�reli yay�nlar�n teslimine ili�kin s�zle�meler. <br>
                    g) Belirli bir tarihte veya d�nemde yap�lmas� gereken, konaklama, e�ya ta��ma, araba kiralama, yiyecek-i�ecek tedariki ve e�lence veya dinlenme amac�yla yap�lan bo� zaman�n de�erlendirilmesine ili�kin s�zle�meler. <br>
                    �) Elektronik ortamda an�nda ifa edilen hizmetler veya t�keticiye an�nda teslim edilen gayrimaddi mallara ili�kin s�zle�meler. <br>
                    h) Cayma hakk� s�resi sona ermeden �nce, t�keticinin onay� ile ifas�na ba�lanan hizmetlere ili�kin s�zle�meler.<br>
                    <br>

                    MADDE 5- GENEL H�K�MLER <br>
                    4.1- ALICI, www.payidar.com.tr internet sitesinde s�zle�me konusu �r�ne ili�kin �n bilgileri okuyup bilgi sahibi oldu�unu ve elektronik ortamda gerekli teyidi verdi�ini beyan eder. <br>
                    4.2- �r�n s�zle�me tarihinden itibaren en ge� 30 g�n i�erisinde teslim edilecektir. �r�n�n�n teslim edilmesi an�na kadar t�m sorumluluk SATICI�ya aittir. <br>
                    4.3- S�zle�me konusu �r�n, ALICI'dan ba�ka bir ki�i/kurulu�a teslim edilecek ise, teslim edilecek ki�i/kurulu�un teslimat� kabul etmemesinden SATICI sorumlu tutulamaz. <br>
                    4.4- SATICI, s�zle�me konusu �r�n�n sa�lam, eksiksiz, sipari�te belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullan�m k�lavuzlar� ile teslim edilmesinden sorumludur. <br>
                    4.5- S�zle�me konusu �r�n�n teslimat� i�in i�bu s�zle�menin bedelinin ALICI'n�n tercih etti�i �deme �ekli ile �denmi� olmas� �artt�r. Herhangi bir nedenle �r�n bedeli �denmez veya banka kay�tlar�nda iptal edilir ise, SATICI �r�n�n teslimi y�k�ml�l���nden kurtulmu� kabul edilir. <br>
                    4.6- �r�n�n tesliminden sonra ALICI'ya ait kredi kart�n�n ALICI'n�n kusurundan kaynaklanmayan bir �ekilde yetkisiz ki�ilerce haks�z veya hukuka ayk�r� olarak kullan�lmas� nedeni ile ilgili banka veya finans kurulu�un �r�n bedelini SATICI'ya �dememesi halinde, ALICI'n�n kendisine teslim edilmi� olmas� kayd�yla �r�n�n SATICI'ya g�nderilmesi zorunludur. <br>
                    4.7- Garanti belgesi ile sat�lan �r�nlerden olan veya olmayan �r�nlerin ay�pl� (ar�zal�, bozuk vb.) halinde veya garanti kapsam�nda ve �artlar� dahilinde ar�zalanmas� veya bozulmas� halinde gerekli onar�m�n yetkili servise yapt�r�lmas� i�in s�z konusu �r�nler SATICI'ya g�nderilebilir.<br>
                    4.8-385 say�l� vergi usul kanunu genel tebli�i uyar�nca iade i�lemlerinin yap�labilmesi i�in taraf�n�za g�ndermi� oldu�umuz iade b�l�m� bulunan faturada ilgili b�l�mlerin eksiksiz olarak doldurulmas� ve imzaland�ktan sonra taraf�m�za �r�n ile birlikte geri g�nderilmesi gerekmektedir. <br>

                    MADDE 8- UYU�MAZLIK VE YETK�L� MAHKEME <br>
                    ��bu s�zle�meden do�an uyu�mazl�klarda do�rudan yerle�im yerinizin bulundu�u veya t�ketici i�leminin yap�ld��� yerdeki T�ketici Sorunlar� Hakem Heyeti veya T�ketici Mahkemesi yetkilidir. <br><br>

                    Sipari�in ger�ekle�mesi durumunda ALICI i�bu s�zle�menin t�m ko�ullar�n� kabul etmi� say�l�r. 
                    <br><br>
                    SATICI: Duha G�m�� Saat/ Payidar.com.tr
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
                    �nvan�: Payidar.com.tr <br>
                    Adresi: Kay��da�� Mh. Akyaz� Cd. No:42/ C Ata�ehir/ �stanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br><br>


                    �N B�LG�LEND�RME FORMU <br>

                    1) S�zle�me konusu mal veya hizmetin ad�, adeti, KDV dahil sat�� fiyat� ve temel nitelikleri <br>

                    [TABLO]
                    <br><br>

                    2) Paketleme, kargo ve teslim masraflar� ALICI taraf�ndan kar��lanmaktad�r. Kargo �creti [KARGOUCRETI] -TL olup, kargo fiyat� sipari� toplam tutar�na eklenmektedir. �r�n bedeline dahil de�ildir. Teslimat, anla�mal� kargo �irketi arac�l��� ile ALICI'n�n yukar�da belirtilen adresinde elden teslim edilecektir. Teslim an�nda ALICI'n�n adresinde bulunmamas� durumunda dahi Firmam�z edimini tam ve eksiksiz olarak yerine getirmi� olarak kabul edilecektir. Bu nedenle, ALICI'n�n �r�n� ge� teslim almas�ndan ve/veya hi� teslim almamas�ndan kaynaklanan zararlardan ve giderlerden SATICI sorumlu de�ildir. SATICI, s�zle�me konusu �r�n�n sa�lam, eksiksiz, sipari�te belirtilen niteliklere uygun ve varsa garanti belgeleri ve kullan�m k�lavuzlar� ile teslim edilmesinden sorumludur.
                    <br>
                    3) �r�n s�zle�me tarihinden itibaren en ge� 30 g�n i�erisinde teslim edilecektir. �r�n�n�n teslim edilmesi an�na kadar t�m sorumluluk SATICI�ya aittir. <br>

                    4) T�ketici (ALICI), 14 (ond�rt) g�n i�inde herhangi bir gerek�e g�stermeksizin ve cezai �art �demeksizin s�zle�meden cayma hakk�na sahiptir. Cayma hakk� s�resi, hizmet ifas�na ili�kin s�zle�melerde s�zle�menin kuruldu�u g�n; mal teslimine ili�kin s�zle�melerde ise t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin mal� teslim ald��� g�n ba�lar. Ancak t�ketici, s�zle�menin kurulmas�ndan mal�n teslimine kadar olan s�re i�inde de cayma hakk�n� kullanabilir. Cayma hakk� s�resinin belirlenmesinde; <br>
                    a) Tek sipari� konusu olup ayr� ayr� teslim edilen mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son mal� teslim ald��� g�n, <br>
                    b) Birden fazla par�adan olu�an mallarda, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin son par�ay� teslim ald��� g�n, <br>
                    c) Belirli bir s�re boyunca mal�n d�zenli tesliminin yap�ld��� s�zle�melerde, t�keticinin veya t�ketici taraf�ndan belirlenen ���nc� ki�inin ilk mal� teslim ald��� g�n esas al�n�r. Cayma bildiriminizi cayma hakk� s�resi dolmadan www.payidar.com.tr�de yer alan ki�isel �yelik sayfan�zdaki kolay iade se�ene�i �zerinden ger�ekle�tirebilirsiniz. Cayma hakk�n�z kapsam�nda �ng�r�len ta��y�c� [KARGO] (veya herhangi bir �irket/ firma) olup, www.payidar.com.tr�de yer alan ki�isel �yelik sayfan�zdaki kolay iade se�ene�inde geri g�nderime ili�kin detaylar a��klanm��t�r. <br>
                    T�ketici a�a��daki s�zle�melerde cayma hakk�n� kullanamaz: <br>
                    a) Fiyat� finansal piyasalardaki dalgalanmalara ba�l� olarak de�i�en ve SATICI veya sa�lay�c�n�n kontrol�nde olmayan mal veya hizmetlere ili�kin s�zle�meler. <br>
                    b) T�keticinin istekleri veya ki�isel ihtiya�lar� do�rultusunda haz�rlanan mallara ili�kin s�zle�meler. <br>
                    c) �abuk bozulabilen veya son kullanma tarihi ge�ebilecek mallar�n teslimine ili�kin s�zle�meler. <br>
                    �) Tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olan mallardan; iadesi sa�l�k ve hijyen a��s�ndan uygun olmayanlar�n teslimine ili�kin s�zle�meler. <br>
                    d) Tesliminden sonra ba�ka �r�nlerle kar��an ve do�as� gere�i ayr��t�r�lmas� m�mk�n olmayan mallara ili�kin s�zle�meler. <br>
                    e) Mal�n tesliminden sonra ambalaj, bant, m�h�r, paket gibi koruyucu unsurlar� a��lm�� olmas� halinde maddi ortamda sunulan kitap, dijital i�erik ve bilgisayar sarf malzemelerine ili�kin s�zle�meler. <br>
                    f) Abonelik s�zle�mesi kapsam�nda sa�lananlar d���nda, gazete ve dergi gibi s�reli yay�nlar�n teslimine ili�kin s�zle�meler. <br>
                    g) Belirli bir tarihte veya d�nemde yap�lmas� gereken, konaklama, e�ya ta��ma, araba kiralama, yiyecek-i�ecek tedariki ve e�lence veya dinlenme amac�yla yap�lan bo� zaman�n de�erlendirilmesine ili�kin s�zle�meler. <br>
                    �) Elektronik ortamda an�nda ifa edilen hizmetler veya t�keticiye an�nda teslim edilen gayrimaddi mallara ili�kin s�zle�meler. <br>
                    h) Cayma hakk� s�resi sona ermeden �nce, t�keticinin onay� ile ifas�na ba�lanan hizmetlere ili�kin s�zle�meler. <br>
                    5) T�keticinin herhangi bir dijital i�erik sat�n almas� halinde dijital i�eriklerin i�levselli�ini etkileyecek teknik koruma �nlemleri ve SATICI�n�n bildi�i ya da makul olarak bilmesinin beklendi�i, dijital i�eri�in hangi donan�m ya da yaz�l�mla birlikte �al��abilece�ine ili�kin bilgiler sat�n al�nan �r�n�n www.payidar.com.tr�de sat��a sunuldu�u sayfadaki tan�t�m i�eri�inde yer almaktad�r. <br>


                    6) T�keticilerin �ikayet ve itirazlar�: Sipari�inize ve/veya sipari�inize konu �r�ne ve/veya �ipari�inizle ilgili herhangi bir konuda �ikayetinizin olmas� halinde �ikayetlerinizi yukar�da belirtilen ileti�im bilgileri veya www.payidar.com.tr internet sitesinde belirtilen ileti�im bilgileri vas�tas�yla SATICI�ya iletebilirsiniz. �letmi� oldu�unuz �ik�yet ba�vurular�n�z derhal kay�tlara al�nacak, yetkili birimler taraf�ndan de�erlendirilerek ��z�mlenmeye �al���lacak ve en k�sa s�rede size geri d�n�� sa�lanacakt�r. Ayr�ca, �ik�yet ba�vurular�n�z� do�rudan yerle�im yerinizin bulundu�u veya t�ketici i�leminin yap�ld��� yerdeki T�ketici Sorunlar� Hakem Heyetine veya T�ketici Mahkemesine yapabilirsiniz.<br>
                    <br>

                    SATICI: <br>
                    �nvan�: Duha G�m�� Saat/ Payidar.com.tr<br>
                    Adresi: Kay��da�� Mh. Akyaz� Cd. No:42/ C Ata�ehir/ �stanbul <br>
                    Telefon: 0545 229 75 29 <br>
                    Email: info@payidar.com.tr <br><br>

                    ALICI: <br>
                    Ad�/ Soyad�/ �nvan�: [ALICI] <br>
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
