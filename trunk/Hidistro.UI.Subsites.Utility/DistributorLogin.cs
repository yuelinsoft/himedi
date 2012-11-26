using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorLogin : HtmlTemplatedWebControl
    {
        Button btnLogin;
        TextBox txtCode;
        TextBox txtPassword;
        TextBox txtUserName;

        protected override void AttachChildControls()
        {
            HiContext current = HiContext.Current;
            if (Context.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
                IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
                if (userCookie != null)
                {
                    userCookie.DeleteCookie(authCookie);
                }
                RoleHelper.SignOut(HiContext.Current.User.Username);
                Page.Response.Cookies["hishopLoginStatus"].Value = "";
            }
            txtUserName = (TextBox)FindControl("txtUserName");
            txtPassword = (TextBox)FindControl("txtPassword");
            btnLogin = (Button)FindControl("btnLogin");
            txtCode = (TextBox)FindControl("txtCode");
            btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            if (!HiContext.Current.CheckVerifyCode(txtCode.Text.Trim()))
            {
                ShowMessage("验证码不正确", false);
            }
            else
            {
                IUser user = Users.GetUser(0, txtUserName.Text, false, true);
                if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
                {
                    ShowMessage("无效的用户信息", false);
                }
                else
                {
                    Distributor distributor = user as Distributor;
                    distributor.Password = txtPassword.Text;
                    if (HiContext.Current.SiteSettings.IsDistributorSettings && (user.UserId != HiContext.Current.SiteSettings.UserId.Value))
                    {
                        ShowMessage("分销商只能在自己的站点或主站上登录", false);
                    }
                    else
                    {
                        LoginUserStatus status = SubsiteStoreHelper.ValidLogin(distributor);
                        if (status == LoginUserStatus.Success)
                        {
                            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(distributor.Username, false);
                            distributor.GetUserCookie().WriteCookie(authCookie, 30, false);
                            Page.Response.Cookies["hishopLoginStatus"].Value = "true";
                            HiContext.Current.User = distributor;
                            distributor.OnLogin();
                            if (SettingsManager.GetSiteSettings(HiContext.Current.User.UserId) == null)
                            {
                                Page.Response.Redirect("nositedefault.aspx", true);
                            }
                            else
                            {
                                Page.Response.Redirect("default.aspx", true);
                            }
                        }
                        else
                        {
                            switch (status)
                            {
                                case LoginUserStatus.AccountPending:
                                    {
                                        ShowMessage("用户账号还没有通过审核", false);
                                        return;
                                    }
                                case LoginUserStatus.AccountLockedOut:
                                    {
                                        ShowMessage("用户账号已被锁定，暂时不能登录系统", false);
                                        return;
                                    }
                                case LoginUserStatus.InvalidCredentials:
                                    {
                                        ShowMessage("用户名或密码错误", false);
                                        return;
                                    }
                            }
                            ShowMessage("登录失败，未知错误", false);
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                SkinName = "Skin-DistributorLogin.html";
            }
            base.OnInit(e);
        }

        void ShowMessage(string msg)
        {
        }
    }
}

