using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class OrderProductAssg
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Unit { get; set; }
        public int KdvOdd { get; set; }
        public decimal Price { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsKdv { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal KdvAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OrginalTotalAmount { get; set; }
        public decimal CargoAmount { get; set; }
        public bool IsCampaignApplied { get; set; }
        public string OtherDetail { get; set; }
        public string CauseOfRefund { get; set; }
        public OrderTypes OrderType { get; set; }
    }
}