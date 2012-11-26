namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Xml;

    public static class ProductBrowser
    {
        public static IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId)
        {
            return ProductProvider.Instance().GetAttributeInfoByCategoryId(categoryId);
        }

        public static DataTable GetBrandsByCategoryId(int categoryId)
        {
            return ProductProvider.Instance().GetBrandsByCategoryId(categoryId);
        }

        public static DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetBrowseProductList(query);
        }

        public static DataTable GetCounDownProducList(int maxnum)
        {
            return ProductProvider.Instance().GetCounDownProducList(maxnum);
        }

        public static CountDownInfo GetCountDownInfo(int productId)
        {
            return ProductProvider.Instance().GetCountDownInfo(productId);
        }

        public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetCountDownProductList(query);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return ProductProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static GiftInfo GetGift(int giftId)
        {
            return ProductProvider.Instance().GetGift(giftId);
        }

        public static DataTable GetGroupByProductList(int maxnum)
        {
            return ProductProvider.Instance().GetGroupByProductList(maxnum);
        }

        public static DataSet GetGroupByProductList(ProductBrowseQuery query, out int count)
        {
            return ProductProvider.Instance().GetGroupByProductList(query, out count);
        }

        public static DataTable GetLineItems(int productId, int maxNum)
        {
            return ProductProvider.Instance().GetLineItems(productId, maxNum);
        }

        public static DbQueryResult GetOnlineGifts(Pagination page)
        {
            return ProductProvider.Instance().GetOnlineGifts(page);
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return ProductProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            return ProductProvider.Instance().GetProductBrowseInfo(productId, maxReviewNum, maxConsultationNum);
        }

        public static int GetProductConsultationNumber(int productId)
        {
            return ProductProvider.Instance().GetProductConsultationNumber(productId);
        }

        public static DbQueryResult GetProductConsultations(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetProductConsultations(page, productId);
        }

        public static GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            return ProductProvider.Instance().GetProductGroupBuyInfo(productId);
        }

        public static int GetProductReviewNumber(int productId)
        {
            return ProductProvider.Instance().GetProductReviewNumber(productId);
        }

        public static DataTable GetProductReviews(int maxNum)
        {
            return ProductProvider.Instance().GetProductReviews(maxNum);
        }

        public static DbQueryResult GetProductReviews(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetProductReviews(page, productId);
        }

        public static ProductInfo GetProductSimpleInfo(int productId)
        {
            return ProductProvider.Instance().GetProductSimpleInfo(productId);
        }

        public static XmlDocument GetProductSubjectDocument()
        {
            string key = "ProductSubjectFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ProductSubjectFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/ProductSubjects.xml");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        public static DataTable GetSaleProductRanking(int maxNum)
        {
            return ProductProvider.Instance().GetSaleProductRanking(maxNum);
        }

        public static DataTable GetSubjectList(SubjectListQuery query)
        {
            return ProductProvider.Instance().GetSubjectList(query);
        }

        public static DbQueryResult GetSubjectProduct(SubjectListQuery query)
        {
            return ProductProvider.Instance().GetSubjectProduct(query);
        }

        public static DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetUnSaleProductList(query);
        }

        public static DataTable GetVistiedProducts(IList<int> productIds)
        {
            return ProductProvider.Instance().GetVistiedProducts(productIds);
        }

        public static bool IsBuyProduct(int productId)
        {
            return ProductProvider.Instance().IsBuyProduct(productId);
        }

        public static void LoadProductReview(int productId, out int buyNum, out int reviewNum)
        {
            ProductProvider.Instance().LoadProductReview(productId, out buyNum, out reviewNum);
        }
    }
}

