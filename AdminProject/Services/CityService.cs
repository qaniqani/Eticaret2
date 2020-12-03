using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class CityService : ICityService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CityService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(City city)
        {
            var db = _dbFactory();
            db.Citys.Add(city);
            db.SaveChanges();
        }

        public void Edit(int id, City cityRequest)
        {
            var db = _dbFactory();
            var city = db.Citys.FirstOrDefault(a => a.Id == id);
            city.CountryId = cityRequest.CountryId;
            city.Name = cityRequest.Name;
            city.SequenceNr = cityRequest.SequenceNr;
            city.Status = cityRequest.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var city = db.Citys.FirstOrDefault(a => a.Id == id);
            db.Citys.Remove(city);
            db.SaveChanges();
        }

        public City GetCity(int id)
        {
            var db = _dbFactory();
            var city = db.Citys.FirstOrDefault(a => a.Id == id);
            return city;
        }

        public City GetCity(string name)
        {
            var db = _dbFactory();
            var city = db.Citys.FirstOrDefault(a => a.Name == name);
            return city;
        }

        public List<City> ActiveCityList(int countryId)
        {
            var db = _dbFactory();
            var city =
                db.Citys.Where(a => a.CountryId == countryId && a.Status == AdminProject.Models.StatusTypes.Active)
                    .OrderBy(a => a.SequenceNr)
                    .ThenBy(a => a.Name)
                    .ToList();
            return city;
        }

        public List<City> AllCityList(int countryId)
        {
            var db = _dbFactory();
            var city =
                db.Citys.Where(a => a.CountryId == countryId)
                    .OrderBy(a => a.SequenceNr)
                    .ThenBy(a => a.Name)
                    .ToList();
            return city;
        }

        public SelectList GetCitySelectList(int countryId, string selectedValue)
        {
            var db = _dbFactory();
            var city = db.Citys.Where(a => a.CountryId == countryId && a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).Select(a => new { a.Name, a.Id });

            var list = city.Select(a => new ListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }
    }
}