using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface ICategoryService
    {
        string GetChainCategory(int id);
        SelectList GetStatusType(StatusTypes status);
        SelectList GetCategoryType(CategoryTypes status);
        List<MenuItem> GetCategories(CategoryTypes categoryType, StatusTypes status);
        void AddCategory(Category category);
        Category GetCategory(string url);
        Category GetCategory(int id);
        void DeleteCategory(int id);
        List<CategoryListDto> ListCategory();
        void OrderCategory(List<SortedMenu> order);
        List<Category> ActiveCategoryList(string language);
        List<Category> GetCategoryParentList(int parentId);
        CategoryLinkDto GetChainCategoryLink(int id, string split);
        SelectList GetCategorySelectList(int parentId, int selectedValue);
        List<CategoryLinkDto> GetChainCategoryLink(int[] id, string split);
        int[] GetCategoryParentIds(int categoryId);
        List<Category> GetCategory(int[] ids);
    }
}