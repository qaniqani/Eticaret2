using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class AddressService : IAddressService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AddressService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Address address)
        {
            var db = _dbFactory();
            db.Address.Add(address);
            db.SaveChanges();
        }

        public void Edit(int id, Address addressRequest)
        {
            var db = _dbFactory();
            var address = db.Address.FirstOrDefault(a => a.Id == id);
            address.AddressDetail = addressRequest.AddressDetail;
            address.AddressNote = addressRequest.AddressNote;
            address.AddressSaveName = addressRequest.AddressSaveName;
            address.City = addressRequest.City;
            address.CreateDate = addressRequest.CreateDate;
            address.Gsm = addressRequest.Gsm;
            address.NameSurname = addressRequest.NameSurname;
            address.Phone = addressRequest.Phone;
            address.Region = addressRequest.Region;
            address.Status = addressRequest.Status;
            address.TcNr = addressRequest.TcNr;
            address.UpdateDate = DateTime.Now;
            address.UserId = addressRequest.UserId;
            db.SaveChanges();
        }

        public void Freeze(int id)
        {
            var db = _dbFactory();
            var address = db.Address.FirstOrDefault(a => a.Id == id);
            address.Status = StatusTypes.Freeze;
            db.SaveChanges();
        }

        public Address GetAddress(int id)
        {
            var db = _dbFactory();
            var address = db.Address.FirstOrDefault(a => a.Id == id);
            return address;
        }

        public Address GetAddress(int id, int userId)
        {
            var db = _dbFactory();
            var address = db.Address.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            return address;
        }

        public List<Address> GetUserAddressAllList(int userId)
        {
            var db = _dbFactory();
            var list = db.Address.Where(a => a.UserId == userId).ToList();
            return list;
        }

        public List<Address> GetUserAddressActiveList(int userId)
        {
            var db = _dbFactory();
            var list = db.Address.Where(a => a.UserId == userId && a.Status == StatusTypes.Active).ToList();
            return list;
        }

        public List<Address> GetUserAddressUserActiveList(int userId)
        {
            var db = _dbFactory();
            var list = db.Address.Where(a => a.UserId == userId && a.Status != StatusTypes.Freeze).ToList();
            return list;
        }

        public List<Address> GetUserAddressActiveList(int userId, StatusTypes status)
        {
            var db = _dbFactory();
            var list = db.Address.Where(a => a.UserId == userId && a.Status == status).ToList();
            return list;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var address = db.Address.FirstOrDefault(a => a.Id == id);
            db.Address.Remove(address);
            db.SaveChanges();
        }
    }
}