using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;

        public UserController(IUserService userService, ICountryService countryService, ICityService cityService, IRegionService regionService, RuntimeSettings setting) : base(setting)
        {
            _countryService = countryService;
            _cityService = cityService;
            _regionService = regionService;
            _userService = userService;
        }

        public ActionResult List(string Name, string Surname, string Email, string TcNr, string CountryName, string CityName, string RegionName, UserTypes Status = UserTypes.Active, int Take = 20, int Skip = 1)
        {
            SetPageHeader("User", "List");

            ViewBag.UserTypes = DropdownTypes.GetUserType(Status);
            ViewBag.TakeList = DropdownTypes.TakeCount(Take);

            ViewBag.CountryList = _countryService.GetCountrySelectList("0");

            var request = new UserSearchRequestDto
            {
                CityName = CityName,
                CountryName = CountryName,
                RegionName = RegionName,
                Email = Email,
                Name = Name,
                Skip = Skip,
                Status = Status,
                Surname = Surname,
                Take = Take,
                TcNr = TcNr
            };

            var result = _userService.AllUserList(request);
            ViewBag.Users = result;

            return View(request);
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("User", "Edit");

            var result = _userService.GetUser(id);

            ViewBag.UserTypes = DropdownTypes.GetUserType(result.Status);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(User requestUser)
        {
            SetPageHeader("User", "Edit");

            ViewBag.UserTypes = DropdownTypes.GetUserType(requestUser.Status);

            if (string.IsNullOrEmpty(requestUser.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(requestUser.Surname))
                ModelState.AddModelError("Surname", "Surname is required.");

            if (string.IsNullOrEmpty(requestUser.Address))
                ModelState.AddModelError("Address", "Address is required.");

            if (string.IsNullOrEmpty(requestUser.City))
                ModelState.AddModelError("City", "City is required.");

            if (string.IsNullOrEmpty(requestUser.Country))
                ModelState.AddModelError("Country", "Country is required.");

            if (string.IsNullOrEmpty(requestUser.Email))
                ModelState.AddModelError("Email", "Email is required.");

            if (string.IsNullOrEmpty(requestUser.Gender))
                ModelState.AddModelError("Gender", "Gender is required.");

            if (string.IsNullOrEmpty(requestUser.Gsm))
                ModelState.AddModelError("Gsm", "Gsm is required.");

            if (string.IsNullOrEmpty(requestUser.Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (string.IsNullOrEmpty(requestUser.Phone))
                ModelState.AddModelError("Phone", "Phone is required.");

            if (string.IsNullOrEmpty(requestUser.Region))
                ModelState.AddModelError("Region", "Region is required.");

            if (string.IsNullOrEmpty(requestUser.TcNr))
                ModelState.AddModelError("TcNr", "TcNr is required.");

            if (!ModelState.IsValid)
                return View(requestUser);

            var user = _userService.GetUser(requestUser.Id);
            user.Address = requestUser.Address;
            user.BirthDate2 = requestUser.BirthDate2;
            user.City = requestUser.City;
            user.Country = requestUser.Country;
            user.Email = requestUser.Email;
            user.Gender = requestUser.Gender;
            user.Gsm = requestUser.Gsm;
            user.Name = requestUser.Name;
            user.Password = requestUser.Password;
            user.Phone = requestUser.Phone;
            user.Region = requestUser.Region;
            user.Status = requestUser.Status;
            user.Surname = requestUser.Surname;
            user.TcNr = requestUser.TcNr;
            user.UpdateDate = DateTime.Now;

            if(requestUser.Status == UserTypes.Banned)
                user.BannedMessage = requestUser.BannedMessage;

            _userService.Edit(requestUser.Id, user);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult View(int id)
        {
            SetPageHeader("User", "User Detail View");

            var result = _userService.GetUserAllDetail(id);

            return View(result);
        }

        public ActionResult Delete(int id)
        {
            _userService.ChangeStatus(id, UserTypes.Deleted);

            Deleted();

            return RedirectToAction("List");
        }

        public JsonResult GetCity(int countryId)
        {
            if (countryId == 0)
                return Json(new {Name = "Select Country" , Id = 0}, JsonRequestBehavior.AllowGet);

            var cities =
                _cityService.AllCityList(countryId).OrderBy(a => a.SequenceNr).Select(a => new {a.Name, a.Id}).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegion(int cityId)
        {
            if (cityId == 0)
                return Json(new { Name = "Select City", Id = 0 }, JsonRequestBehavior.AllowGet);

            var cities =
                _regionService.AllRegionList(cityId).OrderBy(a => a.SequenceNr).Select(a => new { a.Name, a.Id }).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
    }
}