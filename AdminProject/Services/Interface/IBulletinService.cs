using System;
using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IBulletinService
    {
        void Add(string email);
        List<Bulletin> GetBulletinList(DateTime startDate, DateTime endDate);
    }
}