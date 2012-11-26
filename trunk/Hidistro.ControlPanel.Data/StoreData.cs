using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.ControlPanel.Data
{
    public class StoreData : StoreProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override void AddHotkeywords(int categoryId, string Keywords)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Hotkeywords_Log");
            database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override void ClearRolePrivilege(Guid roleId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("SELECT UserName FROM vw_aspnet_Managers WHERE UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", roleId));
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    RoleHelper.SignOut((string)reader["UserName"]);
                }
            }
        }

        public override bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_FriendlyLink_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                database.AddInParameter(storedProcCommand, "LinkId", DbType.Int32, friendlyLink.LinkId);
            }
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
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Votes_Create");
            database.AddInParameter(storedProcCommand, "VoteName", DbType.String, vote.VoteName);
            database.AddInParameter(storedProcCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            database.AddInParameter(storedProcCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
            database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
            database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
            if (dbTran == null)
            {
                return database.ExecuteNonQuery(sqlStringCommand);
            }
            return database.ExecuteNonQuery(sqlStringCommand, dbTran);
        }

        public override bool DeleteAllLogs()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
            try
            {
                database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void DeleteHotKeywords(int hId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" Delete FROM Hishop_Hotkeywords Where Hid =@Hid");
            database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteLog(long logId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Logs WHERE LogId = @LogId");
            database.AddInParameter(sqlStringCommand, "LogId", DbType.Int64, logId);
            return (database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteLogs(string strIds)
        {
            if (strIds.Length <= 0)
            {
                return 0;
            }
            string query = string.Format("DELETE FROM Hishop_Logs WHERE LogId IN ({0})", strIds);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteManager(int userId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Manager_Delete");
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = database.GetParameterValue(storedProcCommand, "ReturnValue");
            return (((parameterValue != null) && (parameterValue != DBNull.Value)) && (Convert.ToInt32(parameterValue) == 0));
        }

        public override void DeleteQueuedCellphone(Guid cellphoneId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_CellphoneQueue WHERE CellphoneId = @CellphoneId");
            database.AddInParameter(sqlStringCommand, "CellphoneId", DbType.Guid, cellphoneId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteVote(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_Votes WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteVoteItem(long voteId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public override DataTable DequeueCellphone()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_CellphoneQueue WHERE NextTryTime < getdate()");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void EnqueuCellphone(string cellphoneNumber, string subject)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_CellphoneQueue (CellphoneId, CellphoneNumber, Subject, NextTryTime, NumberOfTries)VALUES(newid(), @CellphoneNumber,@Subject, getdate(), 0)");
            database.AddInParameter(sqlStringCommand, "CellphoneNumber", DbType.String, cellphoneNumber);
            database.AddInParameter(sqlStringCommand, "Subject", DbType.String, subject);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int FriendlyLinkDelete(int linkId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_FriendlyLinks WHERE linkid = @linkid");
            database.AddInParameter(sqlStringCommand, "Linkid", DbType.Int32, linkId);
            return database.ExecuteNonQuery(sqlStringCommand);
        }

        public override FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            FriendlyLinksInfo info = new FriendlyLinksInfo();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks WHERE LinkId=@LinkId");
            database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks ORDER BY DisplaySequence DESC");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateFriendlyLink(reader));
                }
            }
            return list;
        }

        public override string GetHotkeyword(int id)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT Keywords FROM Hishop_Hotkeywords WHERE Hid=@Hid");
            database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, id);
            return database.ExecuteScalar(sqlStringCommand).ToString();
        }

        public override DataTable GetHotKeywords()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Hishop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Hishop_Hotkeywords h ORDER BY Frequency DESC");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetLogs(OperationLogQuery query)
        {
            StringBuilder builder = new StringBuilder();
            Pagination page = query.Page;
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat("AddedTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" AddedTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            if (!string.IsNullOrEmpty(query.OperationUserName))
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
            }
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", builder.ToString(), "*");
        }

        public override DbQueryResult GetManagers(ManagerQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            if (query.RoleId != Guid.Empty)
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", query.RoleId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Managers", "UserId", builder.ToString(), "*");
        }

        public override IList<string> GetOperationUserNames()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT DISTINCT UserName FROM Hishop_Logs");
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader["UserName"].ToString());
                }
            }
            return list;
        }

        public override VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public override IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = Hishop_Votes.VoteId) AS VoteCounts FROM Hishop_Votes");
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public override void QueueSendingFailure(IList<Guid> cellphoneIds)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_CellphoneQueue SET NextTryTime = getdate(), NumberOfTries = NumberOfTries +1 WHERE CellphoneId = @CellphoneId DELETE FROM Hishop_CellphoneQueue WHERE NumberOfTries > 10");
            database.AddInParameter(sqlStringCommand, "CellphoneId", DbType.Guid);
            foreach (Guid guid in cellphoneIds)
            {
                database.SetParameterValue(sqlStringCommand, "CellphoneId", guid);
                database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override int SetVoteIsBackup(long voteId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Votes_IsBackup");
            database.AddInParameter(storedProcCommand, "VoteId", DbType.Int64, voteId);
            return database.ExecuteNonQuery(storedProcCommand);
        }

        public override void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_Hotkeywords", "Hid", "Frequency", hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("Update Hishop_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid");
            database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
            database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateVote(VoteInfo vote, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_Votes SET VoteName = @VoteName, MaxCheck = @MaxCheck WHERE VoteId = @VoteId");
            database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
        }

        public override void WriteOperationLogEntry(OperationLogEntry entry)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO [Hishop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
            database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, entry.PageUrl);
            database.AddInParameter(sqlStringCommand, "AddedTime", DbType.DateTime, entry.AddedTime);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, entry.UserName);
            database.AddInParameter(sqlStringCommand, "IPAddress", DbType.String, entry.IpAddress);
            database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, (int)entry.Privilege);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, entry.Description);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

