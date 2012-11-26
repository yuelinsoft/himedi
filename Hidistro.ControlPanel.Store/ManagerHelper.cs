namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Cryptography;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Web;
    using System.Xml;

    public static class ManagerHelper
    {
        public static void CheckPrivilege(Privilege privilege)
        {
            IUser user = HiContext.Current.User;
            if (user.IsAnonymous || (user.UserRole != UserRole.SiteManager))
            {
                HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
            }
            else
            {
                SiteManager manager = user as SiteManager;
                if (!manager.IsAdministrator && !manager.HasPrivilege((int) privilege))
                {
                    HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
                }
            }
        }

        public static void ClearRolePrivilege(Guid roleId)
        {
            StoreProvider.Instance().ClearRolePrivilege(roleId);
        }

        public static CreateUserStatus Create(SiteManager managerToCreate, string department)
        {
            if ((managerToCreate == null) || (managerToCreate.UserRole != UserRole.SiteManager))
            {
                return CreateUserStatus.UnknownFailure;
            }
            string[] roles = new string[] { HiContext.Current.Config.RolesConfiguration.Manager, department };
            return Users.CreateUser(managerToCreate, roles);
        }

        public static CreateUserStatus CreateAdministrator(SiteManager administrator)
        {
            return Create(administrator, HiContext.Current.Config.RolesConfiguration.SystemAdministrator);
        }

        public static bool Delete(int userId)
        {
            SiteManager user = HiContext.Current.User as SiteManager;
            if (user.UserId == userId)
            {
                return false;
            }
            return StoreProvider.Instance().DeleteManager(userId);
        }

        public static SiteManager GetManager(int userId)
        {
            IUser user = Users.GetUser(userId, false);
            if (((user != null) && !user.IsAnonymous) && (user.UserRole == UserRole.SiteManager))
            {
                return (user as SiteManager);
            }
            return null;
        }

        public static DbQueryResult GetManagers(ManagerQuery query)
        {
            return StoreProvider.Instance().GetManagers(query);
        }

        public static bool Update(SiteManager manager)
        {
            return Users.UpdateUser(manager);
        }

        public static LoginUserStatus ValidLogin(SiteManager manager)
        {
            if (manager == null)
            {
                return LoginUserStatus.InvalidCredentials;
            }
            LoginUserStatus status = Users.ValidateUser(manager);
            if ((status == LoginUserStatus.Success) && (manager.UserRole == UserRole.SiteManager))
            {
                HttpContext context = HiContext.Current.Context;
                string path = context.Request.MapPath(Globals.ApplicationPath + "/config/Hishop.key");
                if (File.Exists(path))
                {
                    return status;
                }
                try
                {
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        document.Load(context.Request.MapPath(Globals.ApplicationPath + "/config/key.config"));
                    }
                    catch
                    {
                        document.Load(context.Request.MapPath(Globals.ApplicationPath + "/config/key.config.bak"));
                    }
                    if (int.Parse(document.SelectSingleNode("Settings/Token").InnerText) != manager.UserId)
                    {
                        return status;
                    }
                    byte[] userData = Cryptographer.DecryptWithPassword(Convert.FromBase64String(document.SelectSingleNode("Settings/Key").InnerText), manager.Password);
                    byte[] encryptedKey = ProtectedData.Protect(userData, null, DataProtectionScope.LocalMachine);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        KeyManager.Write(stream, encryptedKey, DataProtectionScope.LocalMachine);
                    }
                    CryptographyUtility.ZeroOutBytes(encryptedKey);
                    CryptographyUtility.ZeroOutBytes(userData);
                }
                catch
                {
                }
            }
            return status;
        }
    }
}

