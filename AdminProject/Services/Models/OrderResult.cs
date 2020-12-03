using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class OrderResult
    {
        public Order OrderDetail { get; set; }
        public Invoice InvoiceDetail { get; set; }
        public Cargo CargoDetail { get; set; }
        public Address AddressDetail { get; set; }
        public User UserDetail { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public TotalSum TotalSum { get; set; }
        public CampaignSumCalculate CampaignSumCalculate { get; set; }
    }
}