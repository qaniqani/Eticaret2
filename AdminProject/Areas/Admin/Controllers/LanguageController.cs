using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using System.Linq;
using Ninject;

namespace AdminProject.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly IKernel _kernel;
        private readonly Func<AdminDbContext> _dbFactory;
        public LanguageController(Func<AdminDbContext> dbFactory, IKernel kernel, RuntimeSettings setting) : base(setting)
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

            if (language.Name.Length > 200 || language.Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (language.UrlTag.Length > 10 || language.UrlTag.Length < 2)
                ModelState.AddModelError("UrlTagLength", string.Format("At least {1} {0} can be max {2} characters.", "Url Tag", 2, 10));

            if (!ModelState.IsValid)
                return View(language);

            var db = _dbFactory();

            var languageCheck = db.Languages.FirstOrDefault(a => a.UrlTag == language.UrlTag);
            if(languageCheck != null)
                ModelState.AddModelError("UrlTagIsMatch", "Available languages with the same label.");

            if (!ModelState.IsValid)
                return View(language);

            db.Languages.Add(language);
            db.SaveChanges();

            Added();

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

            if (language.Name.Length > 200 || language.Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (language.UrlTag.Length > 10 || language.UrlTag.Length < 2)
                ModelState.AddModelError("UrlTagLength", string.Format("At least {1} {0} can be max {2} characters.", "Url Tag", 2, 10));

            if (!ModelState.IsValid)
                return View(language);

            var id = language.Id;

            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            if (lang == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            var languageCheck = db.Languages.FirstOrDefault(a => a.Id != id && a.UrlTag == language.UrlTag);
            if (languageCheck != null)
                ModelState.AddModelError("UrlTagIsMatch", "Available languages with the same label.");

            if (!ModelState.IsValid)
                return View(language);

            lang.Name = language.Name;
            lang.Status = language.Status;
            lang.UrlTag = language.UrlTag;

            db.SaveChanges();

            Updated();

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

            Deleted();

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
                return Redirect("/Admin/Default");
            }

            _kernel.Get<RuntimeSettings>().Language = selectedLanguage.UrlTag;
            _kernel.Get<RuntimeSettings>().LanguageId = selectedLanguage.Id;

            return Redirect("/Admin/Default");
        }
    }
}