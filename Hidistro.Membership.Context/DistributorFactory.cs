using Hidistro.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Web.Security;

namespace Hidistro.Membership.Context
{
    /// <summary>
    /// 分销工厂
    /// </summary>
    internal class DistributorFactory : UserFactory
    {
        static readonly DistributorFactory _defaultInstance = new DistributorFactory();
        BizActorProvider provider;

        static DistributorFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        DistributorFactory()
        {
        }

        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangeTradePassword(string username, string newPassword)
        {
            SiteManager user = HiContext.Current.User as SiteManager;

            if (user == null)
            {
                return false;
            }

            string oldPassword = ResetTradePassword(username);

            return ChangeTradePassword(username, oldPassword, newPassword);

        }

        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
        {
            return provider.ChangeDistributorTradePassword(username, oldPassword, newPassword);
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return provider.CreateDistributor(userToCreate as Distributor);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取交易密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordFormat"></param>
        /// <param name="passwordSalt"></param>
        static void GetTradePassword(string username, out int passwordFormat, out string passwordSalt)
        {
            passwordFormat = 0;
            passwordSalt = string.Empty;

            Database database = DatabaseFactory.CreateDatabase();

            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Distributors WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
         
            database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
         
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (null != reader && reader.Read())
                {
                    passwordFormat = reader.GetInt32(0);
                    passwordSalt = reader.GetString(1);
                }
            }

        }

        public override IUser GetUser(HiMembershipUser membershipUser)
        {
            return provider.GetDistributor(membershipUser);
        }

        public static DistributorFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            return true;
        }

        static string ResetTradePassword(string username)
        {
            int format;

            string salt;

            SiteManager user = HiContext.Current.User as SiteManager;

            if (user == null)
            {
                return null;
            }

            string cleanString = System.Web.Security.Membership.GeneratePassword(10, 0);

            GetTradePassword(username, out format, out salt);

            string encodePwd = UserHelper.EncodePassword((MembershipPasswordFormat)format, cleanString, salt);

            if (encodePwd.Length > 128)
            {
                return null;
            }

            Database database = DatabaseFactory.CreateDatabase();

            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Distributors SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
            database.AddInParameter(sqlStringCommand, "NewTradePassword", DbType.String, encodePwd);
            database.AddInParameter(sqlStringCommand, "PasswordSalt", DbType.String, salt);
            database.AddInParameter(sqlStringCommand, "PasswordFormat", DbType.Int32, format);
            database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);

            database.ExecuteNonQuery(sqlStringCommand);

            return cleanString;
        }

        public override bool UpdateUser(IUser user)
        {
            return provider.UpdateDistributor(user as Distributor);
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return provider.ValidDistributorTradePassword(username, password);
        }
    }
}

