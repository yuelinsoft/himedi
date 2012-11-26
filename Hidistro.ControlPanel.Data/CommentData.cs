using Hidistro.ControlPanel.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.ControlPanel.Data
{
    public class CommentData : CommentsProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddAffiche(AfficheInfo affiche)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_Affiche(Title, Content, AddedDate) VALUES (@Title, @Content, @AddedDate)");
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddArticle(ArticleInfo article)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_Articles(CategoryId, Title, Meta_Description, Meta_Keywords, IconUrl, Description, Content, AddedDate,IsRelease) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords,  @IconUrl, @Description, @Content, @AddedDate,@IsRelease)");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
            database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddHelp(HelpInfo help)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_Helps(CategoryId, Title, Meta_Description, Meta_Keywords, Description, Content, AddedDate, IsShowFooter) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords, @Description, @Content, @AddedDate, @IsShowFooter)");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
            database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddReleatesProdcutByArticId(int articId, int prodcutId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_RelatedArticsProducts(ArticleId, RelatedProductId) VALUES (@ArticleId, @RelatedProductId)");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
            try
            {
                return (database.ExecuteNonQuery(sqlStringCommand) > 0);
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
                builder.Append(" AND ReplyUserId IS NULL");
            }
            else if (query.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND ReplyUserId IS NOT NULL");
            }
            builder.Append(" ORDER BY replydate DESC");
            return builder.ToString();
        }

        static string BuildLeaveCommentQuery(LeaveCommentQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT l.LeaveId FROM Hishop_LeaveComments l where 0=0");
            if (query.MessageStatus == MessageStatus.Replied)
            {
                builder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) >0 ");
            }
            if (query.MessageStatus == MessageStatus.NoReply)
            {
                builder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) <=0 ");
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            else
            {
                builder.Append(" ORDER BY LastDate desc");
            }
            return builder.ToString();
        }

        static string BuildProductConsultationQuery(ProductConsultationAndReplyQuery consultationQuery)
        {
            HiContext current = HiContext.Current;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT c.ConsultationId FROM Hishop_Products p inner join Hishop_ProductConsultations c on p.productId=c.ProductId WHERE 0 = 0");
            if (consultationQuery.Type == ConsultationReplyType.NoReply)
            {
                builder.Append(" AND c.ReplyUserId IS NULL ");
            }
            else if (consultationQuery.Type == ConsultationReplyType.Replyed)
            {
                builder.Append(" AND c.ReplyUserId IS NOT NULL");
            }
            if (consultationQuery.ProductId > 0)
            {
                builder.AppendFormat(" AND p.ProductId = {0}", consultationQuery.ProductId);
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
            {
                builder.AppendFormat(" AND p.ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
            }
            if (!string.IsNullOrEmpty(consultationQuery.Keywords))
            {
                builder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
            }
            if (consultationQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}", consultationQuery.CategoryId.Value);
                builder.AppendFormat(" OR p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(consultationQuery.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(consultationQuery.SortBy), consultationQuery.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildReviewsQuery(ProductReviewQuery reviewQuery)
        {
            HiContext current = HiContext.Current;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT r.ReviewId FROM Hishop_Products p inner join Hishop_ProductReviews r on r.productId=p.ProductId WHERE 0 = 0");
            if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
            }
            if (!string.IsNullOrEmpty(reviewQuery.Keywords))
            {
                builder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
            }
            if (reviewQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}", reviewQuery.CategoryId.Value);
                builder.AppendFormat(" OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(reviewQuery.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(reviewQuery.SortBy), reviewQuery.SortOrder.ToString());
            }
            return builder.ToString();
        }

        static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT ProductId FROM Hishop_ProductReviews ");
            builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products)", new object[0]);
            builder.Append(" GROUP BY ProductId");
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action)
        {
            if (null == articleCategory)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ArticleCategory_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, articleCategory.CategoryId);
            }
            if (action != DataProviderAction.Delete)
            {
                database.AddInParameter(storedProcCommand, "Name", DbType.String, articleCategory.Name);
                database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, articleCategory.IconUrl);
                database.AddInParameter(storedProcCommand, "Description", DbType.String, articleCategory.Description);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action)
        {
            if (null == helpCategory)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_HelpCategory_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, helpCategory.CategoryId);
            }
            if (action != DataProviderAction.Delete)
            {
                database.AddInParameter(storedProcCommand, "Name", DbType.String, helpCategory.Name);
                database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, helpCategory.IconUrl);
                database.AddInParameter(storedProcCommand, "IndexChar", DbType.String, helpCategory.IndexChar);
                database.AddInParameter(storedProcCommand, "Description", DbType.String, helpCategory.Description);
                database.AddInParameter(storedProcCommand, "IsShowFooter", DbType.Boolean, helpCategory.IsShowFooter);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool DeleteAffiche(int afficheId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
            database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteAffiches(List<int> afficheIds)
        {
            int num = 0;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId=@AfficheId");
            database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32);
            foreach (int num2 in afficheIds)
            {
                database.SetParameterValue(sqlStringCommand, "AfficheId", num2);
                database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteArticle(int articleId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteArticles(IList<int> articles)
        {
            int num = 0;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32);
            foreach (int num2 in articles)
            {
                database.SetParameterValue(sqlStringCommand, "ArticleId", num2);
                database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteHelp(int helpId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId = @HelpId");
            database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteHelpCategorys(List<int> categoryIds)
        {
            if (null == categoryIds)
            {
                return 0;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32);
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (int num2 in categoryIds)
            {
                database.SetParameterValue(sqlStringCommand, "CategoryId", num2);
                database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override int DeleteHelps(IList<int> helps)
        {
            int num = 0;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId=@HelpId");
            database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32);
            foreach (int num2 in helps)
            {
                database.SetParameterValue(sqlStringCommand, "HelpId", num2);
                database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }

        public override bool DeleteLeaveComment(long leaveId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId;DELETE FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId");
            database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE replyId=@replyId;");
            database.AddInParameter(sqlStringCommand, "replyId", DbType.Int64, leaveReplyId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeleteLeaveComments(IList<long> leaveIds)
        {
            string str = string.Empty;
            foreach (long num in leaveIds)
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId in ({0});DELETE FROM Hishop_LeaveComments WHERE LeaveId in ({1}) ", str, str));
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProductConsultation(int consultationId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ProductConsultations WHERE consultationId = @consultationId");
            database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int64, consultationId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProductReview(long reviewId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ProductReviews WHERE ReviewId = @ReviewId");
            database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int64, reviewId);
            return database.ExecuteNonQuery(sqlStringCommand);
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

        public override int DeleteReview(IList<int> reviews)
        {
            string str = string.Empty;
            foreach (long num in reviews)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = num.ToString();
                }
                else
                {
                    str = str + "," + num.ToString();
                }
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_ProductReviews WHERE ReviewId in ({0})", str));
            return database.ExecuteNonQuery(sqlStringCommand);
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

        public override AfficheInfo GetAffiche(int afficheId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
            database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
            AfficheInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAffiche(reader);
                }
            }
            return info;
        }

        public override List<AfficheInfo> GetAfficheList()
        {
            List<AfficheInfo> list = new List<AfficheInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche ORDER BY AddedDate DESC");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    AfficheInfo item = DataMapper.PopulateAffiche(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override ArticleInfo GetArticle(int articleId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Articles WHERE ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            ArticleInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateArticle(reader);
                }
            }
            return info;
        }

        public override ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories WHERE CategoryId = @CategoryId ORDER BY [DisplaySequence]");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    return DataMapper.PopulateArticleCategory(reader);
                }
                return null;
            }
        }

        public override DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(articleQuery.Keywords));
            if (articleQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND CategoryId = {0}", articleQuery.CategoryId.Value);
            }
            if (articleQuery.StartArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >= '{0}'", articleQuery.StartArticleTime.Value);
            }
            if (articleQuery.EndArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <= '{0}'", articleQuery.EndArticleTime.Value);
            }
            return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_Hishop_Articles", "ArticleId", builder.ToString(), "*");
        }

        public override DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductConsultation_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, consultationQuery.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, consultationQuery.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, consultationQuery.IsCount);
            if (consultationQuery.CategoryId.HasValue)
            {
                database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, consultationQuery.CategoryId.Value);
            }
            database.AddInParameter(storedProcCommand, "SqlPopulate", DbType.String, BuildProductConsultationQuery(consultationQuery));
            using (IDataReader reader = database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (consultationQuery.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override DbQueryResult GetDistributorReceivedMessages(ReceivedMessageQuery query)
        {
            string filter = string.Format("Addressee='{0}'", query.UserName);
            if (query.MessageStatus == MessageStatus.NoReply)
            {
                filter = filter + " AND ReceiveMessageId NOT IN(SELECT ISNULL(ReceiveMessageId,-1) FROM Hishop_SendedMessages)";
            }
            if (query.MessageStatus == MessageStatus.Replied)
            {
                filter = filter + " AND ReceiveMessageId IN(SELECT ReceiveMessageId FROM Hishop_SendedMessages)";
            }
            if (query.IsRead.HasValue)
            {
                filter = filter + string.Format("AND IsRead = {0} ", query.IsRead.Value ? "1" : "0");
            }
            filter = filter + " AND Addresser IN (SELECT UserName FROM vw_aspnet_Distributors)";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ReceivedMessages", "ReceiveMessageId", filter, "*");
        }

        public override IList<Distributor> GetDistributorsByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Distributor> list = new List<Distributor>();
            if (gradeId > 0)
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors WHERE GradeId=@GradeId");
                database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            }
            else
            {
                sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors");
            }
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Distributor item = new Distributor();
                    item.UserId = (int)reader["UserId"];
                    item.Email = reader["Email"].ToString();
                    item.Username = reader["UserName"].ToString();
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetDistributorSendedMessages(SendedMessageQuery query)
        {
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SendedMessages", "SendMessageId", string.Format("Addresser='{0}' AND Addressee IN (SELECT UserName FROM vw_aspnet_Distributors)", query.UserName), "*");
        }

        public override HelpInfo GetHelp(int helpId)
        {
            HelpInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_Helps WHERE HelpId=@HelpId");
            database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateHelp(reader);
                }
            }
            return info;
        }

        public override HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            HelpCategoryInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateHelpCategory(reader);
                }
            }
            return info;
        }

        public override IList<HelpCategoryInfo> GetHelpCategorys()
        {
            IList<HelpCategoryInfo> list = new List<HelpCategoryInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories ORDER BY DisplaySequence");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateHelpCategory(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(helpQuery.Keywords));
            if (helpQuery.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND CategoryId = {0}", helpQuery.CategoryId.Value);
            }
            if (helpQuery.StartArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >= '{0}'", helpQuery.StartArticleTime.Value);
            }
            if (helpQuery.EndArticleTime.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <= '{0}'", helpQuery.EndArticleTime.Value);
            }
            return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_Hishop_Helps", "HelpId", builder.ToString(), "*");
        }

        public override LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            LeaveCommentInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId;");
            database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateLeaveComment(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_LeaveComments_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildLeaveCommentQuery(query));
            database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            DataSet set = database.ExecuteDataSet(storedProcCommand);
            set.Relations.Add("LeaveCommentReplays", set.Tables[0].Columns["LeaveId"], set.Tables[1].Columns["LeaveId"], false);
            result.Data = set;
            result.TotalRecords = (int)database.GetParameterValue(storedProcCommand, "Total");
            return result;
        }

        public override IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories ORDER BY [DisplaySequence]");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ArticleCategoryInfo item = DataMapper.PopulateArticleCategory(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            ProductConsultationInfo info = null;
            string query = "SELECT * FROM Hishop_ProductConsultations WHERE ConsultationId=@ConsultationId";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, consultationId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductConsultation(reader);
                }
            }
            return info;
        }

        public override ProductReviewInfo GetProductReview(int reviewId)
        {
            ProductReviewInfo info = new ProductReviewInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_ProductReviews WHERE ReviewId=@ReviewId");
            database.AddInParameter(sqlStringCommand, "ReviewId", DbType.Int32, reviewId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductReview(reader);
                }
            }
            return info;
        }

        public override DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_ProductReviews_Get");
            database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, reviewQuery.PageIndex);
            database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, reviewQuery.PageSize);
            database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, reviewQuery.IsCount);
            database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildReviewsQuery(reviewQuery));
            database.AddOutParameter(storedProcCommand, "Total", DbType.Int32, 4);
            if (reviewQuery.CategoryId.HasValue)
            {
                database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, reviewQuery.CategoryId.Value);
            }
            DataSet set = database.ExecuteDataSet(storedProcCommand);
            total = (int)database.GetParameterValue(storedProcCommand, "Total");
            return set;
        }

        public override DbQueryResult GetReceivedMessages(ReceivedMessageQuery query)
        {
            string filter = string.Format("Addressee='{0}'", query.UserName);
            if (query.MessageStatus == MessageStatus.NoReply)
            {
                filter = filter + " AND ReceiveMessageId NOT IN(SELECT ISNULL(ReceiveMessageId,-1) FROM Hishop_SendedMessages)";
            }
            if (query.MessageStatus == MessageStatus.Replied)
            {
                filter = filter + " AND ReceiveMessageId IN(SELECT ReceiveMessageId FROM Hishop_SendedMessages)";
            }
            if (query.IsRead.HasValue)
            {
                filter = filter + string.Format("AND IsRead = {0} ", query.IsRead.Value ? "1" : "0");
            }
            filter = filter + " AND Addresser IN (SELECT UserName FROM  vw_aspnet_Members)";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ReceivedMessages", "ReceiveMessageId", filter, "*");
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

        public override DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0}", 1);
            builder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {0})", articId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataTable GetReplyLeaveComments(long leaveId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId");
            database.AddInParameter(sqlStringCommand, "LeaveId", DbType.Int64, leaveId);
            DataTable table = new DataTable();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetSendedMessages(SendedMessageQuery query)
        {
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SendedMessages", "SendMessageId", string.Format("Addresser='{0}' AND Addressee IN (SELECT UserName FROM  vw_aspnet_Members) ", query.UserName), "*");
        }

        public override DbQueryResult GetSendedMessagesForReceivedMessage(long receiveMessageId)
        {
            DataTable table;
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * from  Hishop_SendedMessages WHERE ReceiveMessageId=@ReceiveMessageId ORDER BY PublishDate DESC");
            database.AddInParameter(sqlStringCommand, "ReceiveMessageId", DbType.Int64, receiveMessageId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            result.Data = table;
            result.TotalRecords = table.Rows.Count;
            return result;
        }

        public override bool InsertReceiveMessage(ReceiveMessageInfo receiveMessage)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Insert into Hishop_ReceivedMessages(Addresser,Addressee,Title,PublishContent,PublishDate,LastTime,IsRead) values(@Addresser,@Addressee,@Title,@PublishContent,@PublishDate,@LastTime,@IsRead)");
            database.AddInParameter(sqlStringCommand, "Addresser", DbType.String, receiveMessage.Addresser);
            database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, receiveMessage.Addressee);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, receiveMessage.Title);
            database.AddInParameter(sqlStringCommand, "PublishContent", DbType.String, receiveMessage.PublishContent);
            database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
            database.AddInParameter(sqlStringCommand, "LastTime", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
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
            database.AddInParameter(sqlStringCommand, "PublishDate", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
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

        public override bool RemoveReleatesProductByArticId(int articId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveReleatesProductByArticId(int articId, int productId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId AND RelatedProductId = @RelatedProductId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_LeaveCommentReplys(LeaveId,UserId,ReplyContent,ReplyDate) VALUES(@LeaveId,@UserId,@ReplyContent,@ReplyDate);SELECT @@IDENTITY ");
            database.AddInParameter(sqlStringCommand, "leaveId", DbType.Int64, leaveReply.LeaveId);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, leaveReply.UserId);
            database.AddInParameter(sqlStringCommand, "ReplyContent", DbType.String, leaveReply.ReplyContent);
            database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.String, DataHelper.GetSafeDateTimeFormat(leaveReply.ReplyDate));
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
            database.AddInParameter(sqlStringCommand, "ReplyText", DbType.String, productConsultation.ReplyText);
            database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.DateTime, productConsultation.ReplyDate);
            database.AddInParameter(sqlStringCommand, "ReplyUserId", DbType.Int32, productConsultation.ReplyUserId);
            database.AddInParameter(sqlStringCommand, "ConsultationId", DbType.Int32, productConsultation.ConsultationId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_HelpCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public override bool UpdateAffiche(AfficheInfo affiche)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Affiche SET Title = @Title, AddedDate = @AddedDate, Content = @Content WHERE AfficheId = @AfficheId");
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, affiche.Title);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, affiche.Content);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, affiche.AddedDate);
            database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, affiche.AfficheId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateArticle(ArticleInfo article)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Articles SET CategoryId = @CategoryId,AddedDate = @AddedDate,Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, IconUrl=@IconUrl,Description = @Description,Content = @Content,IsRelease=@IsRelease WHERE ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateHelp(HelpInfo help)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Helps SET CategoryId = @CategoryId, AddedDate = @AddedDate, Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  Description = @Description, Content = @Content, IsShowFooter = @IsShowFooter WHERE HelpId = @HelpId");
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, help.CategoryId);
            database.AddInParameter(sqlStringCommand, "Title", DbType.String, help.Title);
            database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, help.MetaDescription);
            database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, help.MetaKeywords);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, help.Description);
            database.AddInParameter(sqlStringCommand, "Content", DbType.String, help.Content);
            database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, help.AddedDate);
            database.AddInParameter(sqlStringCommand, "IsShowFooter", DbType.Boolean, help.IsShowFooter);
            database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, help.HelpId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateRelease(int articId, bool release)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("update Hishop_Articles set IsRelease=@IsRelease  where ArticleId = @ArticleId");
            database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
            database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, release);
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

