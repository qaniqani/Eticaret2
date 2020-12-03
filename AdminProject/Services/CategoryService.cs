using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using MenuItem = AdminProject.Models.MenuItem;

namespace AdminProject.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public CategoryService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public string GetChainCategory(int id)
        {
            var db = _dbFactory();
            int parentId;
            string categoryName;

            var category = db.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null)
                return "";

            if (category.ParentId != 0)
            {
                parentId = category.ParentId;
                categoryName = category.Name;
                do
                {
                    var masterCategory = db.Categories.FirstOrDefault(u => u.Id == parentId);
                    if (masterCategory == null)
                        continue;

                    categoryName = masterCategory.Name + " > " + categoryName;
                    parentId = masterCategory.ParentId;
                }
                while (parentId != 0);
            }
            else
            {
                categoryName = category.Name;
                return categoryName;
            }
            return categoryName;
        }

        public string GetChainCategoryUrl(int id)
        {
            var db = _dbFactory();
            int parentId;
            string categoryUrl;

            var category = db.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null)
                return "";

            if (category.ParentId != 0)
            {
                parentId = category.ParentId;
                categoryUrl = category.Url;
                do
                {
                    var masterCategory = db.Categories.FirstOrDefault(u => u.Id == parentId);
                    if (masterCategory == null)
                        continue;

                    categoryUrl = masterCategory.Name + "/" + categoryUrl;
                    parentId = masterCategory.ParentId;
                }
                while (parentId != 0);
            }
            else
            {
                categoryUrl = category.Name;
                return categoryUrl;
            }
            return categoryUrl;
        }

        //Product category
        public List<CategoryLinkDto> GetChainCategoryLink(int[] id, string split)
        {
            var links = new List<CategoryLinkDto>();
            id.ToList().ForEach(item =>
            {
                links.Add(GetChainCategoryLink(item, split));
            });
            return links;
        }

        public CategoryLinkDto GetChainCategoryLink(int id, string split)
        {
            var db = _dbFactory();
            int parentId;
            var categoryItem = new CategoryLinkDto();

            var category = db.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null)
                return new CategoryLinkDto();

            if (category.ParentId != 0)
            {
                parentId = category.ParentId;

                var listItem = new CategoryLinkDto
                {
                    Name = category.Name,
                    Url = category.Url
                };

                do
                {
                    var masterCategory = db.Categories.FirstOrDefault(u => u.Id == parentId);
                    if (masterCategory == null)
                        continue;

                    listItem.SingleName = listItem.Name;
                    listItem.Name = $"{masterCategory.Name} {split} {listItem.Name}";
                    listItem.Url = listItem.Url;

                    parentId = masterCategory.ParentId;
                    categoryItem = listItem;
                }
                while (parentId != 0);
            }
            else
            {
                var listItem = new CategoryLinkDto
                {
                    Name = category.Name,
                    Url = category.Url
                };
                categoryItem = listItem;
                return categoryItem;
            }
            return categoryItem;
        }

        //Menu Category
        public List<Category> GetChainMenuLink(int[] id)
        {
            var links = new List<Category>();
            id.ToList().ForEach(item =>
            {
                links.Add(GetChainMenuLink(item));
            });
            return links;
        }

        public Category GetChainMenuLink(int id)
        {
            var db = _dbFactory();
            int parentId;

            var category = db.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null)
                return new Category();

            if (category.ParentId != 0)
            {
                parentId = category.ParentId;

                do
                {
                    var masterCategory = db.Categories.FirstOrDefault(u => u.Id == parentId);
                    if (masterCategory == null)
                        continue;

                    category.Name = category.Name;
                    category.Url = string.Format("{0}/{1}", masterCategory.Url, category.Url);

                    parentId = masterCategory.ParentId;
                }
                while (parentId != 0);
            }
            else
            {
                return category;
            }
            return category;
        }

        public SelectList GetStatusType(StatusTypes status)
        {
            return DropdownTypes.GetStatus(status);
        }

        public SelectList GetCategoryType(CategoryTypes status)
        {
            return DropdownTypes.GetCategoryType(status);
        }

        public SelectList GetCategorySelectList(int parentId, int selectedValue)
        {
            var db = _dbFactory();

            var categories = db.Categories
                .Where(a => a.LanguageId == _setting.LanguageId && a.ParentId == parentId)
                .OrderBy(a => a.Name)
                .Select(a => new ListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
                .ToList();

            return new SelectList(categories, "Value", "Text", selectedValue.ToString());
        }

        public List<MenuItem> GetCategories(CategoryTypes categoryType, StatusTypes status)
        {
            var db = _dbFactory();
            var menus = db.Categories.OrderBy(a => a.SequenceNumber).Where(a => a.LanguageTag == _setting.Language && a.Status == status);
            if (categoryType != CategoryTypes.All)
                menus = menus.Where(a => a.CategoryType == categoryType);

            var treeView = Utility.CreateTree(menus.ToList()).ToList();
            return treeView;
        }

        public void AddCategory(Category category)
        {
            var db = _dbFactory();

            var url = category.Url;
            var urlCount = db.Categories.Count(a => a.Url.Contains(url));
            if (urlCount > 0)
                category.Url = url + "-" + urlCount;

            db.Categories.Add(category);
            db.SaveChanges();
        }

        public Category GetCategory(int id)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Id == id);
            return category;
        }

        public List<Category> GetCategory(int[] ids)
        {
            var db = _dbFactory();
            var category = db.Categories.Where(a => ids.Contains(a.Id)).ToList();
            return category;
        }

        public Category GetCategory(string url)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == url && a.Status == StatusTypes.Active);
            return category;
        }

        public void DeleteCategory(int id)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Id == id);
            db.Categories.Remove(category);
            db.SaveChanges();
        }

        public void OrderCategory(List<SortedMenu> order)
        {
            var db = _dbFactory();
            order.ForEach(a =>
            {
                var row = db.Categories.FirstOrDefault(d => d.Id == a.ItemId);
                if (row == null)
                    return;
                row.SequenceNumber = Convert.ToInt32(a.Right);
                row.ParentId = a.ParentId;
                db.SaveChanges();
            });
        }

        public List<CategoryListDto> ListCategory()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var categories = db.Categories.Where(a => a.LanguageId == languageId).ToList();
            var result = categories.Select(a => new CategoryListDto
            {
                ChainName = GetChainCategory(a.Id),
                Id = a.Id,
                Name = a.Name,
                Status = a.Status,
                Url = a.Url,
                Picture = a.Picture
            }).ToList();

            return result;
        }

        public List<Category> ActiveCategoryList(string language)
        {
            var db = _dbFactory();

            var categoryIds =
                db.Categories.Where(a => a.Status == StatusTypes.Active && a.LanguageTag == language)
                    .OrderBy(a => a.SequenceNumber)
                    .ToList();
            //var list = GetChainMenuLink(categoryIds);
            //var list = db.Categories.Where(a => a.Status == StatusTypes.Active && a.LanguageTag == language).OrderBy(a => a.SequenceNumber).ToList();

            return categoryIds;
        }

        public List<Category> GetCategoryParentList(int parentId)
        {
            var db = _dbFactory();
            var categories = db.Categories.Where(a => a.ParentId == parentId).ToList();
            return categories;
        }

        public int[] GetCategoryParentIds(int categoryId)
        {
            var db = _dbFactory();
            var ids =
                db.Categories.Where(a => a.ParentId == categoryId && a.Status == StatusTypes.Active)
                    .OrderBy(a => a.SequenceNumber)
                    .Select(a => a.Id)
                    .ToArray();
            return ids;
        }
    }
}