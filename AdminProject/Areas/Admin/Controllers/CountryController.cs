using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;

        public CountryController(ICountryService countryService, RuntimeSettings setting, ICityService cityService, IRegionService regionService) : base(setting)
        {
            _countryService = countryService;
            _cityService = cityService;
            _regionService = regionService;
        }

        //Country Actions

        [HttpGet]
        public ActionResult AddCountry()
        {
            SetPageHeader("Country", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult AddCountry(Country request)
        {
            SetPageHeader("Country", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            var country = new Country
            {
                Name = request.Name,
                SequenceNr = request.SequenceNr,
                Status = request.Status
            };

            _countryService.Add(country);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult EditCountry(int id)
        {
            SetPageHeader("Country", "Edit");

            var country = _countryService.GetCountry(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(country.Status);

            return View(country);
        }

        [HttpPost]
        public ActionResult EditCountry(Country request)
        {
            SetPageHeader("Country", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            var country = _countryService.GetCountry(request.Id);
            country.Name = request.Name;
            country.Status = request.Status;
            country.SequenceNr = request.SequenceNr;
            _countryService.Edit(request.Id, country);

            Updated();

            return RedirectToAction("ListCountry");
        }

        [HttpGet]
        public ActionResult ListCountry()
        {
            SetPageHeader("Country", "List");

            var countries = _countryService.AllCountryList();

            return View(countries);
        }

        [HttpGet]
        public ActionResult DeleteCountry(int id)
        {
            _countryService.Delete(id);
            Deleted();
            return RedirectToAction("ListCountry");
        }

        //City Actions

        [HttpGet]
        public ActionResult AddCity()
        {
            SetPageHeader("City", "Add");

            ViewBag.CountryList = _countryService.GetCountrySelectList("TURKIYE");
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult AddCity(City request)
        {
            SetPageHeader("City", "Add");

            ViewBag.CountryList = _countryService.GetCountrySelectList("TÜRKİYE");
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId,
                SequenceNr = request.SequenceNr,
                Status = request.Status
            };

            _cityService.Add(city);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult EditCity(int id)
        {
            SetPageHeader("City", "Edit");

            var city = _cityService.GetCity(id);

            ViewBag.CountryList = _countryService.GetCountrySelectList(city.CountryId.ToString());
            ViewBag.StatusList = DropdownTypes.GetStatus(city.Status);

            return View(city);
        }

        [HttpPost]
        public ActionResult EditCity(City request)
        {
            SetPageHeader("City", "Edit");

            var city = _cityService.GetCity(request.Id);

            ViewBag.CountryList = _countryService.GetCountrySelectList(city.CountryId.ToString());
            ViewBag.StatusList = DropdownTypes.GetStatus(city.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            city.Name = request.Name;
            city.CountryId = request.CountryId;
            city.SequenceNr = request.SequenceNr;
            city.Status = request.Status;

            _cityService.Edit(request.Id, city);

            Updated();

            return RedirectToAction("ListCity");
        }

        [HttpGet]
        public ActionResult ListCity(string countryId = "0")
        {
            SetPageHeader("City", "List");

            var countries = _countryService.ActiveCountryList();
            ViewBag.Citys = _cityService.AllCityList(Convert.ToInt32(countryId));

            return View(countries);
        }

        [HttpGet]
        public ActionResult DeleteCity(int id)
        {
            _cityService.Delete(id);
            Deleted();
            return RedirectToAction("ListCity");
        }

        //Region Actions

        [HttpGet]
        public ActionResult AddRegion(string countryId = "212")
        {
            SetPageHeader("Region", "Add");

            ViewBag.CountryList = _countryService.GetCountrySelectList(countryId);
            ViewBag.CityList = _cityService.GetCitySelectList(countryId.ToInt32(), "");
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult AddRegion(Region request, string countryId = "212")
        {
            SetPageHeader("Region", "Add");

            ViewBag.CountryList = _countryService.GetCountrySelectList(countryId);
            ViewBag.CityList = _cityService.GetCitySelectList(countryId.ToInt32(), request.CityId.ToString());
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            var city = _cityService.GetCity(request.CityId);

            var region = new Region
            {
                Name = request.Name,
                CityId = city.Id,
                CityName = city.Name,
                SequenceNr = request.SequenceNr,
                Status = request.Status
            };

            _regionService.Add(region);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult EditRegion(int id, string countryId)
        {
            SetPageHeader("Region", "Edit");

            var region = _regionService.GetRegion(id);
            ViewBag.StatusList = DropdownTypes.GetStatus(region.Status);

            if (!string.IsNullOrEmpty(countryId))
            {
                ViewBag.CountryList = _countryService.GetCountrySelectList(countryId);
                ViewBag.CityList = _cityService.GetCitySelectList(countryId.ToInt32(), "");
                return View(region);
            }

            var city = _cityService.GetCity(region.CityId);

            ViewBag.CountryList = _countryService.GetCountrySelectList(city.CountryId.ToString());
            ViewBag.CityList = _cityService.GetCitySelectList(city.CountryId, city.Id.ToString());

            return View(region);
        }

        [HttpPost]
        public ActionResult EditRegion(Region request, string countryId = "212")
        {
            SetPageHeader("Region", "Edit");

            var city = _cityService.GetCity(request.CityId);

            ViewBag.CountryList = _countryService.GetCountrySelectList(city.CountryId.ToString());
            ViewBag.CityList = _cityService.GetCitySelectList(countryId.ToInt32(), request.CityId.ToString());
            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            var region = new Region
            {
                Name = request.Name,
                CityId = city.Id,
                CityName = city.Name,
                SequenceNr = request.SequenceNr,
                Status = request.Status
            };

            _regionService.Edit(request.Id, region);

            Updated();

            return RedirectToAction("ListRegion");
        }

        [HttpGet]
        public ActionResult ListRegion(string countryId = "0", string cityId = "0")
        {
            SetPageHeader("Region", "List");

            var countries = _countryService.ActiveCountryList();
            ViewBag.Citys = _cityService.AllCityList(countryId.ToInt32());
            ViewBag.Regions = _regionService.AllRegionList(cityId.ToInt32());

            return View(countries);
        }

        [HttpGet]
        public ActionResult DeleteRegion(int id)
        {
            _regionService.Delete(id);
            Deleted();
            return RedirectToAction("ListRegion");
        }
    }
}