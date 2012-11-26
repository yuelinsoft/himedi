using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.ControlPanel.Data
{
    public class PromotionData : PromotionsProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_CountDown;INSERT INTO Hishop_CountDown(ProductId,CountDownPrice,EndDate,Content,DisplaySequence) VALUES(@ProductId,@CountDownPrice,@EndDate,@Content,@DisplaySequence);");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int AddGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_GroupBuy;INSERT INTO Hishop_GroupBuy(ProductId,NeedPrice,EndDate,MaxCount,Content,Status,DisplaySequence) VALUES(@ProductId,@NeedPrice,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_GroupBuyCondition(GroupBuyId,Count,Price) VALUES(@GroupBuyId,@Count,@Price)");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "Count", DbType.Int32);
            database.AddInParameter(sqlStringCommand, "Price", DbType.Currency);
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
            sqlStringCommand.CommandText = string.Format("DELETE FROM Hishop_PromotionProducts WHERE ActivityId = {0}", activityId);
            foreach (int num in productIds)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" INSERT INTO Hishop_PromotionProducts (ActivityId, ProductId) VALUES ({0}, {1})", activityId, num);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
            if (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) >= 1)
            {
                return CouponActionStatus.DuplicateName;
            }
            sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_Coupons ([Name],  ClosingTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint); SELECT @@IDENTITY");
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
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                unknowError = CouponActionStatus.CreateClaimCodeSuccess;
                int result = 0;
                int.TryParse(obj2.ToString(), out result);
                if ((result > 0) && (count > 0))
                {
                    DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ClaimCode_Create");
                    database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, result);
                    database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
                    database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
                    database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
                    database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
                    try
                    {
                        database.ExecuteNonQuery(storedProcCommand);
                        lotNumber = (string)database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
                        storedProcCommand = database.GetSqlStringCommand("UPDATE Hishop_Coupons SET SentCount=@SentCount WHERE CouponId=@CouponId");
                        database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, result);
                        database.AddInParameter(storedProcCommand, "SentCount", DbType.Int32, count);
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
                        DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Promotions_AddFullDiscount");
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
                        DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Promotions_AddFullFree");
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
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Promotions_Add");
            database.AddInParameter(storedProcCommand, "Name", DbType.String, promotion.Name);
            database.AddInParameter(storedProcCommand, "PromoteType", DbType.Int32, (int)promotion.PromoteType);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, promotion.Description);
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
                        DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Promotions_AddPurchaseGift");
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

        public override GiftActionStatus CreateUpdateDeleteGift(GiftInfo gift, DataProviderAction action)
        {
            if (null == gift)
            {
                return GiftActionStatus.UnknowError;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Gift_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (DataProviderAction.Create != action)
            {
                database.AddInParameter(storedProcCommand, "GiftId", DbType.Int32, gift.GiftId);
            }
            else
            {
                database.AddOutParameter(storedProcCommand, "GiftId", DbType.Int32, 4);
            }
            if (DataProviderAction.Delete != action)
            {
                database.AddInParameter(storedProcCommand, "Name", DbType.String, gift.Name);
                database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, gift.ShortDescription);
                database.AddInParameter(storedProcCommand, "Unit", DbType.String, gift.Unit);
                database.AddInParameter(storedProcCommand, "LongDescription", DbType.String, gift.LongDescription);
                database.AddInParameter(storedProcCommand, "Title", DbType.String, gift.Title);
                database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, gift.Meta_Description);
                database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, gift.Meta_Keywords);
                database.AddInParameter(storedProcCommand, "CostPrice", DbType.Currency, gift.CostPrice);
                database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, gift.ImageUrl);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, gift.ThumbnailUrl40);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, gift.ThumbnailUrl60);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, gift.ThumbnailUrl100);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, gift.ThumbnailUrl160);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, gift.ThumbnailUrl180);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, gift.ThumbnailUrl220);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, gift.ThumbnailUrl310);
                database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, gift.ThumbnailUrl410);
                database.AddInParameter(storedProcCommand, "PurchasePrice", DbType.Currency, gift.PurchasePrice);
                database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, gift.MarketPrice);
                database.AddInParameter(storedProcCommand, "NeedPoint", DbType.Int32, gift.NeedPoint);
                database.AddInParameter(storedProcCommand, "IsDownLoad", DbType.Boolean, gift.IsDownLoad);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (GiftActionStatus)((int)database.GetParameterValue(storedProcCommand, "Status"));
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
                        DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Promotions_AddWholesaleDiscount");
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Coupons WHERE CouponId = @CouponId");
            database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteGroupBuy(int groupBuyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteGroupBuyCondition(int groupBuyId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePromotion(int activityId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Promotions WHERE ActivityId = @ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void DeletePromotionProducts(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeletePromotionProducts(int activeId, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId=@ActivityId AND ProductId=@ProductId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override int GetActiveIdByPromotionName(string name)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ActivityId FROM Hishop_Promotions WHERE Name=@Name");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, name);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetActiveProducts(Pagination page, int activeId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0}", 1);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId = {0})", activeId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override CountDownInfo GetCountDownInfo(int countDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
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
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat("ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "CountDownId,productId,ProductName,CountDownPrice,EndDate,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", builder.ToString(), selectFields);
        }

        public override CouponInfo GetCouponDetails(int couponId)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE CouponId = @CouponId");
            database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
            return (decimal)database.ExecuteScalar(sqlStringCommand);
        }

        public override FullDiscountInfo GetFullDiscountInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions P INNER JOIN Hishop_FullDiscounts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions P INNER JOIN Hishop_FullFree F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId");
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

        public override GiftInfo GetGiftDetails(int giftId)
        {
            GiftInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId = @GiftId");
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGift(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = null;
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public override GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId;SELECT * FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
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
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "GroupBuyId,ProductId,ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,EndDate,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_GroupBuy", "GroupBuyId", builder.ToString(), selectFields);
        }

        public override IList<Member> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Member> list = new List<Member>();
            if (gradeId > 0)
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
                database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            }
            else
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
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

        public override DbQueryResult GetNewCoupons(Pagination page)
        {
            string filter = " ClosingTime > '" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "' AND SentCount = 0";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", filter, "*");
        }

        public override int GetOrderCount(int groupBuyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int)obj2;
            }
            return 0;
        }

        public override DbQueryResult GetOverdueCoupons(Pagination page)
        {
            string filter = " ClosingTime <='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "'";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", filter, "*");
        }

        public override IList<string> GetPromoteMemberGrades(int activityId)
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Name FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId=@ActivityId)");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT PromoteType FROM Hishop_Promotions WHERE ActivityId = @ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                return PromoteType.NotSet;
            }
            return (PromoteType)Convert.ToInt32(obj2);
        }

        public override PromotionInfo GetPromotionInfoById(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
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
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions ORDER BY ActivityId DESC");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override PurchaseGiftInfo GetPurchaseGiftInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions P INNER JOIN Hishop_PurchaseGifts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
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

        public override List<int> GetSendIds(int? gradeId, string userName)
        {
            List<int> list = new List<int>();
            string query = string.Format("SELECT UserId FROM vw_aspnet_Members WHERE UserName Like '%{0}%' ", DataHelper.CleanSearchString(userName));
            if (gradeId.HasValue)
            {
                string str2 = string.Format(" AND GradeId={0}", gradeId);
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
            string filter = " ClosingTime > '" + DataHelper.GetSafeDateTimeFormat(DateTime.Now) + "' AND SentCount > 0";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", filter, "*");
        }

        public override WholesaleDiscountInfo GetWholesaleDiscountInfo(int activeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions P INNER JOIN Hishop_WholesaleDiscounts F ON P.ActivityId=F.ActivityId WHERE P.ActivityId=@ActivityId");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_PromotionProducts VALUES (@ActivityId,@ProductId)");
            database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activeId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ProductCountDownExist(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_CountDown WHERE ProductId=@ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ProductGroupBuyExist(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status=@Status");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        int ReSetPromotionMemberGraders(int activityId, IList<int> memberGradeIds, DbTransaction tran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" ");
            sqlStringCommand.CommandText = string.Format("DELETE FROM Hishop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
            foreach (int num in memberGradeIds)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" INSERT INTO Hishop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, num);
            }
            return database.ExecuteNonQuery(sqlStringCommand, tran);
        }

        public override bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId, EmailAddress) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @EmailAddress)");
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId;UPDATE Hishop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int)status);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapCountDownSequence(int countDownId, int replaceCountDownId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_CountDown", "CountDownId", "DisplaySequence", countDownId, replaceCountDownId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapGroupBuySequence(int groupBuyId, int replaceGroupBuyId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_GroupBuy", "GroupBuyId", "DisplaySequence", groupBuyId, replaceGroupBuyId, displaySequence, replaceDisplaySequence);
        }

        public override bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,EndDate=@EndDate,Content=@Content WHERE CountDownId=@CountDownId");
            database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            if (null != coupon)
            {
                DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
                database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                if (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint WHERE CouponId=@CouponId");
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId");
            database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuy.GroupBuyId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
            database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
            database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
            database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
            if (dbTran != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateIsDownLoad(int giftId, bool isdownload)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update Hishop_Gifts set IsDownLoad=@IsDownLoad  where GiftId = @GiftId");
            database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            database.AddInParameter(sqlStringCommand, "IsDownLoad", DbType.Boolean, isdownload);
            try
            {
                return (database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }
    }
}

