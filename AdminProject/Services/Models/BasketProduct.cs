using System;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class BasketProduct
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public StockTypes StockType { get; set; }
        public ProductTypes ProductType { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string ProductName { get; set; }
        public DateTime DateTime { get; set; }
        public int Unit { get; set; }
        public decimal KdvOdd { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsKdv { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal KdvAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OrginalTotalAmount { get; set; }
        public string Detail { get; set; }
    }
}