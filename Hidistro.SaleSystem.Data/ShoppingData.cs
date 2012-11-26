using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.SaleSystem.Data
{
    public class ShoppingData : ShoppingMasterProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        bool AddCouponUseRecord(string couponCode, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_CouponItems WHERE ClaimCode=@ClaimCode");
            database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddGiftItem(int giftId, int quantity)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId) UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime) VALUES (@UserId, @GiftId, @Quantity, @AddTime)");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override AddCartItemStatus AddLineItem(Hidistro.Membership.Context.Member member, int productId, string skuId, int quantity)
        {
            if ((member == null) || (member.UserRole != UserRole.Member))
            {
                return AddCartItemStatus.InvalidUser;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            database.ExecuteNonQuery(storedProcCommand);
            return AddCartItemStatus.Successed;
        }

        public override bool AddMemberPoint(UserPointInfo point)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)point.TradeType);
            database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        bool AddOrderGiftItems(string orderId, IList<OrderGiftInfo> orderGifts, DbTransaction dbTran)
        {
            if ((orderGifts == null) || (orderGifts.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (OrderGiftInfo info in orderGifts)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO Hishop_OrderGifts(OrderId, GiftId, GiftName, CostPrice, ThumbnailsUrl, Quantity) VALUES( @OrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(");");
                database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info.GiftId);
                database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info.GiftName);
                database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.CostPrice);
                database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

        bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            if ((lineItems == null) || (lineItems.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in lineItems)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId,ProductId, SKU, Quantity, ShipmentQuantity, CostPrice, ").Append("ItemListPrice, ItemAdjustedPrice, ItemDescription, SKUContent, ThumbnailsUrl, Weight,PurchaseGiftId,PurchaseGiftName,WholesaleDiscountId,WholesaleDiscountName) VALUES( @OrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@ShipmentQuantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemAdjustedPrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@SKUContent").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(",@PurchaseGiftId").Append(str).Append(",@PurchaseGiftName").Append(str).Append(",@WholesaleDiscountId").Append(str).Append(",@WholesaleDiscountName").Append(str).Append(");");
                database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + str, DbType.Int32, info.ShipmentQuantity);
                database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + str, DbType.Currency, info.ItemAdjustedPrice);
                database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                database.AddInParameter(sqlStringCommand, "PurchaseGiftId" + str, DbType.Int32, info.PurchaseGiftId);
                database.AddInParameter(sqlStringCommand, "PurchaseGiftName" + str, DbType.String, info.PurchaseGiftName);
                database.AddInParameter(sqlStringCommand, "WholesaleDiscountId" + str, DbType.Int32, info.WholesaleDiscountId);
                database.AddInParameter(sqlStringCommand, "WholesaleDiscountName" + str, DbType.String, info.WholesaleDiscountName);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

        bool AddOrderOptions(string orderId, IList<OrderOptionInfo> orderOptions, DbTransaction dbTran)
        {
            if ((orderOptions == null) || (orderOptions.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (OrderOptionInfo info in orderOptions)
            {
                builder.Append("INSERT INTO Hishop_OrderOptions (OrderId, LookupListId, LookupItemId, ListDescription, ItemDescription, AdjustedPrice, CustomerTitle, CustomerDescription)").Append(" VALUES (@OrderId, @LookupListId").Append(num).Append(", @LookupItemId").Append(num).Append(", @ListDescription").Append(num).Append(", @ItemDescription").Append(num).Append(", @AdjustedPrice").Append(num).Append(", @CustomerTitle").Append(num).Append(", @CustomerDescription").Append(num).Append(")");
                database.AddInParameter(sqlStringCommand, "LookupListId" + num, DbType.Int32, info.LookupListId);
                database.AddInParameter(sqlStringCommand, "LookupItemId" + num, DbType.Int32, info.LookupItemId);
                database.AddInParameter(sqlStringCommand, "ListDescription" + num, DbType.String, info.ListDescription);
                database.AddInParameter(sqlStringCommand, "ItemDescription" + num, DbType.String, info.ItemDescription);
                database.AddInParameter(sqlStringCommand, "AdjustedPrice" + num, DbType.Currency, info.AdjustedPrice);
                database.AddInParameter(sqlStringCommand, "CustomerTitle" + num, DbType.String, info.CustomerTitle);
                database.AddInParameter(sqlStringCommand, "CustomerDescription" + num, DbType.String, info.CustomerDescription);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString();
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId; DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!CreatOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.LineItems.Values.Count > 0) && !AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.Gifts.Count > 0) && !AddOrderGiftItems(orderInfo.OrderId, orderInfo.Gifts, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (((orderInfo.OrderOptions != null) && (orderInfo.OrderOptions.Count > 0)) && !AddOrderOptions(orderInfo.OrderId, orderInfo.OrderOptions, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !AddCouponUseRecord(orderInfo.CouponCode, dbTran))
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
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("ss_CreateOrder");
            database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int)orderInfo.OrderStatus);
            database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            database.AddInParameter(storedProcCommand, "AdjustedPayCharge", DbType.Currency, orderInfo.AdjustedPayCharge);
            database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int)orderInfo.RefundStatus);
            database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal());
            database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.GetTotalPoints());
            database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            database.AddInParameter(storedProcCommand, "OptionPrice", DbType.Currency, orderInfo.GetOptionPrice());
            database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            database.AddInParameter(storedProcCommand, "ActivityName", DbType.String, orderInfo.ActivityName);
            database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, orderInfo.ActivityId);
            database.AddInParameter(storedProcCommand, "EightFree", DbType.Boolean, orderInfo.EightFree);
            database.AddInParameter(storedProcCommand, "ProcedureFeeFree", DbType.Boolean, orderInfo.ProcedureFeeFree);
            database.AddInParameter(storedProcCommand, "OrderOptionFree", DbType.Boolean, orderInfo.OrderOptionFree);
            database.AddInParameter(storedProcCommand, "DiscountName", DbType.String, orderInfo.DiscountName);
            database.AddInParameter(storedProcCommand, "DiscountId", DbType.Int32, orderInfo.DiscountId);
            database.AddInParameter(storedProcCommand, "DiscountValue", DbType.Currency, orderInfo.DiscountValue);
            database.AddInParameter(storedProcCommand, "DiscountValueType", DbType.Int32, (int)orderInfo.DiscountValueType);
            database.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Currency, orderInfo.GetDiscountAmount());
            database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            if (orderInfo.GroupBuyId > 0)
            {
                database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            return (database.ExecuteNonQuery(storedProcCommand, dbTran) == 1);
        }

        public override ShoppingCartItemInfo GetCartItemInfo(Hidistro.Membership.Context.Member member, int productId, string skuId, int quantity)
        {
            string sku = string.Empty;
            int num = quantity;
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                if (reader["SKU"] != DBNull.Value)
                {
                    sku = (string)reader["SKU"];
                }
                num = (int)reader["TotalQuantity"];
                string name = reader["ProductName"].ToString();
                int categoryId = 0;
                if (DBNull.Value != reader["CategoryId"])
                {
                    categoryId = (int)reader["CategoryId"];
                }
                int weight = 0;
                if (DBNull.Value != reader["Weight"])
                {
                    weight = (int)reader["Weight"];
                }
                decimal memberPrice = (decimal)reader["SalePrice"];
                string str3 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    str3 = reader["ThumbnailUrl40"].ToString();
                }
                string str4 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    str4 = reader["ThumbnailUrl60"].ToString();
                }
                string str5 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    str5 = reader["ThumbnailUrl100"].ToString();
                }
                int purchaseGiftId = 0;
                int giveQuantity = 0;
                int wholesaleDiscountId = 0;
                string purchaseGiftName = null;
                string wholesaleDiscountName = null;
                decimal? discountRate = null;
                if (member != null)
                {
                    if (reader.NextResult() && reader.Read())
                    {
                        if (DBNull.Value != reader["ActivityId"])
                        {
                            purchaseGiftId = (int)reader["ActivityId"];
                        }
                        if (DBNull.Value != reader["Name"])
                        {
                            purchaseGiftName = reader["Name"].ToString();
                        }
                        if ((DBNull.Value != reader["BuyQuantity"]) && (DBNull.Value != reader["GiveQuantity"]))
                        {
                            giveQuantity = (num / ((int)reader["BuyQuantity"])) * ((int)reader["GiveQuantity"]);
                        }
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        if (DBNull.Value != reader["ActivityId"])
                        {
                            wholesaleDiscountId = (int)reader["ActivityId"];
                        }
                        if (DBNull.Value != reader["Name"])
                        {
                            wholesaleDiscountName = reader["Name"].ToString();
                        }
                        if (DBNull.Value != reader["DiscountValue"])
                        {
                            discountRate = new decimal?(Convert.ToDecimal(reader["DiscountValue"]));
                        }
                    }
                }
                string skuContent = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string)reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string)reader["ValueStr"])))
                        {
                            object obj2 = skuContent;
                            skuContent = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                return new ShoppingCartItemInfo(skuId, productId, sku, name, memberPrice, skuContent, num, weight, purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str3, str3, str5);
            }
        }

        public override ShoppingCartItemInfo GetCartItemInfo(Hidistro.Membership.Context.Member member, int productId, string skuId, string skuContent, int quantity, out ProductSaleStatus saleStatus, out string sku, out int stock, out int totalQuantity)
        {
            saleStatus = ProductSaleStatus.OnSale;
            sku = string.Empty;
            stock = 0;
            totalQuantity = quantity;
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                saleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
                if (reader["SKU"] != DBNull.Value)
                {
                    sku = (string)reader["SKU"];
                }
                stock = (int)reader["Stock"];
                totalQuantity = (int)reader["TotalQuantity"];
                string name = reader["ProductName"].ToString();
                int categoryId = 0;
                if (DBNull.Value != reader["CategoryId"])
                {
                    categoryId = (int)reader["CategoryId"];
                }
                int weight = 0;
                if (DBNull.Value != reader["Weight"])
                {
                    weight = (int)reader["Weight"];
                }
                decimal memberPrice = (decimal)reader["SalePrice"];
                string str2 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    str2 = reader["ThumbnailUrl40"].ToString();
                }
                string str3 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    str3 = reader["ThumbnailUrl60"].ToString();
                }
                string str4 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    str4 = reader["ThumbnailUrl100"].ToString();
                }
                int purchaseGiftId = 0;
                int giveQuantity = 0;
                int wholesaleDiscountId = 0;
                string purchaseGiftName = null;
                string wholesaleDiscountName = null;
                decimal? discountRate = null;
                if (member != null)
                {
                    if (reader.NextResult() && reader.Read())
                    {
                        if (DBNull.Value != reader["ActivityId"])
                        {
                            purchaseGiftId = (int)reader["ActivityId"];
                        }
                        if (DBNull.Value != reader["Name"])
                        {
                            purchaseGiftName = reader["Name"].ToString();
                        }
                        if ((DBNull.Value != reader["BuyQuantity"]) && (DBNull.Value != reader["GiveQuantity"]))
                        {
                            giveQuantity = (totalQuantity / ((int)reader["BuyQuantity"])) * ((int)reader["GiveQuantity"]);
                        }
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        if (DBNull.Value != reader["ActivityId"])
                        {
                            wholesaleDiscountId = (int)reader["ActivityId"];
                        }
                        if (DBNull.Value != reader["Name"])
                        {
                            wholesaleDiscountName = reader["Name"].ToString();
                        }
                        if (DBNull.Value != reader["DiscountValue"])
                        {
                            discountRate = new decimal?(Convert.ToDecimal(reader["DiscountValue"]));
                        }
                    }
                }
                return new ShoppingCartItemInfo(skuId, productId, sku, name, memberPrice, skuContent, totalQuantity, weight, purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str2, str2, str4);
            }
        }

        /// <summary>
        /// 获取规格成本价
        /// </summary>
        public override decimal GetCostPrice(string skuId)
        {
            decimal skuCostPrice = 0m;

            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT CostPrice FROM Hishop_SKUs WHERE SkuId=@SkuId");

            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);

            object scalar = database.ExecuteScalar(sqlStringCommand);

            if ((scalar != null) && (scalar != DBNull.Value))
            {
                skuCostPrice = (decimal)scalar;
            }

            return skuCostPrice;

        }

        /// <summary>
        /// 获取规格成本价
        /// </summary>
        public override Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
        {

            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();

            if (!string.IsNullOrEmpty(skuIds))
            {
                //DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT s.SkuId, s.CostPrice FROM Hishop_SKUs s WHERE SkuId IN (@SkuIds)");
                DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT s.SkuId, s.CostPrice FROM Hishop_SKUs s WHERE SkuId IN ({0})", skuIds));

                //database.AddInParameter(sqlStringCommand, "SkuIds", DbType.String, skuIds);

                decimal costPrice = 0m;

                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (null != reader && reader.Read())
                    {

                        costPrice = (reader["CostPrice"] == DBNull.Value) ? 0M : ((decimal)reader["CostPrice"]);

                        dictionary.Add((string)reader["SkuId"], costPrice);

                    }

                }

            }

            return dictionary;

        }

        public override DataTable GetCoupon(decimal orderAmount)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ci.ClaimCode,c.DiscountValue,(ClaimCode+'　　　　　面值'+cast(DiscountValue as varchar(10))) as DisplayText FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>Amount) or (Amount=0 and @orderAmount>DiscountValue)) AND UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CouponInfo GetCoupon(string couponCode)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT c.* FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where ci.ClaimCode =@ClaimCode AND @DateTime <c.ClosingTime");
            database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCoupon(reader);
                }
            }
            return info;
        }

        public override IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId)
        {
            IList<ExpressCompanyInfo> list = new List<ExpressCompanyInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ExpressCompanyInfo item = new ExpressCompanyInfo();
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        item.ExpressCompanyName = (string)reader["ExpressCompanyName"];
                    }
                    if (reader["ExpressCompanyAbb"] != DBNull.Value)
                    {
                        item.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo orderInfo = null;

            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT * FROM Hishop_Orders Where OrderId = @OrderId ");
            builder.Append(" SELECT * FROM Hishop_OrderOptions Where OrderId = @OrderId ");
            builder.Append(" SELECT * FROM Hishop_OrderGifts Where OrderId = @OrderId ");
            builder.Append(" SELECT o.*,(SELECT Stock FROM Hishop_SKUs WHERE SkuId=o.SkuId)  as Stock FROM Hishop_OrderItems o Where o.OrderId = @OrderId  ");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    orderInfo = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    orderInfo.OrderOptions.Add(DataMapper.PopulateOrderOption(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    orderInfo.Gifts.Add(DataMapper.PopulateOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    orderInfo.LineItems.Add((string)reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return orderInfo;
        }

        public override OrderLookupItemInfo GetOrderLookupItem(int lookupItemId, string orderId)
        {
            OrderLookupItemInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT CustomerDescription FROM Hishop_OrderOptions WHERE OrderId = @OrderId AND LookupItemId = @LookupItemId) AS UserInputContent FROM Hishop_OrderLookupItems Where LookupItemId = @LookupItemId");
            database.AddInParameter(sqlStringCommand, "LookupItemId", DbType.Int32, lookupItemId);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrderLookupItem(reader);
                }
            }
            return info;
        }

        public override IList<OrderLookupItemInfo> GetOrderLookupItems(int lookupListId)
        {
            IList<OrderLookupItemInfo> list = new List<OrderLookupItemInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * ,NULL AS UserInputContent FROM Hishop_OrderLookupItems Where LookupListId  =@LookupListId");
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, lookupListId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateOrderLookupItem(reader));
                }
            }
            return list;
        }

        public override OrderLookupListInfo GetOrderLookupList(int lookupListId)
        {
            OrderLookupListInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderLookupLists WHERE LookupListId = @LookupListId");
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, lookupListId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info = DataMapper.PopulateOrderLookupList(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId;");
            database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            string query = "SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override SKUItem GetProductAndSku(int productId, string options)
        {
            if (string.IsNullOrEmpty(options))
            {
                return null;
            }
            string[] strArray = options.Split(new char[] { ',' });
            if ((strArray == null) || (strArray.Length <= 0))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Hidistro.Membership.Context.Member user = HiContext.Current.User as Hidistro.Membership.Context.Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", user.GradeId, memberDiscount);
                builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new char[] { ':' });
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", strArray2[0], strArray2[1]);
            }
            SKUItem item = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    item = DataMapper.PopulateSKU(reader);
                }
            }
            return item;
        }

        public override DataTable GetProductInfoBySku(string skuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)");
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void GetPromotionsWithAmount(decimal amount, out int discountActivityId, out string discountName, out decimal discountValue, out DiscountValueType discountValueType, out int feeFreeActivityId, out string feeFreeName, out bool eightFree, out bool orderOptionFree, out bool procedureFeeFree)
        {
            discountActivityId = 0;
            discountName = null;
            discountValue = 0M;
            discountValueType = DiscountValueType.Amount;
            feeFreeActivityId = 0;
            feeFreeName = null;
            eightFree = false;
            orderOptionFree = false;
            procedureFeeFree = false;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, fd.DiscountValue, fd.ValueType FROM Hishop_Promotions p INNER JOIN Hishop_FullDiscounts fd ON p.ActivityId = fd.ActivityId INNER JOIN Hishop_PromotionMemberGrades pm ON pm.ActivityId=fd.ActivityId WHERE @Amount >= fd.Amount  AND @GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE Hishop_PromotionMemberGrades.ActivityId=fd.ActivityId)ORDER BY fd.Amount DESC");
            database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, amount);
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Hidistro.Membership.Context.Member).GradeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    discountActivityId = reader.GetInt32(0);
                    discountName = reader.GetString(1);
                    discountValue = reader.GetDecimal(2);
                    discountValueType = (DiscountValueType)reader.GetInt32(3);
                }
            }
            sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, ff.ShipChargeFree, ff.ServiceChargeFree, ff.OptionFeeFree FROM Hishop_Promotions p INNER JOIN Hishop_FullFree ff ON p.ActivityId = ff.ActivityId INNER JOIN Hishop_PromotionMemberGrades pm ON pm.ActivityId=ff.ActivityId WHERE @SubAmount >= ff.Amount  AND @GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE Hishop_PromotionMemberGrades.ActivityId=ff.ActivityId)  ORDER BY ff.Amount DESC");
            decimal num = 0M;
            if ((discountValueType == DiscountValueType.Percent) && (discountValue > 0M))
            {
                num = (amount * discountValue) / 100M;
            }
            else if ((discountValueType == DiscountValueType.Amount) && (discountValue > 0M))
            {
                num = amount - discountValue;
            }
            if (num > 0M)
            {
                database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, num);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, amount);
            }
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Hidistro.Membership.Context.Member).GradeId);
            using (IDataReader reader2 = database.ExecuteReader(sqlStringCommand))
            {
                if (reader2.Read())
                {
                    feeFreeActivityId = reader2.GetInt32(0);
                    feeFreeName = reader2.GetString(1);
                    eightFree = reader2.GetBoolean(2);
                    procedureFeeFree = reader2.GetBoolean(3);
                    orderOptionFree = reader2.GetBoolean(4);
                }
            }
        }

        public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            PurchaseOrderInfo info = new PurchaseOrderInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info.PurchaseOrderId = (string)reader["PurchaseOrderId"];
                if (DBNull.Value != reader["ExpressCompanyAbb"])
                {
                    info.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
                }
                if (DBNull.Value != reader["ExpressCompanyName"])
                {
                    info.ExpressCompanyName = (string)reader["ExpressCompanyName"];
                }
                if (DBNull.Value != reader["ShipOrderNumber"])
                {
                    info.ShipOrderNumber = (string)reader["ShipOrderNumber"];
                }
                if (DBNull.Value != reader["PurchaseStatus"])
                {
                    info.PurchaseStatus = (OrderStatus)reader["PurchaseStatus"];
                }
            }
            return info;
        }

        public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId;");
            if (includeDetail)
            {
                builder.Append("SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes WHERE ModeId =@ModeId);");
                builder.Append("SELECT TemplateId,GroupId,RegionId FROM Hishop_ShippingRegions Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId);");
                builder.Append(" SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingMode(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    foreach (ShippingModeGroupInfo info2 in info.ModeGroup)
                    {
                        if (info2.GroupId == ((int)reader["GroupId"]))
                        {
                            info2.ModeRegions.Add(DataMapper.PopulateShippingRegion(reader));
                        }
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ExpressCompanyInfo item = new ExpressCompanyInfo();
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        item.ExpressCompanyName = (string)reader["ExpressCompanyName"];
                    }
                    if (reader["ExpressCompanyAbb"] != DBNull.Value)
                    {
                        item.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
                    }
                    info.ExpressCompany.Add(item);
                }
            }
            return info;
        }

        public override IList<ShippingModeInfo> GetShippingModes()
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence desc";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public override ShoppingCartInfo GetShoppingCart(int userId)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId and ProductId in (select ProductId from Hishop_Products where SaleStatus=@SaleStatus); SELECT * FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            database.AddInParameter(sqlStringCommand, "@SaleStatus", DbType.Int32, ProductSaleStatus.OnSale);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                Hidistro.Membership.Context.Member user = HiContext.Current.User as Hidistro.Membership.Context.Member;
                while (reader.Read())
                {
                    ShoppingCartItemInfo info2 = GetCartItemInfo(user, (int)reader["ProductId"], (string)reader["SkuId"], (int)reader["Quantity"]);
                    if (info2 != null)
                    {
                        info.LineItems.Add((string)reader["SkuId"], info2);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShoppingCartGiftInfo item = DataMapper.PopulateGiftCartItem(reader);
                    item.Quantity = (int)reader["Quantity"];
                    info.LineGifts.Add(item);
                }
            }
            return info;
        }

        public override bool GetShoppingProductInfo(Hidistro.Membership.Context.Member member, int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity)
        {
            saleStatus = ProductSaleStatus.Delete;
            stock = 0;
            totalQuantity = 0;
            bool flag = false;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Stock,SaleStatus,AlertStock FROM Hishop_Skus s INNER JOIN Hishop_Products p ON s.ProductId=p.ProductId WHERE s.ProductId=@ProductId AND s.SkuId=@SkuId;SELECT Quantity FROM Hishop_ShoppingCarts sc WHERE sc.ProductId=@ProductId AND sc.SkuId=@SkuId AND sc.UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    saleStatus = (ProductSaleStatus)((int)reader["SaleStatus"]);
                    stock = (int)reader["Stock"];
                    flag = true;
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalQuantity = (int)reader["Quantity"];
                }
            }
            return flag;
        }

        /// <summary>
        /// 获取规格id
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public override IList<string> GetSkuIdsBysku(string sku)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE SKU = @SKU");

            database.AddInParameter(sqlStringCommand, "SKU", DbType.String, sku);

            IList<string> list = new List<string>();

            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (null != reader && reader.Read())
                {
                    list.Add((string)reader["SkuId"]);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取规格库存
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public override int GetSkuStock(string skuId)
        {
            int stock = 0;

            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");

            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);

            object scalar = database.ExecuteScalar(sqlStringCommand);

            if ((scalar != null) && (scalar != DBNull.Value))
            {
                stock = (int)scalar;
            }
            return stock;
        }

        public override IList<OrderLookupListInfo> GetUsableOrderLookupLists()
        {
            IList<OrderLookupListInfo> list = new List<OrderLookupListInfo>();

            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT DISTINCT oll.* FROM Hishop_OrderLookupLists oll INNER JOIN Hishop_OrderLookupItems oli ON oll.LookupListId = oli.LookupListId");

            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (null != reader && reader.Read())
                {
                    list.Add(DataMapper.PopulateOrderLookupList(reader));
                }
            }
            return list;
        }

        public override DataTable GetYetShipOrders(int days)
        {
            if (days <= 0)
            {
                return null;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * from Hishop_Orders where OrderStatus=@OrderStatus and ShippingDate>=@FromDate and ShippingDate<=@ToDate;");
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            database.AddInParameter(sqlStringCommand, "FromDate", DbType.DateTime, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays((double)-days));
            database.AddInParameter(sqlStringCommand, "ToDate", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void RemoveGiftItem(int giftId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateGiftItemQuantity(int giftId, int quantity)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateLineItemQuantity(Hidistro.Membership.Context.Member member, int productId, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

