using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using Hidistro.Core;

namespace Hidistro.ControlPanel.Sales
{
    public abstract class SalesProvider
    {
        static readonly SalesProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.SalesData,Hidistro.ControlPanel.Data") as SalesProvider);

        protected SalesProvider()
        {
        }

        public abstract bool AddExpressTemplate(string expressName, string xmlFile);
        public abstract bool AddMemberPoint(UserPointInfo point);
        public abstract bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, DbTransaction dbTran);
        public abstract bool AddOrderLookupItem(OrderLookupItemInfo orderLookupItem);
        public abstract int AddOrderLookupList(OrderLookupListInfo orderLookupList);
        public abstract bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId);
        public abstract bool AddShipper(ShippersInfo shipper);
        public abstract bool ChangeMemberGrade(int userId, int gradId, int points);
        public abstract bool ClearOrderGifts(string orderId, DbTransaction dbTran);
        public abstract bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract bool CloseTransaction(OrderInfo order);
        public abstract bool ConfirmOrderFinish(OrderInfo order);
        public abstract int ConfirmPay(OrderInfo order);
        public abstract bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder);
        public abstract bool CreatePrintOrders(int taskId, string[] orderIds, DbTransaction dbTran);
        public abstract int CreatePrintTask(string creator, DateTime createTime, bool isPO, DbTransaction dbTran);
        public abstract bool CreateShippingMode(ShippingModeInfo shippingMode);
        public abstract bool CreateShippingTemplate(ShippingModeInfo shippingMode);
        public abstract PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action);
        public abstract bool DeleteExpressTemplate(int expressId);
        public abstract bool DeleteLineItem(string sku, string orderId, DbTransaction dbTran);
        public abstract bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran);
        public abstract bool DeleteOrderLookupItem(int orderLookupItemId);
        public abstract bool DeleteOrderLookupList(int orderLookupListId);
        public abstract int DeleteOrders(string orderIds);
        public abstract bool DeletePrintOrder(string orderId, int taskId);
        public abstract bool DeletePrintOrderByTaskId(int taskId, DbTransaction dbTran);
        public abstract bool DeletePrintTask(int taskId, DbTransaction dbTran);
        public abstract bool DeletePurchaseOrderItem(string POrderId, string skuId);
        public abstract int DeletePurchaseOrders(string purchaseOrderIds);
        public abstract bool DeleteShipper(int shipperId);
        public abstract bool DeleteShippingMode(int modeId);
        public abstract bool DeleteShippingTemplate(int templateId);
        public abstract decimal GetAddUserTotal(int year);
        public abstract int GetCurrentPOrderItemQuantity(string POrderId, string skuId);
        public abstract DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType);
        public abstract IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId);
        public abstract DataTable GetExpressTemplates();
        public abstract IList<GiftInfo> GetGiftList(GiftQuery query);
        public abstract DbQueryResult GetGifts(GiftQuery query);
        public abstract int GetHistoryPoint(int userId);
        public abstract DataTable GetIsUserExpressTemplates();
        public abstract IList<LineItemInfo> GetLineItemInfo(string orderId);
        public abstract LineItemInfo GetLineItemInfo(string sku, string orderId);
        public abstract void GetLineItemPromotions(int productId, int quantity, out int purchaseGiftId, out string purchaseGiftName, out int giveQuantity, out int wholesaleDiscountId, out string wholesaleDiscountName, out decimal? discountRate, int gradeId);
        public abstract DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales);
        public abstract DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query);
        public abstract DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType);
        public abstract decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract OrderGiftInfo GetOrderGift(int giftId, string orderId);
        public abstract DbQueryResult GetOrderGifts(OrderGiftQuery query);
        public abstract OrderInfo GetOrderInfo(string orderId);
        public abstract OrderLookupItemInfo GetOrderLookupItem(int lookupItemId);
        public abstract IList<OrderLookupItemInfo> GetOrderLookupItems(int lookupListId);
        public abstract OrderLookupListInfo GetOrderLookupList(int lookupListId);
        public abstract IList<OrderLookupListInfo> GetOrderLookupLists();
        public abstract IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days);
        public abstract DbQueryResult GetOrders(OrderQuery query);
        public abstract DataSet GetOrdersAndLines(int taskId, bool printAll);
        public abstract PaymentModeInfo GetPaymentMode(int modeId);
        public abstract PaymentModeInfo GetPaymentMode(string gateway);
        public abstract IList<PaymentModeInfo> GetPaymentModes();
        public abstract DbQueryResult GetPrintTasks(Pagination query);
        public abstract DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales);
        public abstract DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales);
        public abstract PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId);
        public abstract DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query);
        public abstract DataSet GetPurchaseOrdersAndLines(int taskId, bool printAll);
        public abstract DataTable GetRecentlyOrders(out int number);
        public abstract DataTable GetRecentlyPurchaseOrders(out int number);
        public abstract DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query);
        public abstract DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query);
        public abstract DbQueryResult GetSaleTargets();
        public abstract DataTable GetSendGoodsOrders(string orderIds);
        public abstract DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds);
        public abstract ShippersInfo GetShipper(int shipperId);
        public abstract IList<ShippersInfo> GetShippers(bool includeDistributor);
        public abstract DataTable GetShippingAllTemplates();
        public abstract ShippingModeInfo GetShippingMode(int modeId, bool includeDetail);
        public abstract IList<ShippingModeInfo> GetShippingModes(string paymentGateway);
        public abstract ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail);
        public abstract DbQueryResult GetShippingTemplates(Pagination pagin);
        public abstract int GetSkuStock(string skuId);
        public abstract AdminStatisticsInfo GetStatistics();
        public abstract bool GetTaskIsPrintedAll(int taskId);
        public abstract TaskPrintInfo GetTaskPrintInfo(int taskId);
        public abstract IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days);
        public abstract OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder);
        public abstract OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder);
        public abstract IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits);
        public abstract DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType);
        public abstract decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType);
        public static SalesProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool RefundOrder(OrderInfo order);
        public abstract bool RefundPurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract int ResetOrderPrintStatus(IList<OrderInfo> orders);
        public abstract bool SaveOrderRemark(OrderInfo order);
        public abstract bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder);
        public abstract bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder);
        public abstract bool SaveShippingAddress(OrderInfo order);
        public abstract int SendGoods(OrderInfo order);
        public abstract bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder);
        public abstract void SetDefalutShipper(int shipperId);
        public abstract bool SetExpressIsUse(int expressId);
        public abstract bool SetOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb);
        public abstract bool SetOrderShipNumber(string purchaseOrderId, string shipNumber);
        public abstract bool SetOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName);
        public abstract bool SetPurchaseOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb);
        public abstract bool SetPurchaseOrderShipNumber(string orderId, string shipNumber);
        public abstract bool SetPurchaseOrderShippingMode(string orderIds, int realShippingModeId, string realModeName);
        public abstract bool SetTaskIsExport(int taskId);
        public abstract void SetTaskOrderPrinted(int taskId);
        public abstract void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence);
        public abstract void UpdateDistributorAccount(decimal expenditure, int distributorId);
        public abstract bool UpdateExpressTemplate(int expressId, string expressName);
        public abstract bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran);
        public abstract bool UpdateOrderAmount(OrderInfo order, DbTransaction dbTran);
        public abstract bool UpdateOrderLookupItem(OrderLookupItemInfo orderLookupItem);
        public abstract bool UpdateOrderLookupList(OrderLookupListInfo orderLookupList);
        public abstract bool UpdateOrderPaymentType(OrderInfo order);
        public abstract bool UpdateOrderShippingMode(OrderInfo order);
        public abstract void UpdatePayOrderStock(string orderId);
        public abstract bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems);
        public abstract void UpdateProductStock(string purchaseOrderId);
        public abstract bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder);
        public abstract bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity);
        public abstract bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder);
        public abstract bool UpdateRefundOrderStock(string orderId);
        public abstract void UpdateRefundSubmitPurchaseOrderStock(PurchaseOrderInfo purchaseOrder);
        public abstract bool UpdateShipper(ShippersInfo shipper);
        public abstract bool UpdateShippingMode(ShippingModeInfo shippingMode);
        public abstract bool UpdateShippingTemplate(ShippingModeInfo shippingMode);
        public abstract bool UpdateUserAccount(decimal orderTotal, int userId);
        public abstract void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund);
    }
}

