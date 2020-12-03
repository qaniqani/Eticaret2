namespace AdminProject.Models
{
    public class BasketItemDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string MinPicture { get; set; }
        public string BigPicture { get; set; }
        public string Url { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TotalAmount { get; set; }
        //public decimal TotalDiscountedAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OrginalTotalAmount { get; set; }
    }
}