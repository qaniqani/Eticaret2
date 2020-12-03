using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IBankService
    {
        void Add(Bank bank);
        void Edit(int id, Bank bankRequest);
        Bank GetBank(int id);
        List<Bank> GetActiveBankList();
        List<Bank> GetActiveBankAllList();
        void Delete(int id);
    }
}