using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.ControlPanel.Data
{
    public class ProductData : ProductProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
            database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((attribute.AttributeValues.Count != 0) && (obj2 != null))
            {
                int num = Convert.ToInt32(obj2);
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    database.AddInParameter(command, "AttributeId", DbType.Int32, num);
                    database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    database.ExecuteNonQuery(command);
                }
            }
            return (obj2 != null);
        }

        public override bool AddAttributeName(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);");
            database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
            database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddAttributeValue(AttributeValueInfo attributeValue)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeValue.AttributeId);
            database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BrandCategories;INSERT INTO Hishop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override void AddBrandProductTypes(int brandId, IList<int> productTypes)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            foreach (int num in productTypes)
            {
                database.SetParameterValue(sqlStringCommand, "ProductTypeId", num);
                database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override int AddProduct(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Product_Create");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int)product.SaleStatus);
            database.AddInParameter(storedProcCommand, "AddedDate", DbType.DateTime, product.AddedDate);
            database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, product.LineId);
            database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            database.AddInParameter(storedProcCommand, "LowestSalePrice", DbType.Currency, product.LowestSalePrice);
            database.AddInParameter(storedProcCommand, "PenetrationStatus", DbType.Int16, (int)product.PenetrationStatus);
            database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            database.AddOutParameter(storedProcCommand, "ProductId", DbType.Int32, 4);
            database.ExecuteNonQuery(storedProcCommand, dbTran);
            return (int)database.GetParameterValue(storedProcCommand, "ProductId");
        }

        public override bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM Hishop_ProductAttributes WHERE ProductId = {0};", productId);
            int num = 0;
            if (attributes != null)
            {
                foreach (int num2 in attributes.Keys)
                {
                    foreach (int num3 in attributes[num2])
                    {
                        num++;
                        builder.AppendFormat(" INSERT INTO Hishop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, num2, num3);
                    }
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            if (dbTran == null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand) >= 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
        }

        public override bool AddProductLine(ProductLineInfo productLine)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ProductLines(Name, SupplierName) VALUES(@Name, @SupplierName)");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, productLine.Name);
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, productLine.SupplierName);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_SKUs(SkuId, ProductId, SKU, Weight, Stock, AlertStock, CostPrice, SalePrice, PurchasePrice) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock, @AlertStock, @CostPrice, @SalePrice, @PurchasePrice)");
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "SKU", DbType.String);
            database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "AlertStock", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency);
            database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Currency);
            database.AddInParameter(sqlStringCommand, "PurchasePrice", DbType.Currency);
            DbCommand command = database.GetSqlStringCommand("INSERT INTO Hishop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
            database.AddInParameter(command, "SkuId", DbType.String);
            database.AddInParameter(command, "AttributeId", DbType.Int32);
            database.AddInParameter(command, "ValueId", DbType.Int32);
            DbCommand command3 = database.GetSqlStringCommand("INSERT INTO Hishop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
            database.AddInParameter(command3, "SkuId", DbType.String);
            database.AddInParameter(command3, "GradeId", DbType.Int32);
            database.AddInParameter(command3, "MemberSalePrice", DbType.Currency);
            DbCommand command4 = database.GetSqlStringCommand("INSERT INTO Hishop_SKUDistributorPrice(SkuId, GradeId, DistributorPurchasePrice) VALUES(@SkuId, @GradeId, @DistributorPurchasePrice)");
            database.AddInParameter(command4, "SkuId", DbType.String);
            database.AddInParameter(command4, "GradeId", DbType.Int32);
            database.AddInParameter(command4, "DistributorPurchasePrice", DbType.Currency);
            try
            {
                database.SetParameterValue(sqlStringCommand, "ProductId", productId);
                foreach (SKUItem item in skus.Values)
                {
                    string str = productId.ToString(CultureInfo.InvariantCulture) + "_" + item.SkuId;
                    database.SetParameterValue(sqlStringCommand, "SkuId", str);
                    database.SetParameterValue(sqlStringCommand, "SKU", item.SKU);
                    database.SetParameterValue(sqlStringCommand, "Weight", item.Weight);
                    database.SetParameterValue(sqlStringCommand, "Stock", item.Stock);
                    database.SetParameterValue(sqlStringCommand, "AlertStock", item.AlertStock);
                    database.SetParameterValue(sqlStringCommand, "CostPrice", item.CostPrice);
                    database.SetParameterValue(sqlStringCommand, "SalePrice", item.SalePrice);
                    database.SetParameterValue(sqlStringCommand, "PurchasePrice", item.PurchasePrice);
                    if (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
                    {
                        return false;
                    }
                    database.SetParameterValue(command, "SkuId", str);
                    foreach (int num in item.SkuItems.Keys)
                    {
                        database.SetParameterValue(command, "AttributeId", num);
                        database.SetParameterValue(command, "ValueId", item.SkuItems[num]);
                        database.ExecuteNonQuery(command, dbTran);
                    }
                    database.SetParameterValue(command3, "SkuId", str);
                    foreach (int num2 in item.MemberPrices.Keys)
                    {
                        database.SetParameterValue(command3, "GradeId", num2);
                        database.SetParameterValue(command3, "MemberSalePrice", item.MemberPrices[num2]);
                        database.ExecuteNonQuery(command3, dbTran);
                    }
                    database.SetParameterValue(command4, "SkuId", str);
                    foreach (int num2 in item.DistributorPrices.Keys)
                    {
                        database.SetParameterValue(command4, "GradeId", num2);
                        database.SetParameterValue(command4, "DistributorPurchasePrice", item.DistributorPrices[num2]);
                        database.ExecuteNonQuery(command4, dbTran);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int AddProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override void AddProductTypeBrands(int typeId, IList<int> brands)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
            foreach (int num in brands)
            {
                database.SetParameterValue(sqlStringCommand, "BrandId", num);
                database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override bool AddRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_RelatedProducts(ProductId, RelatedProductId) VALUES (@ProductId, @RelatedProductId)");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
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

        public override bool AddSkuStock(string productIds, int addStock)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_SubjectProducts(SubjectType, ProductId) VALUES (@SubjectType, @ProductId)");
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

        public override bool AddSupplier(string supplierName, string remark)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("IF (SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)) = 0 INSERT INTO Hishop_Suppliers(SupplierName, Remark) VALUES(@SupplierName, @Remark)");
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool BrandHvaeProducts(int brandId)
        {
            bool flag = false;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = reader.GetInt32(0) > 0;
                }
            }
            return flag;
        }

        static string BuildProductQuery(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT p.ProductId FROM Hishop_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
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
                builder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY p.{0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public override int CanclePenetrationProducts(IList<int> productIds, DbTransaction dbTran)
        {
            string str = "(";
            foreach (int num in productIds)
            {
                str = str + num + ",";
            }
            if (str.Length == 1)
            {
                return 0;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Products SET PenetrationStatus = 2 WHERE ProductId IN " + (str.Substring(0, str.Length - 1) + ")"));
            if (dbTran != null)
            {
                return database.ExecuteNonQuery(sqlStringCommand, dbTran);
            }
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND {1} - {2} < 0", DataHelper.CleanSearchString(productIds), basePriceName, checkPrice));
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ClearAttributeValue(int attributeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearRelatedProducts(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearSubjectProducts(SubjectType subjectType)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_SubjectProducts WHERE SubjectType = @SubjectType");
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, (int)subjectType);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Category_Create");
            database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            database.AddInParameter(storedProcCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
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

        public override bool DeleteAttribute(int attributeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteAttribute(int attributeId, int valueId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId=@ValueId AND AttributeId=@AttributeId;DELETE FROM Hishop_ProductAttributes WHERE AttributeId=@AttributeId AND ValueId=@ValueId");
            database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteAttributeValue(int attributeValueId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId");
            database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValueId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteBrandCategory(int brandId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteBrandProductTypes(int brandId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            try
            {
                database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteCanclePenetrationProducts(IList<int> productIds, DbTransaction dbTran)
        {
            try
            {
                string str = "(";
                foreach (int num in productIds)
                {
                    str = str + num + ",";
                }
                if (str.Length == 1)
                {
                    return false;
                }
                DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_Products WHERE ProductId IN " + (str.Substring(0, str.Length - 1) + ")"));
                if (dbTran != null)
                {
                    database.ExecuteNonQuery(sqlStringCommand, dbTran);
                }
                else
                {
                    database.ExecuteNonQuery(sqlStringCommand);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override CategoryActionStatus DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Category_Delete");
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.ExecuteNonQuery(storedProcCommand);
            return (CategoryActionStatus)((int)database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override void DeleteNotinProductLines(int distributorUserId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_Products WHERE DistributorUserId=@DistributorUserId AND LineId NOT IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId=@DistributorUserId)");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, distributorUserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProduct(string productIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Products WHERE ProductId IN ({0}); DELETE FROM Hishop_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0})", productIds) + string.Format(" DELETE FROM distro_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0})", productIds));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteProductLine(int lineId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductLine_Delete");
            database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, lineId);
            return (database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override bool DeleteProductSKUS(int productId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_SKUs WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            try
            {
                if (dbTran == null)
                {
                    database.ExecuteNonQuery(sqlStringCommand);
                }
                else
                {
                    database.ExecuteNonQuery(sqlStringCommand, dbTran);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteProductTypeBrands(int typeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
            database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            try
            {
                database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteProducType(int typeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void DeleteSkuUnderlingPrice()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_SKUMemberPrice WHERE SkuId NOT IN (SELECT SkuId FROM Hishop_SKUs)");
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeleteSupplier(string supplierName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
            database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void EnsureMapping(DataSet mappingSet)
        {
            using (DbCommand command = database.GetSqlStringCommand("INSERT INTO  Hishop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
            {
                database.AddInParameter(command, "TypeName", DbType.String);
                database.AddInParameter(command, "Remark", DbType.String);
                DataRow[] rowArray = mappingSet.Tables["types"].Select("SelectedTypeId=0");
                foreach (DataRow row in rowArray)
                {
                    database.SetParameterValue(command, "TypeName", row["TypeName"]);
                    database.SetParameterValue(command, "Remark", row["Remark"]);
                    row["SelectedTypeId"] = database.ExecuteScalar(command);
                }
            }
            using (DbCommand command2 = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
            {
                database.AddInParameter(command2, "AttributeName", DbType.String);
                database.AddInParameter(command2, "TypeId", DbType.Int32);
                database.AddInParameter(command2, "UsageMode", DbType.Int32);
                database.AddInParameter(command2, "UseAttributeImage", DbType.Boolean);
                DataRow[] rowArray2 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
                foreach (DataRow row2 in rowArray2)
                {
                    int num = (int)mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", row2["MappedTypeId"]))[0]["SelectedTypeId"];
                    database.SetParameterValue(command2, "AttributeName", row2["AttributeName"]);
                    database.SetParameterValue(command2, "TypeId", num);
                    database.SetParameterValue(command2, "UsageMode", int.Parse(row2["UsageMode"].ToString()));
                    database.SetParameterValue(command2, "UseAttributeImage", bool.Parse(row2["UseAttributeImage"].ToString()));
                    row2["SelectedAttributeId"] = database.ExecuteScalar(command2);
                }
            }
            using (DbCommand command3 = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues;INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY;"))
            {
                database.AddInParameter(command3, "AttributeId", DbType.Int32);
                database.AddInParameter(command3, "ValueStr", DbType.String);
                database.AddInParameter(command3, "ImageUrl", DbType.String);
                DataRow[] rowArray3 = mappingSet.Tables["values"].Select("SelectedValueId=0");
                foreach (DataRow row3 in rowArray3)
                {
                    int num2 = (int)mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", row3["MappedAttributeId"]))[0]["SelectedAttributeId"];
                    database.SetParameterValue(command3, "AttributeId", num2);
                    database.SetParameterValue(command3, "ValueStr", row3["ValueStr"]);
                    database.SetParameterValue(command3, "ImageUrl", row3["ImageUrl"]);
                    row3["SelectedValueId"] = database.ExecuteScalar(command3);
                }
            }
            mappingSet.AcceptChanges();
        }

        public override DbQueryResult GetAlertProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" ProductId IN (SELECT ProductId FROM Hishop_SKUs WHERE Stock<=AlertStock GROUP BY ProductId) ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                builder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                builder.AppendFormat(" AND SaleStatus not in ({0})", 0);
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
            }
            if (query.IsMakeTaobao.HasValue && (query.IsMakeTaobao.Value >= 0))
            {
                builder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,SaleStatus,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override AttributeInfo GetAttribute(int attributeId)
        {
            AttributeInfo info = new AttributeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE AttributeId = @AttributeId;");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
                while (reader.Read())
                {
                    AttributeValueInfo item = new AttributeValueInfo();
                    item.ValueId = (int)reader["ValueId"];
                    item.AttributeId = (int)reader["AttributeId"];
                    item.DisplaySequence = (int)reader["DisplaySequence"];
                    item.ValueStr = (string)reader["ValueStr"];
                    if (reader["ImageUrl"] != DBNull.Value)
                    {
                        item.ImageUrl = (string)reader["ImageUrl"];
                    }
                    list.Add(item);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                if (reader.Read())
                {
                    info.AttributeId = (int)reader["AttributeId"];
                    info.AttributeName = (string)reader["AttributeName"];
                    info.DisplaySequence = (int)reader["DisplaySequence"];
                    info.TypeId = (int)reader["TypeId"];
                    info.UsageMode = (AttributeUseageMode)((int)reader["UsageMode"]);
                    info.UseAttributeImage = (bool)reader["UseAttributeImage"];
                    info.AttributeValues = list;
                }
            }
            return info;
        }

        public override IList<AttributeInfo> GetAttributes(int typeId)
        {
            IList<AttributeInfo> list = new List<AttributeInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo();
                    item.AttributeId = (int)row["AttributeId"];
                    item.AttributeName = (string)row["AttributeName"];
                    item.DisplaySequence = (int)row["DisplaySequence"];
                    item.TypeId = (int)row["TypeId"];
                    item.UsageMode = (AttributeUseageMode)((int)row["UsageMode"]);
                    item.UseAttributeImage = (bool)row["UseAttributeImage"];
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString(CultureInfo.InvariantCulture));
                        foreach (DataRow row2 in rowArray)
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo();
                            info2.ValueId = (int)row2["ValueId"];
                            info2.AttributeId = item.AttributeId;
                            info2.ValueStr = (string)row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
        {
            string str;
            IList<AttributeInfo> list = new List<AttributeInfo>();
            if (attributeUseageMode == AttributeUseageMode.Choose)
            {
                str = "UsageMode = 2";
            }
            else
            {
                str = "UsageMode <> 2";
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId AND " + str + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId AND  " + str + " ) ORDER BY DisplaySequence Desc");
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo();
                    item.AttributeId = (int)row["AttributeId"];
                    item.AttributeName = (string)row["AttributeName"];
                    item.DisplaySequence = (int)row["DisplaySequence"];
                    item.TypeId = (int)row["TypeId"];
                    item.UsageMode = (AttributeUseageMode)((int)row["UsageMode"]);
                    item.UseAttributeImage = (bool)row["UseAttributeImage"];
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString(CultureInfo.InvariantCulture));
                        foreach (DataRow row2 in rowArray)
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo();
                            info2.ValueId = (int)row2["ValueId"];
                            info2.AttributeId = item.AttributeId;
                            if (row2["ImageUrl"] != DBNull.Value)
                            {
                                info2.ImageUrl = (string)row2["ImageUrl"];
                            }
                            info2.ValueStr = (string)row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override AttributeValueInfo GetAttributeValueInfo(int valueId)
        {
            AttributeValueInfo info = new AttributeValueInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE ValueId=@ValueId");
            database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAttributeValue(reader);
                    info.ImageUrl = reader["ImageUrl"].ToString();
                    info.DisplaySequence = (int)reader["DisplaySequence"];
                }
            }
            return info;
        }

        public override DataTable GetBrandCategories()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
            database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override BrandCategoryInfo GetBrandCategory(int brandId)
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBrandCategory(reader);
                }
                IList<int> list = new List<int>();
                reader.NextResult();
                while (reader.Read())
                {
                    list.Add((int)reader["ProductTypeId"]);
                }
                info.ProductTypes = list;
            }
            return info;
        }

        public override DataTable GetCategories()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Categories ORDER BY DisplaySequence");
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
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            if (query.IncludeOnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            builder.Remove(builder.Length - 4, 4);
            builder.Append(")");
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
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
            builder.Append("SELECT [ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [LowestSalePrice], [PenetrationStatus], [HasSKU] ").Append("FROM Hishop_Products WHERE ");
            builder.Append("(");
            if (query.IncludeOnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            builder.Remove(builder.Length - 4, 4);
            builder.Append(")");
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
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
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" WHERE SaleStatus = {0}", 1);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override int GetMaxSequence()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT MAX(DisplaySequence) FROM Hishop_Products");
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            return ((obj2 == DBNull.Value) ? 0 : ((int)obj2));
        }

        public override DataTable GetProductBaseInfo(string productIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, LowestSalePrice,ThumbnailUrl40 FROM Hishop_Products WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds)
        {
            ProductInfo info = null;
            attrs = null;
            distributorUserIds = new List<int>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.PurchasePrice, skus.Stock, skus.AlertStock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Hishop_SKUMemberPrice smp INNER JOIN Hishop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT s.SkuId, sdp.GradeId, sdp.DistributorPurchasePrice FROM Hishop_SKUDistributorPrice sdp INNER JOIN Hishop_SKUs s ON sdp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId; SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId = (SELECT LineId FROM Hishop_Products WHERE ProductId = @ProductId)");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                string str;
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
                if (info == null)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string)reader["SkuId"];
                    if (!info.Skus.ContainsKey(str))
                    {
                        SKUItem item2 = new SKUItem();
                        item2.SkuId = str;
                        item2.ProductId = productId;
                        SKUItem item = item2;
                        if (reader["SKU"] != null)
                        {
                            item.SKU = (string)reader["SKU"];
                        }
                        if (reader["Weight"] != DBNull.Value)
                        {
                            item.Weight = (int)reader["Weight"];
                        }
                        item.Stock = (int)reader["Stock"];
                        item.AlertStock = (int)reader["AlertStock"];
                        if (reader["CostPrice"] != DBNull.Value)
                        {
                            item.CostPrice = (decimal)reader["CostPrice"];
                        }
                        item.SalePrice = (decimal)reader["SalePrice"];
                        item.PurchasePrice = (decimal)reader["PurchasePrice"];
                        info.Skus.Add(str, item);
                    }
                    if ((reader["AttributeId"] != DBNull.Value) && (reader["ValueId"] != DBNull.Value))
                    {
                        info.Skus[str].SkuItems.Add((int)reader["AttributeId"], (int)reader["ValueId"]);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string)reader["SkuId"];
                    info.Skus[str].MemberPrices.Add((int)reader["GradeId"], (decimal)reader["MemberSalePrice"]);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string)reader["SkuId"];
                    info.Skus[str].DistributorPrices.Add((int)reader["GradeId"], (decimal)reader["DistributorPurchasePrice"]);
                }
                reader.NextResult();
                attrs = new Dictionary<int, IList<int>>();
                while (reader.Read())
                {
                    int key = (int)reader["AttributeId"];
                    int num2 = (int)reader["ValueId"];
                    if (!attrs.ContainsKey(key))
                    {
                        IList<int> list = new List<int>();
                        list.Add(num2);
                        attrs.Add(key, list);
                    }
                    else
                    {
                        attrs[key].Add(num2);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    distributorUserIds.Add((int)reader["UserId"]);
                }
            }
            return info;
        }

        public override IList<int> GetProductIds(ProductQuery query)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(BuildProductQuery(query));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int)reader["ProductId"]);
                }
            }
            return list;
        }

        public override ProductLineInfo GetProductLine(int lineId)
        {
            ProductLineInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines WHERE LineId=@LineId");
            database.AddInParameter(sqlStringCommand, "LineId", DbType.Int32, lineId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductLine(reader);
                }
            }
            return info;
        }

        public override IList<ProductLineInfo> GetProductLineList()
        {
            IList<ProductLineInfo> list = new List<ProductLineInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateProductLine(reader));
                }
            }
            return list;
        }

        public override DataTable GetProductLines()
        {
            DataTable table;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT LineId,Name,SupplierName,(SELECT count(*) From Hishop_Products WHERE LineId=pl.LineId AND SaleStatus<>0) AS ProductCount FROM Hishop_ProductLines pl");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override string GetProductNameByProductIds(string productIds, out int sumcount)
        {
            int num = 0;
            string str = "";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT ProductName FROM Hishop_Products WHERE PenetrationStatus=1", new object[0]);
            builder.AppendFormat(" AND SaleStatus!={0}", 0);
            builder.AppendFormat(" AND ProductId IN ({0})", productIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    str = str + ((string)reader["ProductName"]) + ",";
                    num++;
                }
            }
            if (str != "")
            {
                str = str.Substring(0, str.Length - 1);
            }
            sumcount = num;
            return str;
        }

        public override string GetProductNamesByLineId(int lineId, out int count)
        {
            int num = 0;
            string str = "";
            try
            {
                StringBuilder builder = new StringBuilder("select ProductName from Hishop_Products where PenetrationStatus=1");
                builder.AppendFormat(" and SaleStatus!={0}", 0);
                builder.AppendFormat(" and LineId={0}", lineId);
                DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        str = str + reader["ProductName"].ToString() + ",";
                        num++;
                    }
                }
                if (str != "")
                {
                    str = str.Substring(0, str.Length);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            count = num;
            return str;
        }

        public override DbQueryResult GetProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                builder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                builder.AppendFormat(" AND SaleStatus not in ({0})", 0);
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
            }
            if (query.IsMakeTaobao.HasValue && (query.IsMakeTaobao.Value >= 0))
            {
                builder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
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
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId IN " + (str.Substring(0, str.Length - 1) + ")"));
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(DataMapper.PopulateProduct(reader));
                    }
                }
            }
            return list;
        }

        public override ProductTypeInfo GetProductType(int typeId)
        {
            ProductTypeInfo info = new ProductTypeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.TypeId = (int)reader["TypeId"];
                    info.TypeName = (string)reader["TypeName"];
                    info.Remark = (string)reader["Remark"];
                }
                IList<int> list = new List<int>();
                reader.NextResult();
                while (reader.Read())
                {
                    list.Add((int)reader["BrandId"]);
                }
                info.Brands = list;
            }
            return info;
        }

        public override IList<ProductTypeInfo> GetProductTypes()
        {
            IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ProductTypeInfo item = new ProductTypeInfo();
                    item.TypeId = (int)reader["TypeId"];
                    item.TypeName = (string)reader["TypeName"];
                    item.Remark = (string)reader["Remark"];
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetProductTypes(ProductTypeQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
        }

        public override DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0}", 1);
            builder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId)
        {
            IUser user = Users.GetUser(distorUserId, false);
            if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            if (distributor == null)
            {
                return null;
            }
            int distributorDiscount = GetDistributorDiscount(distributor.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT s.SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice,", distributor.GradeId, distributorDiscount);
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

        public override DataTable GetSkuDistributorPrices(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductName, SKU, MarketPrice, CostPrice, PurchasePrice");
            builder.AppendFormat(" FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            builder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS DistributorGradeName,Discount FROM aspnet_DistributorGrades");
            builder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_DistributorGrades WHERE GradeId = sd.GradeId) AS DistributorGradeName,  DistributorPurchasePrice");
            builder.AppendFormat(" FROM Hishop_SKUDistributorPrice sd WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
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
                            table.Columns.Add((string)row["DistributorGradeName"]);
                        }
                    }
                    while (reader.Read())
                    {
                        table.Columns.Add((string)reader["DistributorGradeName"]);
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
                            row2[(string)row4["DistributorGradeName"]] = (decimal)row4["DistributorPurchasePrice"];
                        }
                    }
                }
            }
            if ((table4 != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    decimal num = decimal.Parse(row2["PurchasePrice"].ToString());
                    foreach (DataRow row4 in table4.Rows)
                    {
                        decimal num2 = decimal.Parse(row4["Discount"].ToString());
                        string str2 = (num * (num2 / 100M)).ToString("F2");
                        row2[(string)row4["DistributorGradeName"]] = row2[(string)row4["DistributorGradeName"]] + "|" + str2;
                    }
                }
            }
            return table;
        }

        public override DataTable GetSkuMemberPrices(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            builder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
            builder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
            builder.AppendFormat(" FROM Hishop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
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
                            row2[(string)row4["MemberGradeName"]] = row4["MemberSalePrice"];
                        }
                    }
                }
            }
            if ((table4 != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    decimal num = decimal.Parse(row2["SalePrice"].ToString());
                    foreach (DataRow row5 in table4.Rows)
                    {
                        decimal num2 = decimal.Parse(row5["Discount"].ToString());
                        string str2 = (num * (num2 / 100M)).ToString("F2");
                        row2[(string)row5["MemberGradeName"]] = row2[(string)row5["MemberGradeName"]] + "|" + str2;
                    }
                }
            }
            return table;
        }

        public override DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId)
        {
            IUser user = Users.GetUser(distorUserId, false);
            if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            if (distributor == null)
            {
                return null;
            }
            int distributorDiscount = GetDistributorDiscount(distributor.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", distributor.GradeId, distributorDiscount);
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSkuStocks(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock, AlertStock,ThumbnailUrl40 FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            DataTable table = null;
            DataTable table2 = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            table.Columns.Add("SKUContent");
            if ((((table != null) && (table.Rows.Count > 0)) && (table2 != null)) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    string str = string.Empty;
                    foreach (DataRow row2 in table2.Rows)
                    {
                        if (((string)row["SkuId"]) == ((string)row2["SkuId"]))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, row2["AttributeName"], "：", row2["ValueStr"], "; " });
                        }
                    }
                    row["SKUContent"] = str;
                }
            }
            return table;
        }

        public override IList<int> GetSubjectProductIds(SubjectType subjectType)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId FROM Hishop_SubjectProducts WHERE SubjectType=@SubjectType");
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, (int)subjectType);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int)reader["ProductId"]);
                }
            }
            return list;
        }

        public override DbQueryResult GetSubjectProducts(SubjectType subjectType, Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList", "ProductId", string.Format("ProductId IN (SELECT ProductId FROM Hishop_SubjectProducts WHERE SubjectType = {0})", (int)subjectType), "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
        }

        public override DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) AND SaleStatus<>{1} ", distorUserId, 0);
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

        public override string GetSupplierRemark(string supplierName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Remark FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (string)obj2;
            }
            return string.Empty;
        }

        public override IList<string> GetSuppliers()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SupplierName FROM Hishop_Suppliers");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        public override DbQueryResult GetSuppliers(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, true, "Hishop_Suppliers", "SupplierName", "", "SupplierName,Remark");
        }

        public override DataSet GetTaobaoProductDetails(int productId)
        {
            DataTable table;
            DataTable table2;
            DataTable table3;
            DataTable table4;
            DataTable table5;
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT [Name] FROM Hishop_ProductLines WHERE LineId = p.LineId) AS ProductLineName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT MIN(PurchasePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS PurchasePrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', PurchasePrice AS '采购价', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table3 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table4 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table5 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if (((table3 != null) && (table3.Rows.Count > 0)) && ((table4 != null) && (table4.Rows.Count > 0)))
            {
                foreach (DataRow row in table4.Rows)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = (string)row["AttributeName"];
                    if (!table3.Columns.Contains(column.ColumnName))
                    {
                        table3.Columns.Add(column);
                    }
                }
                foreach (DataRow row2 in table3.Rows)
                {
                    foreach (DataRow row in table4.Rows)
                    {
                        if (string.Compare((string)row2["商家编码"], (string)row["商家编码"]) == 0)
                        {
                            row2[(string)row["AttributeName"]] = row["ValueStr"];
                        }
                    }
                }
            }
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            set.Tables.Add(table5);
            return set;
        }

        public override DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId={0}", Convert.ToInt32(query.BrandId.Value));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
            }
            else
            {
                builder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
            }
            builder.AppendFormat("AND SaleStatus!={0}", (int)query.SaleStatus);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList", "ProductId", builder.ToString(), "CategoryId,MainCategoryPath,ExtendCategoryPath, ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
        }

        public override IList<string> GetUserIdByLineId(int lineId)
        {
            List<string> list = new List<string>();
            try
            {
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId=" + lineId);
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(reader["UserId"].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public override Dictionary<int, IList<string>> GetUserIdByProductId(IList<int> productIds)
        {
            Dictionary<int, IList<string>> dictionary = new Dictionary<int, IList<string>>();
            try
            {
                string str = "(";
                foreach (int num in productIds)
                {
                    str = str + num + ",";
                }
                if (str.Length > 1)
                {
                    str = str.Substring(0, str.Length - 1) + ")";
                }
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT DistributorUserId,ProductName FROM distro_Products WHERE ProductId IN " + str);
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        int key = (int)reader["DistributorUserId"];
                        string item = (string)reader["ProductName"];
                        if (!dictionary.ContainsKey(key))
                        {
                            IList<string> list = new List<string>();
                            list.Add(item);
                            dictionary.Add(key, list);
                        }
                        else
                        {
                            IList<string> list2 = dictionary[key];
                            list2.Add(item);
                            dictionary[key] = list2;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return dictionary;
        }

        public override IList<string> GetUserIdByProductId(string productIds)
        {
            List<string> list = new List<string>();
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(" SELECT UserId from Hishop_DistributorProductLines WHERE LineId in (select LineId from Hishop_Products WHERE SaleStatus!={0}", 0);
                builder.AppendFormat(" AND PenetrationStatus=1 AND ProductId in ({0}) group by LineId) group by UserId", productIds);
                DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(reader["UserId"].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return list;
        }

        public override bool OffShelfProductExcludedSalePrice(int productId, decimal lowestSalePrice, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Products  SET SaleStatus=3 WHERE ProductId = @ProductId AND (SELECT MIN(SalePrice) FROM vw_distro_SkuPrices WHERE DistributoruserId = distro_Products.DistributoruserId AND SkuId IN (SELECT SkuId FROM Hishop_Skus WHERE ProductId = @ProductId)) < @LowestSalePrice");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "LowestSalePrice", DbType.Currency, lowestSalePrice);
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
        }

        decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
        {
            decimal num = 0M;
            string str = operation;
            if (str == null)
            {
                return num;
            }
            if (!(str == "+"))
            {
                if (str != "-")
                {
                    if ((str != "*") && (str != "/"))
                    {
                        return num;
                    }
                    return (opreation1 * opreation2);
                }
            }
            else
            {
                return (opreation1 + opreation2);
            }
            return (opreation1 - opreation2);
        }

        public override int PenetrationProducts(IList<int> productIds)
        {
            string str = "(";
            foreach (int num in productIds)
            {
                str = str + num + ",";
            }
            if (str.Length == 1)
            {
                return 0;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Products SET PenetrationStatus = 1 WHERE ProductId IN " + (str.Substring(0, str.Length - 1) + ")"));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveSubjectProduct(SubjectType subjectType, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_SubjectProducts WHERE SubjectType = @SubjectType AND ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "SubjectType", DbType.Int32, (int)subjectType);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ReplaceProductLine(int fromlineId, int replacelineId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductLine_Replace");
            database.AddInParameter(storedProcCommand, "OldLineId", DbType.Int32, fromlineId);
            database.AddInParameter(storedProcCommand, "NewLineId", DbType.Int32, replacelineId);
            return (database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE ProductId IN ({2})", DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), productIds));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandid);
            database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapCategorySequence(int categoryId, CategoryZIndex zIndex)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Category_SwapDisplaySequence");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(storedProcCommand, "ZIndex", DbType.Int32, (int)zIndex);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
            database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, categoryId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, TypeId = @TypeId, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId; DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId;");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            bool flag = database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag && (attribute.AttributeValues.Count != 0))
            {
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    database.AddInParameter(command, "AttributeId", DbType.Int32, attribute.AttributeId);
                    database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    database.ExecuteNonQuery(command);
                }
            }
            return flag;
        }

        public override bool UpdateAttributeName(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UsageMode = @UsageMode WHERE AttributeId = @AttributeId;");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int)attribute.UsageMode);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateAttributeValue(int attributeId, int valueId, string newValue)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET ValueStr=@ValueStr WHERE ValueId=@valueId AND AttributeId=@AttributeId");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, newValue);
            database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_BrandCategories SET BrandName = @BrandName, Logo = @Logo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
            database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandCategory.BrandId);
            database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
            database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, brandId);
            database.AddInParameter(storedProcCommand, "Sort", DbType.Int32, action);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update Hishop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
            database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            database.AddInParameter(sqlStringCommand, "@BrandId", DbType.Int32, brandId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, Description = @Description, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName WHERE CategoryId = @CategoryId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            database.AddInParameter(sqlStringCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
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
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Product_Update");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int)product.SaleStatus);
            database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Currency, product.DisplaySequence);
            database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, product.LineId);
            database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            database.AddInParameter(storedProcCommand, "LowestSalePrice", DbType.Currency, product.LowestSalePrice);
            database.AddInParameter(storedProcCommand, "PenetrationStatus", DbType.Int16, (int)product.PenetrationStatus);
            database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, product.ProductId);
            return (database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
        }

        public override bool UpdateProductBaseInfo(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            foreach (DataRow row in dt.Rows)
            {
                num++;
                string str = num.ToString();
                builder.AppendFormat(" UPDATE Hishop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0}", str);
                builder.AppendFormat(", LowestSalePrice = {0} WHERE ProductId = {1}", row["LowestSalePrice"], row["ProductId"]);
                database.AddInParameter(sqlStringCommand, "ProductName" + str, DbType.String, row["ProductName"]);
                database.AddInParameter(sqlStringCommand, "ProductCode" + str, DbType.String, row["ProductCode"]);
                database.AddInParameter(sqlStringCommand, "MarketPrice" + str, DbType.String, row["MarketPrice"]);
            }
            sqlStringCommand.CommandText = builder.ToString();
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, newCategoryId);
            database.AddInParameter(sqlStringCommand, "MainCategoryPath", DbType.String, mainCategoryPath);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateProductLine(ProductLineInfo productLine)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ProductLines SET Name=@Name, SupplierName=@SupplierName WHERE LineId=@LineId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, productLine.Name);
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, productLine.SupplierName);
            database.AddInParameter(sqlStringCommand, "LineId", DbType.Int32, productLine.LineId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateProductLine(int lineId, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from distro_Products where ProductId=@productId and DistributorUserId ");
            builder.Append(" in (select UserId from Hishop_DistributorProductLines where LineId in ");
            builder.Append("(select LineId from Hishop_Products where ProductId=@productId))");
            builder.Append("update Hishop_Products set LineId=@lineId where ProductId=@productId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "@productId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "@lineId", DbType.Int32, lineId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE ProductId IN ({2})", DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), productIds));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET SaleStatus = {0} WHERE ProductId IN ({1})", (int)saleStatus, productIds));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
            database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, productType.TypeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSku(AttributeValueInfo attributeValue)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
            database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValue.ValueId);
            database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(DataSet ds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable table = ds.Tables["skuPriceTable"];
            DataTable table2 = ds.Tables["skuDistributorPriceTable"];
            string str = string.Empty;
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "'", row["skuId"], "'," });
                    builder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, PurchasePrice = {1} WHERE SkuId = '{2}'", row["costPrice"], row["purchasePrice"], row["skuId"]);
                }
            }
            if (str.Length > 1)
            {
                builder.AppendFormat(" DELETE FROM Hishop_SKUDistributorPrice WHERE SkuId IN ({0}) ", str.Remove(str.Length - 1));
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table2.Rows)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId, GradeId, DistributorPurchasePrice) VALUES ('{0}', {1}, {2})", row["skuId"], row["gradeId"], row["distributorPrice"]);
                }
            }
            if (builder.Length <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else if (gradeId == -4)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice) SELECT SkuId, {0} AS GradeId, {1} AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} {1} ({2}) WHERE ProductId IN ({3})", new object[] { basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            else if (gradeId == -4)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = {0} {1} ({2}) WHERE ProductId IN ({3})", new object[] { basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice) SELECT SkuId, {0} AS GradeId, {1} {2} ({3}) AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({4})", new object[] { gradeId, basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(DataSet ds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable table = ds.Tables["skuPriceTable"];
            DataTable table2 = ds.Tables["skuMemberPriceTable"];
            string str = string.Empty;
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "'", row["skuId"], "'," });
                    builder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", row["costPrice"], row["salePrice"], row["skuId"]);
                }
            }
            if (str.Length > 1)
            {
                builder.AppendFormat(" DELETE FROM Hishop_SKUMemberPrice WHERE SkuId IN ({0}) ", str.Remove(str.Length - 1));
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table2.Rows)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", row["skuId"], row["gradeId"], row["memberPrice"]);
                }
            }
            if (builder.Length <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else if (gradeId == -3)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} {1} ({2}) WHERE ProductId IN ({3})", new object[] { basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            else if (gradeId == -3)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} {1} ({2}) WHERE ProductId IN ({3})", new object[] { basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} {2} ({3}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({4})", new object[] { gradeId, basePriceName, operation, price, DataHelper.CleanSearchString(productIds) });
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuStock(Dictionary<string, int> skuStocks)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in skuStocks.Keys)
            {
                builder.AppendFormat(" UPDATE Hishop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[str], DataHelper.CleanSearchString(str));
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuStock(string productIds, int stock)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = {0} WHERE ProductId IN ({1})", stock, DataHelper.CleanSearchString(productIds)));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSpecification(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId");
            database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSupplier(string oldName, string newName, string remark)
        {
            if (!oldName.Equals(newName))
            {
                DbCommand command = database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
                database.AddInParameter(command, "SupplierName", DbType.String, newName);
                if (((int)database.ExecuteScalar(command)) == 0)
                {
                    DbCommand command2 = database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET SupplierName=@SupplierName,Remark=@Remark WHERE LOWER(SupplierName) = LOWER(@OldSupplierName)");
                    database.AddInParameter(command2, "SupplierName", DbType.String, newName);
                    database.AddInParameter(command2, "Remark", DbType.String, remark);
                    database.AddInParameter(command2, "OldSupplierName", DbType.String, oldName);
                    return (database.ExecuteNonQuery(command2) >= 1);
                }
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET Remark=@Remark WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, newName);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds);update Taobao_DistroProducts set  updatestatus=1 where  ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, taobaoProduct.ProductId);
            database.AddInParameter(sqlStringCommand, "Cid", DbType.Int32, taobaoProduct.Cid);
            database.AddInParameter(sqlStringCommand, "StuffStatus", DbType.String, taobaoProduct.StuffStatus);
            database.AddInParameter(sqlStringCommand, "ProTitle", DbType.String, taobaoProduct.ProTitle);
            database.AddInParameter(sqlStringCommand, "Num", DbType.Int64, taobaoProduct.Num);
            database.AddInParameter(sqlStringCommand, "LocationState", DbType.String, taobaoProduct.LocationState);
            database.AddInParameter(sqlStringCommand, "LocationCity", DbType.String, taobaoProduct.LocationCity);
            database.AddInParameter(sqlStringCommand, "FreightPayer", DbType.String, taobaoProduct.FreightPayer);
            database.AddInParameter(sqlStringCommand, "PostFee", DbType.Currency, taobaoProduct.PostFee);
            database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Currency, taobaoProduct.ExpressFee);
            database.AddInParameter(sqlStringCommand, "EMSFee", DbType.Currency, taobaoProduct.EMSFee);
            database.AddInParameter(sqlStringCommand, "HasInvoice", DbType.Boolean, taobaoProduct.HasInvoice);
            database.AddInParameter(sqlStringCommand, "HasWarranty", DbType.Boolean, taobaoProduct.HasWarranty);
            database.AddInParameter(sqlStringCommand, "HasDiscount", DbType.Boolean, taobaoProduct.HasDiscount);
            database.AddInParameter(sqlStringCommand, "ValidThru", DbType.Int32, taobaoProduct.ValidThru);
            database.AddInParameter(sqlStringCommand, "ListTime", DbType.DateTime, taobaoProduct.ListTime);
            database.AddInParameter(sqlStringCommand, "PropertyAlias", DbType.String, taobaoProduct.PropertyAlias);
            database.AddInParameter(sqlStringCommand, "InputPids", DbType.String, taobaoProduct.InputPids);
            database.AddInParameter(sqlStringCommand, "InputStr", DbType.String, taobaoProduct.InputStr);
            database.AddInParameter(sqlStringCommand, "SkuProperties", DbType.String, taobaoProduct.SkuProperties);
            database.AddInParameter(sqlStringCommand, "SkuQuantities", DbType.String, taobaoProduct.SkuQuantities);
            database.AddInParameter(sqlStringCommand, "SkuPrices", DbType.String, taobaoProduct.SkuPrices);
            database.AddInParameter(sqlStringCommand, "SkuOuterIds", DbType.String, taobaoProduct.SkuOuterIds);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

