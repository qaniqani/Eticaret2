using System;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class SettingService : ISettingService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public SettingService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(Settings setting)
        {
            var db = _dbFactory();

            db.Settings
            .Where(a => a.LanguageId == _setting.LanguageId)
            .ToList()
            .ForEach(a => a.Status = StatusTypes.Deactive);
            db.SaveChanges();

            db.Settings.Add(setting);
            db.SaveChanges();
        }

        public Settings GetActiveSetting()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var setting =
                db.Settings.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).FirstOrDefault();

            return setting;
        }
    }
}