using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace Hidistro.Subsites.Commodities
{
    public static class SubSiteProducthelper
    {
        public static bool AddRelatedProduct(int productId, int relatedProductId)
        {
            return SubsiteProductProvider.Instance().AddRelatedProduct(productId, relatedProductId);
        }

        public static bool AddSubjectProduct(SubjectType subjectType, int productId)
        {
            IList<int> productIds = new List<int>();
            productIds.Add(productId);
            return SubsiteProductProvider.Instance().AddSubjectProducts(subjectType, productIds);
        }

        public static bool AddSubjectProducts(SubjectType subjectType, IList<int> productIds)
        {
            return SubsiteProductProvider.Instance().AddSubjectProducts(subjectType, productIds);
        }

        public static bool AddTaobaoReturnProductIds(Dictionary<int, long> taobaoReturnProductIds, int Updatestatus)
        {
            if ((taobaoReturnProductIds == null) && (taobaoReturnProductIds.Count <= 0))
            {
                return false;
            }
            return SubsiteProductProvider.Instance().AddTaobaoReturnProductIds(taobaoReturnProductIds, Updatestatus);
        }

        public static bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
        {
            return SubsiteProductProvider.Instance().CheckPrice(productIds, basePriceName, checkPrice);
        }

        public static bool ClearRelatedProducts(int productId)
        {
            return SubsiteProductProvider.Instance().ClearRelatedProducts(productId);
        }

        public static bool ClearSubjectProducts(SubjectType subjectType)
        {
            return SubsiteProductProvider.Instance().ClearSubjectProducts(subjectType);
        }

        public static int DeleteProducts(string productIds)
        {
            return SubsiteProductProvider.Instance().DeleteProducts(productIds);
        }

        public static bool DownloadProduct(int productId)
        {
            return SubsiteProductProvider.Instance().DownloadProduct(productId);
        }

        public static DbQueryResult GetAlertProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetAlertProducts(query);
        }

        public static IList<ProductLineInfo> GetAuthorizeProductLineList()
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProductLineList();
        }

        public static DataTable GetAuthorizeProductLines()
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProductLines();
        }

        public static DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload)
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProducts(query, onlyNotDownload);
        }

        public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            return SubsiteProductProvider.Instance().GetExportProducts(query, removeProductIds);
        }

        public static DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            DataSet set = SubsiteProductProvider.Instance().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
            set.Tables[0].TableName = "types";
            set.Tables[1].TableName = "attributes";
            set.Tables[2].TableName = "values";
            set.Tables[3].TableName = "products";
            set.Tables[4].TableName = "skus";
            set.Tables[5].TableName = "skuItems";
            set.Tables[6].TableName = "productAttributes";
            return set;
        }

        public static DataTable GetGroupBuyProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetGroupBuyProducts(query);
        }

        public static ProductInfo GetProduct(int productId)
        {
            return SubsiteProductProvider.Instance().GetProduct(productId);
        }

        public static DataTable GetProductAttribute(int productId)
        {
            return SubsiteProductProvider.Instance().GetProductAttribute(productId);
        }

        public static IList<int> GetProductIds(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetProductIds(query);
        }

        public static DbQueryResult GetProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetProducts(query);
        }

        public static IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            return SubsiteProductProvider.Instance().GetProducts(productIds);
        }

        public static DataTable GetProductSKU(int productId)
        {
            return SubsiteProductProvider.Instance().GetProductSKU(productId);
        }

        public static DataTable GetPubTaoBaoProducts(string productIds)
        {
            return SubsiteProductProvider.Instance().GetPubTaoBaoProducts(productIds);
        }

        public static DataTable GetPuchaseProduct(string skuId)
        {
            return SubsiteProductProvider.Instance().GetPuchaseProduct(skuId);
        }

        public static DataTable GetPuchaseProducts(ProductQuery query, out int count)
        {
            return SubsiteProductProvider.Instance().GetPuchaseProducts(query, out count);
        }

        public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            return SubsiteProductProvider.Instance().GetRelatedProducts(page, productId);
        }

        public static DataTable GetSkuContent(long taobaoProductId, string taobaoSkuId)
        {
            string skuIdByTaobao = SubsiteProductProvider.Instance().GetSkuIdByTaobao(taobaoProductId, taobaoSkuId);
            return SubsiteProductProvider.Instance().GetSkuContentBySku(skuIdByTaobao);
        }

        public static DataTable GetSkuContentBySku(string skuId)
        {
            return SubsiteProductProvider.Instance().GetSkuContentBySku(skuId);
        }

        public static IList<SKUItem> GetSkus(string productIds)
        {
            return SubsiteProductProvider.Instance().GetSkus(productIds);
        }

        public static DataTable GetSkusByProductId(int productId)
        {
            return SubsiteProductProvider.Instance().GetSkusByProductId(productId);
        }

        public static DataTable GetSkuUnderlingPrices(string productIds)
        {
            return SubsiteProductProvider.Instance().GetSkuUnderlingPrices(productIds);
        }

        public static DbQueryResult GetSubjectProducts(SubjectType subjectType, Pagination page)
        {
            return SubsiteProductProvider.Instance().GetSubjectProducts(subjectType, page);
        }

        public static DbQueryResult GetSubmitPuchaseProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetSubmitPuchaseProducts(query);
        }

        public static DataTable GetTaobaoProducts(string productIds)
        {
            return SubsiteProductProvider.Instance().GetTaobaoProducts(productIds);
        }

        public static DbQueryResult GetToTaobaoProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetToTaobaoProducts(query);
        }

        public static DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetUnclassifiedProducts(query);
        }

        public static int GetUpProducts()
        {
            return SubsiteProductProvider.Instance().GetUpProducts();
        }

        public static bool IsOnSale(string productIds)
        {
            return SubsiteProductProvider.Instance().IsOnSale(productIds);
        }

        public static bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            return SubsiteProductProvider.Instance().RemoveRelatedProduct(productId, relatedProductId);
        }

        public static bool RemoveSubjectProduct(SubjectType subjectType, int productId)
        {
            return SubsiteProductProvider.Instance().RemoveSubjectProduct(subjectType, productId);
        }

        public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            return SubsiteProductProvider.Instance().ReplaceProductNames(productIds, oldWord, newWord);
        }

        public static bool UpdateProduct(ProductInfo product, Dictionary<string, decimal> skuSalePrice)
        {
            bool flag;
            if (null == product)
            {
                return false;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteProductProvider.Instance().UpdateProduct(product, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsiteProductProvider.Instance().AddSkuSalePrice(product.ProductId, skuSalePrice, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
            }
            return flag;
        }

        public static bool UpdateProductCategory(int productId, int newCategoryId)
        {
            if (newCategoryId != 0)
            {
                return SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, SubsiteCatalogHelper.GetCategory(newCategoryId).Path + "|");
            }
            return SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, null);
        }

        public static bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            return SubsiteProductProvider.Instance().UpdateProductNames(productIds, prefix, suffix);
        }

        public static int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            return SubsiteProductProvider.Instance().UpdateProductSaleStatus(productIds, saleStatus);
        }

        public static bool UpdateSkuUnderlingPrices(DataSet ds, string skuIds)
        {
            if ((ds == null) || string.IsNullOrEmpty(skuIds))
            {
                return false;
            }
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(ds, skuIds);
        }

        public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price)
        {
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, price);
        }

        public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
        {
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, basePriceName, operation, price);
        }
    }
}

