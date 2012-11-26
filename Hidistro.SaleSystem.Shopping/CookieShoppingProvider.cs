namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public abstract class CookieShoppingProvider
    {
        protected CookieShoppingProvider()
        {
        }

        public abstract bool AddGiftItem(int giftId, int quantity);
        public abstract AddCartItemStatus AddLineItem(int productId, string skuId, int quantity);
        public abstract void ClearShoppingCart();
        public abstract decimal GetCostPrice(string skuId);
        public abstract Dictionary<string, decimal> GetCostPriceForItems(int userId);
        public abstract void GetPromotionsWithAmount(decimal amount, out int discountActivityId, out string discountName, out decimal discountValue, out DiscountValueType discountValueType, out int feeFreeActivityId, out string feeFreeName, out bool eightFree, out bool orderOptionFree, out bool procedureFeeFree);
        public abstract ShoppingCartInfo GetShoppingCart();
        public abstract bool GetShoppingProductInfo(int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity);
        public abstract int GetSkuStock(string skuId);
        public static CookieShoppingProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return CookieShoppingSubsiteProvider.CreateInstance();
            }
            return CookieShoppingMasterProvider.CreateInstance();
        }

        public abstract void RemoveGiftItem(int giftId);
        public abstract void RemoveLineItem(string skuId);
        public abstract void UpdateGiftItemQuantity(int giftId, int quantity);
        public abstract void UpdateLineItemQuantity(int productId, string skuId, int quantity);
    }
}

