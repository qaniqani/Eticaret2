using System.Collections.Generic;
using AdminProject.Services.Models;

namespace AdminProject.Models
{
    public class BasketListDto
    {
        public List<BasketItemDto> ProductList { get; set; }
        public TotalSum BasketTotalSum { get; set; }
        public CampaignSumCalculate BasketCampigns { get; set; }
    }
}