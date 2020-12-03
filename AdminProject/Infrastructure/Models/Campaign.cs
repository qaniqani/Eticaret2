using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public CampaignType CampaignType { get; set; }
        public string Code { get; set; } //Guid
        public string Name { get; set; }
        public string Detail { get; set; }
        public decimal DiscountOdd { get; set; }
        public decimal DiscountLimit { get; set; }
        public decimal DiscountAmountCriterion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusTypes Status { get; set; }
    }

    public enum CampaignType
    {
        GeneralDiscount = 0,
        Cargo = 1
    }
}