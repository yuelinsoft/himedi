namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class ShoppingProcessor
    {
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

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isGroupBuy, bool isCountDown, bool isSignBuy)
        {
            if ((shoppingCart.LineItems.Count == 0) && (shoppingCart.LineGifts.Count == 0))
            {
                return null;
            }
            OrderInfo info = new OrderInfo();
            info.ActivityId = shoppingCart.FeeFreeActivityId;
            info.ActivityName = shoppingCart.FeeFreeName;
            info.OrderOptionFree = shoppingCart.OrderOptionFree;
            info.ProcedureFeeFree = shoppingCart.ProcedureFeeFree;
            info.EightFree = shoppingCart.EightFree;
            info.DiscountId = shoppingCart.DiscountActivityId;
            info.DiscountName = shoppingCart.DiscountName;
            info.DiscountValue = shoppingCart.DiscountValue;
            info.DiscountValueType = shoppingCart.DiscountValueType;
            //OrderInfo info = info6;

            string skuIds = string.Empty;
            if (shoppingCart.LineItems.Values.Count > 0)
            {
                foreach (ShoppingCartItemInfo item in shoppingCart.LineItems.Values)
                {
                    skuIds = skuIds + string.Format("'{0}',", item.SkuId);
                }
            }

            Dictionary<string, decimal> costPriceForItems = new Dictionary<string, decimal>();
            if (!string.IsNullOrEmpty(skuIds))
            {
                skuIds = skuIds.Substring(0, skuIds.Length - 1);
                costPriceForItems = ShoppingProvider.Instance().GetCostPriceForItems(skuIds);
            }

            if (shoppingCart.LineItems.Values.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems.Values)
                {
                    decimal memberPrice = info2.MemberPrice;
                    decimal costPrice = 0M;
                    if (((info2.WholesaleDiscountId > 0) && !string.IsNullOrEmpty(info2.WholesaleDiscountName)) && info2.DiscountRate.HasValue)
                    {
                        memberPrice = info2.MemberPrice * (info2.DiscountRate.Value / 100M);
                    }
                    if ((isGroupBuy || isCountDown) || isSignBuy)
                    {
                        costPrice = ShoppingProvider.Instance().GetCostPrice(info2.SkuId);
                    }
                    else if (costPriceForItems.ContainsKey(info2.SkuId))
                    {
                        costPrice = costPriceForItems[info2.SkuId];
                    }
                    LineItemInfo info3 = new LineItemInfo(info2.SkuId, info2.ProductId, info2.SKU, info2.Quantity, info2.Quantity + info2.GiveQuantity, costPrice, info2.MemberPrice, memberPrice, info2.Name, info2.ThumbnailUrl40, info2.Weight, info2.PurchaseGiftId, info2.PurchaseGiftName, info2.WholesaleDiscountId, info2.WholesaleDiscountName, info2.SkuContent);
                    info.LineItems.Add(info3.SkuId, info3);
                }
            }
            if (shoppingCart.LineGifts.Count > 0)
            {
                foreach (ShoppingCartGiftInfo info4 in shoppingCart.LineGifts)
                {
                    OrderGiftInfo item = new OrderGiftInfo();
                    item.GiftId = info4.GiftId;
                    item.GiftName = info4.Name;
                    item.Quantity = info4.Quantity;
                    item.ThumbnailsUrl = info4.ThumbnailUrl40;
                    if (HiContext.Current.SiteSettings.IsDistributorSettings)
                    {
                        item.CostPrice = info4.PurchasePrice;
                    }
                    else
                    {
                        item.CostPrice = info4.CostPrice;
                    }
                    info.Gifts.Add(item);
                }
            }
            return info;
        }

        public static bool CreatOrder(OrderInfo orderInfo)
        {
            if (orderInfo == null)
            {
                return false;
            }
            return ShoppingProvider.Instance().CreatOrder(orderInfo);
        }

        public static bool CutNeedPoint(int needPoint)
        {
            Member user = HiContext.Current.User as Member;
            UserPointInfo point = new UserPointInfo();
            point.UserId = user.UserId;
            point.TradeDate = DateTime.Now;
            point.TradeType = UserPointTradeType.Change;
            point.Reduced = new int?(needPoint);
            point.Points = user.Points - needPoint;
            if ((point.Points > 0x7fffffff) || (point.Points < 0))
            {
                point.Points = 0;
            }
            if (ShoppingProvider.Instance().AddMemberPoint(point))
            {
                Users.ClearUserCache(user);
                return true;
            }
            return false;
        }

        public static DataTable GetCoupon(decimal orderAmount)
        {
            return ShoppingProvider.Instance().GetCoupon(orderAmount);
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return ShoppingProvider.Instance().GetCoupon(couponCode);
        }

        public static IList<ExpressCompanyInfo> GetExpressCompanysByMode(int modeId)
        {
            return ShoppingProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return ShoppingProvider.Instance().GetOrderInfo(orderId);
        }

        public static OrderLookupItemInfo GetOrderLookupItem(int lookupItemId, string orderId)
        {
            return ShoppingProvider.Instance().GetOrderLookupItem(lookupItemId, orderId);
        }

        public static IList<OrderLookupItemInfo> GetOrderLookupItems(int lookupListId)
        {
            return ShoppingProvider.Instance().GetOrderLookupItems(lookupListId);
        }

        public static OrderLookupListInfo GetOrderLookupList(int lookupListId)
        {
            return ShoppingProvider.Instance().GetOrderLookupList(lookupListId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return ShoppingProvider.Instance().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return ShoppingProvider.Instance().GetPaymentModes();
        }

        public static SKUItem GetProductAndSku(int productId, string options)
        {
            return ShoppingProvider.Instance().GetProductAndSku(productId, options);
        }

        public static DataTable GetProductInfoBySku(string skuId)
        {
            return ShoppingProvider.Instance().GetProductInfoBySku(skuId);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return ShoppingProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return ShoppingProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return ShoppingProvider.Instance().GetShippingModes();
        }

        public static IList<string> GetSkuIdsBysku(string sku)
        {
            return ShoppingProvider.Instance().GetSkuIdsBysku(sku);
        }

        public static IList<OrderLookupListInfo> GetUsableOrderLookupLists()
        {
            return ShoppingProvider.Instance().GetUsableOrderLookupLists();
        }

        public static DataTable GetYetShipOrders(int days)
        {
            return ShoppingProvider.Instance().GetYetShipOrders(days);
        }

        public static DataTable UseCoupon(decimal orderAmount)
        {
            return GetCoupon(orderAmount);
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon != null)
                {
                    decimal? amount;
                    if (coupon.Amount.HasValue)
                    {
                        amount = coupon.Amount;
                        if (!((amount.GetValueOrDefault() > 0M) && amount.HasValue) || (orderAmount < coupon.Amount.Value))
                        {
                        }
                    }
                    if (!(coupon.Amount.HasValue && (!(((amount = coupon.Amount).GetValueOrDefault() == 0M) && amount.HasValue) || (orderAmount <= coupon.DiscountValue))))
                    {
                        return coupon;
                    }
                }
            }
            return null;
        }
    }
}

