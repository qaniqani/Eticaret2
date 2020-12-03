using System;
using System.Collections.Generic;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class ProductDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public string MinPicture { get; set; }
        public string BigPicture { get; set; }
        public string StockType { get; set; }
        public string ProductType { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public string CauseOfRefund { get; set; }
        public int Unit { get; set; }
        public decimal KdvOdd { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscountPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsKdv { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal KdvAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        //public decimal TotalDiscountedAmount { get; set; }
        public decimal OrginalTotalAmount { get; set; }
        public bool IsCampaignApplied { get; set; }
        public OrderTypes OrderType { get; set; }
        public DateTime DateTime { get; set; }
        public List<ProductDetail> Detail { get; set; }
    }

    public class ProductDetail
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}