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
    public abstract class CommentDataProvider
    {
        protected CommentDataProvider()
        {
        }

        public static CommentDataProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return CommentSubsiteDataProvider.CreateInstance();
            }
            return CommentMasterDataProvider.CreateInstance();
        }

        public abstract bool AddProductToFavorite(int productId);
        public abstract int DeleteFavorite(int favoriteId);
        public abstract bool DeleteFavorites(string ids);
        public abstract int DeleteReceiedMessages(IList<long> receivedMessageList);
        public abstract bool DeleteReceivedMessage(long receiveMessageId);
        public abstract bool DeleteSendedMessage(long sendMessageId);
        public abstract int DeleteSendedMessages(IList<long> sendedMessageList);
        public abstract bool ExistsProduct(int productId);
        public abstract DbQueryResult GetBatchBuyProducts(ProductQuery query);
        public abstract DbQueryResult GetFavorites(int userId, string tags, Pagination page);
        public abstract DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total);
        public abstract ProductInfo GetProductDetails(int productId);
        public abstract DbQueryResult GetReceivedMessages(ReceivedMessageQuery query);
        public abstract ReceiveMessageInfo GetReceiveMessage(long receiveMessageId);
        public abstract SendMessageInfo GetSendedMessage(long sendMessageId);
        public abstract DbQueryResult GetSendedMessages(SendedMessageQuery query);
        public abstract DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total);
        public abstract int GetUserProductReviewsCount();
        public abstract bool InsertReceiveMessage(ReceiveMessageInfo receiveMessage);
        public abstract bool InsertSendMessage(SendMessageInfo sendMessage);
        public abstract bool PostMessageIsRead(long receiveMessageId);
        public abstract bool PostMessageLastTime(long receiveMessageId, DateTime newdate);
        public abstract int UpdateFavorite(int favoriteId, string tags, string remark);
    }
}

