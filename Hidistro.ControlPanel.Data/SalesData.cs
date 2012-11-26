using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.ControlPanel.Data
{
    public class SalesData : SalesProvider
    {

        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddExpressTemplate(string expressName, string xmlFile)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
            database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            database.AddInParameter(sqlStringCommand, "XmlFile", DbType.String, xmlFile);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
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

        public override bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("select * from Hishop_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    DbCommand command2 = database.GetSqlStringCommand("update Hishop_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
                    database.AddInParameter(command2, "OrderId", DbType.String, orderId);
                    database.AddInParameter(command2, "GiftId", DbType.Int32, gift.GiftId);
                    database.AddInParameter(command2, "Quantity", DbType.Int32, ((int)reader["Quantity"]) + quantity);
                    if (dbTran != null)
                    {
                        return (database.ExecuteNonQuery(command2, dbTran) == 1);
                    }
                    return (database.ExecuteNonQuery(command2) == 1);
                }
                DbCommand command = database.GetSqlStringCommand("INSERT INTO Hishop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,ThumbnailsUrl,Quantity) VALUES(@OrderId,@GiftId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity)");
                database.AddInParameter(command, "OrderId", DbType.String, orderId);
                database.AddInParameter(command, "GiftId", DbType.Int32, gift.GiftId);
                database.AddInParameter(command, "GiftName", DbType.String, gift.GiftName);
                database.AddInParameter(command, "CostPrice", DbType.Currency, gift.CostPrice);
                database.AddInParameter(command, "ThumbnailsUrl", DbType.String, gift.ThumbnailsUrl);
                database.AddInParameter(command, "Quantity", DbType.Int32, quantity);
                if (dbTran != null)
                {
                    return (database.ExecuteNonQuery(command, dbTran) == 1);
                }
                return (database.ExecuteNonQuery(command) == 1);
            }
        }

        public override bool AddOrderLookupItem(OrderLookupItemInfo orderLookupItem)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_OrderLookupItems(LookupListId,[Name],IsUserInputRequired,UserInputTitle,AppendMoney,CalculateMode,Remark) VALUES(@LookupListId,@Name,@IsUserInputRequired,@UserInputTitle,@AppendMoney,@CalculateMode,@Remark)");
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, orderLookupItem.LookupListId);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, orderLookupItem.Name);
            database.AddInParameter(sqlStringCommand, "IsUserInputRequired", DbType.Boolean, orderLookupItem.IsUserInputRequired);
            database.AddInParameter(sqlStringCommand, "UserInputTitle", DbType.String, orderLookupItem.UserInputTitle);
            database.AddInParameter(sqlStringCommand, "AppendMoney", DbType.Currency, orderLookupItem.AppendMoney);
            database.AddInParameter(sqlStringCommand, "CalculateMode", DbType.Int32, orderLookupItem.CalculateMode);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, orderLookupItem.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int AddOrderLookupList(OrderLookupListInfo orderLookupList)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_OrderLookupLists([Name],SelectMode,Description)VALUES(@Name,@SelectMode,@Description); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, orderLookupList.Name);
            database.AddInParameter(sqlStringCommand, "SelectMode", DbType.Int32, orderLookupList.SelectMode);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, orderLookupList.Description);
            object ret = database.ExecuteScalar(sqlStringCommand);
            if (ret != null)
            {
                return Convert.ToInt32(ret);
            }
            return 0;
        }

        public override bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PurchaseOrderItems (PurchaseOrderId,SkuId, ProductId,SKU,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ItemHomeSiteDescription,ThumbnailsUrl,Weight,SKUContent)");
            builder.Append("VALUES(@PurchaseOrderId,@SkuId, @ProductId,@SKU,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ItemHomeSiteDescription,@ThumbnailsUrl,@Weight,@SKUContent);");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, item.SkuId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, item.ProductId);
            database.AddInParameter(sqlStringCommand, "SKU", DbType.String, item.SKU);
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, item.Quantity);
            database.AddInParameter(sqlStringCommand, "ItemListPrice", DbType.Currency, item.ItemListPrice);
            database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", DbType.Currency, item.ItemPurchasePrice);
            database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency, item.CostPrice);
            database.AddInParameter(sqlStringCommand, "ItemDescription", DbType.String, item.ItemDescription);
            database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription", DbType.String, item.ItemDescription);
            database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", DbType.String, item.ThumbnailsUrl);
            database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, item.ItemWeight);
            database.AddInParameter(sqlStringCommand, "SKUContent", DbType.String, item.SKUContent);
            try
            {
                return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
            }
            catch
            {
                return false;
            }
        }

        public override bool AddShipper(ShippersInfo shipper)
        {
            string query = string.Empty;
            if (shipper.IsDefault)
            {
                query = query + "UPDATE Hishop_Shippers SET IsDefault = 0";
            }
            query = query + " INSERT INTO Hishop_Shippers (IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark) VALUES (@IsDefault, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark)";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, shipper.IsDefault);
            database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
        {
            StringBuilder builder = new StringBuilder();
            switch (saleStatisticsType)
            {
                case SaleStatisticsType.SaleCounts:
                    {
                        builder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                        builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
                        break;
                    }
                case SaleStatisticsType.SaleTotal:
                    {
                        builder.Append("SELECT Isnull(SUM(OrderTotal),0)");
                        builder.Append(" FROM Hishop_orders WHERE  (OrderDate BETWEEN @StartDate AND @EndDate)");
                        builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
                        break;
                    }
                case SaleStatisticsType.Profits:
                    {
                        builder.Append("SELECT IsNull(SUM(OrderProfit),0) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                        builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
                        break;
                    }
            }
            return builder.ToString();
        }

        static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_aspnet_Members.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_aspnet_Members.UserId) as OrderCount ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            builder.Append(" from vw_aspnet_Members where Expenditure > 0");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            else
            {
                if (query.PaymentType.HasValue)
                {
                    builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
                }
                if (query.GroupBuyId.HasValue)
                {
                    builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (query.RegionId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.Status == OrderStatus.History)
                {
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderDate < '{2}'", 1, 4, DateTime.Now.AddMonths(-3));
                }
                else if (query.Status != OrderStatus.All)
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.ShippingModeId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            builder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            builder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
            builder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1})", 1, 4);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.Append(" (SELECT ProductId FROM Hishop_Products)");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
            builder.Append(" FROM Hishop_products where SaleCounts>0");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildRegionsUserQuery(Pagination page)
        {
            if (null == page)
            {
                throw new ArgumentNullException("page");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT SUM(COUNT) FROM vw_aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
            builder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
            builder.Append(" group by r.RegionId, r.RegionName ");
            builder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT count(*) FROM vw_aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
            builder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
            if (!string.IsNullOrEmpty(page.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildUserOrderQuery(UserOrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public override bool ChangeMemberGrade(int userId, int gradId, int points)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (((int)reader["GradeId"]) == gradId)
                    {
                        break;
                    }
                    if (((int)reader["Point"]) <= points)
                    {
                        return UpdateUserRank(userId, (int)reader["GradeId"]);
                    }
                }
                return true;
            }
        }

        public override bool ClearOrderGifts(string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId =@OrderId");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,CloseReason=@CloseReason WHERE PurchaseOrderId = @PurchaseOrderId ");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, purchaseOrder.CloseReason);
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 4);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CloseTransaction(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderStatus=@OrderStatus,CloseReason=@CloseReason WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
            database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ConfirmOrderFinish(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET FinishDate = @FinishDate, OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int ConfirmPay(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET PayDate = @PayDate, OrderStatus = @OrderStatus ,OrderPoint=@OrderPoint WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.GetTotalPoints());
            database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus WHERE PurchaseOrderId = @PurchaseOrderId ");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, purchaseOrder.CloseReason);
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 2);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET  PurchaseStatus = @PurchaseStatus, FinishDate=@FinishDate WHERE PurchaseOrderId = @PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 5);
            database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool CreatePrintOrders(int taskId, string[] orderIds, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_PrintTaskOrders(taskId, orderId) VALUES (@taskId, @orderId)");
            database.AddInParameter(sqlStringCommand, "taskId", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "orderId", DbType.String);
            try
            {
                database.SetParameterValue(sqlStringCommand, "taskId", taskId);
                foreach (string str in orderIds)
                {
                    database.SetParameterValue(sqlStringCommand, "orderId", str);
                    if (dbTran != null)
                    {
                        database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override int CreatePrintTask(string creator, DateTime createTime, bool isPO, DbTransaction dbTran)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PrintTask(Creator,CreateDate,IsPO) VALUES(@Creator,@CreateDate,@IsPO);SELECT @@IDENTITY;");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "Creator", DbType.String, creator);
            database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, createTime);
            database.AddInParameter(sqlStringCommand, "IsPO", DbType.Boolean, isPO);
            if (dbTran != null)
            {
                return Convert.ToInt32(database.ExecuteScalar(sqlStringCommand, dbTran));
            }
            return Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
        }

        public override bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag = false;

            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ShippingMode_Create");

            database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);

            using (DbConnection connection = database.CreateConnection())
            {

                connection.Open();

                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    database.ExecuteNonQuery(storedProcCommand, transaction);

                    flag = ((int)database.GetParameterValue(storedProcCommand, "Status")) == 0;

                    if (flag)
                    {

                        int parameterValue = (int)database.GetParameterValue(storedProcCommand, "ModeId");

                        DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");

                        database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, parameterValue);

                        if ((shippingMode.ExpressCompany != null) && (shippingMode.ExpressCompany.Count > 0))
                        {
                            StringBuilder builder = new StringBuilder();
                            int num2 = 0;

                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");

                            foreach (ExpressCompanyInfo info in shippingMode.ExpressCompany)
                            {
                                builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName,ExpressCompanyAbb) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num2).Append(",@ExpressCompanyAbb").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num2, DbType.String, info.ExpressCompanyName);
                                database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb" + num2, DbType.String, info.ExpressCompanyAbb);
                                num2++;
                            }

                            sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();

                            int num3 = (int)database.ExecuteScalar(sqlStringCommand, transaction);

                            if (num3 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }

                        }

                    }

                    transaction.Commit();

                }
                catch
                {

                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }

                    flag = false;

                }
                finally
                {
                    if (null != connection)
                    {
                        connection.Close();
                    }
                }

            }

            return flag;

        }

        public override bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_ShippingTemplates(TemplateName,Weight,AddWeight,Price,AddPrice) VALUES(@TemplateName,@Weight,@AddWeight,@Price,@AddPrice);SELECT @@Identity");
            database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, shippingMode.Weight);
            if (shippingMode.AddWeight.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, shippingMode.AddWeight);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, 0);
            }
            database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            if (shippingMode.AddPrice.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, 0);
            }

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    object obj2 = database.ExecuteScalar(sqlStringCommand, transaction);
                    int result = 0;
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        int.TryParse(obj2.ToString(), out result);
                        flag = result > 0;
                    }
                    if (flag)
                    {
                        DbCommand command = database.GetSqlStringCommand(" ");
                        database.AddInParameter(command, "TemplateId", DbType.Int32, result);
                        if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                        {
                            StringBuilder builder = new StringBuilder();
                            int num2 = 0;
                            int num3 = 0;
                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                            builder.Append(" DECLARE @GroupId Int;");
                            foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                            {
                                builder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num2).Append(",@AddPrice").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                database.AddInParameter(command, "Price" + num2, DbType.Currency, info.Price);
                                database.AddInParameter(command, "AddPrice" + num2, DbType.Currency, info.AddPrice);
                                builder.Append("Set @GroupId =@@identity;");
                                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                {
                                    builder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    database.AddInParameter(command, "RegionId" + num3, DbType.Int32, info2.RegionId);
                                    num3++;
                                }
                                num2++;
                            }
                            command.CommandText = builder.Append("SELECT @ERR;").ToString();

                            int num4 = (int)database.ExecuteScalar(command, transaction);

                            if (num4 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }

                        }

                    }

                    transaction.Commit();

                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }
                finally
                {
                    if (null != connection)
                    {
                        connection.Close();
                    }
                }

            }
            return flag;
        }

        DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Date", typeof(int));
            DataColumn column2 = new DataColumn("SaleTotal", typeof(decimal));
            DataColumn column3 = new DataColumn("Percentage", typeof(decimal));
            DataColumn column4 = new DataColumn("Lenth", typeof(decimal));
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            return table;
        }

        public override PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action == DataProviderAction.Create)
            {
                database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            }
            else
            {
                database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, paymentMode.ModeId);
            }
            if (action != DataProviderAction.Delete)
            {
                database.AddInParameter(storedProcCommand, "Name", DbType.String, paymentMode.Name);
                database.AddInParameter(storedProcCommand, "Description", DbType.String, paymentMode.Description);
                database.AddInParameter(storedProcCommand, "Gateway", DbType.String, paymentMode.Gateway);
                database.AddInParameter(storedProcCommand, "IsUseInpour", DbType.Boolean, paymentMode.IsUseInpour);
                database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
                database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
                database.AddInParameter(storedProcCommand, "Settings", DbType.String, paymentMode.Settings);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (PaymentModeActionStatus)((int)database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override bool DeleteExpressTemplate(int expressId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId = @ExpressId");
            database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteOrderLookupItem(int orderLookupItemId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_OrderLookupItems WHERE LookupItemId =@LookupItemId");
            database.AddInParameter(sqlStringCommand, "LookupItemId", DbType.Int32, orderLookupItemId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteOrderLookupList(int orderLookupListId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_OrderLookupLists WHERE LookupListId = @LookupListId");
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, orderLookupListId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeleteOrders(string orderIds)
        {
            string query = string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeletePrintOrder(string orderId, int taskId)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE Hishop_PrintTaskOrders WHERE orderId=@orderId AND taskId=@taskId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "taskId", DbType.Int32, taskId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePrintOrderByTaskId(int taskId, DbTransaction dbTran)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE Hishop_PrintTaskOrders WHERE taskId=@taskId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "taskId", DbType.Int32, taskId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePrintTask(int taskId, DbTransaction dbTran)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE Hishop_PrintTask WHERE taskId=@taskId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "taskId", DbType.Int32, taskId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePurchaseOrderItem(string POrderId, string skuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeletePurchaseOrders(string purchaseOrderIds)
        {
            string query = string.Format("DELETE FROM Hishop_PurchaseOrders WHERE PurchaseOrderId IN({0})", purchaseOrderIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteShipper(int shipperId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteShippingMode(int modeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_TemplateRelatedShipping Where ModeId=@ModeId;DELETE FROM Hishop_ShippingTypes Where ModeId=@ModeId;UPDATE Hishop_PurchaseOrders set ShippingModeId=0 where ShippingModeId=@ModeId");
            database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            database.AddOutParameter(sqlStringCommand, "Status", DbType.Int32, 4);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteShippingTemplate(int templateId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShippingTemplates Where TemplateId=@TemplateId");
            database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override decimal GetAddUserTotal(int year)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate)  AS UserAdd");
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            return ((obj2 == null) ? 0M : Convert.ToDecimal(obj2));
        }

        public override int GetCurrentPOrderItemQuantity(string POrderId, string skuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Quantity FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != DBNull.Value) && (obj2 != null))
            {
                return (int)obj2;
            }
            return 0;
        }

        int GetDayCount(int year, int month)
        {
            if (month == 2)
            {
                if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0))
                {
                    return 0x1d;
                }
                return 0x1c;
            }
            if (((((month == 1) || (month == 3)) || ((month == 5) || (month == 7))) || ((month == 8) || (month == 10))) || (month == 12))
            {
                return 0x1f;
            }
            return 30;
        }

        public override DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = CreateTable();
            decimal allSalesTotal = GetMonthSaleTotal(year, month, saleStatisticsType);
            int dayCount = GetDayCount(year, month);
            int num3 = ((year == DateTime.Now.Year) && (month == DateTime.Now.Month)) ? DateTime.Now.Day : dayCount;
            for (int i = 1; i <= num3; i++)
            {
                DateTime time = new DateTime(year, month, i);
                DateTime time2 = time.AddDays(1.0);
                database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                InsertToTable(table, i, salesTotal, allSalesTotal);
            }
            return table;
        }

        public override decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, day);
            DateTime time2 = time.AddDays(1.0);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
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

        public override DataTable GetExpressTemplates()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<GiftInfo> GetGiftList(GiftQuery query)
        {
            IList<GiftInfo> list = new List<GiftInfo>();
            string str = string.Format("SELECT * FROM Hishop_Gifts WHERE [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            DbCommand sqlStringCommand = database.GetSqlStringCommand(str);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateGift(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = null;
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public override int GetHistoryPoint(int userId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override DataTable GetIsUserExpressTemplates()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates WHERE IsUse = 1");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<LineItemInfo> GetLineItemInfo(string orderId)
        {
            List<LineItemInfo> list = new List<LineItemInfo>();
            try
            {
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
                database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(DataMapper.PopulateLineItem(reader));
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public override LineItemInfo GetLineItemInfo(string skuId, string orderId)
        {
            LineItemInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems WHERE SkuId=@SkuId AND OrderId=@OrderId");
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateLineItem(reader);
                }
            }
            return info;
        }

        public override void GetLineItemPromotions(int productId, int quantity, out int purchaseGiftId, out string purchaseGiftName, out int giveQuantity, out int wholesaleDiscountId, out string wholesaleDiscountName, out decimal? discountRate, int gradeId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_LineItem_GetPromotionsInfo");
            database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, gradeId);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                purchaseGiftId = 0;
                giveQuantity = 0;
                wholesaleDiscountId = 0;
                purchaseGiftName = null;
                wholesaleDiscountName = null;
                discountRate = 0;
                if (reader.Read())
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
                        giveQuantity = (quantity / ((int)reader["BuyQuantity"])) * ((int)reader["GiveQuantity"]);
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
        }

        public override DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_MemberStatistics_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildMemberStatisticsQuery(query));
            database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int)database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(BuildMemberStatisticsQuery(query));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = CreateTable();
            int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
            for (int i = 1; i <= num; i++)
            {
                DateTime time = new DateTime(year, i, 1);
                DateTime time2 = time.AddMonths(1);
                database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                decimal yearSaleTotal = GetYearSaleTotal(year, saleStatisticsType);
                InsertToTable(table, i, salesTotal, yearSaleTotal);
            }
            return table;
        }

        public override decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, 1);
            DateTime time2 = time.AddMonths(1);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public override OrderGiftInfo GetOrderGift(int giftId, string orderId)
        {
            OrderGiftInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrderGift(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select top {0} * from Hishop_OrderGifts where OrderId=@OrderId", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.Append(" ORDER BY GiftId ASC");
            }
            else
            {
                builder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Hishop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(GiftId) as Total from Hishop_OrderGifts where OrderId=@OrderId", new object[0]);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, query.OrderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (query.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId; SELECT  * FROM Hishop_OrderOptions Where OrderId = @OrderId; SELECT  * FROM Hishop_OrderGifts Where OrderId = @OrderId; SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
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
                    info.LineItems.Add((string)reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public override OrderLookupItemInfo GetOrderLookupItem(int lookupItemId)
        {
            OrderLookupItemInfo entity = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderLookupItems Where LookupItemId = @LookupItemId");
            database.AddInParameter(sqlStringCommand, "LookupItemId", DbType.Int32, lookupItemId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    entity = DataMapper.PopulateOrderLookupItem(reader);
                }
            }
            Globals.EntityCoding(entity, false);
            return entity;
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

        public override IList<OrderLookupListInfo> GetOrderLookupLists()
        {
            IList<OrderLookupListInfo> list = new List<OrderLookupListInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_OrderLookupLists");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    OrderLookupListInfo item = new OrderLookupListInfo();
                    item.Name = (string)reader["Name"];
                    item.LookupListId = (int)reader["LookupListId"];
                    item.SelectMode = (SelectModeTypes)reader["SelectMode"];
                    if (DBNull.Value != reader["Description"])
                    {
                        item.Description = (string)reader["Description"];
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
        {
            IList<OrderPriceStatisticsForChartInfo> list = new List<OrderPriceStatisticsForChartInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT isnull((SELECT sum(Amount) FROM Hishop_Orders WHERE OrderDate BETWEEN @StartDate AND @EndDate),0) AS Price");
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DateTime date = new DateTime();
            DateTime time2 = new DateTime();
            date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)-days);
            for (int i = 1; i <= days; i++)
            {
                OrderPriceStatisticsForChartInfo item = new OrderPriceStatisticsForChartInfo();
                if (i > 1)
                {
                    date = time2;
                }
                time2 = date.AddDays(1.0);
                database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
                database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                item.Price = (decimal)database.ExecuteScalar(sqlStringCommand);
                item.TimePoint = date.Day;
                list.Add(item);
            }
            return list;
        }

        public override DbQueryResult GetOrders(OrderQuery query)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Orders_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildOrdersQuery(query));
            database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
            DbQueryResult result = new DbQueryResult();
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            result.TotalRecords = (int)database.GetParameterValue(storedProcCommand, "TotalOrders");
            return result;
        }

        public override DataSet GetOrdersAndLines(int taskId, bool printAll)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_Orders WHERE OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId)");
            if (printAll)
            {
                builder.Append(" AND ISNULL(IsPrinted,0)=0");
            }
            builder.Append(";SELECT * FROM Hishop_OrderItems WHERE OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("SELECT * FROM Hishop_OrderOptions WHERE OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("SELECT * FROM Hishop_OrderGifts WHERE OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
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

        public override PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetPrintTasks(Pagination query)
        {
            DbQueryResult result = new DbQueryResult();
            database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("sp_PrintTasks_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, "SELECT taskId FROM Hishop_PrintTask WHERE 1 = 1 ORDER BY taskId DESC");
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductSales_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int)database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int)database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductVisitAndBuyStatisticsQuery(query));
            database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int)database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
            builder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
            builder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
            sqlStringCommand.CommandText = builder.ToString();
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    table = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalProductSales = (int)reader["TotalProductSales"];
                    return table;
                }
                totalProductSales = 0;
            }
            return table;
        }

        public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            PurchaseOrderInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId SELECT  * FROM Hishop_PurchaseOrderOptions Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderGifts Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderItems Where PurchaseOrderId = @PurchaseOrderId ");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePurchaseOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.PurchaseOrderOptions.Add(DataMapper.PopulatePurchaseOrderOption(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.PurchaseOrderGifts.Add(DataMapper.PopulatePurchaseOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.PurchaseOrderItems.Add(DataMapper.PopulatePurchaseOrderItem(reader));
                }
            }
            return info;
        }

        public override DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            if (!string.IsNullOrEmpty(query.PurchaseOrderId))
            {
                builder.AppendFormat("PurchaseOrderId = '{0}'", query.PurchaseOrderId);
            }
            else
            {
                if (!string.IsNullOrEmpty(query.DistributorName))
                {
                    builder.AppendFormat("DistributorName = '{0}'", query.DistributorName);
                    flag = true;
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" ShipTo LIKE '%{0}%'", query.ShipTo);
                    flag = true;
                }
                if (!string.IsNullOrEmpty(query.OrderId))
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" OrderId = '{0}'", query.OrderId);
                    flag = true;
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                    flag = true;
                }
                if (query.StartDate.HasValue)
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" PurchaseDate >= '{0}'", query.StartDate.Value);
                    flag = true;
                }
                if (query.EndDate.HasValue)
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" PurchaseDate <= '{0}'", query.EndDate.Value);
                    flag = true;
                }
                if (query.PurchaseStatus != OrderStatus.All)
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
                }
                if (query.ShippingModeId.HasValue)
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    if (flag)
                    {
                        builder.Append(" AND");
                    }
                    builder.AppendFormat(" ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_PurchaseOrders", "PurchaseOrderId", builder.ToString(), "OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal, PurchaseStatus,Distributorname,DistributorWangwang,ManagerMark,ManagerRemark,DistributorId,ISNULL(IsPrinted,0) IsPrinted");
        }

        public override DataSet GetPurchaseOrdersAndLines(int taskId, bool printAll)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_PurchaseOrders WHERE PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId)");
            if (printAll)
            {
                builder.Append(" AND ISNULL(IsPrinted,0)=0");
            }
            builder.Append(";SELECT * FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("SELECT * FROM Hishop_PurchaseOrderOptions WHERE PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("SELECT * FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("SELECT * FROM Hishop_Shippers ORDER BY DistributorUserId, IsDefault DESC");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public override DataTable GetRecentlyOrders(out int number)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 12 OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal,ISNULL(GroupBuyId,0) as GroupBuyId,ISNULL(GroupBuyStatus,0) as GroupBuyStatus, PaymentType,ManagerMark, OrderStatus, RefundStatus,ManagerRemark FROM Hishop_Orders ORDER BY OrderDate DESC");
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = database.GetSqlStringCommand("SELECT count(*) FROM Hishop_Orders WHERE  OrderStatus=2");
            number = Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DataTable GetRecentlyPurchaseOrders(out int number)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders ORDER BY PurchaseDate DESC ");
            DataTable table = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE  PurchaseStatus=2 ");
            number = Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value);
            }
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
            if (query.StartDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate >= '{0}'", query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate <= '{0}'", query.EndDate);
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format("AND OrderStatus != {0} AND OrderStatus != {1}", 1, 4);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public override DbQueryResult GetSaleTargets()
        {
            DbQueryResult result = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("select  (select Count(OrderId) from Hishop_orders) as OrderNumb ,ISNULL((select sum(OrderTotal) from hishop_orders),0) as OrderPrice,  (select COUNT(*) from vw_aspnet_Members) as UserNumb,  (select count(*) from vw_aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ", new object[0]);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public override DataTable GetSendGoodsOrders(string orderIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT OrderId, OrderDate,RefundStatus, ShipTo, OrderTotal, OrderStatus,ShippingRegion,Address,ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber," + string.Format(" ExpressCompanyAbb,ExpressCompanyName FROM Hishop_Orders WHERE OrderStatus = 2 AND OrderId IN ({0})", orderIds));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal, PurchaseStatus,Distributorname,ShippingRegion,Address, ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber,ExpressCompanyAbb,ExpressCompanyName FROM Hishop_PurchaseOrders" + string.Format(" WHERE PurchaseStatus = 2 AND PurchaseOrderId IN ({0})", purchaseOrderIds));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ShippersInfo GetShipper(int shipperId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            ShippersInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShipper(reader);
                }
            }
            return info;
        }

        public override IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers");
            if (!includeDistributor)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " WHERE DistributorUserId = 0";
            }
            IList<ShippersInfo> list = new List<ShippersInfo>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShipper(reader));
                }
            }
            return list;
        }

        public override DataTable GetShippingAllTemplates()
        {
            string query = "SELECT * FROM Hishop_ShippingTemplates ";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId";
            }
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

        public override IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId";
            if (!string.IsNullOrEmpty(paymentGateway))
            {
                query = query + " WHERE Gateway = @Gateway)";
            }
            query = query + " Order By DisplaySequence";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            if (!string.IsNullOrEmpty(paymentGateway))
            {
                database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, paymentGateway);
            }
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public override ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTemplates Where TemplateId =@TemplateId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId =@TemplateId";
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
            }
            database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingTemplate(reader);
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
            }
            return info;
        }

        public override DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Hishop_ShippingTemplates", "TemplateId", "", "*");
        }

        public override int GetSkuStock(string skuId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int)obj2;
            }
            return 0;
        }

        public override AdminStatisticsInfo GetStatistics()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT  (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderStatus = 2) AS orderNumbWaitConsignment, (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2) AS purchaseOrderNumbWaitConsignment, (select Count(LeaveId) from Hishop_LeaveComments l where (select count(replyId) from Hishop_LeaveCommentReplys where leaveId =l.leaveId)=0) as leaveComments,(select Count(ConsultationId) from Hishop_ProductConsultations where ReplyUserId is null) as productConsultations,(select Count(ReceiveMessageId) from Hishop_ReceivedMessages where IsRead=0 and Addressee='admin' and Addresser in (select UserName from vw_aspnet_Members)) as messages, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus=2 or  OrderStatus=3 or OrderStatus=5)   and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderPriceToday, isnull((select sum(PurchaseProfit) from Hishop_PurchaseOrders where  (PurchaseStatus=2 or  PurchaseStatus=3 or PurchaseStatus=5)   and PurchaseDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as PurchaseOrderProfitToday, isnull((select sum(OrderProfit) from Hishop_Orders where  (OrderStatus=2 or  OrderStatus=3 or OrderStatus=5)  and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderProfitToday, (select count(*) from vw_aspnet_Members where CreateDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' ) as userNewAddToday, (select count(*) from vw_aspnet_Distributors where CreateDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' ) as distroNewAddToday, isnull((select sum(balance) from vw_aspnet_Members),0) as memberBalance, isnull((select sum(balance) from vw_aspnet_Distributors),0) as distroBalance,(select count(*) from (select ProductId from Hishop_SKUs where Stock<=AlertStock group by ProductId) as a) as productAlert");
            AdminStatisticsInfo info = new AdminStatisticsInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.OrderNumbWaitConsignment = (int)reader["orderNumbWaitConsignment"];
                    info.PurchaseOrderNumbWaitConsignment = (int)reader["purchaseOrderNumbWaitConsignment"];
                    info.LeaveComments = (int)reader["LeaveComments"];
                    info.ProductConsultations = (int)reader["ProductConsultations"];
                    info.Messages = (int)reader["Messages"];
                    info.PurchaseOrderProfitToday = (decimal)reader["PurchaseOrderProfitToday"];
                    info.OrderProfitToday = (decimal)reader["orderProfitToday"];
                    info.UserNewAddToday = (int)reader["userNewAddToday"];
                    info.DistroButorsNewAddToday = (int)reader["distroNewAddToday"];
                    info.MembersBalance = (decimal)reader["memberBalance"];
                    info.DistrosBalance = (decimal)reader["distroBalance"];
                    info.OrderPriceToday = (decimal)reader["orderPriceToday"];
                    info.ProductAlert = (int)reader["productAlert"];
                }
            }
            return info;
        }

        public override bool GetTaskIsPrintedAll(int taskId)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT COUNT(*) FROM Hishop_Orders WHERE ISNULL(IsPrinted,0)=0 AND OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId)");
            builder.Append("SELECT COUNT(*) FROM Hishop_PurchaseOrders WHERE ISNULL(IsPrinted,0)=0 AND PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId)");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            int num = 0;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num += Convert.ToInt32(reader[0]);
                }
                if (reader.NextResult() && reader.Read())
                {
                    num += Convert.ToInt32(reader[0]);
                }
            }
            return (num == 0);
        }

        public override TaskPrintInfo GetTaskPrintInfo(int taskId)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT PTO.*,(SELECT COUNT(*) FROM Hishop_PrintTaskOrders PTO INNER JOIN Hishop_Orders O ON O.OrderId=PTO.OrderId WHERE PTO.taskId=@TaskId AND O.IsPrinted=1) as OrderCount ,(SELECT COUNT(*) FROM Hishop_PrintTaskOrders PTO INNER JOIN Hishop_PurchaseOrders O ON PTO.OrderId=O.PurchaseOrderId WHERE PTO.taskId=@TaskId AND O.IsPrinted=1) as PurchaseCount FROM Hishop_PrintTask PTO WHERE PTO.TaskId=@TaskId;");
            builder.Append("SELECT PTO.OrderId,O.address,O.ModeName,O.ShippingRegion,O.ShipTo,ISNULL(O.IsPrinted,0) IsPrinted,O.Remark,O.ManagerRemark FROM Hishop_PrintTaskOrders PTO INNER JOIN Hishop_Orders O ON O.OrderId=PTO.OrderId WHERE taskId=@TaskId UNION SELECT PTO.OrderId,O.address,O.ModeName,O.ShippingRegion,O.ShipTo,ISNULL(O.IsPrinted,0) IsPrinted,O.Remark,O.ManagerRemark FROM Hishop_PrintTaskOrders PTO INNER JOIN Hishop_PurchaseOrders O ON PTO.OrderId=O.PurchaseOrderId WHERE taskId=@TaskId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            TaskPrintInfo info = new TaskPrintInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.TaskId = (int)reader["TaskId"];
                    if (reader["Creator"] != DBNull.Value)
                    {
                        info.Creator = (string)reader["Creator"];
                    }
                    if (reader["CreateDate"] != DBNull.Value)
                    {
                        info.CreateTime = (DateTime)reader["CreateDate"];
                    }
                    info.HasPrinted = ((int)reader["OrderCount"]) + ((int)reader["PurchaseCount"]);
                    if (reader["IsPO"] != DBNull.Value)
                    {
                        info.IsPO = (bool)reader["IsPO"];
                    }
                }
                if (reader.NextResult())
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    info.Orders = table;
                }
            }
            return info;
        }

        public override IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            int num;
            UserStatisticsForDate date;
            int num4;
            IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate) AS UserAdd ");
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DateTime time = new DateTime();
            DateTime time2 = new DateTime();
            if (days.HasValue)
            {
                time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)-days.Value);
            }
            else if (year.HasValue && month.HasValue)
            {
                time = new DateTime(year.Value, month.Value, 1);
            }
            else if (!(!year.HasValue || month.HasValue))
            {
                time = new DateTime(year.Value, 1, 1);
            }
            if (!days.HasValue)
            {
                if (year.HasValue && month.HasValue)
                {
                    int num2 = DateTime.DaysInMonth(year.Value, month.Value);
                    for (num = 1; num <= num2; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddDays(1.0);
                        database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int)database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                    return list;
                }
                if (year.HasValue && !month.HasValue)
                {
                    int num3 = 12;
                    for (num = 1; num <= num3; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddMonths(1);
                        database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int)database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                }
                return list;
            }
            num = 1;
        Label_01A9:
            num4 = num;
            if (num4 <= days)
            {
                date = new UserStatisticsForDate();
                if (num > 1)
                {
                    time = time2;
                }
                time2 = time.AddDays(1.0);
                database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                date.UserCounts = (int)database.ExecuteScalar(sqlStringCommand);
                date.TimePoint = time.Day;
                list.Add(date);
                num++;
                goto Label_01A9;
            }
            return list;
        }

        public override OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_OrderStatistics_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfPage += (decimal)reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfPage += (decimal)reader["Profits"];
                    }
                }
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal)reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal)reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int)database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public override OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal)reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal)reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int)database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public override IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
            IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                UserStatisticsInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateUserStatistics(reader);
                    list.Add(item);
                }
                if (item != null)
                {
                    totalRegionsUsers = int.Parse(item.AllUserCounts.ToString());
                    return list;
                }
                totalRegionsUsers = 0;
            }
            return list;
        }

        public override DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            DateTime time = DateTime.Now.AddDays(-7.0);
            DateTime time2 = new DateTime(time.Year, time.Month, time.Day);
            DateTime now = DateTime.Now;
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time2);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, now);
            decimal allSalesTotal = 0M;
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                allSalesTotal = Convert.ToDecimal(obj2);
            }
            DataTable table = CreateTable();
            for (int i = 0; i < 7; i++)
            {
                DateTime time4 = DateTime.Now.AddDays((double)-i);
                decimal salesTotal = GetDaySaleTotal(time4.Year, time4.Month, time4.Day, saleStatisticsType);
                InsertToTable(table, time4.Day, salesTotal, allSalesTotal);
            }
            return table;
        }

        public override decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        void InsertToTable(DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
        {
            DataRow row = table.NewRow();
            row["Date"] = date;
            row["SaleTotal"] = salesTotal;
            if (allSalesTotal != 0M)
            {
                row["Percentage"] = (salesTotal / allSalesTotal) * 100M;
            }
            else
            {
                row["Percentage"] = 0;
            }
            row["Lenth"] = ((decimal)row["Percentage"]) * 4M;
            table.Rows.Add(row);
        }

        public override bool RefundOrder(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET RefundAmount=@RefundAmount,RefundRemark=@RefundRemark,RefundStatus=@RefundStatus,OrderStatus=@OrderStatus,OrderProfit=@OrderProfit, FinishDate=@FinishDate WHERE OrderId=@OrderId");
            database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, order.RefundAmount);
            database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Decimal, order.GetProfit());
            database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, order.RefundRemark);
            database.AddInParameter(sqlStringCommand, "RefundStatus", DbType.Int32, (int)order.RefundStatus);
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)order.OrderStatus);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool RefundPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET RefundAmount=@RefundAmount,RefundRemark=@RefundRemark,RefundStatus=@RefundStatus,PurchaseStatus=@PurchaseStatus,PurchaseProfit=@PurchaseProfit,FinishDate=@FinishDate WHERE PurchaseOrderId=@PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, purchaseOrder.RefundAmount);
            database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, purchaseOrder.GetPurchaseProfit());
            database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, purchaseOrder.RefundRemark);
            database.AddInParameter(sqlStringCommand, "RefundStatus", DbType.Int32, 2);
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, (int)purchaseOrder.PurchaseStatus);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, purchaseOrder.FinishDate);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int ResetOrderPrintStatus(IList<OrderInfo> orders)
        {
            int num = 0;
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET IsPrinted=0 WHERE OrderId=@OrderId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String);
            try
            {
                foreach (OrderInfo info in orders)
                {
                    database.SetParameterValue(sqlStringCommand, "OrderId", info.OrderId);
                    num += database.ExecuteNonQuery(sqlStringCommand);
                }
            }
            catch
            {
            }
            return num;
        }

        public override bool SaveOrderRemark(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE OrderId=@OrderId");
            if (order.ManagerMark.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int)order.ManagerMark.Value);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE PurchaseOrderId=@PurchaseOrderId");
            if (purchaseOrder.ManagerMark.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int)purchaseOrder.ManagerMark.Value);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, purchaseOrder.ManagerRemark);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE PurchaseOrderId = @PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, purchaseOrder.RegionId);
            database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, purchaseOrder.ShippingRegion);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, purchaseOrder.Address);
            database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, purchaseOrder.ZipCode);
            database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, purchaseOrder.ShipTo);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, purchaseOrder.TelPhone);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, purchaseOrder.CellPhone);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SaveShippingAddress(OrderInfo order)
        {
            if (order == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int SendGoods(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, PurchaseStatus = @PurchaseStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName , ExpressCompanyAbb = @ExpressCompanyAbb WHERE PurchaseOrderId = @PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, purchaseOrder.ShipOrderNumber);
            database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, purchaseOrder.RealShippingModeId);
            database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, purchaseOrder.RealModeName);
            database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 3);
            database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, purchaseOrder.ExpressCompanyName);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, purchaseOrder.ExpressCompanyAbb);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SetDefalutShipper(int shipperId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0;UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId");
            database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool SetExpressIsUse(int expressId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
            database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE OrderStatus = 2 AND OrderId IN ({0})", orderIds));
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderStatus = 2 AND OrderId =@OrderId");
            database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE OrderStatus = 2 AND OrderId IN ({0})", orderIds));
            database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE PurchaseStatus = 2 AND PurchaseOrderId IN ({0})", purchaseOrderIds));
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderShipNumber(string purchaseOrderId, string shipNumber)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber=@ShipOrderNumber WHERE PurchaseStatus = 2 AND PurchaseOrderId =@PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE PurchaseStatus = 2 AND PurchaseOrderId IN ({0})", purchaseOrderIds));
            database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetTaskIsExport(int taskId)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PrintTask SET IsExport=1 WHERE TaskId=@TaskId");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SetTaskOrderPrinted(int taskId)
        {
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET IsPrinted=1 WHERE OrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId);");
            builder.Append("UPDATE Hishop_PurchaseOrders SET IsPrinted=1 WHERE PurchaseOrderId IN (SELECT Orderid FROM Hishop_PrintTaskOrders WHERE TaskId=@TaskId)");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "TaskId", DbType.Int32, taskId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ShippingTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateDistributorAccount(decimal expenditure, int distributorId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Distributors SET Expenditure=Expenditure+@expenditureAdd, PurchaseOrder = PurchaseOrder + 1 WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            database.AddInParameter(sqlStringCommand, "expenditureAdd", DbType.Decimal, expenditure);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateExpressTemplate(int expressId, string expressName)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
            database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PurchaseGiftId=@PurchaseGiftId,PurchaseGiftName=@PurchaseGiftName,WholesaleDiscountId=@WholesaleDiscountId,WholesaleDiscountName=@WholesaleDiscountName  WHERE OrderId=@OrderId AND SkuId=@SkuId");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
            database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
            database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
            database.AddInParameter(sqlStringCommand, "PurchaseGiftId", DbType.Int32, lineItem.PurchaseGiftId);
            database.AddInParameter(sqlStringCommand, "PurchaseGiftName", DbType.String, lineItem.PurchaseGiftName);
            database.AddInParameter(sqlStringCommand, "WholesaleDiscountId", DbType.Int32, lineItem.WholesaleDiscountId);
            database.AddInParameter(sqlStringCommand, "WholesaleDiscountName", DbType.String, lineItem.WholesaleDiscountName);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderAmount(OrderInfo order, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderTotal = @OrderTotal, OrderProfit=@OrderProfit, AdjustedFreight = @AdjustedFreight, AdjustedPayCharge = @AdjustedPayCharge, AdjustedDiscount=@AdjustedDiscount, OrderPoint=@OrderPoint, Amount=@Amount,OrderCostPrice=@OrderCostPrice WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            database.AddInParameter(sqlStringCommand, "AdjustedPayCharge", DbType.Currency, order.AdjustedPayCharge);
            database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.GetTotalPoints());
            database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderLookupItem(OrderLookupItemInfo orderLookupItem)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_OrderLookupItems SET LookupListId = @LookupListId,[Name] = @Name, IsUserInputRequired = @IsUserInputRequired,UserInputTitle = @UserInputTitle, AppendMoney = @AppendMoney,CalculateMode = @CalculateMode,Remark=@Remark WHERE LookupItemId =@LookupItemId");
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, orderLookupItem.LookupListId);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, orderLookupItem.Name);
            database.AddInParameter(sqlStringCommand, "IsUserInputRequired", DbType.Boolean, orderLookupItem.IsUserInputRequired);
            database.AddInParameter(sqlStringCommand, "UserInputTitle", DbType.String, orderLookupItem.UserInputTitle);
            database.AddInParameter(sqlStringCommand, "AppendMoney", DbType.Currency, orderLookupItem.AppendMoney);
            database.AddInParameter(sqlStringCommand, "CalculateMode", DbType.Int32, orderLookupItem.CalculateMode);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, orderLookupItem.Remark);
            database.AddInParameter(sqlStringCommand, "LookupItemId", DbType.Int32, orderLookupItem.LookupItemId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateOrderLookupList(OrderLookupListInfo orderLookupList)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_OrderLookupLists Set [Name]= @Name,SelectMode = @SelectMode,Description = @Description WHERE LookupListId = @LookupListId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, orderLookupList.Name);
            database.AddInParameter(sqlStringCommand, "SelectMode", DbType.Int32, orderLookupList.SelectMode);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, orderLookupList.Description);
            database.AddInParameter(sqlStringCommand, "LookupListId", DbType.Int32, orderLookupList.LookupListId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateOrderPaymentType(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET PaymentTypeId=@PaymentTypeId ,PaymentType=@PaymentType WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderShippingMode(OrderInfo order)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE OrderId = @OrderId");
            database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void UpdatePayOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems)
        {
            if (lineItems.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (LineItemInfo info in lineItems.Values)
            {
                builder.Append("UPDATE Hishop_Products SET SaleCounts=ISNULL(SaleCounts,0)+@SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(";");
                database.AddInParameter(sqlStringCommand, "SaleCounts" + num, DbType.Int32, info.Quantity);
                database.AddInParameter(sqlStringCommand, "ProductId" + num, DbType.Int32, info.ProductId);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateProductStock(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Weight=@Weight,PurchaseProfit=@PurchaseProfit,PurchaseTotal=@PurchaseTotal,AdjustedFreight=@AdjustedFreight WHERE PurchaseOrderId=@PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, purchaseOrder.Weight);
            database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Decimal, purchaseOrder.GetPurchaseProfit());
            database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Decimal, purchaseOrder.GetPurchaseTotal());
            database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, purchaseOrder.AdjustedFreight);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseTotal=@PurchaseTotal, PurchaseProfit=@PurchaseProfit, AdjustedDiscount=@AdjustedDiscount WHERE PurchaseOrderId=@PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Currency, purchaseOrder.GetPurchaseTotal());
            database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, purchaseOrder.GetPurchaseProfit());
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, purchaseOrder.AdjustedDiscount);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrderItems SET Quantity=@Quantity WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId;");
            database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, Quantity);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName WHERE PurchaseOrderId = @PurchaseOrderId");
            database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, purchaseOrder.ShippingModeId);
            database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, purchaseOrder.ModeName);
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void UpdateRefundSubmitPurchaseOrderStock(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
            database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateShipper(ShippersInfo shipper)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE ShipperId = @ShipperId");
            database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipper.ShipperId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag;
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ShippingMode_Update");
            database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    database.ExecuteNonQuery(storedProcCommand, transaction);

                    flag = ((int)database.GetParameterValue(storedProcCommand, "Status")) == 0;

                    if (flag)
                    {
                        DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");

                        database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, shippingMode.ModeId);

                        if ((shippingMode.ExpressCompany != null) && (shippingMode.ExpressCompany.Count > 0))
                        {
                            StringBuilder builder = new StringBuilder();
                            int num = 0;
                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");

                            foreach (ExpressCompanyInfo info in shippingMode.ExpressCompany)
                            {
                                builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName,ExpressCompanyAbb) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num).Append(",@ExpressCompanyAbb").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num, DbType.String, info.ExpressCompanyName);
                                database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb" + num, DbType.String, info.ExpressCompanyAbb);
                                num++;
                            }

                            sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();

                            int num2 = (int)database.ExecuteScalar(sqlStringCommand, transaction);

                            if (num2 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }


                        }
                    }

                    transaction.Commit();

                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }
                finally
                {
                    if (null != connection)
                    {
                        connection.Close();
                    }
                }

            }
            return flag;
        }

        public override bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;

            DbCommand sqlStringCommand = database.GetSqlStringCommand(new StringBuilder("UPDATE Hishop_ShippingTemplates SET TemplateName=@TemplateName,Weight=@Weight,AddWeight=@AddWeight,Price=@Price,AddPrice=@AddPrice WHERE TemplateId=@TemplateId;").ToString());
            database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, shippingMode.Weight);
            database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, shippingMode.AddWeight);
            database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);

            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    flag = database.ExecuteNonQuery(sqlStringCommand, transaction) > 0;
                    if (flag)
                    {
                        DbCommand command = database.GetSqlStringCommand(" ");
                        database.AddInParameter(command, "TemplateId", DbType.Int32, shippingMode.TemplateId);
                        StringBuilder builder2 = new StringBuilder();
                        int num = 0;
                        int num2 = 0;
                        builder2.Append("DELETE Hishop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
                        builder2.Append("DELETE Hishop_ShippingRegions WHERE TemplateId=@TemplateId;");
                        builder2.Append("DECLARE @ERR INT; Set @ERR =0;");
                        builder2.Append(" DECLARE @GroupId Int;");
                        if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                        {
                            foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                            {
                                builder2.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num).Append(",@AddPrice").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                database.AddInParameter(command, "Price" + num, DbType.Currency, info.Price);
                                database.AddInParameter(command, "AddPrice" + num, DbType.Currency, info.AddPrice);
                                builder2.Append("Set @GroupId =@@identity;");
                                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                {
                                    builder2.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    database.AddInParameter(command, "RegionId" + num2, DbType.Int32, info2.RegionId);
                                    num2++;
                                }
                                num++;
                            }
                        }
                        command.CommandText = builder2.Append("SELECT @ERR;").ToString();
                        int num3 = (int)database.ExecuteScalar(command, transaction);
                        if (num3 != 0)
                        {
                            transaction.Rollback();
                            flag = false;
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }

                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool UpdateUserAccount(decimal orderTotal, int userId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1 WHERE UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        bool UpdateUserRank(int userId, int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) - @refundAmount, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId");
            database.AddInParameter(sqlStringCommand, "refundAmount", DbType.Decimal, refundAmount);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            if (isAllRefund)
            {
                database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 1);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 0);
            }
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

