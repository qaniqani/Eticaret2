namespace AdminProject.Services.Models
{
    public class TotalSum
    {
        public decimal SubTotalAmount { get; set; }
        public decimal CargoAmount { get; set; }
        public decimal KdvTotalAmount { get; set; }
        public decimal KdvOdd { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OrginalTotalAmount { get; set; }
    }
}