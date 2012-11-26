using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.ControlPanel.Promotions
{
    public static class PromoteHelper
    {
        public static bool AddCountDown(CountDownInfo countDownInfo)
        {
            return PromotionsProvider.Instance().AddCountDown(countDownInfo);
        }

        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int groupBuyId = PromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
            return PromotionsProvider.Instance().DeleteCountDown(countDownId);
        }

        public static bool DeleteGroupBuy(int groupBuyId)
        {
            return PromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
        }

        public static bool DeletePromotion(int activityId)
        {
            return PromotionsProvider.Instance().DeletePromotion(activityId);
        }

        public static void DeletePromotionProducts(int activeId)
        {
            PromotionsProvider.Instance().DeletePromotionProducts(activeId);
        }

        public static bool DeletePromotionProducts(int activeId, int productId)
        {
            return PromotionsProvider.Instance().DeletePromotionProducts(activeId, productId);
        }

        public static int GetActiveIdByPromotionName(string name)
        {
            return PromotionsProvider.Instance().GetActiveIdByPromotionName(name);
        }

        public static DbQueryResult GetActiveProducts(Pagination page, int activeId)
        {
            return PromotionsProvider.Instance().GetActiveProducts(page, activeId);
        }

        public static CountDownInfo GetCountDownInfo(int countDownId)
        {
            return PromotionsProvider.Instance().GetCountDownInfo(countDownId);
        }

        public static DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            return PromotionsProvider.Instance().GetCountDownList(query);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return PromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static FullDiscountInfo GetFullDiscountInfo(int activeId)
        {
            return PromotionsProvider.Instance().GetFullDiscountInfo(activeId);
        }

        public static FullFreeInfo GetFullFreeInfo(int activeId)
        {
            return PromotionsProvider.Instance().GetFullFreeInfo(activeId);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return PromotionsProvider.Instance().GetGroupBuy(groupBuyId);
        }

        public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            return PromotionsProvider.Instance().GetGroupBuyList(query);
        }

        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return PromotionsProvider.Instance().GetMembersByRank(gradeId);
        }

        public static IList<Member> GetMemdersByNames(IList<string> names)
        {
            IList<Member> list = new List<Member>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Member))
                {
                    list.Add(user as Member);
                }
            }
            return list;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return PromotionsProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static IList<string> GetPromoteMemberGrades(int activityId)
        {
            return PromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
        }

        public static PromotionInfo GetPromotionInfoById(int activeId)
        {
            return PromotionsProvider.Instance().GetPromotionInfoById(activeId);
        }

        public static IList<int> GetPromotionProducts(int activeId)
        {
            return PromotionsProvider.Instance().GetPromotionProducts(activeId);
        }

        public static DataTable GetPromotions()
        {
            return PromotionsProvider.Instance().GetPromotions();
        }

        public static PurchaseGiftInfo GetPurchaseGiftInfo(int activeId)
        {
            return PromotionsProvider.Instance().GetPurchaseGiftInfo(activeId);
        }

        public static WholesaleDiscountInfo GetWholesaleDiscountInfo(int activeId)
        {
            return PromotionsProvider.Instance().GetWholesaleDiscountInfo(activeId);
        }

        public static bool InsertPromotionProduct(int activeId, int productId)
        {
            return PromotionsProvider.Instance().InsertPromotionProduct(activeId, productId);
        }

        public static bool ProductCountDownExist(int productId)
        {
            return PromotionsProvider.Instance().ProductCountDownExist(productId);
        }

        public static bool ProductGroupBuyExist(int productId)
        {
            return PromotionsProvider.Instance().ProductGroupBuyExist(productId);
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return PromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            return PromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
        }

        public static void SwapCountDownSequence(int countDownId, int replaceCountDownId, int displaySequence, int replaceDisplaySequence)
        {
            PromotionsProvider.Instance().SwapCountDownSequence(countDownId, replaceCountDownId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapGroupBuySequence(int groupBuyId, int replaceGroupBuyId, int displaySequence, int replaceDisplaySequence)
        {
            PromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, replaceGroupBuyId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            return PromotionsProvider.Instance().UpdateCountDown(countDownInfo);
        }

        public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!PromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTran))
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

