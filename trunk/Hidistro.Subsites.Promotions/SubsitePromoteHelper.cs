namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public static class SubsitePromoteHelper
    {
        public static bool AddCountDown(CountDownInfo countDownInfo)
        {
            return SubsitePromotionsProvider.Instance().AddCountDown(countDownInfo);
        }

        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int groupBuyId = SubsitePromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
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

        public static PromotionActionStatus AddPromotion(PromotionInfo promotion)
        {
            Globals.EntityCoding(promotion, true);
            return PromotionFactory.Create(promotion.PromoteType).Create(promotion);
        }

        public static bool DeleteCountDown(int countDownId)
        {
            return SubsitePromotionsProvider.Instance().DeleteCountDown(countDownId);
        }

        public static bool DeleteGroupBuy(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
        }

        public static bool DeletePromotion(int activityId)
        {
            return SubsitePromotionsProvider.Instance().DeletePromotion(activityId);
        }

        public static void DeletePromotionProducts(int activeId)
        {
            SubsitePromotionsProvider.Instance().DeletePromotionProducts(activeId);
        }

        public static bool DeletePromotionProducts(int activeId, int productId)
        {
            return SubsitePromotionsProvider.Instance().DeletePromotionProducts(activeId, productId);
        }

        public static int GetActiveIdByPromotionName(string name)
        {
            return SubsitePromotionsProvider.Instance().GetActiveIdByPromotionName(name);
        }

        public static DbQueryResult GetActiveProducts(Pagination page, int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetActiveProducts(page, activeId);
        }

        public static CountDownInfo GetCountDownInfo(int countDownId)
        {
            return SubsitePromotionsProvider.Instance().GetCountDownInfo(countDownId);
        }

        public static DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetCountDownList(query);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return SubsitePromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static FullDiscountInfo GetFullDiscountInfo(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetFullDiscountInfo(activeId);
        }

        public static FullFreeInfo GetFullFreeInfo(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetFullFreeInfo(activeId);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().GetGroupBuy(groupBuyId);
        }

        public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetGroupBuyList(query);
        }

        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return SubsitePromotionsProvider.Instance().GetMembersByRank(gradeId);
        }

        public static IList<Member> GetMemdersByNames(IList<string> names)
        {
            IList<Member> list = new List<Member>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    list.Add(user as Member);
                }
            }
            return list;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static DbQueryResult GetProducts(ProductQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetProducts(query);
        }

        public static IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            return SubsitePromotionsProvider.Instance().GetProducts(productIds);
        }

        public static IList<string> GetPromoteMemberGrades(int activityId)
        {
            return SubsitePromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
        }

        public static PromotionInfo GetPromotionInfoById(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetPromotionInfoById(activeId);
        }

        public static IList<int> GetPromotionProducts(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetPromotionProducts(activeId);
        }

        public static DataTable GetPromotions()
        {
            return SubsitePromotionsProvider.Instance().GetPromotions();
        }

        public static PurchaseGiftInfo GetPurchaseGiftInfo(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetPurchaseGiftInfo(activeId);
        }

        public static WholesaleDiscountInfo GetWholesaleDiscountInfo(int activeId)
        {
            return SubsitePromotionsProvider.Instance().GetWholesaleDiscountInfo(activeId);
        }

        public static bool InsertPromotionProduct(int activeId, int productId)
        {
            return SubsitePromotionsProvider.Instance().InsertPromotionProduct(activeId, productId);
        }

        public static bool ProductCountDownExist(int productId)
        {
            return SubsitePromotionsProvider.Instance().ProductCountDownExist(productId);
        }

        public static bool ProductGroupBuyExist(int productId)
        {
            return SubsitePromotionsProvider.Instance().ProductGroupBuyExist(productId);
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            return SubsitePromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
        }

        public static void SwapCountDownSequence(int countDownId, int replaceCountDownId, int displaySequence, int replaceDisplaySequence)
        {
            SubsitePromotionsProvider.Instance().SwapCountDownSequence(countDownId, replaceCountDownId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapGroupBuySequence(int groupBuyId, int replaceGroupBuyId, int displaySequence, int replaceDisplaySequence)
        {
            SubsitePromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, replaceGroupBuyId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            return SubsitePromotionsProvider.Instance().UpdateCountDown(countDownInfo);
        }

        public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsitePromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
    }
}

