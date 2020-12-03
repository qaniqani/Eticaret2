using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class CampaignSumCalculate
    {
        public CampaignSumCalculate()
        {
            DiscountItems = new List<DiscountItem>();
        }
        public string CampaingName { get; set; }
        public decimal DiscountOdd { get; set; }
        public List<DiscountItem> DiscountItems { get; set; }
        public decimal DiscountTotalAmount { get; set; }
    }

    public class DiscountItem
    {
        public CampaignType CampaignType { get; set; }
        public string Name { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}