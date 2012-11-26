namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Data;

    public static class ShoppingCartProcessor
    {
        public static bool AddGiftItem(int giftId, int quantity)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                return CookieShoppingProvider.Instance().AddGiftItem(giftId, quantity);
            }
            return ShoppingProvider.Instance().AddGiftItem(giftId, quantity);
        }

        public static AddCartItemStatus AddLineItem(int productId, string skuId, string skuContent, int quantity)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                quantity = 1;
            }
            if (user != null)
            {
                int num;
                int num2;
                ProductSaleStatus status;
                ShoppingProvider provider = ShoppingProvider.Instance();
                if (!provider.GetShoppingProductInfo(user, productId, skuId, out status, out num, out num2))
                {
                    return AddCartItemStatus.ProductNotExists;
                }
                if (status != ProductSaleStatus.OnSale)
                {
                    return AddCartItemStatus.Offsell;
                }
                if ((num <= 0) || (num < num2))
                {
                    return AddCartItemStatus.Shortage;
                }
                return provider.AddLineItem(user, productId, skuId, quantity);
            }
            return CookieShoppingProvider.Instance().AddLineItem(productId, skuId, quantity);
        }

        public static void ClearShoppingCart()
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().ClearShoppingCart();
            }
            else
            {
                ShoppingProvider.Instance().ClearShoppingCart(HiContext.Current.User.UserId);
            }
        }

        public static void ConvertShoppingCartToDataBase(ShoppingCartInfo shoppingCart)
        {
            ShoppingProvider provider = ShoppingProvider.Instance();
            Member user = HiContext.Current.User as Member;
            if (user != null)
            {
                if (shoppingCart.LineItems.Count > 0)
                {
                    foreach (ShoppingCartItemInfo info in shoppingCart.LineItems.Values)
                    {
                        provider.AddLineItem(user, info.ProductId, info.SkuId, info.Quantity);
                    }
                }
                if (shoppingCart.LineGifts.Count > 0)
                {
                    foreach (ShoppingCartGiftInfo info2 in shoppingCart.LineGifts)
                    {
                        provider.AddGiftItem(info2.GiftId, info2.Quantity);
                    }
                }
            }
        }

        public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount)
        {
            int num;
            int num2;
            string str2;
            ProductSaleStatus status;
            ShoppingCartInfo info = new ShoppingCartInfo();
            DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(productSkuId);
            if ((productInfoBySku == null) || (productInfoBySku.Rows.Count <= 0))
            {
                return null;
            }
            CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo((int) productInfoBySku.Rows[0]["ProductId"]);
            if (countDownInfo == null)
            {
                return null;
            }
            string skuContent = string.Empty;
            foreach (DataRow row in productInfoBySku.Rows)
            {
                if (!((((row["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) row["AttributeName"])) || (row["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) row["ValueStr"])))
                {
                    object obj2 = skuContent;
                    skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                }
            }
            ShoppingProvider provider = ShoppingProvider.Instance();
            Member user = HiContext.Current.User as Member;
            ShoppingCartItemInfo info3 = provider.GetCartItemInfo(user, (int) productInfoBySku.Rows[0]["ProductId"], productSkuId, skuContent, buyAmount, out status, out str2, out num, out num2);
            if ((((info3 == null) || (status != ProductSaleStatus.OnSale)) || (num <= 0)) || (num < num2))
            {
                return null;
            }
            ShoppingCartItemInfo info4 = new ShoppingCartItemInfo(info3.SkuId, info3.ProductId, info3.SKU, info3.Name, countDownInfo.CountDownPrice, info3.SkuContent, buyAmount, info3.Weight, 0, string.Empty, 0, 0, string.Empty, null, info3.CategoryId, info3.ThumbnailUrl40, info3.ThumbnailUrl60, info3.ThumbnailUrl100);
            info.LineItems.Add(productSkuId, info4);
            return info;
        }

        public static ShoppingCartInfo GetGroupBuyShoppingCart(string productSkuId, int buyAmount)
        {
            int num3;
            int num4;
            string str2;
            ProductSaleStatus status;
            ShoppingCartInfo info = new ShoppingCartInfo();
            DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(productSkuId);
            if ((productInfoBySku == null) || (productInfoBySku.Rows.Count <= 0))
            {
                return null;
            }
            GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo((int) productInfoBySku.Rows[0]["ProductId"]);
            if (productGroupBuyInfo == null)
            {
                return null;
            }
            int orderCount = ProductBrowser.GetOrderCount(productGroupBuyInfo.GroupBuyId);
            decimal currentPrice = ProductBrowser.GetCurrentPrice(productGroupBuyInfo.GroupBuyId, orderCount);
            string skuContent = string.Empty;
            foreach (DataRow row in productInfoBySku.Rows)
            {
                if (!((((row["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) row["AttributeName"])) || (row["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) row["ValueStr"])))
                {
                    object obj2 = skuContent;
                    skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                }
            }
            ShoppingProvider provider = ShoppingProvider.Instance();
            Member user = HiContext.Current.User as Member;
            ShoppingCartItemInfo info3 = provider.GetCartItemInfo(user, (int) productInfoBySku.Rows[0]["ProductId"], productSkuId, skuContent, buyAmount, out status, out str2, out num3, out num4);
            if ((((info3 == null) || (status != ProductSaleStatus.OnSale)) || (num3 <= 0)) || (num3 < num4))
            {
                return null;
            }
            ShoppingCartItemInfo info4 = new ShoppingCartItemInfo(info3.SkuId, info3.ProductId, info3.SKU, info3.Name, currentPrice, info3.SkuContent, buyAmount, info3.Weight, 0, string.Empty, 0, 0, string.Empty, null, info3.CategoryId, info3.ThumbnailUrl40, info3.ThumbnailUrl60, info3.ThumbnailUrl100);
            info.LineItems.Add(productSkuId, info4);
            return info;
        }

        public static ShoppingCartInfo GetShoppingCart()
        {
            int num;
            int num2;
            string str;
            string str2;
            decimal num3;
            DiscountValueType type;
            bool flag;
            bool flag2;
            bool flag3;
            if (HiContext.Current.User.IsAnonymous)
            {
                return CookieShoppingProvider.Instance().GetShoppingCart();
            }
            ShoppingProvider provider = ShoppingProvider.Instance();
            ShoppingCartInfo shoppingCart = provider.GetShoppingCart(HiContext.Current.User.UserId);
            if ((shoppingCart.LineItems.Count == 0) && (shoppingCart.LineGifts.Count == 0))
            {
                return null;
            }
            provider.GetPromotionsWithAmount(shoppingCart.GetAmount(), out num, out str, out num3, out type, out num2, out str2, out flag, out flag2, out flag3);
            if (!((num <= 0) || string.IsNullOrEmpty(str)))
            {
                shoppingCart.DiscountActivityId = num;
                shoppingCart.DiscountName = str;
                shoppingCart.DiscountValue = num3;
                shoppingCart.DiscountValueType = type;
            }
            if (!((num2 <= 0) || string.IsNullOrEmpty(str2)))
            {
                shoppingCart.FeeFreeActivityId = num2;
                shoppingCart.FeeFreeName = str2;
                shoppingCart.EightFree = flag;
                shoppingCart.ProcedureFeeFree = flag3;
                shoppingCart.OrderOptionFree = flag2;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            int num;
            int num2;
            string str2;
            ProductSaleStatus status;
            ShoppingCartInfo info = new ShoppingCartInfo();
            DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(productSkuId);
            if ((productInfoBySku == null) || (productInfoBySku.Rows.Count <= 0))
            {
                return null;
            }
            string skuContent = string.Empty;
            foreach (DataRow row in productInfoBySku.Rows)
            {
                if (!((((row["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) row["AttributeName"])) || (row["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) row["ValueStr"])))
                {
                    object obj2 = skuContent;
                    skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                }
            }
            ShoppingCartItemInfo info2 = ShoppingProvider.Instance().GetCartItemInfo(HiContext.Current.User as Member, (int) productInfoBySku.Rows[0]["ProductId"], productSkuId, skuContent, buyAmount, out status, out str2, out num, out num2);
            if ((((info2 == null) || (status != ProductSaleStatus.OnSale)) || (num <= 0)) || (num < num2))
            {
                return null;
            }
            info.LineItems.Add(productSkuId, info2);
            if (!HiContext.Current.User.IsAnonymous)
            {
                int num3;
                int num4;
                string str3;
                string str4;
                decimal num5;
                DiscountValueType type;
                bool flag;
                bool flag2;
                bool flag3;
                ShoppingProvider.Instance().GetPromotionsWithAmount(info.GetAmount(), out num3, out str3, out num5, out type, out num4, out str4, out flag, out flag2, out flag3);
                if (!((num3 <= 0) || string.IsNullOrEmpty(str3)))
                {
                    info.DiscountActivityId = num3;
                    info.DiscountName = str3;
                    info.DiscountValue = num5;
                    info.DiscountValueType = type;
                }
                if (!((num4 <= 0) || string.IsNullOrEmpty(str4)))
                {
                    info.FeeFreeActivityId = num4;
                    info.FeeFreeName = str4;
                    info.EightFree = flag;
                    info.ProcedureFeeFree = flag3;
                    info.OrderOptionFree = flag2;
                }
            }
            return info;
        }

        public static int GetSkuStock(string skuId)
        {
            return ShoppingProvider.Instance().GetSkuStock(skuId);
        }

        public static void RemoveGiftItem(int giftId)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().RemoveGiftItem(giftId);
            }
            else
            {
                ShoppingProvider.Instance().RemoveGiftItem(giftId);
            }
        }

        public static void RemoveLineItem(string skuId)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().RemoveLineItem(skuId);
            }
            else
            {
                ShoppingProvider.Instance().RemoveLineItem(HiContext.Current.User.UserId, skuId);
            }
        }

        public static void UpdateGiftItemQuantity(int giftId, int quantity)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                RemoveGiftItem(giftId);
            }
            if (user == null)
            {
                CookieShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity);
            }
            else
            {
                ShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity);
            }
        }

        public static void UpdateLineItemQuantity(int productId, string skuId, int quantity)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                RemoveLineItem(skuId);
            }
            if (user == null)
            {
                CookieShoppingProvider.Instance().UpdateLineItemQuantity(productId, skuId, quantity);
            }
            else
            {
                ShoppingProvider.Instance().UpdateLineItemQuantity(user, productId, skuId, quantity);
            }
        }
    }
}

