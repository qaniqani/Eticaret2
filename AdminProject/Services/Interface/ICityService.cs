using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ICityService
    {
        void Add(City city);
        void Edit(int id, City cityRequest);
        void Delete(int id);
        City GetCity(int id);
        City GetCity(string name);
        List<City> ActiveCityList(int countryId);
        List<City> AllCityList(int countryId);
        SelectList GetCitySelectList(int countryId, string selectedValue);
    }
}