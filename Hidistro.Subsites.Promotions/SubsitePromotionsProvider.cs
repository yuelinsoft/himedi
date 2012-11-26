using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using Hidistro.Membership.Context;
using Hidistro.Core;

namespace Hidistro.Subsites.Promotions
{

    public abstract class SubsitePromotionsProvider
    {
       static readonly SubsitePromotionsProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.PromotionData,Hidistro.Subsites.Data") as SubsitePromotionsProvider);

        protected SubsitePromotionsProvider()
        {
        }

        public abstract bool AddCountDown(CountDownInfo countDownInfo);
        public abstract int AddGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran);
        public abstract bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, DbTransaction dbTran);
        public abstract CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber);
        public abstract PromotionActionStatus CreateFullDiscount(FullDiscountInfo promote);
        public abstract PromotionActionStatus CreateFullFree(FullFreeInfo promote);
        public abstract PromotionActionStatus CreatePurchaseGift(PurchaseGiftInfo promote);
        public abstract PromotionActionStatus CreateWholesaleDiscount(WholesaleDiscountInfo promote);
        public abstract bool DeleteCountDown(int countDownId);
        public abstract bool DeleteCoupon(int couponId);
        public abstract bool DeleteGiftById(int giftId);
        public abstract bool DeleteGroupBuy(int groupBuyId);
        public abstract bool DeleteGroupBuyCondition(int groupBuyId, DbTransaction dbTran);
        public abstract bool DeletePromotion(int activityId);
        public abstract void DeletePromotionProducts(int activeId);
        public abstract bool DeletePromotionProducts(int activeId, int productId);
        public abstract bool DownLoadGift(GiftInfo giftinfo);
        public abstract DbQueryResult GetAbstroGiftsById(GiftQuery query);
        public abstract int GetActiveIdByPromotionName(string name);
        public abstract DbQueryResult GetActiveProducts(Pagination page, int activeId);
        public abstract CountDownInfo GetCountDownInfo(int countDownId);
        public abstract DbQueryResult GetCountDownList(GroupBuyQuery query);
        public abstract CouponInfo GetCouponDetails(int couponId);
        public abstract IList<CouponItemInfo> GetCouponItemInfos(string lotNumber);
        public abstract decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity);
        public abstract FullDiscountInfo GetFullDiscountInfo(int activeId);
        public abstract FullFreeInfo GetFullFreeInfo(int activeId);
        public abstract DbQueryResult GetGifts(GiftQuery query);
        public abstract GroupBuyInfo GetGroupBuy(int groupBuyId);
        public abstract DbQueryResult GetGroupBuyList(GroupBuyQuery query);
        public abstract IList<Member> GetMembersByRank(int? gradeId);
        public abstract GiftInfo GetMyGiftsDetails(int giftId);
        public abstract DbQueryResult GetNewCoupons(Pagination page);
        public abstract int GetOrderCount(int groupBuyId);
        public abstract DbQueryResult GetOverdueCoupons(Pagination page);
        public abstract DbQueryResult GetProducts(ProductQuery query);
        public abstract IList<ProductInfo> GetProducts(IList<int> productIds);
        public abstract IList<string> GetPromoteMemberGrades(int activityId);
        public abstract PromoteType GetPromoteType(int activityId);
        public abstract PromotionInfo GetPromotionInfoById(int activeId);
        public abstract IList<int> GetPromotionProducts(int activeId);
        public abstract DataTable GetPromotions();
        public abstract PurchaseGiftInfo GetPurchaseGiftInfo(int activeId);
        public abstract List<int> GetUderlingIds(int? gradeId, string userName);
        public abstract DbQueryResult GetUsingCoupons(Pagination page);
        public abstract WholesaleDiscountInfo GetWholesaleDiscountInfo(int activeId);
        public abstract bool InsertPromotionProduct(int activeId, int productId);
        public static SubsitePromotionsProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool ProductCountDownExist(int productId);
        public abstract bool ProductGroupBuyExist(int productId);
        public abstract bool SendClaimCodes(int couponId, CouponItemInfo couponItem);
        public abstract bool SetGroupBuyEndUntreated(int groupBuyId);
        public abstract bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status);
        public abstract void SwapCountDownSequence(int countDownId, int replaceCountDownId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapGroupBuySequence(int groupBuyId, int replaceGroupBuyId, int displaySequence, int replaceDisplaySequence);
        public abstract bool UpdateCountDown(CountDownInfo countDownInfo);
        public abstract CouponActionStatus UpdateCoupon(CouponInfo coupon);
        public abstract bool UpdateGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran);
        public abstract bool UpdateMyGifts(GiftInfo giftInfo);
    }
}

