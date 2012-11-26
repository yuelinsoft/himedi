namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public sealed class SalesHelper
    {
       SalesHelper()
        {
        }

        public static bool AddExpressTemplate(string expressName, string xmlFile)
        {
            return SalesProvider.Instance().AddExpressTemplate(expressName, xmlFile);
        }

        public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            SalesProvider provider = SalesProvider.Instance();
            int currentPOrderItemQuantity = provider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
            if (currentPOrderItemQuantity == 0)
            {
                return provider.AddPurchaseOrderItem(item, POrderId);
            }
            return provider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
        }

        public static bool AddShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return SalesProvider.Instance().AddShipper(shipper);
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

        public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            bool flag = SalesProvider.Instance().ClosePurchaseOrder(purchaseOrder);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "关闭了编号为\"{0}\"的采购单", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            bool flag = SalesProvider.Instance().ConfirmPayPurchaseOrder(purchaseOrder);
            if (flag)
            {
                SalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                SalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal(), purchaseOrder.DistributorId);
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单线下收款", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
            {
                bool flag = SalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的采购单", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static int CreatePrintTask(string creator, DateTime createTime, bool isPO, string[] orderList)
        {
            int taskId = 0;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                IUser user = HiContext.Current.User;
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    taskId = SalesProvider.Instance().CreatePrintTask(creator, createTime, isPO, dbTran);
                    if (taskId <= 0)
                    {
                        dbTran.Rollback();
                        return 0;
                    }
                    if (!SalesProvider.Instance().CreatePrintOrders(taskId, orderList, dbTran))
                    {
                        dbTran.Rollback();
                        return 0;
                    }
                    dbTran.Commit();
                }
                catch
                {
                    dbTran.Rollback();
                    connection.Close();
                    taskId = 0;
                }
            }
            return taskId;
        }

        public static bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            if (null == shippingMode)
            {
                return false;
            }
            return SalesProvider.Instance().CreateShippingMode(shippingMode);
        }

        public static bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return SalesProvider.Instance().CreateShippingTemplate(shippingMode);
        }

        public static bool DeleteExpressTemplate(int expressId)
        {
            return SalesProvider.Instance().DeleteExpressTemplate(expressId);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = new PaymentModeInfo();
            paymentMode.ModeId = modeId;
            return (SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete) == PaymentModeActionStatus.Success);
        }

        public static bool DeletePrint(int taskId)
        {
            bool flag = SalesProvider.Instance().DeletePrintTask(taskId, null);
            if (flag)
            {
                SalesProvider.Instance().DeletePrintOrderByTaskId(taskId, null);
            }
            return flag;
        }

        public static bool DeletePrintOrder(string orderId, int taskId)
        {
            return SalesProvider.Instance().DeletePrintOrder(orderId, taskId);
        }

        public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
        {
            return SalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
        }

        public static int DeletePurchaseOrders(string purchaseOrderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
            int num = SalesProvider.Instance().DeletePurchaseOrders(purchaseOrderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeletePurchaseorder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的采购单", new object[] { purchaseOrderIds }));
            }
            return num;
        }

        public static bool DeleteShipper(int shipperId)
        {
            return SalesProvider.Instance().DeleteShipper(shipperId);
        }

        public static bool DeleteShippingMode(int modeId)
        {
            return SalesProvider.Instance().DeleteShippingMode(modeId);
        }

        public static bool DeleteShippingTemplate(int templateId)
        {
            return SalesProvider.Instance().DeleteShippingTemplate(templateId);
        }

        public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
        }

        public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetDaySaleTotal(year, month, day, saleStatisticsType);
        }

        public static IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId)
        {
            return SalesProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static DataTable GetExpressTemplates()
        {
            return SalesProvider.Instance().GetExpressTemplates();
        }

        public static DataTable GetIsUserExpressTemplates()
        {
            return SalesProvider.Instance().GetIsUserExpressTemplates();
        }

        public static DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetMemberStatistics(query, out totalProductSales);
        }

        public static DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetMemberStatisticsNoPage(query);
        }

        public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
        }

        public static IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
        {
            return SalesProvider.Instance().GetOrderPriceStatisticsOfSevenDays(days);
        }

        public static DataSet GetOrdersAndLines(int taskId, bool printAll)
        {
            return SalesProvider.Instance().GetOrdersAndLines(taskId, printAll);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return SalesProvider.Instance().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return SalesProvider.Instance().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return SalesProvider.Instance().GetPaymentModes();
        }

        public static DbQueryResult GetPrintTasks(Pagination query)
        {
            return SalesProvider.Instance().GetPrintTasks(query);
        }

        public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return SalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            return SalesProvider.Instance().GetPurchaseOrders(query);
        }

        public static DataSet GetPurchaseOrdersAndLines(int taskId, bool printAll)
        {
            return SalesProvider.Instance().GetPurchaseOrdersAndLines(taskId, printAll);
        }

        public static DataTable GetRecentlyPurchaseOrders(out int number)
        {
            return SalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetSaleOrderLineItemsStatisticsNoPage(query);
        }

        public static DbQueryResult GetSaleTargets()
        {
            return SalesProvider.Instance().GetSaleTargets();
        }

        public static DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
        {
            return SalesProvider.Instance().GetSendGoodsPurchaseOrders(purchaseOrderIds);
        }

        public static ShippersInfo GetShipper(int shipperId)
        {
            return SalesProvider.Instance().GetShipper(shipperId);
        }

        public static IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            return SalesProvider.Instance().GetShippers(includeDistributor);
        }

        public static DataTable GetShippingAllTemplates()
        {
            return SalesProvider.Instance().GetShippingAllTemplates();
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return SalesProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return SalesProvider.Instance().GetShippingModes(string.Empty);
        }

        public static IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
        {
            return SalesProvider.Instance().GetShippingModes(paymentGateway);
        }

        public static ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            return SalesProvider.Instance().GetShippingTemplate(templateId, includeDetail);
        }

        public static DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return SalesProvider.Instance().GetShippingTemplates(pagin);
        }

        public static AdminStatisticsInfo GetStatistics()
        {
            return SalesProvider.Instance().GetStatistics();
        }

        public static bool GetTaskIsPrintAll(int taskId)
        {
            return SalesProvider.Instance().GetTaskIsPrintedAll(taskId);
        }

        public static TaskPrintInfo GetTaskPrintInfo(int taskId)
        {
            return SalesProvider.Instance().GetTaskPrintInfo(taskId);
        }

        public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            return SalesProvider.Instance().GetUserAdd(year, month, days);
        }

        public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            return SalesProvider.Instance().GetUserOrders(userOrder);
        }

        public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            return SalesProvider.Instance().GetUserOrdersNoPage(userOrder);
        }

        public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
        {
            if (page == null)
            {
                totalProductSaleVisits = 0;
                return null;
            }
            return SalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
        }

        public static DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetWeekSaleTota(saleStatisticsType);
        }

        public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
        }

        public static bool RefundPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseorderRefund);
            bool flag = SalesProvider.Instance().RefundPurchaseOrder(purchaseOrder);
            if (flag)
            {
                if (purchaseOrder.PurchaseStatus == OrderStatus.Closed)
                {
                    SalesProvider.Instance().UpdateRefundSubmitPurchaseOrderStock(purchaseOrder);
                }
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                EventLogs.WriteOperationLog(Privilege.PurchaseorderRefund, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单退款", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static int ResetOrderPrintStatus(IList<OrderInfo> orders)
        {
            return SalesProvider.Instance().ResetOrderPrintStatus(orders);
        }

        public static bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseorderMark);
            bool flag = SalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrder);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.PurchaseorderMark, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单备注", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS))
            {
                bool flag = SalesProvider.Instance().SavePurchaseOrderShippingAddress(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的收货地址", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
        {
            Globals.EntityCoding(purchaseOrder, true);
            ManagerHelper.CheckPrivilege(Privilege.PurchaseorderSendGood);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
            {
                bool flag = SalesProvider.Instance().SendPurchaseOrderGoods(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.PurchaseorderSendGood, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单发货", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static void SetDefalutShipper(int shipperId)
        {
            SalesProvider.Instance().SetDefalutShipper(shipperId);
        }

        public static bool SetExpressIsUse(int expressId)
        {
            return SalesProvider.Instance().SetExpressIsUse(expressId);
        }

        public static bool SetPurchaseOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return SalesProvider.Instance().SetPurchaseOrderExpressComputerpe(orderIds, expressCompanyName, expressCompanyAbb);
        }

        public static void SetPurchaseOrderShipNumber(string[] orderIds, string startNumber)
        {
            int num = 0;
            for (int i = orderIds.Length - 1; i >= 0; i--)
            {
                long num3 = long.Parse(startNumber) + num;
                if (SalesProvider.Instance().SetPurchaseOrderShipNumber(orderIds[i], num3.ToString()))
                {
                    num++;
                }
            }
        }

        public static bool SetPurchaseOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            return SalesProvider.Instance().SetPurchaseOrderShippingMode(orderIds, realShippingModeId, realModeName);
        }

        public static bool SetTaskIsExport(int taskId)
        {
            return SalesProvider.Instance().SetTaskIsExport(taskId);
        }

        public static void SetTaskOrderPrinted(int taskId)
        {
            SalesProvider.Instance().SetTaskOrderPrinted(taskId);
        }

        public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SalesProvider.Instance().SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateExpressTemplate(int expressId, string expressName)
        {
            return SalesProvider.Instance().UpdateExpressTemplate(expressId, expressName);
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return SalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
        }

        public static bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_AMOUNT))
            {
                bool flag = SalesProvider.Instance().UpdatePurchaseOrderAmount(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改编号为\"{0}\"的采购单的金额", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
        {
            return SalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
        }

        public static bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE))
            {
                bool flag = SalesProvider.Instance().UpdatePurchaseOrderShippingMode(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的配送方式", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool UpdateShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return SalesProvider.Instance().UpdateShipper(shipper);
        }

        public static bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return SalesProvider.Instance().UpdateShippingTemplate(shippingMode);
        }

        public static bool UpdateShippMode(ShippingModeInfo shippingMode)
        {
            if (shippingMode == null)
            {
                return false;
            }
            Globals.EntityCoding(shippingMode, true);
            return SalesProvider.Instance().UpdateShippingMode(shippingMode);
        }
    }
}

