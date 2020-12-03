using System.Collections.Generic;
using AdminProject.Services.Models;
using Newtonsoft.Json;

namespace AdminProject.Helpers
{
    public class Tool
    {
        public static string ProductDetailSerialization(List<ProductDetail> detailList)
        {
            return JsonConvert.SerializeObject(detailList);
        }

        public static List<ProductDetail> ProductDetailDeserialization(string detailList)
        {
            return JsonConvert.DeserializeObject<List<ProductDetail>>(detailList);
        }

        public static readonly Dictionary<string, string> GetCreditCardErrorDescription = new Dictionary<string, string>
        {
            {"NOT_SUFFICIENT_FUNDS", "Kart limiti yetersiz, yetersiz bakiye"},
            {"DO_NOT_HONOUR", "İşlem onaylanmadı"},
            {"INVALID_TRANSACTION", "Geçersiz işlem"},
            {"LOST_CARD", "Kayıp kart, karta el koyunuz"},
            {"STOLEN_CARD", "Çalıntı kart, karta el koyunuz"},
            {"EXPIRED_CARD", "Vadesi dolmuş kart"},
            {"INVALID_CVC2", "CVC2 bilgisi hatalı"},
            {"NOT_PERMITTED_TO_CARDHOLDER", "Kart sahibi bu işlemi yapamaz"},
            {"NOT_PERMITTED_TO_TERMINAL", "Terminalin bu işlemi yapmaya yetkisi yok"},
            {"FRAUD_SUSPECT", "Dolandırıcılık şüphesi"},
            {"PICKUP_CARD", "Karta el koy"},
            {"CARD_NOT_PERMITTED", "Kart, işleme izin vermedi"},
            {"UNKNOWN", "Ödeme işlemi esnasında genel bir hata oluştu"},
            {"APPROVED_COMPLETED", "Önceden onaylanan işlem"},
            {"INVALID_CHARS_IN_EMAIL", "E-posta geçerli formata değil"},
            {"INVALID_CVC2_LENGTH", "CVC uzunluğu geçersiz"},
            {"REFER_TO_CARD_ISSUER", "Bankanızdan onay alınız"},
            {"INVALID_MERCHANT_OR_SP", "Üye işyeri kategori kodu hatalı"},
            {"BLOCKED_CARD", "Bloke statülü kart"},
            {"INVALID_CAVV", "Hatalı CAVV bilgisi"},
            {"INVALID_ECI", "Hatalı ECI bilgisi"},
            {"CVC2_MAX_ATTEMPT", "CVC2 yanlış girme deneme sayısı aşıldı"},
            {"BIN_NOT_FOUND", "BIN bulunamadı"},
            {"COMMUNICATION_OR_SYSTEM_ERROR", "İletişim veya sistem hatası"},
            {"INVALID_CARD_NUMBER", "Geçersiz kart numarası"},
            {"NO_SUCH_ISSUER", "Bankası bulunamadı"},
            {"DEBIT_CARDS_REQUIRES_3DS", "Banka kartları sadece 3D Secure işleminde kullanılabilir"},
            {"DEBIT_CARDS_INSTALLMENT_NOT_ALLOWED", "Banka kartları ile taksit yapılamaz"},
            {"REQUEST_TIMEOUT", "Bankaya gönderilen istek zaman aşımına uğradı"},
            {"DECLINED", "Ödeme alınamadı"},
            {"NOT_PERMITTED_TO_FOREIGN_CARD", "Terminal yurtdışı kartlara kapalı"},
            {"NOT_PERMITTED_TO_INSTALLMENT", "Terminal taksitli işleme kapalı"},
            {"REQUIRES_DAY_END", "Gün sonu yapılmalı"},
            {"EXCEEDS_WITHDRAWAL_AMOUNT_LIMIT", "Para çekme limiti aşılmış"},
            {"RESTRICTED_CARD", "Kısıtlı kart"},
            {"EXCEEDS_ALLOWABLE_PIN_TRIES", "İzin verilen PIN giriş sayısı aşılmış"},
            {"INVALID_PIN", "Geçersiz PIN"},
            {"ISSUER_OR_SWITCH_INOPERATIVE", "Banka veya terminal işlem yapamıyor"},
            {"INVALID_EXPIRE_YEAR_MONTH", "Son kullanma tarihi geçersiz"},
            {"REQUEST_BLOCKED_BY_BANK", "İstek bankadan hata aldı"},
            {"SALES_AMOUNT_LESS_THAN_AWARD", "Satış tutarı ödül puanından düşük olamaz"},
            {"INVALID_AMOUNT", "Geçersiz tutar"},
            {"INVALID_CARD_TYPE", "Geçersiz kart tipi"},
            {"NOT_SUFFICIENT_AWARD", "Yetersiz ödül puanı"},
            {"AMEX_CAN_USE_ONLY_MR", "American Express kart hatası"}
        };

        public static readonly Dictionary<string, string> GetPaymentErrorDescription = new Dictionary<string, string>
        {
            {"FAILURE", "Başarısız, geçersiz işlem."},
            {"INIT_THREEDS", "Bağlantı sağlanamadı."},
            {"CALLBACK_THREEDS", "Çağrı boyut işlem aşıldı."},
            {"BKM_POS_SELECTED", "BKM Pos seçilmiş."},
            {"CALLBACK_PECCO", "Ödeme Pecco ile başlatıldı."}
        };
    }
}