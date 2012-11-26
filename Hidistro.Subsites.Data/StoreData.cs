using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Subsites.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.Subsites.Data
{
    public class StoreData : SubsiteStoreProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override bool AddBalanceDetail(BalanceDetailInfo balanceDetails, string inpourId)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName, TradeDate, TradeType, Income, Balance, Remark) VALUES (@UserId, @UserName, @TradeDate, @TradeType, @Income, @Balance, @Remark); DELETE FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId;");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)balanceDetails.TradeType);
            database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void AddHotkeywords(int categoryId, string Keywords)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Hotkeywords_Log");
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool AddInpourBlance(InpourRequestInfo inpourRequest)
        {
            if (null == inpourRequest)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_DistributorInpourRequest_Create");
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "InpourId", DbType.String, inpourRequest.InpourId);
            database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, inpourRequest.TradeDate);
            database.AddInParameter(storedProcCommand, "InpourBlance", DbType.Currency, inpourRequest.InpourBlance);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, inpourRequest.UserId);
            database.AddInParameter(storedProcCommand, "PaymentId", DbType.String, inpourRequest.PaymentId);
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool AddSiteRequest(SiteRequestInfo siteRequest)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_SiteRequest(UserId,FirstSiteUrl,FirstRecordCode,SecondSiteUrl,SecondRecordCode,RequestTime,RequestStatus,RefuseReason)VALUES(@UserId,@FirstSiteUrl,@FirstRecordCode,@SecondSiteUrl,@SecondRecordCode,@RequestTime,@RequestStatus,@RefuseReason)");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "FirstSiteUrl", DbType.String, siteRequest.FirstSiteUrl);
            database.AddInParameter(sqlStringCommand, "FirstRecordCode", DbType.String, siteRequest.FirstRecordCode);
            database.AddInParameter(sqlStringCommand, "SecondSiteUrl", DbType.String, siteRequest.SecondSiteUrl);
            database.AddInParameter(sqlStringCommand, "SecondRecordCode", DbType.String, siteRequest.SecondRecordCode);
            database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, siteRequest.RequestTime);
            database.AddInParameter(sqlStringCommand, "RequestStatus", DbType.Int32, (int)siteRequest.RequestStatus);
            database.AddInParameter(sqlStringCommand, "RefuseReason", DbType.String, siteRequest.RefuseReason);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDrawRequest VALUES(@UserId,@UserName,@RequestTime,@Amount,@AccountName,@BankName,@MerchantCode)");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDrawRequest.UserId);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDrawRequest.UserName);
            database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, balanceDrawRequest.RequestTime);
            database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, balanceDrawRequest.Amount);
            database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, balanceDrawRequest.MerchantCode);
            database.AddInParameter(sqlStringCommand, "BankName", DbType.String, balanceDrawRequest.BankName);
            database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, balanceDrawRequest.AccountName);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            IUser user = HiContext.Current.User;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" AND UserId = {0}", user.UserId);
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_FriendlyLink_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                database.AddInParameter(storedProcCommand, "LinkId", DbType.Int32, friendlyLink.LinkId);
            }
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (action != DataProviderAction.Delete)
            {
                database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, friendlyLink.ImageUrl);
                database.AddInParameter(storedProcCommand, "LinkUrl", DbType.String, friendlyLink.LinkUrl);
                database.AddInParameter(storedProcCommand, "Title", DbType.String, friendlyLink.Title);
                database.AddInParameter(storedProcCommand, "Visible", DbType.Boolean, friendlyLink.Visible);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override long CreateVote(VoteInfo vote)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Votes_Create");
            database.AddInParameter(storedProcCommand, "VoteName", DbType.String, vote.VoteName);
            database.AddInParameter(storedProcCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            database.AddInParameter(storedProcCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddOutParameter(storedProcCommand, "VoteId", DbType.Int64, 8);
            long parameterValue = 0;
            if (database.ExecuteNonQuery(storedProcCommand) > 0)
            {
                parameterValue = (long)database.GetParameterValue(storedProcCommand, "VoteId");
            }
            return parameterValue;
        }

        public override int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO distro_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
            database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
            database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
            if (dbTran == null)
            {
                return database.ExecuteNonQuery(sqlStringCommand);
            }
            return database.ExecuteNonQuery(sqlStringCommand, dbTran);
        }

        public override void DeleteHotKeywords(int hId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" Delete FROM distro_Hotkeywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeleteSiteRequest()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_SiteRequest WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteVote(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteVoteItem(long voteId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_VoteItems WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public override bool DistroHasDrawRequest()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) >= 1);
        }

        public override int FriendlyLinkDelete(int linkId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_FriendlyLinks WHERE LinkId = @LinkId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
        {
            DistributorGradeInfo info = new DistributorGradeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulDistributorGrade(reader);
                }
            }
            return info;
        }

        public override FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            FriendlyLinksInfo info = new FriendlyLinksInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE LinkId=@LinkId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateFriendlyLink(reader);
                }
            }
            return info;
        }

        public override IList<FriendlyLinksInfo> GetFriendlyLinks()
        {
            IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence DESC");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateFriendlyLink(reader));
                }
            }
            return list;
        }

        public override DataTable GetHotKeywords()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT Name FROM distro_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM distro_Hotkeywords h WHERE DistributorUserId=@DistributorUserId ORDER BY Frequency DESC");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override InpourRequestInfo GetInpouRequest(string inpourId)
        {
            InpourRequestInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
            database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info = DataMapper.PopulateInpourRequest(reader);
                }
            }
            return info;
        }

        public override AccountSummaryInfo GetMyAccountSummary()
        {
            AccountSummaryInfo info = new AccountSummaryInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT SUM(Amount) AS FreezeBalance FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId; SELECT TOP 1 Balance AS AccountAmount FROM Hishop_DistributorBalanceDetails WHERE UserId= @UserId ORDER BY JournalNumber DESC;");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read() && (DBNull.Value != reader["FreezeBalance"]))
                {
                    info.DrawRequestBalance = info.FreezeBalance = (decimal)reader["FreezeBalance"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["AccountAmount"]))
                {
                    info.AccountAmount = (decimal)reader["AccountAmount"];
                }
            }
            info.UseableBalance = info.AccountAmount - info.FreezeBalance;
            return info;
        }

        public override DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("select top {0} B.JournalNumber,B.UserId,B.UserName, B.TradeDate,B.TradeType,B.Income,B.Expenses,B.Balance", query.PageSize);
            builder.Append(" from Hishop_DistributorBalanceDetails B where 0=0 ");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY JournalNumber DESC", str);
            }
            else
            {
                builder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_DistributorBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", str);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (query.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override SiteRequestInfo GetMySiteRequest()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_SiteRequest WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            SiteRequestInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulSiteRequest(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
            database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            VoteInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateVote(reader);
                }
            }
            return info;
        }

        public override int GetVoteCounts(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_VoteItems WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            IList<VoteItemInfo> list = new List<VoteItemInfo>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                VoteItemInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateVoteItem(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DataSet GetVotes()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = distro_Votes.VoteId) AS VoteCounts FROM distro_Votes WHERE DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public override void RemoveInpourRequest(string inpourId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
            database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int SetVoteIsBackup(long voteId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("sub_Votes_IsBackup");
            database.AddInParameter(storedProcCommand, "VoteId", DbType.Int64, voteId);
            return database.ExecuteNonQuery(storedProcCommand);
        }

        public override void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_Hotkeywords", "Hid", "Frequency", hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update distro_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateVote(VoteInfo vote, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Votes SET VoteName = @VoteName, IsBackup = @IsBackup, MaxCheck = @MaxCheck WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            database.AddInParameter(sqlStringCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
        }
    }
}

