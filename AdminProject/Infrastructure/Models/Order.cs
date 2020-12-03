using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ParentOrderId { get; set; } //Siparis gecmisini tutmak icin
        public int UserId { get; set; }
        public int InvoiceId { get; set; }
        public int AddressId { get; set; }
        public string OrderNr { get; set; } //musteriye verilecek siparis no
        public int CargoId { get; set; }
        public string CargoNr { get; set; } //cargo numarasi
        public string Description { get; set; } //iade aciklamasi
        public PayTypes PayType { get; set; }
        public OrderTypes OrderType { get; set; }
        public string OrderNote { get; set; } //siparis notu
        public string OtherDetail { get; set; } //sepetteki other detail alani. json olacak.
        public string CauseOfRefund { get; set; } //iade sebebi
        public decimal KdvAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsCampaignApplied { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateUserId { get; set; }
        public int CreateUserId { get; set; }
    }
}