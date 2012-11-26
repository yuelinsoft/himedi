using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;

namespace Hidistro.SaleSystem.Catalog
{
    public static class CategoryBrowser
    {
        const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";
        const string MainCategoriesCachekey = "DataCache-Categories";

        public static DataTable GetBrandCategories(int maxNum)
        {
            return CategoryProvider.Instance().GetBrandCategories(maxNum);
        }

        public static DataTable GetBrandCategories(int categoryId, int maxNum)
        {
            return CategoryProvider.Instance().GetBrandCategories(categoryId, maxNum);
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return CategoryProvider.Instance().GetBrandCategory(brandId);
        }

        public static DataTable GetCategories()
        {
            DataTable categories = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                categories = HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value)) as DataTable;
            }
            else
            {
                categories = HiCache.Get("DataCache-Categories") as DataTable;
            }
            if (categories == null)
            {
                categories = CategoryProvider.Instance().GetCategories();
                if (HiContext.Current.SiteSettings.IsDistributorSettings)
                {
                    HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value), categories, 360, CacheItemPriority.Normal);
                    return categories;
                }
                HiCache.Insert("DataCache-Categories", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            DataRow[] rowArray = GetCategories().Select("CategoryId=" + categoryId.ToString());
            if (rowArray.Length > 0)
            {
                return DataMapper.ConvertDataRowToProductCategory(rowArray[0]);
            }
            return null;
        }

        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetMaxMainCategories(int maxNum)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("ParentCategoryId = " + parentCategoryId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static DataSet GetThreeLayerCategories()
        {
            return CategoryProvider.Instance().GetThreeLayerCategories();
        }

        public static IList<CategoryInfo> SearchCategories(int parentCategoryId, string keyword)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = "ParentCategoryId = " + parentCategoryId;
            if (!string.IsNullOrEmpty(keyword))
            {
                filterExpression = filterExpression + string.Format(" AND Name like '%{0}%'", DataHelper.CleanSearchString(keyword));
            }
            DataRow[] rowArray = GetCategories().Select(filterExpression);
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }
    }
}

