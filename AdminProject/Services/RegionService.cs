using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class RegionService : IRegionService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public RegionService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Region region)
        {
            var db = _dbFactory();
            db.Regions.Add(region);
            db.SaveChanges();
        }

        public void Edit(int id, Region regionRequest)
        {
            var db = _dbFactory();
            var region = db.Regions.FirstOrDefault(a => a.Id == id);
            region.CityId = regionRequest.CityId;
            region.Name = regionRequest.Name;
            region.SequenceNr = regionRequest.SequenceNr;
            region.Status = regionRequest.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var region = db.Regions.FirstOrDefault(a => a.Id == id);
            db.Regions.Remove(region);
            db.SaveChanges();
        }

        public Region GetRegion(int id)
        {
            var db = _dbFactory();
            var region = db.Regions.FirstOrDefault(a => a.Id == id);
            return region;
        }

        public List<Region> ActiveRegionList(int cityId)
        {
            var db = _dbFactory();
            var list = db.Regions.Where(a => a.CityId == cityId && a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public List<Region> AllRegionList(int cityId)
        {
            var db = _dbFactory();
            var list = db.Regions.Where(a => a.CityId == cityId).OrderBy(a => a.SequenceNr).ThenBy(a => a.Name).ToList();
            return list;
        }

        public SelectList GetRegionSelectList(int cityId, string selectedValue)
        {
            var db = _dbFactory();
            var region = db.Regions.Where(a => a.CityId == cityId && a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.SequenceNr).Select(a => new { a.Name, a.Id });

            var list = region.Select(a => new ListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }
    }
}