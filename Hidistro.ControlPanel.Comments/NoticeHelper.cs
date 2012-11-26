using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.Comments
{
    public sealed class NoticeHelper
    {
        NoticeHelper()
        {
        }

        public static bool CreateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return CommentsProvider.Instance().AddAffiche(affiche);
        }

        public static bool DeleteAffiche(int afficheId)
        {
            return CommentsProvider.Instance().DeleteAffiche(afficheId);
        }

        public static int DeleteAffiches(List<int> affiches)
        {
            if ((affiches == null) || (affiches.Count == 0))
            {
                return 0;
            }
            return CommentsProvider.Instance().DeleteAffiches(affiches);
        }

        public static bool DeleteLeaveComment(long leaveId)
        {
            return CommentsProvider.Instance().DeleteLeaveComment(leaveId);
        }

        public static bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            return CommentsProvider.Instance().DeleteLeaveCommentReply(leaveReplyId);
        }

        public static int DeleteLeaveComments(IList<long> leaveIds)
        {
            return CommentsProvider.Instance().DeleteLeaveComments(leaveIds);
        }

        public static int DeleteReceiedMessages(IList<long> receivedMessageList)
        {
            return CommentsProvider.Instance().DeleteReceiedMessages(receivedMessageList);
        }

        public static bool DeleteReceivedMessage(long receiveMessageId)
        {
            return CommentsProvider.Instance().DeleteReceivedMessage(receiveMessageId);
        }

        public static bool DeleteSendedMessage(long sendMessageId)
        {
            return CommentsProvider.Instance().DeleteSendedMessage(sendMessageId);
        }

        public static int DeleteSendedMessages(IList<long> sendedMessageList)
        {
            return CommentsProvider.Instance().DeleteSendedMessages(sendedMessageList);
        }

        public static AfficheInfo GetAffiche(int afficheId)
        {
            return CommentsProvider.Instance().GetAffiche(afficheId);
        }

        public static List<AfficheInfo> GetAfficheList()
        {
            return CommentsProvider.Instance().GetAfficheList();
        }

        public static DbQueryResult GetDistributorReceivedMessages(ReceivedMessageQuery query)
        {
            return CommentsProvider.Instance().GetDistributorReceivedMessages(query);
        }

        public static IList<Distributor> GetDistributorsByNames(IList<int> userids)
        {
            IList<Distributor> list = new List<Distributor>();
            foreach (int num in userids)
            {
                IUser user = Users.GetUser(num, null, false, false);
                if ((user != null) && (user.UserRole == UserRole.Distributor))
                {
                    list.Add(user as Distributor);
                }
            }
            return list;
        }

        public static IList<Distributor> GetDistributorsByNames(IList<string> names)
        {
            IList<Distributor> list = new List<Distributor>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Distributor))
                {
                    list.Add(user as Distributor);
                }
            }
            return list;
        }

        public static IList<Distributor> GetDistributorsByRank(int? gradeId)
        {
            return CommentsProvider.Instance().GetDistributorsByRank(gradeId);
        }

        public static DbQueryResult GetDistributorSendedMessages(SendedMessageQuery query)
        {
            return CommentsProvider.Instance().GetDistributorSendedMessages(query);
        }

        public static LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            return CommentsProvider.Instance().GetLeaveComment(leaveId);
        }

        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return CommentsProvider.Instance().GetLeaveComments(query);
        }

        public static DbQueryResult GetReceivedMessages(ReceivedMessageQuery query)
        {
            return CommentsProvider.Instance().GetReceivedMessages(query);
        }

        public static ReceiveMessageInfo GetReceiveMessage(long receiveMessageId)
        {
            return CommentsProvider.Instance().GetReceiveMessage(receiveMessageId);
        }

        public static DataTable GetReplyLeaveComments(long leaveId)
        {
            return CommentsProvider.Instance().GetReplyLeaveComments(leaveId);
        }

        public static DbQueryResult GetSendedMessages(SendedMessageQuery query)
        {
            return CommentsProvider.Instance().GetSendedMessages(query);
        }

        public static DbQueryResult GetSendedMessagesForReceivedMessage(long receiveMessageId)
        {
            return CommentsProvider.Instance().GetSendedMessagesForReceivedMessage(receiveMessageId);
        }

        public static bool PostMessageIsRead(long receiveMessageId)
        {
            return CommentsProvider.Instance().PostMessageIsRead(receiveMessageId);
        }

        public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            leaveReply.ReplyDate = DateTime.Now;
            return CommentsProvider.Instance().ReplyLeaveComment(leaveReply);
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
                CommentsProvider.Instance().PostMessageLastTime(reply.ReceiveMessageId.Value, item.PublishDate);
            }
            return true;
        }

        public static int SendMessage(IList<SendMessageInfo> sendMessageList, IList<ReceiveMessageInfo> receiveMessageList)
        {
            int num = 0;
            foreach (SendMessageInfo info in sendMessageList)
            {
                Globals.EntityCoding(info, true);
                CommentsProvider.Instance().InsertSendMessage(info);
                num++;
            }
            foreach (ReceiveMessageInfo info2 in receiveMessageList)
            {
                Globals.EntityCoding(info2, true);
                CommentsProvider.Instance().InsertReceiveMessage(info2);
                num++;
            }
            return num;
        }

        public static bool UpdateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return CommentsProvider.Instance().UpdateAffiche(affiche);
        }
    }
}

