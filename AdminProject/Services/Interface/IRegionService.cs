using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IRegionService
    {
        void Add(Region region);
        void Edit(int id, Region regionRequest);
        void Delete(int id);
        Region GetRegion(int id);
        List<Region> ActiveRegionList(int cityId);
        List<Region> AllRegionList(int cityId);
        SelectList GetRegionSelectList(int cityId, string selectedValue);
    }
}