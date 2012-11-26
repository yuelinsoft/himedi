using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.ControlPanel.Data
{
    public class MemberData : MemberProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
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

        static string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND RequestTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            return builder.ToString();
        }

        public override bool CreateMemberGrade(MemberGradeInfo memberGrade)
        {
            string query = string.Empty;
            if (memberGrade.IsDefault)
            {
                query = query + "UPDATE aspnet_MemberGrades SET IsDefault = 0";
            }
            query = query + " INSERT INTO aspnet_MemberGrades ([Name], Description, Points, IsDefault, Discount) VALUES (@Name, @Description, @Points, @IsDefault, @Discount)";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, memberGrade.Name);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, memberGrade.Description);
            database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
            database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, memberGrade.IsDefault);
            database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, memberGrade.Discount);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DealBalanceDrawRequest(int userId, bool agree)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_BalanceDrawRequest_Update");
            database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            database.AddInParameter(storedProcCommand, "Agree", DbType.Boolean, agree);
            database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = database.GetParameterValue(storedProcCommand, "Status");
            if ((parameterValue == DBNull.Value) || (parameterValue == null))
            {
                return false;
            }
            return (((int)database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool Delete(int userId)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Member_Delete");
            Member user = Users.GetUser(userId) as Member;
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            database.AddInParameter(storedProcCommand, "UserName", DbType.String, user.Username);
            database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = database.GetParameterValue(storedProcCommand, "ReturnValue");
            return (((parameterValue != null) && (parameterValue != DBNull.Value)) && (Convert.ToInt32(parameterValue) == 0));
        }

        public override bool DeleteMemberGrade(int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM aspnet_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND NOT EXISTS(SELECT * FROM aspnet_Members WHERE GradeId = @GradeId)");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("SELECT TOP {0} *", query.PageSize);
            builder.Append(" FROM Hishop_BalanceDetails B where 0=0 ");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY JournalNumber DESC", str);
            }
            else
            {
                builder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_BalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", str);
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

        public override DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.Append("SELECT * FROM Hishop_BalanceDetails WHERE 0=0 ");
            builder.AppendFormat("{0} ORDER BY JournalNumber DESC", str);
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", str);
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

        public override DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDrawRequestQuery(query);
            builder.AppendFormat("select top {0} *", query.PageSize);
            builder.Append(" from Hishop_BalanceDrawRequest B where 0=0 ");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY RequestTime DESC", str);
            }
            else
            {
                builder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_BalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(*) as Total from Hishop_BalanceDrawRequest where 0=0 {0}", str);
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

        public override DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDrawRequestQuery(query);
            builder.Append("select *");
            builder.Append(" from Hishop_BalanceDrawRequest B where 0=0 ");
            builder.AppendFormat("{0} ORDER BY RequestTime DESC", str);
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(*) as Total from Hishop_BalanceDrawRequest where 0=0 {0}", str);
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

        public override DbQueryResult GetMemberBlanceList(MemberQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = string.Empty;
            if (!string.IsNullOrEmpty(query.Username))
            {
                str = string.Format("AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName LIKE '%{0}%')", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                str = str + string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            builder.AppendFormat("SELECT TOP {0} *", query.PageSize);
            builder.Append(" FROM vw_aspnet_Members WHERE 0=0");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY CreateDate DESC", str);
            }
            else
            {
                builder.AppendFormat("AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_aspnet_Members WHERE 0=0 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";SELECT COUNT(CreateDate) AS Total FROM vw_aspnet_Members WHERE 0=0 {0}", str);
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
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetMembers(MemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat("GradeId = {0}", query.GradeId.Value);
            }
            if (query.IsApproved.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsApproved = '{0}'", query.IsApproved.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                if (builder.Length > 0)
                {
                    builder.AppendFormat("AND", new object[0]);
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            DataTable table = null;
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT {0} FROM vw_aspnet_Members WHERE 1=1 ", str);
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId={0}", query.GradeId);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE Points=@Points AND GradeId<>@GradeId;");
            database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
            return (((int)database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool InsertBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand("INSERT INTO Hishop_BalanceDetails (UserId, UserName, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId, @UserName, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark);");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)balanceDetails.TradeType);
            database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balanceDetails.Expenses);
            database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SetDefalutMemberGrade(int gradeId)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET IsDefault = 0;UPDATE aspnet_MemberGrades SET IsDefault = 1 WHERE GradeId = @GradeId");
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateMemberGrade(MemberGradeInfo memberGrade)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET [Name] = @Name, Description = @Description, Points = @Points, Discount = @Discount WHERE GradeId = @GradeId");
            database.AddInParameter(sqlStringCommand, "Name", DbType.String, memberGrade.Name);
            database.AddInParameter(sqlStringCommand, "Description", DbType.String, memberGrade.Description);
            database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
            database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, memberGrade.Discount);
            database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

