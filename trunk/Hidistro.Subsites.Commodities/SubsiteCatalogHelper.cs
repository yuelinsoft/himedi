using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.Caching;

namespace Hidistro.Subsites.Commodities
{
    public class SubsiteCatalogHelper
    {
        const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";

        SubsiteCatalogHelper()
        {
        }

        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            if (SubsiteProductProvider.Instance().CreateCategory(category) > 0)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return CategoryActionStatus.Success;
        }

        public static CategoryActionStatus DeleteCategory(int categoryId)
        {
            CategoryActionStatus unknowError = CategoryActionStatus.UnknowError;
            unknowError = SubsiteProductProvider.Instance().DeleteCategory(categoryId);
            if (unknowError == CategoryActionStatus.Success)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return unknowError;
        }

        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return SubsiteProductProvider.Instance().DisplaceCategory(oldCategoryId, newCategory);
        }

        public static DataTable GetCategories()
        {
            DataTable categories = HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId)) as DataTable;
            if (null == categories)
            {
                categories = SubsiteProductProvider.Instance().GetCategories();
                HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId), categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            DataRow[] rowArray = GetCategories().Select("CategoryId=" + categoryId.ToString(CultureInfo.InvariantCulture) + " AND DistributorUserId =" + HiContext.Current.User.UserId.ToString(CultureInfo.InvariantCulture));
            if (rowArray.Length > 0)
            {
                return DataMapper.ConvertDataRowToProductCategory(rowArray[0]);
            }
            return null;
        }

        public static string GetFullCategory(int categoryId)
        {
            CategoryInfo category = GetCategory(categoryId);
            if (category == null)
            {
                return null;
            }
            string name = category.Name;
            while ((category != null) && category.ParentCategoryId.HasValue)
            {
                category = GetCategory(category.ParentCategoryId.Value);
                if (category != null)
                {
                    name = category.Name + " >> " + name;
                }
            }
            return name;
        }

        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1 AND DistributorUserId = " + HiContext.Current.User.UserId.ToString(CultureInfo.InvariantCulture));
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> SearchCategories(int parentCategoryId, string keyword)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = ("ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture)) + " AND DistributorUserId = " + HiContext.Current.User.UserId.ToString(CultureInfo.InvariantCulture);
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

        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            if (SubsiteProductProvider.Instance().SetCategoryThemes(categoryId, themeName))
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return false;
        }

        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return SubsiteProductProvider.Instance().SetProductExtendCategory(productId, extendCategoryPath);
        }

        public static void SwapCategorySequence(int categoryId, CategoryZIndex zIndex)
        {
            if (categoryId > 0)
            {
                SubsiteProductProvider.Instance().SwapCategorySequence(categoryId, zIndex);
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
        }

        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            CategoryActionStatus unknowError = CategoryActionStatus.UnknowError;
            unknowError = SubsiteProductProvider.Instance().UpdateCategory(category);
            if (unknowError == CategoryActionStatus.Success)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return unknowError;
        }
    }
}

