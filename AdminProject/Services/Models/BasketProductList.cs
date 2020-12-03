using System.Collections.Generic;

namespace AdminProject.Services.Models
{
    public class BasketProductList
    {
        public IEnumerable<ProductDto> ProductList { get; set; }
        public TotalSum BasketTotalSum { get; set; }
        public CampaignSumCalculate BasketCampigns { get; set; }
    }
}