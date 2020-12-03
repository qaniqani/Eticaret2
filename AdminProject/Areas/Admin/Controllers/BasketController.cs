using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Helpers;
using AdminProject.Services.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(RuntimeSettings setting, IBasketService basketService) : base(setting)
        {
            _basketService = basketService;
        }

        public ActionResult List(string Name, string Surname, string StartDate, string EndDate)
        {
            SetPageHeader("Basket", "List");

            var request = new BasketSearchDto
            {
                Name = Name,
                Surname = Surname
            };

            DateTime sDate = DateTime.Now.AddDays(-31), eDate = DateTime.Now.AddDays(1);

            ViewBag.Baskets = new List<ProductDto>();

            if (!string.IsNullOrEmpty(StartDate))
                if (!Utility.DateTimeParsing(StartDate, out sDate))
                    ModelState.AddModelError("StartDate", "Start date format error.");

            if (!string.IsNullOrEmpty(EndDate))
                if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End date format error.");

            if (!ModelState.IsValid)
                return View(request);

            request.StartDate = sDate;
            request.EndDate = eDate;

            var result = _basketService.GetUserBasketList(request);
            ViewBag.Baskets = result;

            return View(request);
        }

        public ActionResult ChangeUnit(int id, string unit, string Name, string Surname, string StartDate, string EndDate)
        {
            if (string.IsNullOrEmpty(unit))
                return RedirectToAction("List", new { Name, Surname, StartDate, EndDate });

            int u;
            if (int.TryParse(unit, out u))
                _basketService.EditUnitChange(id, u);

            return RedirectToAction("List", new { Name, Surname, StartDate, EndDate });
        }

        public ActionResult Delete(int id)
        {
            _basketService.Delete(id);

            return RedirectToAction("List");
        }

        //public ActionResult View(string startDate, string endDate)
        //{


        //    return View();
        //}
    }
}