namespace AdminProject.Models
{
    public class ProductViewDto
    {
        public int Id { get; set; }
        public StockTypes StockType { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string MinPicture { get; set; }
        public string BigPicture { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountOdd { get; set; }
        public bool IsDiscountApplied { get; set; }
        public int Hit { get; set; }
        public string Url { get; set; }
    }
}