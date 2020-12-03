using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class BankService : IBankService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public BankService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Bank bank)
        {
            var db = _dbFactory();
            db.Banks.Add(bank);
            db.SaveChanges();
        }

        public void Edit(int id, Bank bankRequest)
        {
            var db = _dbFactory();

            var bank = db.Banks.FirstOrDefault(a => a.Id == id);
            bank.AccountNo = bankRequest.AccountNo;
            bank.Branch = bankRequest.Branch;
            bank.BranchCode = bankRequest.BranchCode;
            bank.ExchangeType = bankRequest.ExchangeType;
            bank.Iban = bankRequest.Iban;
            bank.Name = bankRequest.Name;
            bank.SequenceNr = bankRequest.SequenceNr;
            bank.Status = bankRequest.Status;
            db.SaveChanges();
        }

        public Bank GetBank(int id)
        {
            var db = _dbFactory();

            var bank = db.Banks.FirstOrDefault(a => a.Id == id);
            return bank;
        }

        public List<Bank> GetActiveBankList()
        {
            var db = _dbFactory();
            var list = db.Banks.Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public List<Bank> GetActiveBankAllList()
        {
            var db = _dbFactory();
            var list = db.Banks.OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var bank = db.Banks.FirstOrDefault(a => a.Id == id);
            db.Banks.Remove(bank);
            db.SaveChanges();
        }
    }
}