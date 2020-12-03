using System;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class ContentService : IContentService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public ContentService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Content GetContent(string url)
        {
            var db = _dbFactory();

            var content =
                db.Contents.FirstOrDefault(a => a.Url == url && a.Status == AdminProject.Models.StatusTypes.Active);
            return content;
        }
    }
}