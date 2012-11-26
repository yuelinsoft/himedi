using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.Subsites.Data
{
    public class ProductData : SubsiteProductProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_RelatedProducts(ProductId, DistributorUserId, RelatedProductId) VALUES (@ProductId, @DistributorUserId, @RelatedProductId)");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            try
            {
                return (database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public override bool AddSkuSalePrice(int productId, Dictionary<string, decimal> skuSalePrice, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE DistributoruserId = {0} AND GradeId = 0 AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = {1});", HiContext.Current.User.UserId, productId);
            if ((skuSalePrice != null) && (skuSalePrice.Count > 0))
            {
                foreach (string str in skuSalePrice.Keys)
                {
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice(SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, 0, {2})", str, HiContext.Current.User.UserId, skuSalePrice[str]);
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) >= 0);
        }

        public override bool AddSubjectProducts(SubjectType subjectType, IList<int> productIds)
        {
            if (productIds.Count <= 0)
            {
                return false;
            }
            foreach (int num in productIds)
            {
                RemoveSubjectProduct(subjectType, num);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_SubjectProducts(DistributorUserId, SubjectType, ProductId) VALUES (@DistributorUserId, @SubjectType, @ProductId)");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, (int)subjectType);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            try
            {
                foreach (int num2 in productIds)
                {
                    database.SetParameterValue(sqlStringCommand, "ProductId", num2);
                    database.ExecuteNonQuery(sqlStringCommand);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool AddTaobaoReturnProductIds(Dictionary<int, long> taobaoReturnProductIds, int updatestatus)
        {
            int userId = HiContext.Current.User.UserId;
            StringBuilder builder = new StringBuilder();
            foreach (int num2 in taobaoReturnProductIds.Keys)
            {
                builder.AppendFormat(" DELETE FROM Taobao_DistroProducts WHERE DistributorUserId = {0} AND ProductId  = {1};", userId, num2);
                builder.AppendFormat(" INSERT INTO Taobao_DistroProducts (DistributorUserId, ProductId, TaobaoProductId,UpdateStatus) VALUES ({0}, {1}, {2},{3});", new object[] { userId, num2, taobaoReturnProductIds[num2], updatestatus });
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            if (basePriceName == "PurchasePrice")
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND", DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) - {2} < 0", user.GradeId, distributorDiscount, checkPrice);
            }
            else if (basePriceName == "SalePrice")
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {1}) - {2} < 0", DataHelper.CleanSearchString(productIds), user.UserId, checkPrice);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ClearRelatedProducts(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_RelatedProducts WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearSubjectProducts(SubjectType subjectType)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_SubjectProducts WHERE SubjectType = @SubjectType AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, (int)subjectType);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Category_Create");
            database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, category.DisplaySequence);
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                database.AddInParameter(storedProcCommand, "Meta_Title", DbType.String, category.MetaTitle);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, category.MetaDescription);
            }
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            }
            if (!string.IsNullOrEmpty(category.Description))
            {
                database.AddInParameter(storedProcCommand, "Description", DbType.String, category.Description);
            }
            if (!string.IsNullOrEmpty(category.Notes1))
            {
                database.AddInParameter(storedProcCommand, "Notes1", DbType.String, category.Notes1);
            }
            if (!string.IsNullOrEmpty(category.Notes2))
            {
                database.AddInParameter(storedProcCommand, "Notes2", DbType.String, category.Notes2);
            }
            if (!string.IsNullOrEmpty(category.Notes3))
            {
                database.AddInParameter(storedProcCommand, "Notes3", DbType.String, category.Notes3);
            }
            if (!string.IsNullOrEmpty(category.Notes4))
            {
                database.AddInParameter(storedProcCommand, "Notes4", DbType.String, category.Notes4);
            }
            if (!string.IsNullOrEmpty(category.Notes5))
            {
                database.AddInParameter(storedProcCommand, "Notes5", DbType.String, category.Notes5);
            }
            if (category.ParentCategoryId.HasValue)
            {
                database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, category.ParentCategoryId.Value);
            }
            else
            {
                database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, 0);
            }
            if (category.AssociatedProductType.HasValue)
            {
                database.AddInParameter(storedProcCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType.Value);
            }
            if (!string.IsNullOrEmpty(category.RewriteName))
            {
                database.AddInParameter(storedProcCommand, "RewriteName", DbType.String, category.RewriteName);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (int)database.GetParameterValue(storedProcCommand, "CategoryId");
        }

        public override CategoryActionStatus DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Category_Delete");
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.ExecuteNonQuery(storedProcCommand);
            return (CategoryActionStatus)((int)database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override int DeleteProducts(string productIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("DELETE FROM distro_Products WHERE DistributorUserId = {0} AND ProductId IN ({1})", HiContext.Current.User.UserId, productIds) + string.Format(" DELETE FROM distro_RelatedProducts WHERE DistributorUserId = {0} AND (ProductId IN ({1}) OR RelatedProductId IN ({1}))", HiContext.Current.User.UserId, productIds));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Products SET CategoryId=@newCategory,MainCategoryPath=(SELECT Path FROM distro_Categories WHERE CategoryId=@newCategory AND DistributorUserId=@DistributorUserId)+'|' WHERE CategoryId=@oldCategoryId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DownloadProduct(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_Products (CategoryId, TypeId, ProductId, DistributorUserId, ProductCode, ProductName, ShortDescription, Description,  Title, Meta_Description, Meta_Keywords,  MarketPrice, SaleStatus, AddedDate,VistiCounts, SaleCounts, DisplaySequence, LineId, BrandId, ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, HasSKU) SELECT 0, TypeId, ProductId, @DistributorUserId, ProductCode, ProductName, ShortDescription,Description,  Title, Meta_Description, Meta_Keywords, MarketPrice, 3, AddedDate,0, 0, DisplaySequence, LineId, BrandId, ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, HasSKU FROM Hishop_Products WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override DbQueryResult GetAlertProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_SKUs WHERE Stock<=AlertStock GROUP BY ProductId)", new object[0]);
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                builder.AppendFormat(" AND SaleStatus = {0} ", (int)query.SaleStatus);
            }
            else
            {
                builder.AppendFormat(" AND SaleStatus!={0} ", 0);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,  MarketPrice, Stock, DisplaySequence,SaleStatus," + string.Format(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice,LowestSalePrice,", user.UserId) + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", user.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", user.GradeId) + string.Format(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override IList<ProductLineInfo> GetAuthorizeProductLineList()
        {
            IList<ProductLineInfo> list = new List<ProductLineInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines pl join Hishop_DistributorProductLines dpl on dpl.LineId=pl.LineId WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateProductLine(reader));
                }
            }
            return list;
        }

        public override DataTable GetAuthorizeProductLines()
        {
            DataTable table;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT pl.LineId,Name,(SELECT count(*) From Hishop_Products WHERE LineId=pl.LineId AND PenetrationStatus = 1) AS ProductCount FROM Hishop_DistributorProductLines dpl join Hishop_ProductLines pl on dpl.LineId=pl.LineId WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload)
        {
            StringBuilder builder = new StringBuilder();
            if (onlyNotDownload)
            {
                builder.AppendFormat("ProductId NOT IN (SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0}) AND", HiContext.Current.User.UserId);
            }
            builder.AppendFormat(" PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) And SaleStatus<>{1}", HiContext.Current.User.UserId, 0);
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.ProductLineId.HasValue)
            {
                builder.AppendFormat(" AND LineId = {0}", query.ProductLineId);
            }
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,LowestSalePrice, Stock, DisplaySequence,");
            builder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder2.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", user.GradeId);
            builder2.AppendFormat(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), builder2.ToString());
        }

        public override DataTable GetCategories()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, NULL AS SKUPrefix FROM distro_Categories WHERE DistributorUserId = @DistributorUserId ORDER BY DisplaySequence");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        int GetDistributorDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Discount FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            StringBuilder builder = new StringBuilder("");
            builder.AppendFormat("PenetrationStatus = 1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = {0})", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                builder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
            }
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT [ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [LowestSalePrice], [PenetrationStatus], [HasSKU] ").AppendFormat("FROM Hishop_Products WHERE PenetrationStatus = 1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = {0})", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                builder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
            }
            builder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Product_GetExportList");
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, builder.ToString());
            return database.ExecuteDataSet(storedProcCommand);
        }

        public override DataTable GetGroupBuyProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder(string.Format(" WHERE DistributorUserId = {0}", HiContext.Current.User.UserId));
            builder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId,ProductName FROM distro_Products" + builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ProductInfo GetProduct(int productId)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int discount = SubsiteStoreProvider.Instance().GetDistributorGradeInfo(user.GradeId).Discount;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT dp.*, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5, p.LowestSalePrice, p.PenetrationStatus FROM distro_Products dp join Hishop_Products p ON dp.ProductId = p.ProductId WHERE dp.DistributorUserId = @DistributorUserId AND dp.ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            ProductInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info = DataMapper.PopulateSubProduct(reader);
                if (DBNull.Value != reader["ImageUrl1"])
                {
                    info.ImageUrl1 = (string)reader["ImageUrl1"];
                }
                if (DBNull.Value != reader["ImageUrl2"])
                {
                    info.ImageUrl2 = (string)reader["ImageUrl2"];
                }
                if (DBNull.Value != reader["ImageUrl3"])
                {
                    info.ImageUrl3 = (string)reader["ImageUrl3"];
                }
                if (DBNull.Value != reader["ImageUrl4"])
                {
                    info.ImageUrl4 = (string)reader["ImageUrl4"];
                }
                if (DBNull.Value != reader["ImageUrl5"])
                {
                    info.ImageUrl5 = (string)reader["ImageUrl5"];
                }
                if (DBNull.Value != reader["Unit"])
                {
                    info.Unit = (string)reader["Unit"];
                }
            }
            return info;
        }

        public override DataTable GetProductAttribute(int productId)
        {
            DataTable table = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                DataTable table2 = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table2 == null) || (table2.Rows.Count <= 0))
                {
                    return table;
                }
                DataTable table3 = table2.Clone();
                foreach (DataRow row in table2.Rows)
                {
                    bool flag = false;
                    if (table3.Rows.Count > 0)
                    {
                        foreach (DataRow row2 in table3.Rows)
                        {
                            if (((int)row2["AttributeId"]) == ((int)row["AttributeId"]))
                            {
                                DataRow row4;
                                flag = true;
                                (row4 = row2)["ValueStr"] = row4["ValueStr"] + ", " + row["ValueStr"];
                            }
                        }
                    }
                    if (!flag)
                    {
                        DataRow row3 = table3.NewRow();
                        row3["AttributeId"] = row["AttributeId"];
                        row3["AttributeName"] = row["AttributeName"];
                        row3["ValueStr"] = row["ValueStr"];
                        table3.Rows.Add(row3);
                    }
                }
                return table3;
            }
        }

        public override IList<int> GetProductIds(ProductQuery query)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(SubsiteProductProvider.BuildProductQuery(query));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int)reader["ProductId"]);
                }
            }
            return list;
        }

        public override DbQueryResult GetProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND SaleStatus = {0} ", (int)query.SaleStatus);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,  MarketPrice, Stock, DisplaySequence," + string.Format(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice,LowestSalePrice,", user.UserId) + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", user.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", user.GradeId) + string.Format(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            IList<ProductInfo> list = new List<ProductInfo>();
            string str = "(";
            foreach (int num in productIds)
            {
                str = str + num + ",";
            }
            if (str.Length > 1)
            {
                Distributor user = HiContext.Current.User as Distributor;
                int distributorDiscount = GetDistributorDiscount(user.GradeId);
                DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT  PurchasePrice*{0}/100  AS PurchasePrice,0 as PenetrationStatus,*  FROM distro_Products WHERE ProductId IN ", distributorDiscount) + (str.Substring(0, str.Length - 1) + ")") + " AND DistributorUserId=@DistributorUserId");
                database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(DataMapper.PopulateSubProduct(reader));
                    }
                }
            }
            return list;
        }

        public override DataTable GetProductSKU(int productId)
        {
            DataTable table;
            DataTable table2;
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT SkuId, (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS 一口价,", HiContext.Current.User.UserId) + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS '采购价',", user.GradeId, distributorDiscount) + " AlertStock AS '警戒库存', Stock AS '库存', Weight AS '重量', SKU AS '货号' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId,AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                if ((table2 == null) || (table2.Rows.Count <= 0))
                {
                    return table;
                }
                foreach (DataRow row in table2.Rows)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = (string)row["AttributeName"];
                    if (!table.Columns.Contains(column.ColumnName))
                    {
                        table.Columns.Add(column);
                    }
                }
                foreach (DataRow row2 in table.Rows)
                {
                    foreach (DataRow row in table2.Rows)
                    {
                        if (string.Compare((string)row2["SkuId"], (string)row["SkuId"]) == 0)
                        {
                            if (((bool)row["UseAttributeImage"]) && (row["ImageUrl"] != DBNull.Value))
                            {
                                row2[(string)row["AttributeName"]] = row["ImageUrl"];
                            }
                            else
                            {
                                row2[(string)row["AttributeName"]] = row["ValueStr"];
                            }
                        }
                    }
                }
            }
            return table;
        }

        public override DataTable GetPubTaoBaoProducts(string productIds)
        {
            int userId = HiContext.Current.User.UserId;
            string query = string.Format("select * from Taobao_DistroProducts   WHERE DistributorUserId = {0} AND ProductId  = {1};", userId, productIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetPuchaseProduct(string skuId)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU," + string.Format(" SalePrice, PurchasePrice*{0}/100 AS PurchasePrice, Stock, hp.DisplaySequence", distributorDiscount) + " FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId WHERE SkuId = @SkuId");
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetPuchaseProducts(ProductQuery query, out int count)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("AND hp.PenetrationStatus=1 AND hp.LineId IN (SELECT hd.LineId FROM Hishop_DistributorProductLines hd WHERE hd.UserId={0})", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!(string.IsNullOrEmpty(query.Keywords) || (query.PageSize != 1)))
            {
                builder.AppendFormat(" AND hp.ProductName = '{0}'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (!(string.IsNullOrEmpty(query.Keywords) || (query.PageSize == 1)))
            {
                builder.AppendFormat(" AND hp.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.ProductLineId.HasValue)
            {
                builder.AppendFormat(" AND hp.LineId = {0}", query.ProductLineId);
            }
            StringBuilder builder2 = new StringBuilder();
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            if (query.PageIndex == 1)
            {
                builder2.AppendFormat("SELECT TOP {0} hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU, CostPrice,MarketPrice,  SalePrice, PurchasePrice*{1}/100 AS PurchasePrice, LowestSalePrice, Stock, hp.DisplaySequence  FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId WHERE 1=1", query.PageSize, distributorDiscount);
            }
            else
            {
                builder2.AppendFormat("SELECT TOP {0} hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU, CostPrice,MarketPrice,  SalePrice, PurchasePrice*{1}/100 AS PurchasePrice, LowestSalePrice, Stock, hp.DisplaySequence FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where SkuId NOT IN(SELECT top {2} SkuId FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where 1=1 {3})", new object[] { query.PageSize, distributorDiscount, query.PageSize * (query.PageIndex - 1), builder });
            }
            builder2.Append(builder.ToString());
            builder2.AppendFormat(";SELECT COUNT(*) as count FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where 1=1 {0}", builder);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder2.ToString());
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    count = (int)reader["count"];
                    return table;
                }
                count = 0;
            }
            return table;
        }

        public override DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            Distributor user = HiContext.Current.User as Distributor;
            builder.AppendFormat(" SaleStatus = {0} AND DistributorUserId = {1}", 1, user.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {0} AND DistributorUserId = {1})", productId, user.UserId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, Stock, DisplaySequence,";
            selectFields = selectFields + string.Format(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", user.UserId);
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataTable GetSkuContentBySku(string skuId)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT s.SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", user.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice,", user.GradeId, distributorDiscount);
            builder.Append(" (SELECT ProductName FROM Hishop_Products WHERE ProductId = s.ProductId) AS ProductName,");
            builder.Append(" (SELECT ThumbnailUrl40 FROM Hishop_Products WHERE ProductId = s.ProductId) AS ThumbnailUrl40,AttributeName, ValueStr");
            builder.Append(" FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId");
            builder.Append(" left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override string GetSkuIdByTaobao(long taobaoProductId, string taobaoSkuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = @DistributorUserId AND TaobaoProductId = @TaobaoProductId)");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "TaobaoProductId", DbType.Int64, taobaoProductId);
            string str = string.Empty;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    str = (string)reader["SkuId"];
                    if (taobaoSkuId.ToLower() == str.ToLower())
                    {
                        return str;
                    }
                }
            }
            return str;
        }

        public override IList<SKUItem> GetSkus(string productIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_SKUs WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
            IList<SKUItem> list = new List<SKUItem>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateSKU(reader));
                }
            }
            return list;
        }

        public override DataTable GetSkusByProductId(int productId)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", user.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", user.GradeId, distributorDiscount);
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSkuUnderlingPrices(string productIds)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductName, SKU, MarketPrice,");
            builder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS SalePrice,", user.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", user.GradeId, distributorDiscount);
            builder.AppendFormat(" FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            builder.AppendFormat(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM distro_MemberGrades WHERE CreateUserId = {0}", user.UserId);
            builder.AppendFormat(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM distro_MemberGrades WHERE GradeId = sm.GradeId AND CreateUserId = {0}) AS MemberGradeName", user.UserId);
            builder.AppendFormat(" ,  MemberSalePrice FROM distro_SKUMemberPrice sm WHERE GradeId <> 0 AND  DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", user.UserId, DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            DataTable table = null;
            DataTable table2 = null;
            DataTable table3 = null;
            DataTable table4 = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    table.Columns.Add("SKUContent");
                    reader.NextResult();
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                    reader.NextResult();
                    table4 = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table4 != null) && (table4.Rows.Count > 0))
                    {
                        foreach (DataRow row in table4.Rows)
                        {
                            table.Columns.Add((string)row["MemberGradeName"]);
                        }
                    }
                    while (reader.Read())
                    {
                        table.Columns.Add((string)reader["MemberGradeName"]);
                    }
                    reader.NextResult();
                    table3 = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    string str = string.Empty;
                    foreach (DataRow row3 in table2.Rows)
                    {
                        if (((string)row2["SkuId"]) == ((string)row3["SkuId"]))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, row3["AttributeName"], "：", row3["ValueStr"], "; " });
                        }
                    }
                    row2["SKUContent"] = str;
                }
            }
            if ((table3 != null) && (table3.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    foreach (DataRow row4 in table3.Rows)
                    {
                        if (((string)row2["SkuId"]) == ((string)row4["SkuId"]))
                        {
                            row2[(string)row4["MemberGradeName"]] = (decimal)row4["MemberSalePrice"];
                        }
                    }
                }
            }
            if ((table4 != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    decimal num2 = decimal.Parse(row2["SalePrice"].ToString());
                    foreach (DataRow row5 in table4.Rows)
                    {
                        decimal num3 = decimal.Parse(row5["Discount"].ToString());
                        string str2 = (num2 * (num3 / 100M)).ToString("F2");
                        row2[(string)row5["MemberGradeName"]] = row2[(string)row5["MemberGradeName"]] + "|" + str2;
                    }
                }
            }
            return table;
        }

        public override DbQueryResult GetSubjectProducts(SubjectType subjectType, Pagination page)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM distro_SubjectProducts WHERE SubjectType = {0} AND DistributorUserId = {1}) ", (int)subjectType, HiContext.Current.User.UserId);
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,  MarketPrice, SalePrice, Stock, DisplaySequence," + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", user.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", user.GradeId) + string.Format(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DbQueryResult GetSubmitPuchaseProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) AND SaleStatus<>{1} ", HiContext.Current.User.UserId, 0);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Products", "ProductId", builder.ToString(), "ProductId, ProductCode, ProductName,ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, MarketPrice,DisplaySequence,LowestSalePrice, PenetrationStatus");
        }

        public override DataTable GetTaobaoProducts(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            int userId = HiContext.Current.User.UserId;
            builder.Append("SELECT tp.*, b.taobaoproductid,p.ProductCode, p.Description, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,");
            builder.Append(" (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,");
            builder.Append(" (SELECT MIN(Weight) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Weight");
            builder.Append(" FROM Hishop_Products p JOIN Taobao_Products tp ON p.ProductId = tp.ProductId");
            builder.AppendFormat(" left join (select * from taobao_distroproducts where distributoruserid={0}) b", userId);
            builder.Append(" on b.productid=tp.productid");
            builder.AppendFormat("  WHERE p.ProductId IN ({0})   ", productIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetToTaobaoProducts(ProductQuery query)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("ProductId IN (SELECT ProductId FROM Taobao_Products)");
            builder.AppendFormat(" AND PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) And SaleStatus<>{1}", HiContext.Current.User.UserId, 0);
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            if (query.ProductLineId.HasValue)
            {
                builder.AppendFormat(" AND LineId = {0}", query.ProductLineId);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.PublishStatus == PublishStatus.Already)
            {
                builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0})", user.UserId);
            }
            else if (query.PublishStatus == PublishStatus.Notyet)
            {
                builder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0})", user.UserId);
            }
            else if (query.PublishStatus == PublishStatus.Update)
            {
                builder.AppendFormat(" AND ProductId  IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0} and updatestatus=1)", user.UserId);
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,LowestSalePrice, Stock, DisplaySequence,");
            builder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder2.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", user.GradeId);
            builder2.AppendFormat(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice,", distributorDiscount);
            builder2.AppendFormat(" (SELECT updatestatus FROM Taobao_DistroProducts WHERE DistributorUserId ={0} AND ProductId = p.ProductId) AS IsPublish", user.UserId);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), builder2.ToString());
        }

        public override DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId = {0} AND ProductName LIKE '%{1}%'", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.Keywords));
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            else
            {
                builder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList", "ProductId", builder.ToString(), string.Format("CategoryId,MainCategoryPath,ExtendCategoryPath, ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence", distributorDiscount));
        }

        public override int GetUpProducts()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Count(*) FROM Taobao_DistroProducts WHERE updatestatus=1 and DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override bool IsOnSale(string productIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products p WHERE ProductId IN ({0}) AND LowestSalePrice <= (SELECT MIN(SalePrice) FROM vw_distro_SkuPrices", productIds) + string.Format(" WHERE DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_Skus WHERE ProductId = p.ProductId))", HiContext.Current.User.UserId));
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveSubjectProduct(SubjectType subjectType, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_SubjectProducts WHERE SubjectType = @SubjectType AND ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
            int num = (int)subjectType;
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, num);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE DistributorUserId = {2} AND ProductId IN ({3})", new object[] { DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), HiContext.Current.User.UserId, productIds }));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SwapCategorySequence(int categoryId, CategoryZIndex zIndex)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Category_SwapDisplaySequence");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(storedProcCommand, "ZIndex", DbType.Int32, (int)zIndex);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Categories SET [Name] = @Name, Meta_Description = @Meta_Description,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Keywords = @Meta_Keywords, Description = @Description, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName WHERE CategoryId = @CategoryId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            database.AddInParameter(sqlStringCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType);
            database.AddInParameter(sqlStringCommand, "Meta_Title", DbType.String, category.MetaTitle);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, category.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, category.Description);
            database.AddInParameter(sqlStringCommand, "Notes1", DbType.String, category.Notes1);
            database.AddInParameter(sqlStringCommand, "Notes2", DbType.String, category.Notes2);
            database.AddInParameter(sqlStringCommand, "Notes3", DbType.String, category.Notes3);
            database.AddInParameter(sqlStringCommand, "Notes4", DbType.String, category.Notes4);
            database.AddInParameter(sqlStringCommand, "Notes5", DbType.String, category.Notes5);
            database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, category.RewriteName);
            return ((database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError);
        }

        public override bool UpdateProduct(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Product_Update");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int)product.SaleStatus);
            database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Currency, product.DisplaySequence);
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, product.ProductId);
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override bool UpdateProductCategory(int productId, int newCategoryId, string maiCategoryPath)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE DistributorUserId = @DistributorUserId AND ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, newCategoryId);
            database.AddInParameter(sqlStringCommand, "MainCategoryPath", DbType.String, maiCategoryPath);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE DistributorUserId = {2} AND ProductId IN ({3})", new object[] { DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), HiContext.Current.User.UserId, productIds }));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET SaleStatus = {0} WHERE DistributorUserId = {1} AND ProductId IN ({2})", (int)saleStatus, HiContext.Current.User.UserId, productIds));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateSkuUnderlingPrices(DataSet ds, string skuIds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable table = ds.Tables["skuPriceTable"];
            DataTable table2 = ds.Tables["skuMemberPriceTable"];
            builder.AppendFormat(" DELETE FROM distro_SKUMemberPrice WHERE DistributoruserId = {0} AND SkuId IN ({1}) ", HiContext.Current.User.UserId, skuIds);
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, 0, {2})", row["skuId"], HiContext.Current.User.UserId, row["salePrice"]);
                }
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table2.Rows)
                {
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2}, {3})", new object[] { row["skuId"], HiContext.Current.User.UserId, row["gradeId"], row["memberPrice"] });
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) >= 0);
        }

        public override bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -3)
            {
                builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId, {1} AS MemberSalePrice", HiContext.Current.User.UserId, price);
                builder.AppendFormat(" FROM Hishop_SKUs WHERE SalePrice <> {0} AND ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = {0} AND DistributoruserId = {1} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({2}))", gradeId, HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId, {2} AS MemberSalePrice", HiContext.Current.User.UserId, gradeId, price);
                builder.AppendFormat(" FROM Hishop_SKUs WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
        {
            Distributor user = HiContext.Current.User as Distributor;
            int distributorDiscount = GetDistributorDiscount(user.GradeId);
            StringBuilder builder = new StringBuilder();
            if (gradeId == -3)
            {
                if (basePriceName == "PurchasePrice")
                {
                    builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId,", user.UserId);
                    builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                    builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3} AS MemberSalePrice", new object[] { user.GradeId, distributorDiscount, operation, price });
                    builder.AppendFormat(" FROM Hishop_SKUs s WHERE SalePrice <>", new object[0]);
                    builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                    builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3}", new object[] { user.GradeId, distributorDiscount, operation, price });
                    builder.AppendFormat(" AND ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
                }
                else if (basePriceName == "SalePrice")
                {
                    builder.AppendFormat("  SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId,", user.UserId);
                    builder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2} AS MemberSalePrice", user.UserId, operation, price);
                    builder.AppendFormat(" INTO #myTemp FROM Hishop_SKUs s WHERE SalePrice <> (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2}", user.UserId, operation, price);
                    builder.AppendFormat(" AND ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
                    builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT * FROM #myTemp", new object[0]);
                    builder.Append(" DROP TABLE #myTemp");
                }
            }
            else
            {
                builder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId ={0} AND DistributoruserId = {1} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({2}))", gradeId, user.UserId, DataHelper.CleanSearchString(productIds));
                if (basePriceName == "PurchasePrice")
                {
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId,", user.UserId, gradeId);
                    builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                    builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3} AS MemberSalePrice", new object[] { user.GradeId, distributorDiscount, operation, price });
                    builder.AppendFormat(" FROM Hishop_SKUs s WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
                }
                else if (basePriceName == "SalePrice")
                {
                    builder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId,", user.UserId, gradeId);
                    builder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2} AS MemberSalePrice", user.UserId, operation, price);
                    builder.AppendFormat(" FROM Hishop_SKUs s WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

