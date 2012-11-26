using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace Hidistro.Subsites.Sales
{
    public static class SubsiteSalesHelper
    {
        public static bool AddOrderGift(OrderInfo order, GiftInfo gift, int quantity)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().AddOrderGift(order.OrderId, gift, quantity, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    bool flag2 = false;
                    foreach (OrderGiftInfo info in order.Gifts)
                    {
                        if (info.GiftId == gift.GiftId)
                        {
                            flag2 = true;
                            info.Quantity += quantity;
                        }
                    }
                    if (!flag2)
                    {
                        OrderGiftInfo item = new OrderGiftInfo();
                        item.GiftId = gift.GiftId;
                        item.OrderId = order.OrderId;
                        item.GiftName = gift.Name;
                        item.Quantity = quantity;
                        item.CostPrice = gift.PurchasePrice;
                        item.ThumbnailsUrl = gift.ThumbnailUrl40;
                        order.Gifts.Add(item);
                    }
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool AddPurchaseItem(PurchaseShoppingCartItemInfo item)
        {
            return SubsiteSalesProvider.Instance().AddPurchaseItem(item);
        }

        public static bool AddPurchaseOrderGift(PurchaseOrderInfo purchaseOrder, GiftInfo gift, int quantity)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().AddPurchaseOrderGift(purchaseOrder.PurchaseOrderId, gift, quantity, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    bool flag2 = false;
                    foreach (PurchaseOrderGiftInfo info in purchaseOrder.PurchaseOrderGifts)
                    {
                        if (info.GiftId == gift.GiftId)
                        {
                            flag2 = true;
                            info.Quantity += quantity;
                        }
                    }
                    if (!flag2)
                    {
                        PurchaseOrderGiftInfo item = new PurchaseOrderGiftInfo();
                        item.GiftId = gift.GiftId;
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.GiftName = gift.Name;
                        item.Quantity = quantity;
                        item.PurchasePrice = gift.PurchasePrice;
                        item.ThumbnailsUrl = gift.ThumbnailUrl40;
                        purchaseOrder.PurchaseOrderGifts.Add(item);
                    }
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
            int currentPOrderItemQuantity = provider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
            if (currentPOrderItemQuantity == 0)
            {
                return provider.AddPurchaseOrderItem(item, POrderId);
            }
            return provider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
        }

        public static decimal CalcFreight(int regionId, int totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            int num3 = totalWeight;
            int num4 = 1;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0)))
            {
                int num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0)
                {
                    num4 = (num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value;
                }
                else
                {
                    num4 = ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static bool ClearOrderGifts(OrderInfo order)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().ClearOrderGifts(order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    order.Gifts.Clear();
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool ClearPurchaseOrderGifts(PurchaseOrderInfo purchaseOrder)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().ClearPurchaseOrderGifts(purchaseOrder.PurchaseOrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    purchaseOrder.PurchaseOrderGifts.Clear();
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static void ClearPurchaseShoppingCart()
        {
            SubsiteSalesProvider.Instance().ClearPurchaseShoppingCart();
        }

        public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CLOSE) && SubsiteSalesProvider.Instance().ClosePurchaseOrder(purchaseOrder));
        }

        public static bool CloseTransaction(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_CLOSE) && SubsiteSalesProvider.Instance().CloseTransaction(order));
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_FINISH_TRADE) && SubsiteSalesProvider.Instance().ConfirmOrderFinish(order));
        }

        public static bool ConfirmPay(OrderInfo order)
        {
            if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                bool flag;
                using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                        if (provider.ConfirmPay(order, dbTran) <= 0)
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        if (order.GroupBuyId <= 0)
                        {
                            PurchaseOrderInfo purchaseOrder = provider.ConvertOrderToPurchaseOrder(order);
                            if (!provider.CreatePurchaseOrder(purchaseOrder, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
                        }
                        dbTran.Commit();
                        flag = true;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        flag = false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                if (flag)
                {
                    SubsiteSalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
                    UpdateUserAccount(order);
                }
                return flag;
            }
            return false;
        }

        public static bool ConfirmPay(BalanceDetailInfo balance, PurchaseOrderInfo purchaseOrder)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
            {
                return false;
            }
            bool flag = SubsiteSalesProvider.Instance().ConfirmPay(balance, purchaseOrder.PurchaseOrderId);
            if (flag)
            {
                SubsiteSalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                SubsiteSalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
            }
            return flag;
        }

        public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            return (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS) && SubsiteSalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder));
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static bool CreatePurchaseOrder(OrderInfo order)
        {
            if (order.CheckAction(OrderActions.SUBSITE_CREATE_PURCHASEORDER))
            {
                using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                        PurchaseOrderInfo purchaseOrder = provider.ConvertOrderToPurchaseOrder(order);
                        if (!provider.CreatePurchaseOrder(purchaseOrder, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        dbTran.Commit();
                        return true;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return false;
        }

        public static bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrderInfo)
        {
            return SubsiteSalesProvider.Instance().CreatePurchaseOrder(purchaseOrderInfo, null);
        }

        public static bool DeleteOrderGift(OrderInfo order, int giftId)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeleteOrderGift(order.OrderId, giftId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    foreach (OrderGiftInfo info in order.Gifts)
                    {
                        if (info.GiftId == giftId)
                        {
                            order.Gifts.Remove(info);
                            break;
                        }
                    }
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool DeleteOrderProduct(string sku, OrderInfo order)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeleteOrderProduct(sku, order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    order.LineItems.Remove(sku);
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static int DeleteOrders(string orderIds)
        {
            return SubsiteSalesProvider.Instance().DeleteOrders(orderIds);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = new PaymentModeInfo();
            paymentMode.ModeId = modeId;
            return (SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete) == PaymentModeActionStatus.Success);
        }

        public static int DeletePurchaseOrde(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseOrde(purchaseOrderId);
        }

        public static bool DeletePurchaseOrderGift(PurchaseOrderInfo purchaseOrder, int giftId)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeletePurchaseOrderGift(purchaseOrder.PurchaseOrderId, giftId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    foreach (PurchaseOrderGiftInfo info in purchaseOrder.PurchaseOrderGifts)
                    {
                        if (info.GiftId == giftId)
                        {
                            purchaseOrder.PurchaseOrderGifts.Remove(info);
                            break;
                        }
                    }
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
        }

        public static bool DeletePurchaseShoppingCartItem(string sku)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseShoppingCartItem(sku);
        }

        public static bool GetAlertStock(string skuId)
        {
            return SubsiteSalesProvider.Instance().GetAlertStock(skuId);
        }

        public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
        }

        public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, day, saleStatisticsType);
        }

        public static IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId)
        {
            return SubsiteSalesProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static GiftInfo GetGiftDetails(int giftId)
        {
            return SubsiteSalesProvider.Instance().GetGiftDetails(giftId);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetGifts(query);
        }

        public static LineItemInfo GetLineItemInfo(string sku, string orderId)
        {
            return SubsiteSalesProvider.Instance().GetLineItemInfo(sku, orderId);
        }

        public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
        }

        public static ShippersInfo GetMyShipper()
        {
            return SubsiteSalesProvider.Instance().GetMyShipper();
        }

        public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetOrderGifts(query);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return SubsiteSalesProvider.Instance().GetOrderInfo(orderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetOrders(query);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return SubsiteSalesProvider.Instance().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return SubsiteSalesProvider.Instance().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return SubsiteSalesProvider.Instance().GetPaymentModes();
        }

        public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
        }

        public static PurchaseOrderInfo GetPurchaseByOrderId(string orderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseByOrderId(orderId);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrderGifts(query);
        }

        public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrders(query);
        }

        public static PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrderTaobaoInfo(tbOrderId);
        }

        public static IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos()
        {
            return SubsiteSalesProvider.Instance().GetPurchaseShoppingCartItemInfos();
        }

        public static DataTable GetRecentlyManualPurchaseOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyManualPurchaseOrders(out number);
        }

        public static DataTable GetRecentlyOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyOrders(out number);
        }

        public static DataTable GetRecentlyPurchaseOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return SubsiteSalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
        }

        public static DbQueryResult GetSaleTargets()
        {
            return SubsiteSalesProvider.Instance().GetSaleTargets();
        }

        public static DbQueryResult GetSendGoodsOrders(OrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetSendGoodsOrders(query);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return SubsiteSalesProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static int GetSkuStock(string skuId)
        {
            return SubsiteSalesProvider.Instance().GetSkuStock(skuId);
        }

        public static StatisticsInfo GetStatistics()
        {
            return SubsiteSalesProvider.Instance().GetStatistics();
        }

        public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            return SubsiteSalesProvider.Instance().GetUserOrders(userOrder);
        }

        public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            return SubsiteSalesProvider.Instance().GetUserOrdersNoPage(userOrder);
        }

        public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
        {
            if (page == null)
            {
                totalProductSaleVisits = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
        }

        public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
        }

        public static bool IsExitPurchaseOrder(long tid)
        {
            return SubsiteSalesProvider.Instance().IsExitPurchaseOrder(tid);
        }

        public static bool MondifyAddress(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS) && SubsiteSalesProvider.Instance().SaveShippingAddress(order));
        }

        static void ReduceDeduct(OrderInfo order, Member member)
        {
            int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
            if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
            {
                IUser user = Users.GetUser(member.ReferralUserId.Value, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    Member member2 = user as Member;
                    if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                    {
                        decimal num2 = member2.Balance - ((order.RefundAmount * referralDeduct) / 100M);
                        BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
                        balanceDetails.UserId = member2.UserId;
                        balanceDetails.UserName = member2.Username;
                        balanceDetails.TradeDate = DateTime.Now;
                        balanceDetails.TradeType = TradeTypes.ReferralDeduct;
                        balanceDetails.Expenses = new decimal?((order.RefundAmount * referralDeduct) / 100M);
                        balanceDetails.Balance = num2;
                        balanceDetails.Remark = string.Format("退回提成因为'{0}'的订单{1}已退款", order.Username, order.OrderId);
                        UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails);
                    }
                }
            }
        }

        static void ReducedPoint(OrderInfo order, Member member)
        {
            UserPointInfo point = new UserPointInfo();
            point.OrderId = order.OrderId;
            point.UserId = member.UserId;
            point.TradeDate = DateTime.Now;
            point.TradeType = UserPointTradeType.Refund;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            point.Reduced = new int?(Convert.ToInt32((decimal)(order.RefundAmount / masterSettings.PointsRate)));
            point.Points = member.Points - point.Reduced.Value;
            SubsiteSalesProvider.Instance().AddMemberPoint(point);
        }

        public static bool RefundOrder(OrderInfo order)
        {
            IUser user;
            bool flag = false;
            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                order.OrderStatus = OrderStatus.Closed;
                flag = SubsiteSalesProvider.Instance().RefundOrder(order);
                if (flag)
                {
                    user = Users.GetUser(order.UserId, false);
                    if ((user != null) && (user.UserRole == UserRole.Underling))
                    {
                        ReducedPoint(order, user as Member);
                        ReduceDeduct(order, user as Member);
                        Users.ClearUserCache(user);
                    }
                    UpdateUserStatistics(order.UserId, order.RefundAmount, true);
                    SubsiteSalesProvider.Instance().UpdateRefundOrderStock(order.OrderId);
                }
                return flag;
            }
            if (order.OrderStatus == OrderStatus.SellerAlreadySent)
            {
                order.OrderStatus = OrderStatus.Finished;
                flag = SubsiteSalesProvider.Instance().RefundOrder(order);
                if (flag)
                {
                    user = Users.GetUser(order.UserId, false);
                    if ((user != null) && (user.UserRole == UserRole.Underling))
                    {
                        ReducedPoint(order, user as Member);
                        ReduceDeduct(order, user as Member);
                        Users.ClearUserCache(user);
                    }
                    UpdateUserStatistics(order.UserId, order.RefundAmount, false);
                }
                return flag;
            }
            return false;
        }

        public static bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder)
        {
            return SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, null);
        }

        public static bool SavePurchaseOrderRemark(string purchaseOrderId, string remark)
        {
            return SubsiteSalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrderId, remark);
        }

        public static bool SaveRemark(OrderInfo order)
        {
            return SubsiteSalesProvider.Instance().SaveRemark(order);
        }

        public static bool SendGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                flag = SubsiteSalesProvider.Instance().SendGoods(order) > 0;
            }
            return flag;
        }

        public static bool SetMyShipper(ShippersInfo shipper)
        {
            ShippersInfo myShipper = SubsiteSalesProvider.Instance().GetMyShipper();
            Globals.EntityCoding(shipper, true);
            if (myShipper == null)
            {
                return SubsiteSalesProvider.Instance().AddShipper(shipper);
            }
            return SubsiteSalesProvider.Instance().UpdateShipper(shipper);
        }

        public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteSalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateLineItem(string sku, OrderInfo order, int quantity)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                    int userId = order.UserId;
                    if (userId == 0x44c)
                    {
                        userId = 0;
                    }
                    Member user = Users.GetUser(userId) as Member;
                    if (user != null)
                    {
                        int num;
                        string str;
                        int num2;
                        int num3;
                        string str2;
                        decimal? nullable;
                        int gradeId = user.GradeId;
                        provider.GetLineItemPromotions(order.LineItems[sku].ProductId, quantity, out num, out str, out num2, out num3, out str2, out nullable, gradeId);
                        order.LineItems[sku].PurchaseGiftId = num;
                        order.LineItems[sku].PurchaseGiftName = str;
                        order.LineItems[sku].ShipmentQuantity = quantity + num2;
                        order.LineItems[sku].WholesaleDiscountId = num3;
                        order.LineItems[sku].WholesaleDiscountName = str2;
                        order.LineItems[sku].Quantity = quantity;
                        if (nullable.HasValue)
                        {
                            decimal itemListPrice = order.LineItems[sku].ItemListPrice;
                            order.LineItems[sku].ItemAdjustedPrice = Convert.ToDecimal((itemListPrice * nullable) / 100M);
                        }
                        else
                        {
                            order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
                        }
                    }
                    else
                    {
                        order.LineItems[sku].ShipmentQuantity = quantity;
                        order.LineItems[sku].Quantity = quantity;
                        order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
                    }
                    if (!provider.UpdateLineItem(order.OrderId, order.LineItems[sku], dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && SubsiteSalesProvider.Instance().UpdateOrderAmount(order, null));
        }

        public static bool UpdateOrderItem(string orderId, LineItemInfo lineItem)
        {
            return SubsiteSalesProvider.Instance().UpdateLineItem(orderId, lineItem, null);
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE) && SubsiteSalesProvider.Instance().UpdateOrderPaymentType(order));
        }

        public static bool UpdateOrderShippingMode(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE) && SubsiteSalesProvider.Instance().UpdateOrderShippingMode(order));
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return SubsiteSalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
        }

        public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
        {
            return SubsiteSalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
        }

        static void UpdateUserAccount(OrderInfo order)
        {
            int userId = order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Underling))
            {
                Member member = user as Member;
                UserPointInfo point = new UserPointInfo();
                point.OrderId = order.OrderId;
                point.UserId = member.UserId;
                point.TradeDate = DateTime.Now;
                point.TradeType = UserPointTradeType.Bounty;
                point.Increased = new int?(order.GetTotalPoints());
                point.Points = order.GetTotalPoints() + member.Points;
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                SubsiteSalesProvider.Instance().AddMemberPoint(point);
                int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
                if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
                {
                    IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
                    if ((user2 != null) && (user2.UserRole == UserRole.Underling))
                    {
                        Member member2 = user2 as Member;
                        if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                        {
                            decimal num3 = member2.Balance + ((order.GetTotal() * referralDeduct) / 100M);
                            BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
                            balanceDetails.UserId = member2.UserId;
                            balanceDetails.UserName = member2.Username;
                            balanceDetails.TradeDate = DateTime.Now;
                            balanceDetails.TradeType = TradeTypes.ReferralDeduct;
                            balanceDetails.Income = new decimal?((order.GetTotal() * referralDeduct) / 100M);
                            balanceDetails.Balance = num3;
                            balanceDetails.Remark = string.Format("提成来自'{0}'的订单{1}", order.Username, order.OrderId);
                            UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails);
                        }
                    }
                }
                SubsiteSalesProvider.Instance().UpdateUserAccount(order.GetTotal(), point.Points, order.UserId);
                int historyPoint = SubsiteSalesProvider.Instance().GetHistoryPoint(member.UserId);
                SubsiteSalesProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
            }
            Users.ClearUserCache(user);
        }

        static void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            SubsiteSalesProvider.Instance().UpdateUserStatistics(userId, refundAmount, isAllRefund);
        }
    }
}

