using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface ICampaignService
    {
        void Add(Campaign campaign);
        void Edit(int id, Campaign campaignRequest);
        Campaign GetCampaign(int id);
        void Delete(int id);
        List<Campaign> AllCampaignList();
        List<Campaign> ActiveCampaignList();
        List<Campaign> AvailableCampaignList();
        CampaignSumCalculate CampaignCheck(Cargo cargo, TotalSum totalSum);
    }
}