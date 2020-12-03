using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;

namespace AdminProject.Services.Interface
{
    public interface ICountryService
    {
        void Add(Country country);
        void Edit(int id, Country countryRequest);
        void Delete(int id);
        Country GetCountry(int id);
        List<Country> ActiveCountryList();
        List<Country> AllCountryList();
        SelectList GetCountrySelectList(string selectedValue);
        Country GetCountry(string name);
    }
}