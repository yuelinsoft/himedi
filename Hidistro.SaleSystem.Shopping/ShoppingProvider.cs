namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public abstract class ShoppingProvider
    {
        protected ShoppingProvider()
        {
        }

        public abstract bool AddGiftItem(int giftId, int quantity);
        public abstract AddCartItemStatus AddLineItem(Member member, int productId, string skuId, int quantity);
        public abstract bool AddMemberPoint(UserPointInfo point);
        public abstract void ClearShoppingCart(int userId);
        public abstract bool CreatOrder(OrderInfo orderInfo);
        public abstract ShoppingCartItemInfo GetCartItemInfo(Member member, int productId, string skuId, int quantity);
        public abstract ShoppingCartItemInfo GetCartItemInfo(Member member, int productId, string skuId, string skuContent, int quantity, out ProductSaleStatus saleStatus, out string sku, out int stock, out int totalQuantity);
        public abstract decimal GetCostPrice(string skuId);
        public abstract Dictionary<string, decimal> GetCostPriceForItems(string skuIds);
        public abstract DataTable GetCoupon(decimal orderAmount);
        public abstract CouponInfo GetCoupon(string couponCode);
        public abstract IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId);
        public abstract OrderInfo GetOrderInfo(string orderId);
        public abstract OrderLookupItemInfo GetOrderLookupItem(int lookupItemId, string orderId);
        public abstract IList<OrderLookupItemInfo> GetOrderLookupItems(int lookupListId);
        public abstract OrderLookupListInfo GetOrderLookupList(int lookupListId);
        public abstract PaymentModeInfo GetPaymentMode(int modeId);
        public abstract IList<PaymentModeInfo> GetPaymentModes();
        public abstract SKUItem GetProductAndSku(int productId, string options);
        public abstract DataTable GetProductInfoBySku(string skuId);
        public abstract void GetPromotionsWithAmount(decimal amount, out int discountActivityId, out string discountName, out decimal discountValue, out DiscountValueType discountValueType, out int feeFreeActivityId, out string feeFreeName, out bool eightFree, out bool orderOptionFree, out bool procedureFeeFree);
        public abstract PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId);
        public abstract ShippingModeInfo GetShippingMode(int modeId, bool includeDetail);
        public abstract IList<ShippingModeInfo> GetShippingModes();
        public abstract ShoppingCartInfo GetShoppingCart(int userId);
        public abstract bool GetShoppingProductInfo(Member member, int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity);
        public abstract IList<string> GetSkuIdsBysku(string sku);
        public abstract int GetSkuStock(string skuId);
        public abstract IList<OrderLookupListInfo> GetUsableOrderLookupLists();
        public abstract DataTable GetYetShipOrders(int days);
        public static ShoppingProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return ShoppingSubsiteProvider.CreateInstance();
            }
            return ShoppingMasterProvider.CreateInstance();
        }

        public abstract void RemoveGiftItem(int giftId);
        public abstract void RemoveLineItem(int userId, string skuId);
        public abstract void UpdateGiftItemQuantity(int giftId, int quantity);
        public abstract void UpdateLineItemQuantity(Member member, int productId, string skuId, int quantity);
    }
}

