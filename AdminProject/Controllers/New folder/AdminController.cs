using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using System.Linq;

namespace AdminProject.Controllers
{
    public class AdminController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AdminController(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Admin", "Add New Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, string Username, string Password, string Password2, string MasterMenu, string MediaMenu, string SettingMenu, StatusTypes Status)
        {
            SetPageHeader("Admin", "Add New Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Username))
                ModelState.AddModelError("Username", "Username is required.");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (string.IsNullOrEmpty(Password2))
                ModelState.AddModelError("PasswordAgain", "Password again is required.");

            if (Password != Password2)
                ModelState.AddModelError("PasswordNotMatch", "Passwords is not match.");

            if (!ModelState.IsValid)
                return View();

            var masterMenu = "Master";
            var mediaMenu = "Media";
            var settingMenu = "Setting";

            var authorization = !string.IsNullOrEmpty(MasterMenu) ? masterMenu + "," : "";
            authorization += !string.IsNullOrEmpty(MediaMenu) ? mediaMenu + "," : "";
            authorization += !string.IsNullOrEmpty(SettingMenu) ? settingMenu + "," : "";

            var admin = new Admin
            {
                CreatedDate = DateTime.Now,
                LastLoginDate = new DateTime(1970, 1, 1),
                Name = Name,
                Password = Password,
                Status = Status,
                Username = Username,
                Authorization = authorization
            };

            var db = _dbFactory();
            db.Admins.Add(admin);
            db.SaveChanges();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Admin", "Edit Admin");

            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(admin.Status);

            return View(admin);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, string Username, string Password, string Password2, string MasterMenu, string MediaMenu, string SettingMenu, StatusTypes Status)
        {
            SetPageHeader("Admin", "Edit Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Username))
                ModelState.AddModelError("Username", "Username is required.");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (string.IsNullOrEmpty(Password2))
                ModelState.AddModelError("PasswordAgain", "Password again is required.");

            if (Password != Password2)
                ModelState.AddModelError("PasswordNotMatch", "Passwords do not match.");

            if (!ModelState.IsValid)
                return View();

            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                ModelState.AddModelError("AdminNotFound", "Admin was not found.");
                return View();
            }

            const string masterMenu = "Master";
            const string mediaMenu = "Media";
            const string settingMenu = "Setting";

            var authorization = !string.IsNullOrEmpty(MasterMenu) ? masterMenu + "," : "";
            authorization += !string.IsNullOrEmpty(MediaMenu) ? mediaMenu + "," : "";
            authorization += !string.IsNullOrEmpty(SettingMenu) ? settingMenu + "," : "";

            admin.Authorization = authorization;
            admin.Name = Name;
            admin.Password = Password;
            admin.Status = Status;
            admin.Username = Username;

            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Admin", "List Admin");

            var db = _dbFactory();

            var admins = db.Admins.ToList();

            return View(admins);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Admins.Remove(admin);
            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
        }
    }
}