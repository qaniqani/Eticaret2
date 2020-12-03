using System;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly RuntimeSettings _setting;

        public SettingsController(RuntimeSettings setting, ISettingService settingService) : base(setting)
        {
            _setting = setting;
            _settingService = settingService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var setting = _settingService.GetActiveSetting();

            return View(setting ?? new Settings());
        }

        [HttpPost]
        public ActionResult Index(string Title, string Description, string Keyword, string MailAddress, string MailPassword, string Smtp, string Port)
        {
            SetPageHeader("Settings", "Update Settings");

            if (string.IsNullOrEmpty(Title))
                ModelState.AddModelError("Title", "Title is required");

            if (Title.Length > 200 || Title.Length < 10)
                ModelState.AddModelError("TitleLength", string.Format("At least {1} {0} can be max {2} characters.", "Title", 10, 200));

            if (Description.Length > 200)
                ModelState.AddModelError("DescriptionLength", $"{"Description"} can be max {200} characters.");

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

            _settingService.Add(setting);

            Added();

            return View();
        }
    }
}