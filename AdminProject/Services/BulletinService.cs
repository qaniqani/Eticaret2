using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public BulletinService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(string email)
        {
            var db = _dbFactory();
            db.Bulletines.Add(new Bulletin {CreatedDate = DateTime.Now, Email = email});
            db.SaveChanges();
        }

        public List<Bulletin> GetBulletinList(DateTime startDate, DateTime endDate)
        {
            var db = _dbFactory();
            var list =
                db.Bulletines.Where(
                    a => a.CreatedDate >= startDate && a.CreatedDate <= endDate.AddDays(1).AddSeconds(-1)).ToList();
            return list;
        }
    }
}