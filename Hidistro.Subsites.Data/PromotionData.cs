using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.Subsites.Data
{
    public class PromotionData : SubsitePromotionsProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM distro_CountDown; INSERT INTO distro_CountDown(ProductId,DistributorUserId,CountDownPrice,EndDate,Content,DisplaySequence) VALUES(@ProductId,@DistributorUserId,@CountDownPrice,@EndDate,@Content,@DisplaySequence);");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int AddGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM distro_GroupBuy;INSERT INTO distro_GroupBuy(ProductId,DistributorUserId,NeedPrice,EndDate,MaxCount,Content,Status,DisplaySequence) VALUES(@ProductId,@DistributorUserId,@NeedPrice,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
            database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, DbTransaction dbTran)
        {
            if (gropBuyConditions.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_GroupBuyCondition(GroupBuyId,Count,Price,DistributorUserId) VALUES(@GroupBuyId,@Count,@Price,@DistributorUserId)");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "Count", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "Price", DbType.Currency);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            try
            {
                foreach (GropBuyConditionInfo info in gropBuyConditions)
                {
                    database.SetParameterValue(sqlStringCommand, "Count", info.Count);
                    database.SetParameterValue(sqlStringCommand, "Price", info.Price);
                    if (dbTran != null)
                    {
                        database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        int AddPromotionProducts(int activityId, IList<int> productIds, DbTransaction tran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            sqlStringCommand.CommandText = string.Format("DELETE FROM distro_PromotionProducts WHERE ActivityId = {0}", activityId);
            foreach (int num in productIds)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" INSERT INTO distro_PromotionProducts (ActivityId, ProductId,DistributorUserId) VALUES ({0}, {1},{2})", activityId, num, HiContext.Current.User.UserId);
            }
            return database.ExecuteNonQuery(sqlStringCommand, tran);
        }

        public override CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            CouponActionStatus unknowError = CouponActionStatus.UnknowError;
            lotNumber = string.Empty;
            if (null == coupon)
            {
                return CouponActionStatus.UnknowError;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT CouponId  FROM distro_Coupons WHERE Name=@Name AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) >= 1)
            {
                return CouponActionStatus.DuplicateName;
            }
            sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_Coupons ([Name],  ClosingTime, Description, Amount, DiscountValue,DistributorUserId,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime, @Description, @Amount, @DiscountValue,@DistributorUserId,0,0,@NeedPoint); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
            database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
            database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
            if (coupon.Amount.HasValue)
            {
                database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
            }
            else
            {
                database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
            }
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                unknowError = CouponActionStatus.CreateClaimCodeSuccess;
                int result = 0;
                int.TryParse(obj2.ToString(), out result);
                if ((result > 0) && (count > 0))
                {
                    DbCommand storedProcCommand = database.GetStoredProcCommand("ss_ClaimCode_Create");
                    database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, result);
                    database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
                    database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
                    database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
                    database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
                    try
                    {
                        database.ExecuteNonQuery(storedProcCommand);
                        lotNumber = (string)database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
                        storedProcCommand = database.GetSqlStringCommand("UPDATE distro_Coupons SET SentCount=@SentCount WHERE CouponId=@CouponId AND DistributorUserId=@DistributorUserId");
                        database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, result);
                        database.AddInParameter(storedProcCommand, "SentCount", DbType.Int32, count);
                        database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                        database.ExecuteNonQuery(storedProcCommand);
                    }
                    catch
                    {
                        unknowError = CouponActionStatus.CreateClaimCodeError;
                    }
                }
            }
            return unknowError;
        }

        public override PromotionActionStatus CreateFullDiscount(FullDiscountInfo promote)
        {
            PromotionActionStatus unknowError = PromotionActionStatus.UnknowError;
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction tran = connection.BeginTransaction();
                try
                {
                    int activityId = CreatePromotion(promote, tran);
                    if (activityId > 0)
                    {
                        if (((promote.MemberGradeIds != null) && (promote.MemberGradeIds.Count > 0)) && (ReSetPromotionMemberGraders(activityId, promote.MemberGradeIds, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_AddFullDiscount");
                        database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                        database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, activityId);
                        database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, promote.Amount);
                        database.AddInParameter(storedProcCommand, "DiscountValue", DbType.Currency, promote.DiscountValue);
                        database.AddInParameter(storedProcCommand, "ValueType", DbType.Int32, (int)promote.ValueType);
                        database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                        database.ExecuteNonQuery(storedProcCommand, tran);
                        unknowError = (PromotionActionStatus)Convert.ToInt32(database.GetParameterValue(storedProcCommand, "ReturnValue"));
                        if (unknowError == PromotionActionStatus.Success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    else if (activityId == 0)
                    {
                        unknowError = PromotionActionStatus.DuplicateName;
                        tran.Rollback();
                    }
                }
                catch
                {
                    if (tran.Connection != null)
                    {
                        tran.Rollback();
                    }
                    unknowError = PromotionActionStatus.UnknowError;
                }
                connection.Close();
            }
            return unknowError;
        }

        public override PromotionActionStatus CreateFullFree(FullFreeInfo promote)
        {
            PromotionActionStatus unknowError = PromotionActionStatus.UnknowError;
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction tran = connection.BeginTransaction();
                try
                {
                    int activityId = CreatePromotion(promote, tran);
                    if (activityId > 0)
                    {
                        if (((promote.MemberGradeIds != null) && (promote.MemberGradeIds.Count > 0)) && (ReSetPromotionMemberGraders(activityId, promote.MemberGradeIds, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_AddFullFree");
                        database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                        database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, activityId);
                        database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, promote.Amount);
                        database.AddInParameter(storedProcCommand, "ShipChargeFree", DbType.Boolean, promote.ShipChargeFree);
                        database.AddInParameter(storedProcCommand, "ServiceChargeFree", DbType.Boolean, promote.ServiceChargeFree);
                        database.AddInParameter(storedProcCommand, "OptionFeeFree", DbType.Boolean, promote.OptionFeeFree);
                        database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                        database.ExecuteNonQuery(storedProcCommand, tran);
                        unknowError = (PromotionActionStatus)Convert.ToInt32(database.GetParameterValue(storedProcCommand, "ReturnValue"));
                        if (unknowError == PromotionActionStatus.Success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    else if (activityId == 0)
                    {
                        unknowError = PromotionActionStatus.DuplicateName;
                        tran.Rollback();
                    }
                }
                catch
                {
                    if (tran.Connection != null)
                    {
                        tran.Rollback();
                    }
                    unknowError = PromotionActionStatus.UnknowError;
                }
                connection.Close();
            }
            return unknowError;
        }

        int CreatePromotion(PromotionInfo promotion, DbTransaction tran)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_Add");
            database.AddInParameter(storedProcCommand, "Name", DbType.String, promotion.Name);
            database.AddInParameter(storedProcCommand, "PromoteType", DbType.Int32, (int)promotion.PromoteType);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, promotion.Description);
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return Convert.ToInt32(database.ExecuteScalar(storedProcCommand, tran));
        }

        public override PromotionActionStatus CreatePurchaseGift(PurchaseGiftInfo promote)
        {
            PromotionActionStatus unknowError = PromotionActionStatus.UnknowError;
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction tran = connection.BeginTransaction();
                try
                {
                    int activityId = CreatePromotion(promote, tran);
                    if (activityId > 0)
                    {
                        if (((promote.MemberGradeIds != null) && (promote.MemberGradeIds.Count > 0)) && (ReSetPromotionMemberGraders(activityId, promote.MemberGradeIds, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        if (((promote.ProductList != null) && (promote.ProductList.Count > 0)) && (AddPromotionProducts(activityId, promote.ProductList, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_AddPurchaseGift");
                        database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                        database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, activityId);
                        database.AddInParameter(storedProcCommand, "BuyQuantity", DbType.Int32, promote.BuyQuantity);
                        database.AddInParameter(storedProcCommand, "GiveQuantity", DbType.Int32, promote.GiveQuantity);
                        database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                        database.ExecuteNonQuery(storedProcCommand, tran);
                        unknowError = (PromotionActionStatus)Convert.ToInt32(database.GetParameterValue(storedProcCommand, "ReturnValue"));
                        if (unknowError == PromotionActionStatus.Success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    else if (activityId == 0)
                    {
                        unknowError = PromotionActionStatus.DuplicateName;
                        tran.Rollback();
                    }
                }
                catch
                {
                    if (tran.Connection != null)
                    {
                        tran.Rollback();
                    }
                    unknowError = PromotionActionStatus.UnknowError;
                }
                connection.Close();
            }
            return unknowError;
        }

        public override PromotionActionStatus CreateWholesaleDiscount(WholesaleDiscountInfo promote)
        {
            PromotionActionStatus unknowError = PromotionActionStatus.UnknowError;
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction tran = connection.BeginTransaction();
                try
                {
                    int activityId = CreatePromotion(promote, tran);
                    if (activityId > 0)
                    {
                        if (((promote.MemberGradeIds != null) && (promote.MemberGradeIds.Count > 0)) && (ReSetPromotionMemberGraders(activityId, promote.MemberGradeIds, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        if (((promote.ProductList != null) && (promote.ProductList.Count > 0)) && (AddPromotionProducts(activityId, promote.ProductList, tran) <= 0))
                        {
                            tran.Rollback();
                        }
                        DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_AddWholesaleDiscount");
                        database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                        database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, activityId);
                        database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, promote.Quantity);
                        database.AddInParameter(storedProcCommand, "DiscountValue", DbType.Int32, promote.DiscountValue);
                        database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                        database.ExecuteNonQuery(storedProcCommand, tran);
                        unknowError = (PromotionActionStatus)Convert.ToInt32(database.GetParameterValue(storedProcCommand, "ReturnValue"));
                        if (unknowError == PromotionActionStatus.Success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    else if (activityId == 0)
                    {
                        unknowError = PromotionActionStatus.DuplicateName;
                        tran.Rollback();
                    }
                }
                catch
                {
                    if (tran.Connection != null)
                    {
                        tran.Rollback();
                    }
                    unknowError = PromotionActionStatus.UnknowError;
                }
                connection.Close();
            }
            return unknowError;
        }

        public override bool DeleteCountDown(int countDownId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_Coupons WHERE CouponId = @CouponId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteGiftById(int giftId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("delete from distro_Gifts where GiftId=@Id and DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, giftId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool DeleteGroupBuy(int groupBuyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteGroupBuyCondition(int groupBuyId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePromotion(int activityId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" DELETE FROM distro_PromotionMemberGrades WHERE ActivityId = @ActivityId DELETE FROM distro_Promotions WHERE ActivityId = @ActivityId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void DeletePromotionProducts(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_PromotionProducts WHERE ActivityId=@ActivityId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeletePromotionProducts(int activeId, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_PromotionProducts WHERE ActivityId=@ActivityId AND ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool DownLoadGift(GiftInfo gift)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("insert into distro_Gifts VALUES (@GiftId,@DistributorUserId,@Name,@ShortDescription,@Title,@Meta_Description,@Meta_Keywords,@NeedPoint)");
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, gift.Name);
            database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, gift.ShortDescription);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, gift.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, gift.Meta_Description);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, gift.Meta_Keywords);
            database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, gift.NeedPoint);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override DbQueryResult GetAbstroGiftsById(GiftQuery query)
        {
            string filter = "d_DistributorUserId=" + HiContext.Current.User.UserId;
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = string.Format(" ([Name] LIKE '%{0}%' or d_Name like '%{1}%')", DataHelper.CleanSearchString(query.Name), DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_Gifts", "d_GiftId", filter, "d_GiftId,d_DistributorUserId,d_Name,d_NeedPoint,Name,GiftId,PurchasePrice");
        }

        public override int GetActiveIdByPromotionName(string name)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ActivityId FROM distro_Promotions WHERE Name=@Name AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, name);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetActiveProducts(Pagination page, int activeId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0} AND DistributorUserId={1}", 1, HiContext.Current.User.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM distro_PromotionProducts WHERE ActivityId = {0} and DistributorUserId={1})", activeId, HiContext.Current.User.UserId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override CountDownInfo GetCountDownInfo(int countDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "CountDownId,ProductId, ProductName,CountDownPrice,EndDate,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_CountDown", "CountDownId", builder.ToString(), selectFields);
        }

        public override CouponInfo GetCouponDetails(int couponId)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Coupons WHERE CouponId = @CouponId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCoupon(reader);
                }
            }
            return info;
        }

        public override IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            IList<CouponItemInfo> list = new List<CouponItemInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
            database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
            CouponItemInfo item = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    item = DataMapper.PopulateCouponItem(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity AND DistributorUserId=@DistributorUserId;if @price IS NULL SELECT @price = max(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;select @price");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (decimal)database.ExecuteScalar(sqlStringCommand);
        }

        public override FullDiscountInfo GetFullDiscountInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions P INNER JOIN distro_FullDiscounts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId AND P.DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            PromotionInfo info = new PromotionInfo();
            FullDiscountInfo info2 = new FullDiscountInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                    info2 = DataMapper.PopulateFullDiscount(reader);
                }
                info2.Name = info.Name;
                info2.Description = info.Description;
            }
            return info2;
        }

        public override FullFreeInfo GetFullFreeInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions P INNER JOIN distro_FullFree F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId AND P.DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            PromotionInfo info = new PromotionInfo();
            FullFreeInfo info2 = new FullFreeInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                    info2 = DataMapper.PopulateFullFree(reader);
                }
                info2.Name = info.Name;
                info2.Description = info.Description;
            }
            return info2;
        }

        public override DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = "IsDownLoad=1 and GiftId not in (select GiftId from distro_Gifts where DistributorUserId=" + HiContext.Current.User.UserId + ")";
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = filter + string.Format(" and [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public override GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGroupBuy(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    GropBuyConditionInfo item = new GropBuyConditionInfo();
                    item.Count = (int)reader["Count"];
                    item.Price = (decimal)reader["Price"];
                    info.GroupBuyConditions.Add(item);
                }
            }
            return info;
        }

        public override DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "GroupBuyId,ProductId, ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,EndDate,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_GroupBuy", "GroupBuyId", builder.ToString(), selectFields);
        }

        public override IList<Member> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Member> list = new List<Member>();
            if (gradeId > 0)
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_distro_Members WHERE GradeId=@GradeId AND ParentUserId=@ParentUserId");
                database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
                database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            }
            else
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_distro_Members WHERE ParentUserId=@ParentUserId");
                database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            }
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Member item = new Member(UserRole.Member);
                    item.UserId = (int)reader["UserId"];
                    item.Email = reader["Email"].ToString();
                    item.Username = reader["UserName"].ToString();
                    list.Add(item);
                }
            }
            return list;
        }

        public override GiftInfo GetMyGiftsDetails(int Id)
        {
            GiftInfo entity = new GiftInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("select d_Name as [Name],d_Title as Title,d_Meta_Description as Meta_Description,d_Meta_Keywords as Meta_Keywords,d_NeedPoint as NeedPoint,GiftId,ShortDescription,Unit, LongDescription,CostPrice,ImageUrl, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, PurchasePrice,MarketPrice,IsDownLoad from vw_distro_Gifts where d_DistributorUserId=@DistributorUserId and GiftId=@Id");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    entity = DataMapper.PopulateGift(reader);
                }
            }
            Globals.EntityCoding(entity, false);
            return entity;
        }

        public override DbQueryResult GetNewCoupons(Pagination page)
        {
            string filter = " ClosingTime > '" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "' AND SentCount = 0" + string.Format(" AND DistributorUserId={0}", HiContext.Current.User.UserId);
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_Coupons", "CouponId", filter, "*");
        }

        public override int GetOrderCount(int groupBuyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND DistributorUserId=@DistributorUserId AND OrderStatus <> 1 AND OrderStatus <> 4)");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int)obj2;
            }
            return 0;
        }

        public override DbQueryResult GetOverdueCoupons(Pagination page)
        {
            string filter = " ClosingTime <='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "'" + string.Format(" AND DistributorUserId={0}", HiContext.Current.User.UserId);
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_Coupons", "CouponId", filter, "*");
        }

        public override IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            IList<ProductInfo> list = new List<ProductInfo>();
            string str = "(";
            foreach (int num in productIds)
            {
                str = str + num + ",";
            }
            if (str.Length <= 1)
            {
                return list;
            }
            return list;
        }

        public override DbQueryResult GetProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SaleStatus = {0}", (int)query.SaleStatus);
            builder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND (CategoryId = {0}", query.CategoryId.Value);
                builder.AppendFormat(" OR CategoryId IN (SELECT CategoryId FROM distro_Categories WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList", "ProductId", builder.ToString(), "ProductId, ProductName, ThumbnailUrl40, (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, MarketPrice, SalePrice,(SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice, Stock, DisplaySequence");
        }

        public override IList<string> GetPromoteMemberGrades(int activityId)
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Name FROM distro_MemberGrades WHERE GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE ActivityId=@ActivityId) AND CreateUserId=@CreateUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            database.AddInParameter(sqlStringCommand, "CreateUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader["Name"].ToString());
                }
            }
            return list;
        }

        public override PromoteType GetPromoteType(int activityId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT PromoteType FROM distro_Promotions WHERE ActivityId = @ActivityId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                return PromoteType.NotSet;
            }
            return (PromoteType)Convert.ToInt32(obj2);
        }

        public override PromotionInfo GetPromotionInfoById(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions WHERE ActivityId=@ActivityId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            PromotionInfo entity = new PromotionInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    entity = DataMapper.PopulatePromote(reader);
                }
            }
            Globals.EntityCoding(entity, false);
            return entity;
        }

        public override IList<int> GetPromotionProducts(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId FROM distro_PromotionProducts WHERE ActivityId=@ActivityId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            IList<int> list = new List<int>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(int.Parse(reader["ProductId"].ToString()));
                }
            }
            return list;
        }

        public override DataTable GetPromotions()
        {
            DataTable table = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions WHERE DistributorUserId=@DistributorUserId ORDER BY ActivityId DESC");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override PurchaseGiftInfo GetPurchaseGiftInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions P INNER JOIN distro_PurchaseGifts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId AND P.DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            PromotionInfo info = new PromotionInfo();
            PurchaseGiftInfo info2 = new PurchaseGiftInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                    info2 = DataMapper.PopulatePurchaseGift(reader);
                }
                info2.Name = info.Name;
                info2.Description = info.Description;
            }
            return info2;
        }

        public override List<int> GetUderlingIds(int? gradeId, string userName)
        {
            List<int> list = new List<int>();
            string query = string.Format("SELECT UserId FROM vw_distro_Members WHERE UserName Like '%{0}%' AND ParentUserId={1}", DataHelper.CleanSearchString(userName), HiContext.Current.User.UserId);
            if (gradeId.HasValue)
            {
                string str2 = string.Format(" AND GradeId={0} ", gradeId);
                query = query + str2;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    int item = Convert.ToInt32(reader[0]);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetUsingCoupons(Pagination page)
        {
            string filter = " ClosingTime > '" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "' AND SentCount > 0" + string.Format(" AND DistributorUserId={0}", HiContext.Current.User.UserId);
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_Coupons", "CouponId", filter, "*");
        }

        public override WholesaleDiscountInfo GetWholesaleDiscountInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_Promotions P INNER JOIN distro_WholesaleDiscounts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId AND P.DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            PromotionInfo info = new PromotionInfo();
            WholesaleDiscountInfo info2 = new WholesaleDiscountInfo();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                    info2 = DataMapper.PopulateWholesaleDiscount(reader);
                }
                info2.Name = info.Name;
                info2.Description = info.Description;
            }
            return info2;
        }

        public override bool InsertPromotionProduct(int activeId, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_PromotionProducts VALUES (@ActivityId,@ProductId,@DistributorUserId)");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ProductCountDownExist(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_CountDown WHERE ProductId=@ProductId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ProductGroupBuyExist(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_GroupBuy WHERE ProductId=@ProductId AND Status=@Status AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        int ReSetPromotionMemberGraders(int activityId, IList<int> memberGradeIds, DbTransaction tran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            sqlStringCommand.CommandText = string.Format("DELETE FROM distro_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
            foreach (int num in memberGradeIds)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" INSERT INTO distro_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, num);
            }
            return database.ExecuteNonQuery(sqlStringCommand, tran);
        }

        public override bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_CouponItems(CouponId, ClaimCode, GenerateTime, UserId, EmailAddress,LotNumber) VALUES(@CouponId, @ClaimCode, @GenerateTime, @UserId, @EmailAddress,@LotNumber)");
            database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            if (couponItem.UserId.HasValue)
            {
                database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;UPDATE distro_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int)status);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapCountDownSequence(int countDownId, int replaceCountDownId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_CountDown", "CountDownId", "DisplaySequence", countDownId, replaceCountDownId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapGroupBuySequence(int groupBuyId, int replaceGroupBuyId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_GroupBuy", "GroupBuyId", "DisplaySequence", groupBuyId, replaceGroupBuyId, displaySequence, replaceDisplaySequence);
        }

        public override bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,EndDate=@EndDate,Content=@Content WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            if (null != coupon)
            {
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT CouponId  FROM distro_Coupons WHERE Name=@Name AND CouponId<>@CouponId AND DistributorUserId=@DistributorUserId");
                database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                if (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint WHERE CouponId=@CouponId");
                database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, coupon.CouponId);
                database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                if (coupon.Amount.HasValue)
                {
                    database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                if (database.ExecuteNonQuery(sqlStringCommand) == 1)
                {
                    return CouponActionStatus.Success;
                }
            }
            return CouponActionStatus.UnknowError;
        }

        public override bool UpdateGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuy.GroupBuyId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
            database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
            database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateMyGifts(GiftInfo giftInfo)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update distro_Gifts set [Name]=@Name,ShortDescription=@ShortDescription,Title=@Title,Meta_Description=@Meta_Description,Meta_Keywords=@Meta_Keywords,NeedPoint=@NeedPoint where GiftId=@Id and DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, giftInfo.Name);
            database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, giftInfo.ShortDescription);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, giftInfo.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, giftInfo.Meta_Description);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, giftInfo.Meta_Keywords);
            database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, giftInfo.NeedPoint);
            database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, giftInfo.GiftId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        PromotionActionStatus UpdatePromotion(PromotionInfo promotion, DbTransaction tran)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Promotions_Update");
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(storedProcCommand, "ActivityId", DbType.Int32, promotion.ActivityId);
            database.AddInParameter(storedProcCommand, "Name", DbType.String, promotion.Name);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, promotion.Description);
            database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            database.ExecuteNonQuery(storedProcCommand, tran);
            return (PromotionActionStatus)Convert.ToInt32(database.GetParameterValue(storedProcCommand, "ReturnValue"));
        }
    }
}

