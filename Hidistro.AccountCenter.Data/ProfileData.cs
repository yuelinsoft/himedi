using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace Hidistro.AccountCenter.Data
{

    public class ProfileData : PersonalMasterProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

        public override bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            string query = "INSERT INTO Hishop_BalanceDetails(UserId, UserName, TradeDate, TradeType, Income, Balance,Remark) VALUES (@UserId, @UserName,@TradeDate, @TradeType, @Income, @Balance, @Remark)";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)balanceDetails.TradeType);
            database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool AddInpourBlance(InpourRequestInfo inpourRequest)
        {
            if (null == inpourRequest)
            {
                return false;
            }
            DbCommand storedProcCommand = database.GetStoredProcCommand("ac_Member_InpourRequest_Create");
            database.AddInParameter(storedProcCommand, "InpourId", DbType.String, inpourRequest.InpourId);
            database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, inpourRequest.TradeDate);
            database.AddInParameter(storedProcCommand, "InpourBlance", DbType.Currency, inpourRequest.InpourBlance);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, inpourRequest.UserId);
            database.AddInParameter(storedProcCommand, "PaymentId", DbType.String, inpourRequest.PaymentId);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,EmailAddress,TelPhone,CellPhone) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@EmailAddress,@TelPhone,@CellPhone); SELECT @@IDENTITY");
            database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
            database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
            database.AddInParameter(sqlStringCommand, "Address", DbType.String, shippingAddress.Address);
            database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
            database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
            database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            return Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
        }

        public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_BalanceDrawRequest (UserId,UserName,RequestTime, Amount, AccountName, BankName, MerchantCode, Remark) VALUES (@UserId,@UserName,@RequestTime, @Amount, @AccountName, @BankName, @MerchantCode, @Remark)");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDrawRequest.UserId);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDrawRequest.UserName);
            database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, balanceDrawRequest.RequestTime);
            database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, balanceDrawRequest.Amount);
            database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, balanceDrawRequest.AccountName);
            database.AddInParameter(sqlStringCommand, "BankName", DbType.String, balanceDrawRequest.BankName);
            database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, balanceDrawRequest.MerchantCode);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDrawRequest.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

       static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            if (query.TradeType != TradeTypes.NotSet)
            {
                builder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
            }
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteShippingAddress(ShippingAddressInfo shippingAddress, DataProviderAction action)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("ac_Member_ShippingAddress_CreateUpdateDelete");
            database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                database.AddInParameter(storedProcCommand, "ShippingId", DbType.Int32, shippingAddress.ShippingId);
            }
            if (action != DataProviderAction.Delete)
            {
                database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
                database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, shippingAddress.UserId);
                database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
                database.AddInParameter(storedProcCommand, "Address", DbType.String, shippingAddress.Address);
                database.AddInParameter(storedProcCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
                database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
                database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            }
            database.ExecuteNonQuery(storedProcCommand);
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return null;
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("SELECT TOP {0} * FROM Hishop_BalanceDetails B WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat(" {0} ORDER BY JournalNumber DESC;", str);
            }
            else
            {
                builder.AppendFormat(" AND JournalNumber < (SELECT MIN(JournalNumber) FROM (SELECT TOP {0} JournalNumber FROM Hishop_BalanceDetails WHERE 0=0 {1} ORDER BY JournalNumber DESC ) AS T) {1} ORDER BY JournalNumber DESC;", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(" SELECT COUNT(JournalNumber) AS Total from Hishop_BalanceDetails WHERE 0=0 {0}", str);
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

        public override InpourRequestInfo GetInpourBlance(string inpourId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_InpourRequest WHERE InpourId = @InpourId;");
            database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            InpourRequestInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateInpourRequest(reader);
                }
            }
            return info;
        }

        public override MemberGradeInfo GetMemberGrade(int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            MemberGradeInfo info = null;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateMemberGrade(reader);
                }
            }
            return info;
        }

        public override IList<MemberGradeInfo> GetMemberGrades()
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades");
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetMyReferralMembers(MemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ReferralUserId = {0}", HiContext.Current.User.UserId);
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId = {0}", query.GradeId.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override IList<ShippingAddressInfo> GetShippingAddress(int userId)
        {
            IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingAddress(reader));
                }
            }
            return list;
        }

        public override int GetShippingAddressCount(int userId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT count(ShippingId) as Count FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            int num = 0;
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int)reader["Count"];
                }
            }
            return num;
        }

        public override void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT COUNT(*) AS NoPayOrderNum FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus = {1};", HiContext.Current.User.UserId, 1);
            builder.AppendFormat(" SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_ReceivedMessages WHERE Addressee = '{0}' AND IsRead=0 ;", HiContext.Current.User.Username);
            builder.AppendFormat(" SELECT COUNT(*) AS NoReplyLeaveCommentNum FROM Hishop_ProductConsultations WHERE UserId = {0} AND ReplyUserId is not null;", HiContext.Current.User.UserId);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read() && (DBNull.Value != reader["NoPayOrderNum"]))
                {
                    num = (int)reader["NoPayOrderNum"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["NoReadMessageNum"]))
                {
                    num2 = (int)reader["NoReadMessageNum"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["NoReplyLeaveCommentNum"]))
                {
                    num3 = (int)reader["NoReplyLeaveCommentNum"];
                }
            }
            noPayOrderNum = num;
            noReadMessageNum = num2;
            noReplyLeaveCommentNum = num3;
        }

        public override ShippingAddressInfo GetUserShippingAddress(int shippingId)
        {
            ShippingAddressInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId");
            database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingAddress(reader);
                }
            }
            return info;
        }

        public override void RemoveInpourRequest(string inpourId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_InpourRequest WHERE InpourId = @InpourId");
            database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

