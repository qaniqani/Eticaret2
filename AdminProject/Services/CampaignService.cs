using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CampaignService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Campaign campaign)
        {
            var db = _dbFactory();
            db.Campaign.Add(campaign);
            db.SaveChanges();
        }

        public void Edit(int id, Campaign campaignRequest)
        {
            var db = _dbFactory();
            var campaign = db.Campaign.FirstOrDefault(a => a.Id == id);
            campaign.CampaignType = campaignRequest.CampaignType;
            campaign.Code = campaignRequest.Code;
            campaign.Detail = campaignRequest.Detail;
            campaign.DiscountAmountCriterion = campaignRequest.DiscountAmountCriterion;
            campaign.DiscountLimit = campaignRequest.DiscountLimit;
            campaign.DiscountOdd = campaignRequest.DiscountOdd;
            campaign.EndDate = campaignRequest.EndDate;
            campaign.Name = campaignRequest.Name;
            campaign.StartDate = campaignRequest.StartDate;
            campaign.Status = campaignRequest.Status;
            db.SaveChanges();
        }

        public Campaign GetCampaign(int id)
        {
            var db = _dbFactory();
            var campaign = db.Campaign.FirstOrDefault(a => a.Id == id);
            return campaign;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var campaign = db.Campaign.FirstOrDefault(a => a.Id == id);
            db.Campaign.Remove(campaign);
            db.SaveChanges();
        }

        public List<Campaign> AllCampaignList()
        {
            var db = _dbFactory();
            var list = db.Campaign.OrderBy(a => a.Name).ToList();
            return list;
        }

        public List<Campaign> ActiveCampaignList()
        {
            var db = _dbFactory();
            var list = db.Campaign.Where(a => a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.Name).ToList();
            return list;
        }

        public List<Campaign> AvailableCampaignList()
        {
            var db = _dbFactory();

            var list = db.Campaign.Where(a => 
                a.StartDate < DateTime.Now 
                && a.EndDate > DateTime.Now &&
                a.Status == AdminProject.Models.StatusTypes.Active
            ).OrderBy(a => a.Name).ToList();

            return list;
        }

        public CampaignSumCalculate CampaignCheck(Cargo cargo, TotalSum totalSum)
        {
            //kampanya varmi?
            var campaign = AvailableCampaignList();

            var totalCampaign = new CampaignSumCalculate();
            campaign.ForEach(a =>
            {
                if (a.CampaignType == CampaignType.Cargo
                    && a.DiscountAmountCriterion <= totalSum.TotalAmount)
                {
                    if (totalSum.TotalAmount >= a.DiscountAmountCriterion)
                    {
                        totalCampaign.CampaingName = string.IsNullOrEmpty(totalCampaign.CampaingName) ? totalCampaign.CampaingName += a.Name : totalCampaign.CampaingName += " - " + a.Name;

                        totalCampaign.DiscountItems.Add(new DiscountItem
                        {
                            Name = a.Name,
                            DiscountAmount = totalSum.CargoAmount,
                            CampaignType = CampaignType.Cargo
                        });

                        totalCampaign.DiscountTotalAmount += cargo.Price;
                        //totalSum.TotalAmount -= cargo.Price;
                        totalSum.CargoAmount = 0;
                    }
                }

                if (a.CampaignType == CampaignType.GeneralDiscount
                && a.DiscountAmountCriterion <= totalSum.TotalAmount)
                {
                    var discountAmount = totalSum.TotalAmount / 100 * a.DiscountOdd;
                    if (discountAmount >= a.DiscountLimit) // DiscountLimit gereginden fazla indirim yapmayi onler.
                        discountAmount = a.DiscountLimit;

                    totalCampaign.DiscountItems.Add(new DiscountItem
                    {
                        Name = a.Name,
                        DiscountAmount = discountAmount,
                        CampaignType = CampaignType.GeneralDiscount
                    });

                    totalCampaign.DiscountOdd += discountAmount / (totalSum.TotalAmount / 100); //indirim oranini hesapla

                    //kampanyanin adini yaz
                    totalCampaign.CampaingName = string.IsNullOrEmpty(totalCampaign.CampaingName) ? totalCampaign.CampaingName += a.Name : totalCampaign.CampaingName += " - " + a.Name;

                    //indirim miktarini toplama ekle
                    totalCampaign.DiscountTotalAmount += discountAmount;
                }
            });

            if (totalCampaign.DiscountOdd > 0)
                totalSum.TotalAmount = totalSum.TotalAmount - (totalSum.TotalAmount / 100 * totalCampaign.DiscountOdd); //indirimi uygula

            return totalCampaign;
        }
    }
}