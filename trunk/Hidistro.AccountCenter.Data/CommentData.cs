using Hidistro.AccountCenter.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.AccountCenter.Data
{
    public class CommentData : CommentMasterDataProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

        public override bool AddProductToFavorite(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, @Tags, @Remark)");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            database.AddInParameter(sqlStringCommand, "Tags", DbType.String, string.Empty);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, string.Empty);
            try
            {
                database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

       static string BuildConsultationAndReplyQuery(ProductConsultationAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ConsultationId FROM Hishop_ProductConsultations ");
            builder.Append(" WHERE 1 = 1");
            if (query.ProductId > 0)
            {
                builder.AppendFormat(" AND ProductId = {0} ", query.ProductId);
            }
            if (query.UserId > 0)
            {
                builder.AppendFormat(" AND UserId = {0} ", query.UserId);
            }
            if (query.Type == ConsultationReplyType.NoReply)
            {
                builder.Append(" AND ReplyText IS NULL");
            }
            else if (query.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND ReplyText IS NOT NULL");
            }
            builder.Append(" ORDER BY replydate DESC");
            return builder.ToString();
        }

       static string BuildFavoriteQuery(int userId, string tags)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SELECT FavoriteId FROM Hishop_Favorite WHERE UserId = {0} ", userId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1) ", new object[0]);
            if (!string.IsNullOrEmpty(tags))
            {
                builder.AppendFormat(" AND (ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1 AND ProductName LIKE '%{0}%') ", DataHelper.CleanSearchString(tags));
                builder.AppendFormat(" OR Tags LIKE '%{0}%')", DataHelper.CleanSearchString(tags));
            }
            builder.AppendFormat(" ORDER BY FavoriteId DESC", new object[0]);
            return builder.ToString();
        }

       static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ProductId FROM Hishop_ProductReviews ");
            builder.AppendFormat(" WHERE UserId = {0} ", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products)", new object[0]);
            builder.Append(" GROUP BY ProductId");
            return builder.ToString();
        }

        public override int DeleteFavorite(int favoriteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE FavoriteId = @FavoriteId");
            database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteFavorites(string ids)
        {
            string query = "DELETE from Hishop_Favorite WHERE FavoriteId IN (" + ids + ")";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            try
            {
                database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override int DeleteReceiedMessages(IList<long> receivedMessageList)
        {
            string str = string.Empty;
            foreach (long num in receivedMessageList)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + num.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    str = str + "," + num.ToString(CultureInfo.InvariantCulture);
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("delete from Hishop_ReceivedMessages where ReceiveMessageId in ({0}) ", str));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteReceivedMessage(long receiveMessageId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("delete from Hishop_ReceivedMessages where ReceiveMessageId=@ReceiveMessageId");
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, receiveMessageId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteSendedMessage(long sendMessageId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("delete from Hishop_SendedMessages where SendMessageId=@SendMessageId");
            database.AddInParameter(sqlStringCommand, "SendMessageId", DbType.Int64, sendMessageId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeleteSendedMessages(IList<long> sendedMessageList)
        {
            string str = string.Empty;
            foreach (long num in sendedMessageList)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + num.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    str = str + "," + num.ToString(CultureInfo.InvariantCulture);
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("delete from Hishop_SendedMessages where SendMessageId in ({0}) ", str));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ExistsProduct(int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SaleStatus=1");
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND SKU LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            Member user = HiContext.Current.User as Member;
            int memberDiscount = GetMemberDiscount(user.GradeId);
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("SkuId, ProductId, SKU,ProductName, ThumbnailUrl40, DisplaySequence, Stock,");
            builder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            builder2.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0})", user.GradeId);
            builder2.AppendFormat(" ELSE SalePrice * {0} /100 END) AS SalePrice", memberDiscount);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_ProductSkuList s", "SkuId", builder.ToString(), builder2.ToString());
        }

        public override DbQueryResult GetFavorites(int userId, string tags, Pagination page)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = database.GetStoredProcCommand("ac_Member_Favorites_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, page.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, page.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, page.IsCount);
            Member user = HiContext.Current.User as Member;
            database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, user.GradeId);
            database.AddInParameter(storedProcCommand, "SqlPopulate", DbType.String, BuildFavoriteQuery(userId, tags));
            database.AddOutParameter(storedProcCommand, "TotalFavorites", DbType.Int32, 4);
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (page.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

       int GetMemberDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId=@GradeId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("ac_Member_ConsultationsAndReplys_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildConsultationAndReplyQuery(query));
            database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("ConsultationReplys", set.Tables[0].Columns["ConsultationId"], set.Tables[1].Columns["ConsultationId"], false);
            total = (int)database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override ProductInfo GetProductDetails(int productId)
        {
            ProductInfo info = new ProductInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId");
            database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetReceivedMessages(ReceivedMessageQuery query)
        {
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ReceivedMessages", "ReceiveMessageId", string.Format("Addressee='{0}'", query.UserName), "*");
        }

        public override ReceiveMessageInfo GetReceiveMessage(long receiveMessageId)
        {
            ReceiveMessageInfo info = new ReceiveMessageInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ReceivedMessages WHERE ReceiveMessageId=@ReceiveMessageId;");
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, receiveMessageId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateReceiveMessage(reader);
                }
            }
            return info;
        }

        public override SendMessageInfo GetSendedMessage(long sendMessageId)
        {
            SendMessageInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_SendedMessages WHERE SendMessageId=@SendMessageId");
            database.AddInParameter(sqlStringCommand, "SendMessageId", DbType.Int64, sendMessageId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateSendMessage(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetSendedMessages(SendedMessageQuery query)
        {
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SendedMessages", "SendMessageId", string.Format("Addresser='{0}'", query.UserName), "*");
        }

        public override DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("ac_Member_UserReviewsAndReplys_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserReviewsAndReplysQuery(query));
            database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("PtReviews", set.Tables[0].Columns["ProductId"], set.Tables[1].Columns["ProductId"], false);
            total = (int)database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override int GetUserProductReviewsCount()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(ReviewId) AS Count FROM Hishop_ProductReviews WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            int num = 0;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return num;
                }
                if (DBNull.Value != reader["Count"])
                {
                    num = (int)reader["Count"];
                }
            }
            return num;
        }

        public override bool InsertReceiveMessage(ReceiveMessageInfo receiveMessage)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Insert into Hishop_ReceivedMessages(Addresser,Addressee,Title,PublishContent,PublishDate,LastTime,IsRead) values(@Addresser,@Addressee,@Title,@PublishContent,@PublishDate,@LastTime,@IsRead)");
            database.AddInParameter(sqlStringCommand, "Addresser", DbType.String, receiveMessage.Addresser);
            database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, receiveMessage.Addressee);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, receiveMessage.Title);
            database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, receiveMessage.PublishContent);
            database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "LastTime", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "IsRead", DbType.Boolean, false);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool InsertSendMessage(SendMessageInfo message)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Insert into Hishop_SendedMessages(Addresser,Addressee,Title,PublishContent,PublishDate,ReceiveMessageId) values(@Addresser,@Addressee,@Title,@PublishContent,@PublishDate,@ReceiveMessageId)");
            database.AddInParameter(sqlStringCommand, "Addresser", DbType.String, message.Addresser);
            database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, message.Addressee);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, message.Title);
            database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, message.PublishContent);
            database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DateTime.Now);
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, message.ReceiveMessageId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool PostMessageIsRead(long receiveMessageId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_ReceivedMessages set IsRead=1 where ReceiveMessageId=@ReceiveMessageId");
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, receiveMessageId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool PostMessageLastTime(long receiveMessageId, DateTime newDate)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_ReceivedMessages set LastTime=@LastTime where ReceiveMessageId=@ReceiveMessageId");
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, receiveMessageId);
            database.AddInParameter(sqlStringCommand, "LastTime", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(newDate));
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
            database.AddInParameter(sqlStringCommand, "Tags", DbType.String, tags);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

