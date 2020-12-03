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
    public class BrandService : IBrandService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public BrandService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Brand brand)
        {
            var db = _dbFactory();
            db.Brands.Add(brand);
            db.SaveChanges();
        }

        public void Edit(int id, Brand brandRequest)
        {
            var db = _dbFactory();
            var brand = db.Brands.FirstOrDefault(a => a.Id == id);
            brand.Name = brandRequest.Name;
            brand.SequenceNr = brandRequest.SequenceNr;
            brand.Status = brandRequest.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var brand = db.Brands.FirstOrDefault(a => a.Id == id);
            db.Brands.Remove(brand);
            db.SaveChanges();
        }

        public Brand GetBrand(int id)
        {
            var db = _dbFactory();
            var brand = db.Brands.FirstOrDefault(a => a.Id == id);
            return brand;
        }

        public List<Brand> ActiveBrandList()
        {
            var db = _dbFactory();
            var list = db.Brands.Where(a => a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public List<Brand> AllBrandList()
        {
            var db = _dbFactory();
            var list = db.Brands.OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public SelectList BrandList(string selectedValue)
        {
            var db = _dbFactory();
            var list = db.Brands.Where(a => a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).Select(a => new ListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            var selectList = new SelectList(list, "Value", "Text", selectedValue);
            return selectList;
        }
    }
}