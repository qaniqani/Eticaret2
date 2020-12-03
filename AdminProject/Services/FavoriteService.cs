using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public FavoriteService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Favorite favorite)
        {
            var db = _dbFactory();
            db.Favorites.Add(favorite);
            db.SaveChanges();
        }

        public void Delete(int userId, int id)
        {
            var db = _dbFactory();
            var favori = db.Favorites.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            db.Favorites.Remove(favori);
            db.SaveChanges();
        }

        public int GetCount(string url)
        {
            var db = _dbFactory();
            var count = db.Favorites.Count(a => a.ProductUrl == url);
            return count;
        }

        public List<Favorite> GetList(int userId)
        {
            var db = _dbFactory();
            var favorites = db.Favorites.Where(a => a.UserId == userId).ToList();
            return favorites;
        }
    }
}