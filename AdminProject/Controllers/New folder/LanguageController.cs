using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using System.Linq;
using Ninject;

namespace AdminProject.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly IKernel _kernel;
        private readonly Func<AdminDbContext> _dbFactory;

        public LanguageController(Func<AdminDbContext> dbFactory, IKernel kernel)
        {
            _dbFactory = dbFactory;
            _kernel = kernel;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Language", "Add New Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(Language language)
        {
            SetPageHeader("Language", "Add New Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (!ModelState.IsValid)
                return View(language);

            var db = _dbFactory();
            db.Languages.Add(language);
            db.SaveChanges();

            TempData["Success"] = "Added successfully.";

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Language", "Edit Language");

            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            if (lang == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(lang.Status);

            return View(lang);
        }

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            SetPageHeader("Language", "Edit Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(language.Status);

            if (!ModelState.IsValid)
                return View(language);

            var id = language.Id;

            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            lang.Name = language.Name;
            lang.Status = language.Status;
            lang.UrlTag = language.UrlTag;

            db.SaveChanges();

            TempData["Success"] = "Updated successfully.";

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            if (lang == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Languages.Remove(lang);
            db.SaveChanges();

            TempData["Warning"] = "Deleted successfully.";

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Language", "All Languages");

            var db = _dbFactory();

            var languages = db.Languages.ToList();

            return View(languages);
        }

        [HttpGet]
        public ActionResult Change(string language)
        {
            var db = _dbFactory();
            var selectedLanguage = db.Languages.FirstOrDefault(a => a.UrlTag == language);

            if (selectedLanguage == null)
            {
                TempData["Warning"] = "Selected language not found.";
                return Redirect("/");
            }

            _kernel.Get<RuntimeSettings>().Language = selectedLanguage.UrlTag;
            _kernel.Get<RuntimeSettings>().LanguageId = selectedLanguage.Id;

            return Redirect("/");
        }
    }
}