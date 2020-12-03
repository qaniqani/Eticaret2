using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IPropertyService
    {
        void Add(Property property);
        void Edit(int id, Property request);
        Property GetProperty(int id);
        List<Property> AllList();
        List<Property> AllActiveList();
        void AddPropertyDetail(PropertyItem item);
        void EditPropertyDetail(int id, PropertyItem request);
        List<PropertyItem> GetPropertyItemList(int propertyId);
        PropertyItem GetPropertyDetail(int id);
        void DeleteProperty(int id);
        void DeletePropertyItem(int id);
        void DeletePropertyItem(List<PropertyItem> items);
        List<PropertyListDto> GetActivePropertyAndDetails();
    }
}