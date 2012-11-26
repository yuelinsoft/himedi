using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 后台登录
    /// </summary>
    public partial class Login : Page
    {

        readonly string licenseMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的易分销系统未经官方授权，无法登录后台管理。请联系易分销官方(www.shopefx.com)购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 海商网络技术有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        const string NoticeMsg = "<div class=\"checkInfo\">\r\n   <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/checkInfo.gif\" width=\"30\" height=\"30\" /></td>\r\n        <td class=\"td2\" width=\"100%\">您正在使用的易分销系统已过授权有效期！<br/>请联系官方(www.shopefx.com)购买软件使用权。</td>\r\n      </tr>\r\n   </table>\r\n</div>";

        //登录按钮事件
        public void btnAdminLogin_Click(object sender, EventArgs e)
        {

            if (!HiContext.Current.CheckVerifyCode(txtCode.Text.Trim()))
            {
                ShowMessage("验证码不正确");
            }
            else
            {

                IUser user = Users.GetUser(0, txtAdminName.Text, false, true);

                if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.SiteManager))
                {
                    ShowMessage("无效的用户信息");
                }
                else
                {
                    string referralLink = null;

                    SiteManager manager = user as SiteManager;

                    manager.Password = txtAdminPassWord.Text;

                    switch (ManagerHelper.ValidLogin(manager))
                    {
                        case LoginUserStatus.InvalidCredentials:
                            {
                                ShowMessage("用户名或密码错误");
                                return;
                            }
                        case LoginUserStatus.AccountPending:
                            {
                                ShowMessage("用户账号还没有通过审核");
                                return;
                            }
                        case LoginUserStatus.AccountLockedOut:
                            {
                                ShowMessage("用户账号已被锁定，暂时不能登录系统");
                                return;
                            }
                        case LoginUserStatus.Success:
                            {
                                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(manager.Username, false);

                                manager.GetUserCookie().WriteCookie(authCookie, 30, false);

                                HiContext.Current.User = manager;

                                if (!string.IsNullOrEmpty(Page.Request.QueryString["returnUrl"]))
                                {
                                    referralLink = Page.Request.QueryString["returnUrl"];
                                }

                                if (!(((referralLink != null) || (ReferralLink == null)) || string.IsNullOrEmpty(ReferralLink.Trim())))
                                {
                                    referralLink = ReferralLink;
                                }

                                if (!(string.IsNullOrEmpty(referralLink) || (((referralLink.ToLower().IndexOf(Globals.GetSiteUrls().Logout.ToLower()) < 0) && (referralLink.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("register").ToLower()) < 0)) && (referralLink.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("vote").ToLower()) < 0))))
                                {
                                    referralLink = null;
                                }

                                if (referralLink != null)
                                {
                                    Page.Response.Redirect(referralLink, true);
                                }
                                else
                                {
                                    Page.Response.Redirect("default.aspx", true);
                                }

                                return;
                            }
                    }
                    ShowMessage("登录失败，未知错误");
                }
            }
        }

        //控件初始经事件
        protected override void OnInit(EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            //验证域名
            if (HiContext.Current.SiteUrl != masterSettings.SiteUrl)
            {
                Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }


            if (Page.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
                IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
                if (userCookie != null)
                {
                    userCookie.DeleteCookie(authCookie);
                }
                RoleHelper.SignOut(HiContext.Current.User.Username);
            }
            base.OnInit(e);
        }

        //控件加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Uri urlReferrer = Context.Request.UrlReferrer;

                if (urlReferrer != null)
                {
                    ReferralLink = urlReferrer.ToString();
                }

                txtAdminName.Focus();

                PageTitle.AddSiteNameTitle("后台登录", HiContext.Current.Context);

            }
        }



        //控件呈现事件
        protected override void Render(HtmlTextWriter writer)
        {
            int siteQty;
            bool isValid, isExpired;

            //授权验证
            LicenseChecker.Check(out isValid, out isExpired, out siteQty);

            if (!isValid)
            {
                writer.Write(licenseMsg);
            }
            else if (isExpired)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        base.Render(htw);

                        string str = sw.ToString();

                        str = str.Insert(str.IndexOf("</body>"), "<div class=\"checkInfo\">\r\n   <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/checkInfo.gif\" width=\"30\" height=\"30\" /></td>\r\n        <td class=\"td2\" width=\"100%\">您正在使用的易分销系统已过授权有效期！<br/>请联系官方(www.shopefx.com)购买软件使用权。</td>\r\n      </tr>\r\n   </table>\r\n</div>");

                        writer.Write(str);

                    }
                }
            }
            else
            {
                base.Render(writer);
            }
        }

        //消息显示
        void ShowMessage(string msg)
        {
            lblStatus.Text = msg;
            lblStatus.Success = false;
            lblStatus.Visible = true;
        }

        string ReferralLink
        {
            get
            {
                return (ViewState["ReferralLink"] as string);
            }
            set
            {
                ViewState["ReferralLink"] = value;
            }
        }
    }
}

