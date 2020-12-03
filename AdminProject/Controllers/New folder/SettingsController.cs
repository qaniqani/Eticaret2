using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;

namespace AdminProject.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public SettingsController(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var setting =
                db.Settings.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).FirstOrDefault();

            return View(setting ?? new Settings());
        }

        [HttpPost]
        public ActionResult Index(string Title, string Description, string Keyword, string MailAddress, string MailPassword, string Smtp, string Port)
        {
            SetPageHeader("Settings", "Update Settings");

            if (string.IsNullOrEmpty(Title))
                ModelState.AddModelError("Title", "Title is required");

            if (!ModelState.IsValid)
                return View();

            var setting = new Settings
            {
                CreateDate = DateTime.Now,
                Description = Description,
                Keyword = Keyword,
                LanguageId = _setting.LanguageId,
                MailAddress = MailAddress,
                MailPassword = MailPassword,
                Port = Port,
                Smtp = Smtp,
                Status = StatusTypes.Active,
                Title = Title
            };

            var db = _dbFactory();

            db.Settings
            .Where(a => a.LanguageId == _setting.LanguageId)
            .ToList()
            .ForEach(a => a.Status = StatusTypes.Deactive);

            db.SaveChanges();

            db.Settings.Add(setting);
            db.SaveChanges();

            Added();

            return View();
        }
    }
}