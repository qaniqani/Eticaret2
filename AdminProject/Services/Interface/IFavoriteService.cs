using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IFavoriteService
    {
        void Add(Favorite favorite);
        void Delete(int userId, int id);
        int GetCount(string url);
        List<Favorite> GetList(int userId);
    }
}