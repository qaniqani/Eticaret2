using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class CountryService : ICountryService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CountryService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Country country)
        {
            var db = _dbFactory();
            db.Countries.Add(country);
            db.SaveChanges();
        }

        public void Edit(int id, Country countryRequest)
        {
            var db = _dbFactory();
            var country = db.Countries.FirstOrDefault(a => a.Id == id);
            country.Name = countryRequest.Name;
            country.SequenceNr = countryRequest.SequenceNr;
            country.Status = countryRequest.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var country = db.Countries.FirstOrDefault(a => a.Id == id);
            db.Countries.Remove(country);
            db.SaveChanges();
        }

        public Country GetCountry(int id)
        {
            var db = _dbFactory();
            var country = db.Countries.FirstOrDefault(a => a.Id == id);
            return country;
        }

        public Country GetCountry(string name)
        {
            var db = _dbFactory();
            var country = db.Countries.FirstOrDefault(a => a.Name == name);
            return country;
        }

        public List<Country> ActiveCountryList()
        {
            var db = _dbFactory();
            var country =
                db.Countries.Where(a => a.Status == StatusTypes.Active)
                    .OrderBy(a => a.SequenceNr)
                    .ThenBy(a => a.Name)
                    .ToList();
            return country;
        }

        public List<Country> AllCountryList()
        {
            var db = _dbFactory();
            var country =
                db.Countries
                    .OrderBy(a => a.SequenceNr)
                    .ThenBy(a => a.Name)
                    .ToList();
            return country;
        }

        public SelectList GetCountrySelectList(string selectedValue)
        {
            var db = _dbFactory();
            var country = db.Countries.Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).Select(a => new ListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            country.Insert(0, new ListItem { Text = "All", Value = "0" });

            return new SelectList(country, "Value", "Text", selectedValue);
        }
    }
}