using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class ProductSearchDto
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public StatusTypes Status { get; set; }
        public StockTypes StockType { get; set; }
        public ProductTypes ProductType { get; set; }
    }
}