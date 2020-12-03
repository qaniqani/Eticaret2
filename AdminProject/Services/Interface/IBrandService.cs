using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IBrandService
    {
        void Add(Brand brand);
        void Edit(int id, Brand brandRequest);
        void Delete(int id);
        Brand GetBrand(int id);
        List<Brand> ActiveBrandList();
        List<Brand> AllBrandList();
        SelectList BrandList(string selectedValue);
    }
}