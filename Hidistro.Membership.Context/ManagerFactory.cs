using Hidistro.Membership.Core;
using System;

namespace Hidistro.Membership.Context
{
    internal class ManagerFactory : UserFactory
    {
        static readonly ManagerFactory _defaultInstance = new ManagerFactory();
        BizActorProvider provider;

        static ManagerFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        ManagerFactory()
        {
        }

        public override bool ChangeTradePassword(string username, string newPassword)
        {
            return true;
        }

        public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return this.provider.CreateManager(userToCreate as SiteManager);
            }
            catch
            {
                return false;
            }
        }

        public override IUser GetUser(HiMembershipUser membershipUser)
        {
            return this.provider.GetManager(membershipUser);
        }

        public static ManagerFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            return true;
        }

        public override bool UpdateUser(IUser user)
        {
            return true;
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return true;
        }
    }
}

