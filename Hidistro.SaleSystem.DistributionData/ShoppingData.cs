namespace Hidistro.SaleSystem.DistributionData
{
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

    public class ShoppingData : ShoppingSubsiteProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

       bool AddCouponUseRecord(string couponCode, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_CouponItems WHERE ClaimCode=@ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddGiftItem(int giftId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM distro_GiftShopingCarts WHERE UserId = @UserId AND GiftId = @GiftId) UPDATE distro_GiftShopingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId; ELSE INSERT INTO distro_GiftShopingCarts(UserId, GiftId, Quantity, AddTime) VALUES (@UserId, @GiftId, @Quantity, @AddTime)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override AddCartItemStatus AddLineItem(Member member, int productId, string skuId, int quantity)
        {
            if ((((member == null) || !HiContext.Current.SiteSettings.IsDistributorSettings) || (member.UserRole != UserRole.Underling)) || (member.ParentUserId.Value != HiContext.Current.SiteSettings.UserId.Value))
            {
                return AddCartItemStatus.InvalidUser;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.ExecuteNonQuery(storedProcCommand);
            return AddCartItemStatus.Successed;
        }

        public override bool AddMemberPoint(UserPointInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) point.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

       bool AddOrderGiftItems(string orderId, IList<OrderGiftInfo> orderGifts, DbTransaction dbTran)
        {
            if ((orderGifts == null) || (orderGifts.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (OrderGiftInfo info in orderGifts)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO distro_OrderGifts(OrderId, GiftId, GiftName, CostPrice, ThumbnailsUrl, Quantity,DistributorUserId) VALUES( @OrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(",@DistributorUserId").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info.GiftId);
                this.database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info.GiftName);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.CostPrice);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "DistributorUserId" + str, DbType.Int32, HiContext.Current.SiteSettings.UserId);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

       bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            if ((lineItems == null) || (lineItems.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in lineItems)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO distro_OrderItems(OrderId, DistributorUserId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice, ").Append("ItemListPrice, ItemAdjustedPrice, ItemDescription, SKUContent, ThumbnailsUrl, Weight,PurchaseGiftId,PurchaseGiftName,WholesaleDiscountId,WholesaleDiscountName) VALUES(@OrderId,").Append("@DistributorUserId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@ShipmentQuantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemAdjustedPrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@SKUContent").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(",@PurchaseGiftId").Append(str).Append(",@PurchaseGiftName").Append(str).Append(",@WholesaleDiscountId").Append(str).Append(",@WholesaleDiscountName").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + str, DbType.Int32, info.ShipmentQuantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + str, DbType.Currency, info.ItemAdjustedPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                this.database.AddInParameter(sqlStringCommand, "PurchaseGiftId" + str, DbType.Int32, info.PurchaseGiftId);
                this.database.AddInParameter(sqlStringCommand, "PurchaseGiftName" + str, DbType.String, info.PurchaseGiftName);
                this.database.AddInParameter(sqlStringCommand, "WholesaleDiscountId" + str, DbType.Int32, info.WholesaleDiscountId);
                this.database.AddInParameter(sqlStringCommand, "WholesaleDiscountName" + str, DbType.String, info.WholesaleDiscountName);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

       bool AddOrderOptions(string orderId, IList<OrderOptionInfo> orderOptions, DbTransaction dbTran)
        {
            if ((orderOptions == null) || (orderOptions.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (OrderOptionInfo info in orderOptions)
            {
                builder.Append("INSERT INTO distro_OrderOptions (OrderId, DistributorUserId, LookupListId, LookupItemId, ListDescription, ItemDescription, AdjustedPrice, CustomerTitle, CustomerDescription)").Append(" VALUES (@OrderId").Append(",@DistributorUserId").Append(num).Append(",@LookupListId").Append(num).Append(", @LookupItemId").Append(num).Append(", @ListDescription").Append(num).Append(", @ItemDescription").Append(num).Append(", @AdjustedPrice").Append(num).Append(", @CustomerTitle").Append(num).Append(", @CustomerDescription").Append(num).Append(")");
                this.database.AddInParameter(sqlStringCommand, "LookupListId" + num, DbType.Int32, info.LookupListId);
                this.database.AddInParameter(sqlStringCommand, "LookupItemId" + num, DbType.Int32, info.LookupItemId);
                this.database.AddInParameter(sqlStringCommand, "ListDescription" + num, DbType.String, info.ListDescription);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + num, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "AdjustedPrice" + num, DbType.Currency, info.AdjustedPrice);
                this.database.AddInParameter(sqlStringCommand, "CustomerTitle" + num, DbType.String, info.CustomerTitle);
                this.database.AddInParameter(sqlStringCommand, "CustomerDescription" + num, DbType.String, info.CustomerDescription);
                this.database.AddInParameter(sqlStringCommand, "DistributorUserId" + num, DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString();
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ShoppingCarts WHERE UserId = @UserId AND DistributorUserId = @DistributorUserId;DELETE FROM  distro_GiftShopingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!this.CreatOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.LineItems.Values.Count > 0) && !this.AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.Gifts.Count > 0) && !this.AddOrderGiftItems(orderInfo.OrderId, orderInfo.Gifts, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (((orderInfo.OrderOptions != null) && (orderInfo.OrderOptions.Count > 0)) && !this.AddOrderOptions(orderInfo.OrderId, orderInfo.OrderOptions, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !this.AddCouponUseRecord(orderInfo.CouponCode, dbTran))
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int) orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "AdjustedPayCharge", DbType.Currency, orderInfo.AdjustedPayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int) orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal());
            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.GetTotalPoints());
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "OptionPrice", DbType.Currency, orderInfo.GetOptionPrice());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ActivityName", DbType.String, orderInfo.ActivityName);
            this.database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, orderInfo.ActivityId);
            this.database.AddInParameter(storedProcCommand, "EightFree", DbType.Boolean, orderInfo.EightFree);
            this.database.AddInParameter(storedProcCommand, "ProcedureFeeFree", DbType.Boolean, orderInfo.ProcedureFeeFree);
            this.database.AddInParameter(storedProcCommand, "OrderOptionFree", DbType.Boolean, orderInfo.OrderOptionFree);
            this.database.AddInParameter(storedProcCommand, "DiscountName", DbType.String, orderInfo.DiscountName);
            this.database.AddInParameter(storedProcCommand, "DiscountId", DbType.Int32, orderInfo.DiscountId);
            this.database.AddInParameter(storedProcCommand, "DiscountValue", DbType.Currency, orderInfo.DiscountValue);
            this.database.AddInParameter(storedProcCommand, "DiscountValueType", DbType.Int32, (int) orderInfo.DiscountValueType);
            this.database.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Currency, orderInfo.GetDiscountAmount());
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1);
        }

        public override ShoppingCartItemInfo GetCartItemInfo(Member member, int productId, string skuId, int quantity)
        {
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                string sku = string.Empty;
                int num = 0;
                if (reader["SKU"] != DBNull.Value)
                {
                    sku = (string) reader["SKU"];
                }
                if (reader["TotalQuantity"].ToString() != "")
                {
                    num = (int) reader["TotalQuantity"];
                }
                string name = reader["ProductName"].ToString();
                int categoryId = 0;
                if (DBNull.Value != reader["CategoryId"])
                {
                    categoryId = (int) reader["CategoryId"];
                }
                int weight = 0;
                if (DBNull.Value != reader["Weight"])
                {
                    weight = (int) reader["Weight"];
                }
                decimal memberPrice = (decimal) reader["SalePrice"];
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
                if (reader.NextResult() && reader.Read())
                {
                    if (DBNull.Value != reader["ActivityId"])
                    {
                        purchaseGiftId = (int) reader["ActivityId"];
                    }
                    if (DBNull.Value != reader["Name"])
                    {
                        purchaseGiftName = reader["Name"].ToString();
                    }
                    if ((DBNull.Value != reader["BuyQuantity"]) && (DBNull.Value != reader["GiveQuantity"]))
                    {
                        giveQuantity = (num / ((int) reader["BuyQuantity"])) * ((int) reader["GiveQuantity"]);
                    }
                }
                if (reader.NextResult() && reader.Read())
                {
                    if (DBNull.Value != reader["ActivityId"])
                    {
                        wholesaleDiscountId = (int) reader["ActivityId"];
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
                string skuContent = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) reader["ValueStr"])))
                        {
                            object obj2 = skuContent;
                            skuContent = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                return new ShoppingCartItemInfo(skuId, productId, sku, name, memberPrice, skuContent, num, weight, purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str3, str4, str5);
            }
        }

        public override ShoppingCartItemInfo GetCartItemInfo(Member member, int productId, string skuId, string skuContent, int quantity, out ProductSaleStatus saleStatus, out string sku, out int stock, out int totalQuantity)
        {
            saleStatus = ProductSaleStatus.Delete;
            sku = string.Empty;
            stock = 0;
            totalQuantity = quantity;
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                saleStatus = (ProductSaleStatus) ((int) reader["SaleStatus"]);
                if (reader["SKU"] != DBNull.Value)
                {
                    sku = (string) reader["SKU"];
                }
                stock = (int) reader["Stock"];
                int num = (int) reader["AlertStock"];
                if (stock <= num)
                {
                    saleStatus = ProductSaleStatus.UnSale;
                }
                if (reader["TotalQuantity"].ToString() != "")
                {
                    totalQuantity = (int) reader["TotalQuantity"];
                }
                string name = reader["ProductName"].ToString();
                int categoryId = 0;
                if (DBNull.Value != reader["CategoryId"])
                {
                    categoryId = (int) reader["CategoryId"];
                }
                int weight = 0;
                if (DBNull.Value != reader["Weight"])
                {
                    weight = (int) reader["Weight"];
                }
                decimal memberPrice = (decimal) reader["SalePrice"];
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
                            purchaseGiftId = (int) reader["ActivityId"];
                        }
                        if (DBNull.Value != reader["Name"])
                        {
                            purchaseGiftName = reader["Name"].ToString();
                        }
                        if ((DBNull.Value != reader["BuyQuantity"]) && (DBNull.Value != reader["GiveQuantity"]))
                        {
                            giveQuantity = (totalQuantity / ((int) reader["BuyQuantity"])) * ((int) reader["GiveQuantity"]);
                        }
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        if (DBNull.Value != reader["ActivityId"])
                        {
                            wholesaleDiscountId = (int) reader["ActivityId"];
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
                return new ShoppingCartItemInfo(skuId, productId, sku, name, memberPrice, skuContent, totalQuantity, weight, purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str2, str3, str4);
            }
        }

        public override decimal GetCostPrice(string skuId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_GetGroupBuyProductCostPrices");
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            decimal num = 0M;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                int num2 = 100;
                if (reader.Read())
                {
                    num2 = reader.GetInt32(0);
                }
                if (!reader.NextResult() || !reader.Read())
                {
                    return num;
                }
                if (reader["DistributorPurchasePrice"] != DBNull.Value)
                {
                    return (decimal) reader["DistributorPurchasePrice"];
                }
                return Math.Round((decimal) (((decimal) reader["PurchasePrice"]) * (num2 / 100M)), 2);
            }
        }

        public override Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @GradeId INT; DECLARE @Discount INT; SELECT @GradeId = GradeId FROM aspnet_Distributors WHERE UserId = @DistributorUserId; SELECT @Discount = Discount FROM aspnet_DistributorGrades WHERE GradeId = @GradeId SELECT sc.SkuId, (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = sc.SkuId AND GradeId = @GradeId) = 1 THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = sc.SkuId AND GradeId = @GradeId) ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = sc.SkuId)*@Discount/100 END) AS PurchasePrice" + string.Format(" FROM Hishop_Skus sc  WHERE SkuId IN ({0})", skuIds));
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    dictionary.Add((string)reader["SkuId"], (decimal)reader["PurchasePrice"]);
                }
            }
            return dictionary;
            //Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            //DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetCostPrices");
            //this.database.AddInParameter(storedProcCommand, "SkuIds", DbType.String, skuIds);
            //this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            //using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            //{
            //    while (reader.Read())
            //    {
            //        dictionary.Add((string) reader["SkuId"], (decimal) reader["PurchasePrice"]);
            //    }
            //}
            //return dictionary;
        }

        public override DataTable GetCoupon(decimal orderAmount)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ci.ClaimCode,c.DiscountValue,(ClaimCode+'　　　　　面值'+cast(DiscountValue as varchar(10))) as DisplayText FROM distro_Coupons c INNER  JOIN distro_CouponItems ci ON ci.CouponId = c.CouponId Where @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>Amount) or (Amount=0 and @orderAmount>DiscountValue)) AND DistributorUserId = @DistributorUserId AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CouponInfo GetCoupon(string couponCode)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.* FROM distro_Coupons c INNER  JOIN distro_CouponItems ci ON ci.CouponId = c.CouponId Where ci.ClaimCode = @ClaimCode AND @DateTime < c.ClosingTime AND DistributorUserId = @DistributorUserId;");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ExpressCompanyInfo item = new ExpressCompanyInfo();
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        item.ExpressCompanyName = (string) reader["ExpressCompanyName"];
                    }
                    if (reader["ExpressCompanyAbb"] != DBNull.Value)
                    {
                        item.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            string query = "SELECT * FROM distro_Orders WHERE OrderId = @OrderId;";
            query = (query + "SELECT * FROM distro_OrderOptions WHERE OrderId = @OrderId;") + "SELECT * FROM distro_OrderGifts WHERE OrderId = @OrderId;" + "SELECT o.*,(SELECT Stock FROM Hishop_SKUs WHERE SkuId=o.SkuId) as Stock FROM distro_OrderItems o WHERE o.OrderId = @OrderId;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.OrderOptions.Add(DataMapper.PopulateOrderOption(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.Gifts.Add(DataMapper.PopulateOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add((string) reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public override OrderLookupItemInfo GetOrderLookupItem(int lookupItemId, string orderId)
        {
            OrderLookupItemInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT CustomerDescription FROM Hishop_OrderOptions WHERE OrderId = @OrderId AND LookupItemId = @LookupItemId) AS UserInputContent FROM Hishop_OrderLookupItems Where LookupItemId = @LookupItemId");
            this.database.AddInParameter(sqlStringCommand, "LookupItemId", DbType.Int32, lookupItemId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * ,NULL AS UserInputContent FROM Hishop_OrderLookupItems Where LookupListId  =@LookupListId");
            this.database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, lookupListId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderLookupLists WHERE LookupListId = @LookupListId");
            this.database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, lookupListId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId AND DistributorUserId = @DistributorUserId;");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE DistributorUserId = @DistributorUserId ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            int num = HiContext.Current.SiteSettings.UserId.Value;
            StringBuilder builder = new StringBuilder();
            if (HiContext.Current.User.UserRole == UserRole.Underling)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1", user.GradeId, num);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1})", user.GradeId, num);
                builder.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0})", num);
                builder.AppendFormat("*{0}/100 END AS SalePrice", memberDiscount);
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS SalePrice", num);
            }
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new char[] { ':' });
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", strArray2[0], strArray2[1]);
            }
            SKUItem item = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, fd.DiscountValue, fd.ValueType FROM distro_Promotions p INNER JOIN distro_FullDiscounts fd ON p.ActivityId = fd.ActivityId  INNER JOIN distro_PromotionMemberGrades pm ON pm.ActivityId=fd.ActivityId WHERE @Amount >= fd.Amount  AND @GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE distro_PromotionMemberGrades.ActivityId=fd.ActivityId)  AND p.DistributorUserId = @DistributorUserId ORDER BY fd.Amount DESC");
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, amount);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Member).GradeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    discountActivityId = reader.GetInt32(0);
                    discountName = reader.GetString(1);
                    discountValue = reader.GetDecimal(2);
                    discountValueType = (DiscountValueType) reader.GetInt32(3);
                }
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, ff.ShipChargeFree, ff.ServiceChargeFree, ff.OptionFeeFree FROM distro_Promotions p INNER JOIN distro_FullFree ff ON p.ActivityId = ff.ActivityId INNER JOIN distro_PromotionMemberGrades pm ON pm.ActivityId=ff.ActivityId WHERE DistributorUserId = @DistributorUserId AND @SubAmount >= ff.Amount  AND @GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE distro_PromotionMemberGrades.ActivityId=ff.ActivityId) ORDER BY ff.Amount DESC");
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
                this.database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, num);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, amount);
            }
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Member).GradeId);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
                if (DBNull.Value != reader["ExpressCompanyAbb"])
                {
                    info.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
                }
                if (DBNull.Value != reader["ExpressCompanyName"])
                {
                    info.ExpressCompanyName = (string) reader["ExpressCompanyName"];
                }
                if (DBNull.Value != reader["ShipOrderNumber"])
                {
                    info.ShipOrderNumber = (string) reader["ShipOrderNumber"];
                }
                if (DBNull.Value != reader["PurchaseStatus"])
                {
                    info.PurchaseStatus = (OrderStatus) reader["PurchaseStatus"];
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
                        if (info2.GroupId == ((int) reader["GroupId"]))
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
                        item.ExpressCompanyName = (string) reader["ExpressCompanyName"];
                    }
                    if (reader["ExpressCompanyAbb"] != DBNull.Value)
                    {
                        item.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
                    }
                    info.ExpressCompany.Add(item);
                }
            }
            return info;
        }

        public override IList<ShippingModeInfo> GetShippingModes()
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ShoppingCarts WHERE UserId = @UserId AND DistributorUserId = @DistributorUserId and ProductId in (select ProductId from distro_Products where SaleStatus=@SaleStatus AND DistributorUserId=@DistributorUserId);SELECT gc.UserId,gc.Quantity,gc.AddTime,g.*,hg.Unit,hg.LongDescription,hg.CostPrice,hg.ImageUrl,hg.ThumbnailUrl40,hg.ThumbnailUrl60,hg.ThumbnailUrl100,hg.PurchasePrice,hg.MarketPrice,hg.IsDownLoad FROM distro_GiftShopingCarts gc JOIN distro_Gifts g ON gc.GiftId = g.GiftId join Hishop_Gifts hg on hg.GiftId=g.GiftId WHERE gc.UserId =@UserId AND g.DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId);
            this.database.AddInParameter(sqlStringCommand, "@SaleStatus", DbType.Int32, ProductSaleStatus.OnSale);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                Member user = HiContext.Current.User as Member;
                while (reader.Read())
                {
                    ShoppingCartItemInfo info2 = this.GetCartItemInfo(user, (int) reader["ProductId"], (string) reader["SkuId"], (int) reader["Quantity"]);
                    if (info2 != null)
                    {
                        info.LineItems.Add((string) reader["SkuId"], info2);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShoppingCartGiftInfo item = DataMapper.PopulateGiftCartItem(reader);
                    item.Quantity = (int) reader["Quantity"];
                    info.LineGifts.Add(item);
                }
            }
            return info;
        }

        public override bool GetShoppingProductInfo(Member member, int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity)
        {
            saleStatus = ProductSaleStatus.Delete;
            stock = 0;
            totalQuantity = 0;
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock,SaleStatus,AlertStock FROM Hishop_Skus s INNER JOIN distro_Products p ON s.ProductId=p.ProductId WHERE s.ProductId=@ProductId AND s.SkuId=@SkuId AND p.DistributorUserId=@DistributorUserId;SELECT Quantity FROM distro_ShoppingCarts sc WHERE sc.ProductId=@ProductId AND sc.SkuId=@SkuId AND sc.UserId=@UserId AND sc.DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    saleStatus = (ProductSaleStatus) ((int) reader["SaleStatus"]);
                    stock = (int) reader["Stock"];
                    int num = (int) reader["AlertStock"];
                    if (stock <= num)
                    {
                        saleStatus = ProductSaleStatus.UnSale;
                    }
                    flag = true;
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalQuantity = (int) reader["Quantity"];
                }
            }
            return flag;
        }

        public override IList<string> GetSkuIdsBysku(string sku)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE SKU = @SKU");
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, sku);
            IList<string> list = new List<string>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((string) reader["SkuId"]);
                }
            }
            return list;
        }

        public override int GetSkuStock(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override IList<OrderLookupListInfo> GetUsableOrderLookupLists()
        {
            IList<OrderLookupListInfo> list = new List<OrderLookupListInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT oll.* FROM Hishop_OrderLookupLists oll INNER JOIN Hishop_OrderLookupItems oli ON oll.LookupListId = oli.LookupListId");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from distro_Orders where OrderStatus=@OrderStatus and orderdate<=@ToDate and orderdate>=@FromDate AND DistributorUserId = @DistributorUserId;");
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "FromDate", DbType.DateTime, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays((double) -days));
            this.database.AddInParameter(sqlStringCommand, "ToDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void RemoveGiftItem(int giftId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_GiftShopingCarts WHERE UserId = @UserId AND GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId AND DistributorUserId = @DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateGiftItemQuantity(int giftId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GiftShopingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateLineItemQuantity(Member member, int productId, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId AND DistributorUserId = @DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

