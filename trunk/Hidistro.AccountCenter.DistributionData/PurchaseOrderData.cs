using Hidistro.AccountCenter.Business;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.AccountCenter.DistributionData
{
    public class PurchaseOrderData : PurchaseOrderProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order)
        {
            if (order == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            string query = "";
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                builder.AppendFormat("'" + info.SkuId + "',", new object[0]);
            }
            if (builder.Length > 0)
            {
                builder = builder.Remove(builder.Length - 1, 1);
                query = string.Format("SELECT S.SkuId, S.CostPrice, p.ProductName FROM Hishop_Products P JOIN Hishop_SKUs S ON P.ProductId = S.ProductId WHERE S.SkuId IN({0});", builder);
            }
            if (order.Gifts.Count > 0)
            {
                StringBuilder builder2 = new StringBuilder();
                foreach (OrderGiftInfo info2 in order.Gifts)
                {
                    builder2.AppendFormat(info2.GiftId.ToString() + ",", new object[0]);
                }
                builder2.Remove(builder2.Length - 1, 1);
                query = query + string.Format(" SELECT GiftId, CostPrice FROM Hishop_Gifts WHERE GiftId IN({0});", builder2.ToString());
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            Dictionary<string, PurchaseOrderItemInfo> dictionary = new Dictionary<string, PurchaseOrderItemInfo>();
            Dictionary<int, decimal> dictionary2 = new Dictionary<int, decimal>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (order.LineItems.Values.Count > 0)
                {
                    while (reader.Read())
                    {
                        PurchaseOrderItemInfo info3 = new PurchaseOrderItemInfo();
                        if (reader["CostPrice"] != DBNull.Value)
                        {
                            info3.ItemCostPrice = (decimal)reader["CostPrice"];
                        }
                        info3.ItemHomeSiteDescription = (string)reader["ProductName"];
                        dictionary.Add((string)reader["SkuId"], info3);
                    }
                }
                if (order.Gifts.Count > 0)
                {
                    if (order.LineItems.Count > 0)
                    {
                        reader.NextResult();
                    }
                    while (reader.Read())
                    {
                        dictionary2.Add((int)reader["GiftId"], (DBNull.Value == reader["CostPrice"]) ? 0M : Convert.ToDecimal(reader["CostPrice"]));
                    }
                }
            }
            IUser user = Users.GetUser(HiContext.Current.SiteSettings.UserId.Value, false);
            if ((user == null) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            PurchaseOrderInfo info4 = new PurchaseOrderInfo();
            info4.PurchaseOrderId = "PO" + order.OrderId;
            info4.OrderId = order.OrderId;
            info4.Remark = order.Remark;
            info4.PurchaseStatus = OrderStatus.WaitBuyerPay;
            info4.DistributorId = distributor.UserId;
            info4.Distributorname = distributor.Username;
            info4.DistributorEmail = distributor.Email;
            info4.DistributorRealName = distributor.RealName;
            info4.DistributorQQ = distributor.QQ;
            info4.DistributorWangwang = distributor.Wangwang;
            info4.DistributorMSN = distributor.MSN;
            info4.ShippingRegion = order.ShippingRegion;
            info4.Address = order.Address;
            info4.ZipCode = order.ZipCode;
            info4.ShipTo = order.ShipTo;
            info4.TelPhone = order.TelPhone;
            info4.CellPhone = order.CellPhone;
            info4.ShippingModeId = order.ShippingModeId;
            info4.ModeName = order.ModeName;
            info4.RegionId = order.RegionId;
            info4.Freight = order.Freight;
            info4.AdjustedFreight = order.Freight;
            info4.ShipOrderNumber = order.ShipOrderNumber;
            info4.Weight = order.Weight;
            info4.RefundStatus = RefundStatus.None;
            info4.OrderTotal = order.GetTotal();
            info4.ExpressCompanyName = order.ExpressCompanyName;
            info4.ExpressCompanyAbb = order.ExpressCompanyAbb;
            foreach (LineItemInfo info5 in order.LineItems.Values)
            {
                PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                item.PurchaseOrderId = info4.PurchaseOrderId;
                item.SkuId = info5.SkuId;
                item.ProductId = info5.ProductId;
                item.SKU = info5.SKU;
                item.Quantity = info5.ShipmentQuantity;
                foreach (KeyValuePair<string, PurchaseOrderItemInfo> pair in dictionary)
                {
                    if (pair.Key == info5.SkuId)
                    {
                        item.ItemCostPrice = pair.Value.ItemCostPrice;
                        item.ItemHomeSiteDescription = pair.Value.ItemHomeSiteDescription;
                    }
                }
                item.ItemPurchasePrice = info5.ItemCostPrice;
                item.ItemListPrice = info5.ItemListPrice;
                item.ItemDescription = info5.ItemDescription;
                item.SKUContent = info5.SKUContent;
                item.ThumbnailsUrl = info5.ThumbnailsUrl;
                item.ItemWeight = info5.ItemWeight;
                if (string.IsNullOrEmpty(item.ItemHomeSiteDescription))
                {
                    item.ItemHomeSiteDescription = item.ItemDescription;
                }
                info4.PurchaseOrderItems.Add(item);
            }
            foreach (OrderGiftInfo info7 in order.Gifts)
            {
                PurchaseOrderGiftInfo info8 = new PurchaseOrderGiftInfo();
                info8.PurchaseOrderId = info4.PurchaseOrderId;
                foreach (KeyValuePair<int, decimal> pair2 in dictionary2)
                {
                    if (pair2.Key == info7.GiftId)
                    {
                        info8.CostPrice = pair2.Value;
                    }
                }
                info8.PurchasePrice = info7.CostPrice;
                info8.GiftId = info7.GiftId;
                info8.GiftName = info7.GiftName;
                info8.Quantity = info7.Quantity;
                info8.ThumbnailsUrl = info7.ThumbnailsUrl;
                info4.PurchaseOrderGifts.Add(info8);
            }
            foreach (OrderOptionInfo info9 in order.OrderOptions)
            {
                PurchaseOrderOptionInfo info10 = new PurchaseOrderOptionInfo();
                info10.PurchaseOrderId = info4.PurchaseOrderId;
                info10.LookupListId = info9.LookupListId;
                info10.LookupItemId = info9.LookupItemId;
                info10.ListDescription = info9.ListDescription;
                info10.ItemDescription = info9.ItemDescription;
                info10.AdjustedPrice = info9.AdjustedPrice;
                info10.CustomerTitle = info9.CustomerTitle;
                info10.CustomerDescription = info9.CustomerDescription;
                info4.PurchaseOrderOptions.Add(info10);
            }
            return info4;
        }

        public override bool CreatePurchaseOrder(OrderInfo order, DbTransaction dbTran)
        {
            string str;
            PurchaseOrderInfo info = ConvertOrderToPurchaseOrder(order);
            if (info == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PurchaseOrders(PurchaseOrderId, OrderId, Remark, ManagerMark, ManagerRemark, AdjustedDiscount,PurchaseStatus, CloseReason, PurchaseDate, DistributorId, Distributorname,DistributorEmail, DistributorRealName, DistributorQQ, DistributorWangwang, DistributorMSN,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShippingModeId, ModeName,RealShippingModeId, RealModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, Weight,RefundStatus, RefundAmount, RefundRemark, OrderTotal, PurchaseProfit, PurchaseTotal,ExpressCompanyName,ExpressCompanyAbb)VALUES (@PurchaseOrderId, @OrderId, @Remark, @ManagerMark, @ManagerRemark, @AdjustedDiscount,@PurchaseStatus, @CloseReason, @PurchaseDate, @DistributorId, @Distributorname,@DistributorEmail, @DistributorRealName, @DistributorQQ, @DistributorWangwang, @DistributorMSN,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShippingModeId, @ModeName,@RealShippingModeId, @RealModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @PurchaseWeight,@RefundStatus, @RefundAmount, @RefundRemark, @OrderTotal, @PurchaseProfit, @PurchaseTotal,@ExpressCompanyName,@ExpressCompanyAbb);");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, info.PurchaseOrderId);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, info.OrderId);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);
            if (info.ManagerMark.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int)info.ManagerMark.Value);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, info.ManagerRemark);
            database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, info.AdjustedDiscount);
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, (int)info.PurchaseStatus);
            database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, info.CloseReason);
            database.AddInParameter(sqlStringCommand, "PurchaseDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, info.DistributorId);
            database.AddInParameter(sqlStringCommand, "Distributorname", DbType.String, info.Distributorname);
            database.AddInParameter(sqlStringCommand, "DistributorEmail", DbType.String, info.DistributorEmail);
            database.AddInParameter(sqlStringCommand, "DistributorRealName", DbType.String, info.DistributorRealName);
            database.AddInParameter(sqlStringCommand, "DistributorQQ", DbType.String, info.DistributorQQ);
            database.AddInParameter(sqlStringCommand, "DistributorWangwang", DbType.String, info.DistributorWangwang);
            database.AddInParameter(sqlStringCommand, "DistributorMSN", DbType.String, info.DistributorMSN);
            database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, info.ShippingRegion);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, info.Address);
            database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, info.ZipCode);
            database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, info.ShipTo);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, info.TelPhone);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, info.CellPhone);
            database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, info.ShippingModeId);
            database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, info.ModeName);
            database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, info.RealShippingModeId);
            database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, info.RealModeName);
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, info.RegionId);
            database.AddInParameter(sqlStringCommand, "Freight", DbType.Currency, info.Freight);
            database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, info.AdjustedFreight);
            database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, info.ShipOrderNumber);
            database.AddInParameter(sqlStringCommand, "PurchaseWeight", DbType.Int32, info.Weight);
            database.AddInParameter(sqlStringCommand, "RefundStatus", DbType.Int32, (int)info.RefundStatus);
            database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Currency, info.RefundAmount);
            database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, info.RefundRemark);
            database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, info.OrderTotal);
            database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, info.GetPurchaseProfit());
            database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Currency, info.GetPurchaseTotal());
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, info.ExpressCompanyAbb);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, info.ExpressCompanyName);
            int num = 0;
            foreach (PurchaseOrderItemInfo info2 in info.PurchaseOrderItems)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderItems(PurchaseOrderId, SkuId, ProductId, SKU, Quantity,  CostPrice, ").Append("ItemListPrice, ItemPurchasePrice, ItemDescription, ItemHomeSiteDescription, SKUContent, ThumbnailsUrl, Weight) VALUES( @PurchaseOrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemPurchasePrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ItemHomeSiteDescription").Append(str).Append(",@SKUContent").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(");");
                database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info2.SkuId);
                database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info2.ProductId);
                database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info2.SKU);
                database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info2.Quantity);
                database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info2.ItemCostPrice);
                database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info2.ItemListPrice);
                database.AddInParameter(sqlStringCommand, "ItemPurchasePrice" + str, DbType.Currency, info2.ItemPurchasePrice);
                database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info2.ItemDescription);
                database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription" + str, DbType.String, info2.ItemHomeSiteDescription);
                database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info2.SKUContent);
                database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info2.ThumbnailsUrl);
                database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info2.ItemWeight);
                num++;
            }
            foreach (PurchaseOrderGiftInfo info3 in info.PurchaseOrderGifts)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId, GiftId, GiftName, CostPrice, PurchasePrice, ").Append("ThumbnailsUrl, Quantity) VALUES( @PurchaseOrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@PurchasePrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(");");
                database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info3.GiftId);
                database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info3.GiftName);
                database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info3.Quantity);
                database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info3.CostPrice);
                database.AddInParameter(sqlStringCommand, "PurchasePrice" + str, DbType.Currency, info3.PurchasePrice);
                database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info3.ThumbnailsUrl);
                num++;
            }
            foreach (PurchaseOrderOptionInfo info4 in info.PurchaseOrderOptions)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderOptions (PurchaseOrderId, LookupListId, LookupItemId, ListDescription, ItemDescription, AdjustedPrice, CustomerTitle, CustomerDescription)").Append(" VALUES (@PurchaseOrderId, @LookupListId").Append(str).Append(", @LookupItemId").Append(str).Append(", @ListDescription").Append(str).Append(", @ItemDescription").Append(str).Append(", @AdjustedPrice").Append(str).Append(", @CustomerTitle").Append(str).Append(", @CustomerDescription").Append(str).Append(");");
                database.AddInParameter(sqlStringCommand, "LookupListId" + str, DbType.Int32, info4.LookupListId);
                database.AddInParameter(sqlStringCommand, "LookupItemId" + str, DbType.Int32, info4.LookupItemId);
                database.AddInParameter(sqlStringCommand, "ListDescription" + str, DbType.String, info4.ListDescription);
                database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info4.ItemDescription);
                database.AddInParameter(sqlStringCommand, "AdjustedPrice" + str, DbType.Currency, info4.AdjustedPrice);
                database.AddInParameter(sqlStringCommand, "CustomerTitle" + str, DbType.String, info4.CustomerTitle);
                database.AddInParameter(sqlStringCommand, "CustomerDescription" + str, DbType.String, info4.CustomerDescription);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

