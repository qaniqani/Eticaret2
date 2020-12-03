using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public PropertyService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Property property)
        {
            var db = _dbFactory();
            db.Properties.Add(property);
            db.SaveChanges();
        }

        public void Edit(int id, Property request)
        {
            var db = _dbFactory();
            var property = db.Properties.FirstOrDefault(a => a.Id == id);
            if (property == null)
                return;

            property.Name = request.Name;
            property.SequenceNr = request.SequenceNr;
            property.Status = request.Status;
            db.SaveChanges();
        }

        public Property GetProperty(int id)
        {
            var db = _dbFactory();
            var property = db.Properties.FirstOrDefault(a => a.Id == id);
            return property;
        }

        public void DeleteProperty(int id)
        {
            var db = _dbFactory();
            var details = GetPropertyItemList(id);

            var property = db.Properties.FirstOrDefault(a => a.Id == id);
            db.Properties.Remove(property);
            db.SaveChanges();

            DeletePropertyItem(details);
        }

        public List<Property> AllList()
        {
            var db = _dbFactory();
            var list = db.Properties.OrderBy(a => a.SequenceNr).ToList();
            return list;
        }

        public List<Property> AllActiveList()
        {
            var db = _dbFactory();
            var list = db.Properties.OrderBy(a => a.SequenceNr).Where(a => a.Status == AdminProject.Models.StatusTypes.Active).ToList();
            return list;
        }

        public void AddPropertyDetail(PropertyItem item)
        {
            var db = _dbFactory();
            db.PropertyItems.Add(item);
            db.SaveChanges();
        }

        public void EditPropertyDetail(int id, PropertyItem request)
        {
            var db = _dbFactory();
            var item = db.PropertyItems.FirstOrDefault(a => a.Id == id);
            if (item == null)
                return;

            item.Name = request.Name;
            item.SequenceNr = request.SequenceNr;
            item.Status = request.Status;
            db.SaveChanges();
        }

        public List<PropertyItem> GetPropertyItemList(int propertyId)
        {
            var db = _dbFactory();

            var items = db.PropertyItems.Where(a => a.PropertyId == propertyId).ToList();
            return items;
        }

        public PropertyItem GetPropertyDetail(int id)
        {
            var db = _dbFactory();
            var detail = db.PropertyItems.FirstOrDefault(a => a.Id == id);
            return detail;
        }

        public void DeletePropertyItem(int id)
        {
            var db = _dbFactory();
            var item = db.PropertyItems.FirstOrDefault(a => a.Id == id);
            db.PropertyItems.Remove(item);
            db.SaveChanges();
        }

        public void DeletePropertyItem(List<PropertyItem> items)
        {
            var db = _dbFactory();
            db.PropertyItems.RemoveRange(items);
            db.SaveChanges();
        }

        public List<PropertyListDto> GetActivePropertyAndDetails()
        {
            var db = _dbFactory();

            var properties =
                db.Properties.Where(a => a.Status == AdminProject.Models.StatusTypes.Active)
                    .OrderBy(a => a.SequenceNr)
                    .ToList();

            var propertyDetails = properties.Select(propertie =>
            {
                var item = new PropertyListDto
                {
                    Name = propertie.Name,
                    PropertyItem =
                        db.PropertyItems.Where(
                                a => a.Status == AdminProject.Models.StatusTypes.Active && a.PropertyId == propertie.Id)
                            .OrderBy(a => a.SequenceNr)
                            .ToList()
                };
                return item;
            }).ToList();

            return propertyDetails;
        }
    }
}