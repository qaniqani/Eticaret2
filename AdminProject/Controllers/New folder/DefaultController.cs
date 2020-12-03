using System;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Models;

namespace AdminProject.Controllers
{
    public class DefaultController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;
        public DefaultController(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public ActionResult Index()
        {
            SetPageHeader("Dashboard", "");

            return View();
        }

        [ChildActionOnly]
        public ActionResult LoadLanguages()
        {
            var db = _dbFactory();
            var languages = db.Languages.Where(a => a.Status == StatusTypes.Active);

            if (!languages.Any())
            {
                TempData["Warning"] = "Before you add the language!!!";
            }

            ViewData["SelectedLanguage"] = _setting.Language;

            return PartialView("../Partial/Language", languages);
        }

        [ChildActionOnly]
        public ActionResult Authorizations()
        {
            var authorization = Utility.SessionCheck().Authorization;
            ViewData["Authorizations"] = authorization;

            return PartialView("../Partial/LeftMenu", authorization);
        }
    }
}