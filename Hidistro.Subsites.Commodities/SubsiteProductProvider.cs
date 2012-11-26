using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.Subsites.Commodities
{
    public abstract class SubsiteProductProvider
    {
        static readonly SubsiteProductProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.ProductData,Hidistro.Subsites.Data") as SubsiteProductProvider);

        protected SubsiteProductProvider()
        {
        }

        public abstract bool AddRelatedProduct(int productId, int relatedProductId);
        public abstract bool AddSkuSalePrice(int productId, Dictionary<string, decimal> skuSalePrice, DbTransaction dbTran);
        public abstract bool AddSubjectProducts(SubjectType subjectType, IList<int> productIds);
        public abstract bool AddTaobaoReturnProductIds(Dictionary<int, long> taobaoReturnProductIds, int updatestatus);
        public static string BuildProductQuery(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT p.ProductId FROM distro_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
            builder.AppendFormat(" AND p.DistributorUserId={0} ", HiContext.Current.User.UserId);
            if (!(string.IsNullOrEmpty(query.ProductCode) || (query.ProductCode.Length <= 0)))
            {
                builder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM distro_Categories WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY p.{0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public abstract bool CheckPrice(string productIds, string basePriceName, decimal checkPrice);
        public abstract bool ClearRelatedProducts(int productId);
        public abstract bool ClearSubjectProducts(SubjectType subjectType);
        public abstract int CreateCategory(CategoryInfo category);
        public abstract CategoryActionStatus DeleteCategory(int categoryId);
        public abstract int DeleteProducts(string productIds);
        public abstract int DisplaceCategory(int oldCategoryId, int newCategory);
        public abstract bool DownloadProduct(int productId);
        public abstract DbQueryResult GetAlertProducts(ProductQuery query);
        public abstract IList<ProductLineInfo> GetAuthorizeProductLineList();
        public abstract DataTable GetAuthorizeProductLines();
        public abstract DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload);
        public abstract DataTable GetCategories();
        public abstract DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds);
        public abstract DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds);
        public abstract DataTable GetGroupBuyProducts(ProductQuery query);
        public abstract ProductInfo GetProduct(int productId);
        public abstract DataTable GetProductAttribute(int productId);
        public abstract IList<int> GetProductIds(ProductQuery query);
        public abstract IList<ProductInfo> GetProducts(IList<int> productIds);
        public abstract DbQueryResult GetProducts(ProductQuery query);
        public abstract DataTable GetProductSKU(int productId);
        public abstract DataTable GetPubTaoBaoProducts(string productIds);
        public abstract DataTable GetPuchaseProduct(string skuId);
        public abstract DataTable GetPuchaseProducts(ProductQuery query, out int count);
        public abstract DbQueryResult GetRelatedProducts(Pagination page, int productId);
        public abstract DataTable GetSkuContentBySku(string skuId);
        public abstract string GetSkuIdByTaobao(long taobaoProductId, string taobaoSkuId);
        public abstract IList<SKUItem> GetSkus(string productIds);
        public abstract DataTable GetSkusByProductId(int productId);
        public abstract DataTable GetSkuUnderlingPrices(string productIds);
        public abstract DbQueryResult GetSubjectProducts(SubjectType subjectType, Pagination page);
        public abstract DbQueryResult GetSubmitPuchaseProducts(ProductQuery query);
        public abstract DataTable GetTaobaoProducts(string productIds);
        public abstract DbQueryResult GetToTaobaoProducts(ProductQuery query);
        public abstract DbQueryResult GetUnclassifiedProducts(ProductQuery query);
        public abstract int GetUpProducts();
        public static SubsiteProductProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool IsOnSale(string productIds);
        public abstract bool RemoveRelatedProduct(int productId, int relatedProductId);
        public abstract bool RemoveSubjectProduct(SubjectType subjectType, int productId);
        public abstract bool ReplaceProductNames(string productIds, string oldWord, string newWord);
        public abstract bool SetCategoryThemes(int categoryId, string themeName);
        public abstract bool SetProductExtendCategory(int productId, string extendCategoryPath);
        public abstract void SwapCategorySequence(int categoryId, CategoryZIndex zIndex);
        public abstract CategoryActionStatus UpdateCategory(CategoryInfo category);
        public abstract bool UpdateProduct(ProductInfo product, DbTransaction dbTran);
        public abstract bool UpdateProductCategory(int productId, int newCategoryId, string maiCategoryPath);
        public abstract bool UpdateProductNames(string productIds, string prefix, string suffix);
        public abstract int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus);
        public abstract bool UpdateSkuUnderlingPrices(DataSet ds, string skuIds);
        public abstract bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price);
        public abstract bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price);
    }
}

