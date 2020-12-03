using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public StockTypes StockType { get; set; }
        public ProductTypes ProductType { get; set; }
        public ProductGroupTypes GroupType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoDescription { get; set; }
        public decimal Price { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal KdvOdd { get; set; }
        public bool IsKdv { get; set; }
        public decimal StockNr { get; set; }
        public string Url { get; set; }
        public string Properties { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public StatusTypes Status { get; set; }
        public int SingleHit { get; set; }
        public int PluralHit { get; set; }
    }
}