using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace Hidistro.AccountCenter.Comments
{
    public static class CommentsHelper
    {
        public static bool AddProductToFavorite(int productId)
        {
            return CommentDataProvider.Instance().AddProductToFavorite(productId);
        }

        public static int DeleteFavorite(int favoriteId)
        {
            return CommentDataProvider.Instance().DeleteFavorite(favoriteId);
        }

        public static bool DeleteFavorites(string ids)
        {
            return CommentDataProvider.Instance().DeleteFavorites(ids);
        }

        public static int DeleteReceiedMessages(IList<long> receivedMessageList)
        {
            return CommentDataProvider.Instance().DeleteReceiedMessages(receivedMessageList);
        }

        public static bool DeleteReceivedMessage(long receiveMessageId)
        {
            return CommentDataProvider.Instance().DeleteReceivedMessage(receiveMessageId);
        }

        public static bool DeleteSendedMessage(long sendMessageId)
        {
            return CommentDataProvider.Instance().DeleteSendedMessage(sendMessageId);
        }

        public static int DeleteSendedMessages(IList<long> sendedMessageList)
        {
            return CommentDataProvider.Instance().DeleteSendedMessages(sendedMessageList);
        }

        public static bool ExistsProduct(int productId)
        {
            return CommentDataProvider.Instance().ExistsProduct(productId);
        }

        public static DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            return CommentDataProvider.Instance().GetBatchBuyProducts(query);
        }

        public static DbQueryResult GetFavorites(string tags, Pagination page)
        {
            return CommentDataProvider.Instance().GetFavorites(HiContext.Current.User.UserId, tags, page);
        }

        public static DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
        {
            return CommentDataProvider.Instance().GetProductConsultationsAndReplys(query, out total);
        }

        public static ProductInfo GetProductDetails(int productId)
        {
            return CommentDataProvider.Instance().GetProductDetails(productId);
        }

        public static DbQueryResult GetReceivedMessages(ReceivedMessageQuery query)
        {
            return CommentDataProvider.Instance().GetReceivedMessages(query);
        }

        public static ReceiveMessageInfo GetReceiveMessage(long receiveMessageId)
        {
            return CommentDataProvider.Instance().GetReceiveMessage(receiveMessageId);
        }

        public static SendMessageInfo GetSendedMessage(long sendMessageId)
        {
            return CommentDataProvider.Instance().GetSendedMessage(sendMessageId);
        }

        public static DbQueryResult GetSendedMessages(SendedMessageQuery query)
        {
            return CommentDataProvider.Instance().GetSendedMessages(query);
        }

        public static DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
        {
            return CommentDataProvider.Instance().GetUserProductReviewsAndReplys(query, out total);
        }

        public static int GetUserProductReviewsCount()
        {
            return CommentDataProvider.Instance().GetUserProductReviewsCount();
        }

        public static bool PostMessageIsRead(long receiveMessageId)
        {
            return CommentDataProvider.Instance().PostMessageIsRead(receiveMessageId);
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
                CommentDataProvider.Instance().PostMessageLastTime(reply.ReceiveMessageId.Value, item.PublishDate);
            }
            return true;
        }

        public static int SendMessage(IList<SendMessageInfo> sendMessageList, IList<ReceiveMessageInfo> receiveMessageList)
        {
            int num = 0;
            foreach (SendMessageInfo info in sendMessageList)
            {
                Globals.EntityCoding(info, true);
                CommentDataProvider.Instance().InsertSendMessage(info);
                num++;
            }
            foreach (ReceiveMessageInfo info2 in receiveMessageList)
            {
                Globals.EntityCoding(info2, true);
                CommentDataProvider.Instance().InsertReceiveMessage(info2);
                num++;
            }
            return num;
        }

        public static int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            return CommentDataProvider.Instance().UpdateFavorite(favoriteId, tags, remark);
        }
    }
}

