using System;
using System.Collections.Generic;
using AdminProject.Services.Models;

namespace AdminProject.Models
{
    public class ProductDetailViewDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public StockTypes StockType { get; set; }
        public string StockTypeText { get; set; }
        public ProductTypes ProductType { get; set; }
        public string ProductTypeText { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoDescription { get; set; }
        public bool IsKdv { get; set; }
        public decimal KdvOdd { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal KdvAmount { get; set; }
        public bool IsDiscountApplied { get; set; }
        public DateTime DateTime { get; set; }
        public List<ProductPropertyModel> Properties { get; set; }
    }
}