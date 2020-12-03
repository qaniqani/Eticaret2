using System;
using System.Globalization;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class CampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(RuntimeSettings setting, ICampaignService campaignService) : base(setting)
        {
            _campaignService = campaignService;
        }

        public ActionResult Add()
        {
            SetPageHeader("Campaign", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.CampaignList = DropdownTypes.GetCampaignType(CampaignType.GeneralDiscount);

            var campaign = new Campaign
            {
                Code = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CampaignType = CampaignType.GeneralDiscount,
                Status = StatusTypes.Active
            };

            return View(campaign);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string Name, string Detail, string DiscountOdd, string DiscountLimit, string DiscountAmountCriterion, string StartDate, string EndDate, StatusTypes Status, CampaignType CampaignType)
        {
            SetPageHeader("Campaign", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.CampaignList = DropdownTypes.GetCampaignType(CampaignType);

            var campaign = new Campaign
            {
                Code = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CampaignType = CampaignType.GeneralDiscount,
                Status = StatusTypes.Active
            };

            #region //checking
            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(DiscountAmountCriterion))
                ModelState.AddModelError("DiscountAmountCriterion", "Discount amount criterion is required.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (string.IsNullOrEmpty(DiscountOdd))
                ModelState.AddModelError("DiscountOdd", "Discount odd is required.");

            if (string.IsNullOrEmpty(DiscountLimit))
                ModelState.AddModelError("DiscountLimit", "Discount limit is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndName", "End name is required.");

            decimal odd, amountCriterion, limit;
            var ci = new CultureInfo("tr-TR");

            if (!decimal.TryParse(DiscountOdd, NumberStyles.AllowCurrencySymbol, ci, out odd))
                ModelState.AddModelError("DiscountOddFormat", "Discount odd is format error.");

            if (!decimal.TryParse(DiscountAmountCriterion, NumberStyles.AllowCurrencySymbol, ci, out amountCriterion))
                ModelState.AddModelError("DiscountAmountCriterionFormat", "Discount amount criterion is format error.");

            if (!decimal.TryParse(DiscountLimit, NumberStyles.AllowCurrencySymbol, ci, out limit))
                ModelState.AddModelError("DiscountLimitFormat", "Discount limit is format error.");

            DateTime sDate, eDate;
            if(!Utility.DateTimeParsing(StartDate, out sDate))
                ModelState.AddModelError("StartDate", "Start date is format error.");

            if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End date is format error.");

            if (!ModelState.IsValid)
                return View(campaign);
            #endregion

            var campaing = new Campaign
            {
                CampaignType = CampaignType,
                Code = Guid.NewGuid().ToString(),
                Detail = Detail,
                DiscountAmountCriterion = amountCriterion,
                DiscountLimit = limit,
                DiscountOdd = odd,
                EndDate = eDate,
                Name = Name,
                StartDate = sDate,
                Status = Status
            };

            _campaignService.Add(campaing);

            Added();

            return View(campaign);
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("Campaign", "Edit");

            var campaign = _campaignService.GetCampaign(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(campaign.Status);
            ViewBag.CampaignList = DropdownTypes.GetCampaignType(campaign.CampaignType);

            return View(campaign);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string Name, string Detail, string DiscountOdd, string DiscountLimit, string DiscountAmountCriterion, string StartDate, string EndDate, StatusTypes Status, CampaignType CampaignType)
        {
            SetPageHeader("Campaign", "Edit");

            var campaign = _campaignService.GetCampaign(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.CampaignList = DropdownTypes.GetCampaignType(CampaignType);

            #region //checking
            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(DiscountAmountCriterion))
                ModelState.AddModelError("DiscountAmountCriterion", "Discount amount criterion is required.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (string.IsNullOrEmpty(DiscountOdd))
                ModelState.AddModelError("DiscountOdd", "Discount odd is required.");

            if (string.IsNullOrEmpty(DiscountLimit))
                ModelState.AddModelError("DiscountLimit", "Discount limit is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndName", "End name is required.");

            decimal odd, amountCriterion, limit;
            var ci = new CultureInfo("tr-TR");

            if (!decimal.TryParse(DiscountOdd, NumberStyles.AllowCurrencySymbol, ci, out odd))
                ModelState.AddModelError("DiscountOddFormat", "Discount odd is format error.");

            if (!decimal.TryParse(DiscountAmountCriterion, NumberStyles.AllowCurrencySymbol, ci, out amountCriterion))
                ModelState.AddModelError("DiscountAmountCriterionFormat", "Discount amount criterion is format error.");

            if (!decimal.TryParse(DiscountLimit, NumberStyles.AllowCurrencySymbol, ci, out limit))
                ModelState.AddModelError("DiscountLimitFormat", "Discount limit is format error.");

            DateTime sDate, eDate;
            if (!Utility.DateTimeParsing(StartDate, out sDate))
                ModelState.AddModelError("StartDate", "Start date is format error.");

            if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End date is format error.");

            if (!ModelState.IsValid)
                return View();
            #endregion

            campaign.CampaignType = CampaignType;
            campaign.Detail = Detail;
            campaign.DiscountAmountCriterion = amountCriterion;
            campaign.DiscountLimit = limit;
            campaign.DiscountOdd = odd;
            campaign.EndDate = eDate;
            campaign.Name = Name;
            campaign.StartDate = sDate;
            campaign.Status = Status;

            _campaignService.Edit(id, campaign);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Campaign", "List");

            var campaigns = _campaignService.AllCampaignList();

            return View(campaigns);
        }

        public ActionResult Delete(int id)
        {
            _campaignService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}