using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;

namespace Hidistro.Subsites.Comments
{
    public static class SubsiteCommentsHelper
    {
        public static bool CreateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return SubsiteCommentsProvider.Instance().AddAffiche(affiche);
        }

        public static bool CreateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return SubsiteCommentsProvider.Instance().AddArticle(article);
        }

        public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
        }

        public static bool CreateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return SubsiteCommentsProvider.Instance().AddHelp(help);
        }

        public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
        }

        public static bool DeleteAffiche(int afficheId)
        {
            return SubsiteCommentsProvider.Instance().DeleteAffiche(afficheId);
        }

        public static int DeleteAffiches(List<int> affiches)
        {
            if ((affiches == null) || (affiches.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteAffiches(affiches);
        }

        public static bool DeleteArticle(int articleId)
        {
            return SubsiteCommentsProvider.Instance().DeleteArticle(articleId);
        }

        public static bool DeleteArticleCategory(int categoryId)
        {
            ArticleCategoryInfo articleCategory = new ArticleCategoryInfo();
            articleCategory.CategoryId = categoryId;
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Delete);
        }

        public static int DeleteArticles(IList<int> articles)
        {
            if ((articles == null) || (articles.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteArticles(articles);
        }

        public static bool DeleteHelp(int helpId)
        {
            return SubsiteCommentsProvider.Instance().DeleteHelp(helpId);
        }

        public static bool DeleteHelpCategory(int categoryId)
        {
            HelpCategoryInfo helpCategory = new HelpCategoryInfo();
            helpCategory.CategoryId = new int?(categoryId);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Delete);
        }

        public static int DeleteHelps(IList<int> helps)
        {
            if ((helps == null) || (helps.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteHelps(helps);
        }

        public static bool DeleteLeaveComment(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveComment(leaveId);
        }

        public static bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveCommentReply(leaveReplyId);
        }

        public static int DeleteLeaveComments(IList<long> leaveIds)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveComments(leaveIds);
        }

        public static int DeleteProductConsultation(int consultationId)
        {
            return SubsiteCommentsProvider.Instance().DeleteProductConsultation(consultationId);
        }

        public static int DeleteProductReview(long reviewId)
        {
            return SubsiteCommentsProvider.Instance().DeleteProductReview(reviewId);
        }

        public static int DeleteReceiedMessages(IList<long> receivedMessageList)
        {
            return SubsiteCommentsProvider.Instance().DeleteReceiedMessages(receivedMessageList);
        }

        public static int DeleteReceiedMessagesToAdmin(IList<long> receivedMessageList)
        {
            return SubsiteCommentsProvider.Instance().DeleteReceiedMessagesToAdmin(receivedMessageList);
        }

        public static bool DeleteReceiedMessageToAdmin(long receivedMessageId)
        {
            return SubsiteCommentsProvider.Instance().DeleteReceiedMessageToAdmin(receivedMessageId);
        }

        public static bool DeleteReceivedMessage(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().DeleteReceivedMessage(receiveMessageId);
        }

        public static int DeleteReview(IList<int> reviews)
        {
            if ((reviews == null) || (reviews.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteReview(reviews);
        }

        public static bool DeleteSendedMessage(long sendMessageId)
        {
            return SubsiteCommentsProvider.Instance().DeleteSendedMessage(sendMessageId);
        }

        public static int DeleteSendedMessages(IList<long> sendedMessageList)
        {
            return SubsiteCommentsProvider.Instance().DeleteSendedMessages(sendedMessageList);
        }

        public static int DeleteSendedMessagesToAdmin(IList<long> sendedMessageList)
        {
            return SubsiteCommentsProvider.Instance().DeleteSendedMessagesToAdmin(sendedMessageList);
        }

        public static bool DeleteSendedMessageToAdmin(long sendMessageId)
        {
            return SubsiteCommentsProvider.Instance().DeleteSendedMessageToAdmin(sendMessageId);
        }

        public static AfficheInfo GetAffiche(int afficheId)
        {
            return SubsiteCommentsProvider.Instance().GetAffiche(afficheId);
        }

        public static List<AfficheInfo> GetAfficheList()
        {
            return SubsiteCommentsProvider.Instance().GetAfficheList();
        }

        public static ArticleInfo GetArticle(int articleId)
        {
            return SubsiteCommentsProvider.Instance().GetArticle(articleId);
        }

        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return SubsiteCommentsProvider.Instance().GetArticleCategory(categoryId);
        }

        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return SubsiteCommentsProvider.Instance().GetArticleList(articleQuery);
        }

        public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            return SubsiteCommentsProvider.Instance().GetConsultationProducts(consultationQuery);
        }

        public static HelpInfo GetHelp(int helpId)
        {
            return SubsiteCommentsProvider.Instance().GetHelp(helpId);
        }

        public static HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            return SubsiteCommentsProvider.Instance().GetHelpCategory(categoryId);
        }

        public static IList<HelpCategoryInfo> GetHelpCategorys()
        {
            return SubsiteCommentsProvider.Instance().GetHelpCategorys();
        }

        public static DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            return SubsiteCommentsProvider.Instance().GetHelpList(helpQuery);
        }

        public static int GetIsReadMessageToAdmin()
        {
            return SubsiteCommentsProvider.Instance().GetIsReadMessageToAdmin();
        }

        public static LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().GetLeaveComment(leaveId);
        }

        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetLeaveComments(query);
        }

        public static IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            return SubsiteCommentsProvider.Instance().GetMainArticleCategories();
        }

        public static ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            return SubsiteCommentsProvider.Instance().GetProductConsultation(consultationId);
        }

        public static ProductReviewInfo GetProductReview(int reviewId)
        {
            return SubsiteCommentsProvider.Instance().GetProductReview(reviewId);
        }

        public static DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
        {
            return SubsiteCommentsProvider.Instance().GetProductReviews(out total, reviewQuery);
        }

        public static DbQueryResult GetReceivedMessages(ReceivedMessageQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetReceivedMessages(query);
        }

        public static DbQueryResult GetReceivedMessagesToAdmin(ReceivedMessageQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetReceivedMessagesToAdmin(query);
        }

        public static ReceiveMessageInfo GetReceivedMessageToAdminInfo(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().GetReceivedMessageToAdminInfo(receiveMessageId);
        }

        public static ReceiveMessageInfo GetReceiveMessage(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().GetReceiveMessage(receiveMessageId);
        }

        public static DataTable GetReplyLeaveComments(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().GetReplyLeaveComments(leaveId);
        }

        public static DbQueryResult GetSendedMessages(SendedMessageQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetSendedMessages(query);
        }

        public static DbQueryResult GetSendedMessagesForReceivedMessage(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().GetSendedMessagesForReceivedMessage(receiveMessageId);
        }

        public static DbQueryResult GetSendedMessagesForReceivedMessageToAdmin(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().GetSendedMessagesForReceivedMessageToAdmin(receiveMessageId);
        }

        public static DbQueryResult GetSendedMessagesToAdmin(SendedMessageQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetSendedMessagesToAdmin(query);
        }

        public static bool InsertSendMessageToAdmin(SendMessageInfo message, ReceiveMessageInfo receiveMessage)
        {
            return (SubsiteCommentsProvider.Instance().InsertSendMessageToAdmin(message) && SubsiteCommentsProvider.Instance().InsertReceiveMessageToAdmin(receiveMessage));
        }

        public static bool PostMessageIsRead(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().PostMessageIsRead(receiveMessageId);
        }

        public static bool PostMessageToAdminIsRead(long receiveMessageId)
        {
            return SubsiteCommentsProvider.Instance().PostMessageToAdminIsRead(receiveMessageId);
        }

        public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            leaveReply.ReplyDate = DateTime.Now;
            return SubsiteCommentsProvider.Instance().ReplyLeaveComment(leaveReply);
        }

        public static bool ReplyMessage(SendMessageInfo reply)
        {
            IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
            IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
            ReceiveMessageInfo item = new ReceiveMessageInfo();
            item.Addressee = reply.Addressee;
            item.Addresser = reply.Addresser;
            item.Title = reply.Title;
            item.PublishContent = reply.PublishContent;
            item.PublishDate = DateTime.Now;
            sendMessageList.Add(reply);
            receiveMessageList.Add(item);
            if (SendMessage(sendMessageList, receiveMessageList) > 0)
            {
                SubsiteCommentsProvider.Instance().PostMessageLastTime(reply.ReceiveMessageId.Value, item.PublishDate);
            }
            return true;
        }

        public static bool ReplyMessageToAdmin(SendMessageInfo reply)
        {
            IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
            IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
            ReceiveMessageInfo item = new ReceiveMessageInfo();
            item.Addressee = reply.Addressee;
            item.Addresser = reply.Addresser;
            item.Title = reply.Title;
            item.PublishContent = reply.PublishContent;
            item.PublishDate = DateTime.Now;
            sendMessageList.Add(reply);
            receiveMessageList.Add(item);
            if (SendMessageToAdmin(sendMessageList, receiveMessageList) > 0)
            {
                SubsiteCommentsProvider.Instance().PostMessageToAdminLastTime(reply.ReceiveMessageId.Value, item.PublishDate);
            }
            return true;
        }

        public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            return SubsiteCommentsProvider.Instance().ReplyProductConsultation(productConsultation);
        }

        public static int SendMessage(IList<SendMessageInfo> sendMessageList, IList<ReceiveMessageInfo> receiveMessageList)
        {
            int num = 0;
            foreach (SendMessageInfo info in sendMessageList)
            {
                Globals.EntityCoding(info, true);
                SubsiteCommentsProvider.Instance().InsertSendMessage(info);
                num++;
            }
            foreach (ReceiveMessageInfo info2 in receiveMessageList)
            {
                Globals.EntityCoding(info2, true);
                SubsiteCommentsProvider.Instance().InsertReceiveMessage(info2);
                num++;
            }
            return num;
        }

        public static int SendMessageToAdmin(IList<SendMessageInfo> sendMessageList, IList<ReceiveMessageInfo> receiveMessageList)
        {
            int num = 0;
            foreach (SendMessageInfo info in sendMessageList)
            {
                Globals.EntityCoding(info, true);
                SubsiteCommentsProvider.Instance().InsertSendMessageToAdmin(info);
                num++;
            }
            foreach (ReceiveMessageInfo info2 in receiveMessageList)
            {
                Globals.EntityCoding(info2, true);
                SubsiteCommentsProvider.Instance().InsertReceiveMessageToAdmin(info2);
                num++;
            }
            return num;
        }

        public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteCommentsProvider.Instance().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteCommentsProvider.Instance().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return SubsiteCommentsProvider.Instance().UpdateAffiche(affiche);
        }

        public static bool UpdateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return SubsiteCommentsProvider.Instance().UpdateArticle(article);
        }

        public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
        }

        public static bool UpdateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return SubsiteCommentsProvider.Instance().UpdateHelp(help);
        }

        public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
        }

        public static bool UpdateMyArticRelease(int articlId, bool isrealse)
        {
            return SubsiteCommentsProvider.Instance().UpdateMyArticRelease(articlId, isrealse);
        }

        public static string UploadArticleImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadHelpImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

