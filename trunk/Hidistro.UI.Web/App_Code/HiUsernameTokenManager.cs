using Hidistro.Core.Cryptography;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Xml;

namespace Hidistro.UI.Web
{
    public partial class HiUsernameTokenManager : UsernameTokenManager
    {

        public HiUsernameTokenManager()
        {
        }

        public HiUsernameTokenManager(XmlNodeList xmlNodeList): base(xmlNodeList)
        {
        }

        protected override string AuthenticateToken(UsernameToken token)
        {

            LoginUserStatus invalidCredentials = LoginUserStatus.InvalidCredentials;

            try
            {

                SiteManager user = Users.GetUser(0, token.Identity.Name, false, false) as SiteManager;

                if ((user != null) && user.IsAdministrator)
                {

                    HiContext current = HiContext.Current;

                    user.Password = Cryptographer.Decrypt(token.Password);

                    invalidCredentials = Users.ValidateUser(user);

                }
                else
                {

                    invalidCredentials = LoginUserStatus.InvalidCredentials;

                }

            }
            catch
            {

                invalidCredentials = LoginUserStatus.InvalidCredentials;

            }

            if (invalidCredentials == LoginUserStatus.Success)
            {
                return token.Password;
            }

            return Cryptographer.CreateHash(token.Password);

        }

    }

}

