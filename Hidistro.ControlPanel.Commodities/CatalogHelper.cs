using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Commodities
{
    public sealed class CatalogHelper
    {
        const string CategoriesCachekey = "DataCache-Categories";

        CatalogHelper()
        {
        }

        public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            int brandId = ProductProvider.Instance().AddBrandCategory(brandCategory);
            if (brandId <= 0)
            {
                return false;
            }
            if (brandCategory.ProductTypes.Count > 0)
            {
                ProductProvider.Instance().AddBrandProductTypes(brandId, brandCategory.ProductTypes);
            }
            return true;
        }

        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            if (ProductProvider.Instance().CreateCategory(category) > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[] { category.Name }));
                HiCache.Remove("DataCache-Categories");
            }
            return CategoryActionStatus.Success;
        }

        public static bool BrandHvaeProducts(int brandId)
        {
            return ProductProvider.Instance().BrandHvaeProducts(brandId);
        }

        public static bool DeleteBrandCategory(int brandId)
        {
            return ProductProvider.Instance().DeleteBrandCategory(brandId);
        }

        public static CategoryActionStatus DeleteCategory(int categoryId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
            CategoryActionStatus status = ProductProvider.Instance().DeleteCategory(categoryId);
            if (status == CategoryActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[] { categoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return status;
        }

        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return ProductProvider.Instance().DisplaceCategory(oldCategoryId, newCategory);
        }

        public static DataTable GetBrandCategories()
        {
            return ProductProvider.Instance().GetBrandCategories();
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return ProductProvider.Instance().GetBrandCategory(brandId);
        }

        public static DataTable GetCategories()
        {
            DataTable categories = HiCache.Get("DataCache-Categories") as DataTable;
            if (null == categories)
            {
                categories = ProductProvider.Instance().GetCategories();
                HiCache.Insert("DataCache-Categories", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            DataRow[] rowArray = GetCategories().Select("CategoryId=" + categoryId.ToString(CultureInfo.InvariantCulture));
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
                    name = category.Name + " &raquo; " + name;
                }
            }
            return name;
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

        public static CategoryInfo GetMainCategory(int categoryId)
        {
            CategoryInfo category = GetCategory(categoryId);
            if (category.Depth == 1)
            {
                return category;
            }
            return GetCategory(Convert.ToInt32(category.Path.Split(new char[] { '|' })[0]));
        }

        public static IList<CategoryInfo> SearchCategories(int parentCategoryId, string keyword)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
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

        public static bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            bool flag = ProductProvider.Instance().SetBrandCategoryThemes(brandid, themeName);
            if (flag)
            {
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }

        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            if (ProductProvider.Instance().SetCategoryThemes(categoryId, themeName))
            {
                HiCache.Remove("DataCache-Categories");
            }
            return false;
        }

        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return ProductProvider.Instance().SetProductExtendCategory(productId, extendCategoryPath);
        }

        public static void SwapCategorySequence(int categoryId, CategoryZIndex zIndex)
        {
            if (categoryId > 0)
            {
                ProductProvider.Instance().SwapCategorySequence(categoryId, zIndex);
                HiCache.Remove("DataCache-Categories");
            }
        }

        public static bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            bool flag = false;
            if (categoryId <= 0)
            {
                return false;
            }
            flag = ProductProvider.Instance().SwapCategorySequence(categoryId, displaysequence);
            HiCache.Remove("DataCache-Categories");
            return flag;
        }

        public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
        {
            ProductProvider.Instance().UpdateBrandCategoryDisplaySequence(brandId, action);
        }

        public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            bool flag = ProductProvider.Instance().UpdateBrandCategory(brandCategory);
            if (flag && ProductProvider.Instance().DeleteBrandProductTypes(brandCategory.BrandId))
            {
                ProductProvider.Instance().AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
            }
            return flag;
        }

        public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
        {
            return ProductProvider.Instance().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
        }

        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            CategoryActionStatus status = ProductProvider.Instance().UpdateCategory(category);
            if (status == CategoryActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的店铺分类", new object[] { category.CategoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return status;
        }

        public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

