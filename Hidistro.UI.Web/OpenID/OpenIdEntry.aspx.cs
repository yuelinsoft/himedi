using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.OpenID
{
    public partial class OpenIdEntry : Page
    {
        string openIdType;

        NameValueCollection parameters;

        private string GeneratePassword()
        {
            return GenerateRndString(8, "");
        }

        private string GenerateRndString(int length, string prefix)
        {
            string str = string.Empty;

            Random random = new Random();

            char ch;

            while (str.Length < 10)
            {

                int num = random.Next();

                if ((num % 3) == 0)
                {
                    ch = (char)(0x61 + ((ushort)(num % 0x1a)));
                }
                else
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                str = str + ch.ToString();
            }
            return (prefix + str);
        }

        private string GenerateUsername()
        {
            return GenerateRndString(10, "u_");
        }

        private string GenerateUsername(int length)
        {
            return GenerateRndString(length, "u_");
        }

        private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
        {
            string str;

            parameters.Add("CurrentOpenId", e.OpenId);

            HiContext current = HiContext.Current;

            string usernameWithOpenId = UserHelper.GetUsernameWithOpenId(e.OpenId, openIdType);

            if (!string.IsNullOrEmpty(usernameWithOpenId))
            {

                IUser user = Users.GetUser(0, usernameWithOpenId, false, true);

                if (((user == null) || user.IsAnonymous) || (user.UserRole == UserRole.SiteManager))
                {

                    Response.Write("登录失败，信任登录只能用于会员登录。");

                    return;

                }

                if (user.IsLockedOut)
                {

                    Response.Write("登录失败，您的用户账号还在等待审核。");

                    return;

                }

                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Username, false);

                user.GetUserCookie().WriteCookie(authCookie, 30, false);

                HiContext.Current.User = user;

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();

                CookieShoppingProvider.Instance().ClearShoppingCart();

                current.User = user;

                if (shoppingCart != null)
                {
                    ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                }

                if (!string.IsNullOrEmpty(parameters["token"]))
                {
                    HttpCookie cookie3 = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                    cookie3.Expires = DateTime.Now.AddMinutes(30.0);
                    cookie3.Value = parameters["token"];
                    HttpCookie cookie = cookie3;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

                goto Label_0214;
            }
            string str3 = openIdType.ToLower();

            if (str3 == null)
            {
                goto Label_01F9;
            }
            if (!(str3 == "hishop.plugins.openid.alipay.alipayservice"))
            {
                if (!(str3 == "hishop.plugins.openid.qq.qqservice"))
                {
                    goto Label_01F9;
                }
                SkipQQOpenId();
            }
            else
            {
                SkipAlipayOpenId();
            }
            goto Label_0214;
        Label_01F9:
            Page.Response.Redirect(Globals.GetSiteUrls().Home);
        Label_0214:
            str = parameters["HITO"];
            if (str == "1")
            {
                Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
            }
            else
            {
                Page.Response.Redirect(Globals.GetSiteUrls().Home);
            }
        }

        private void Notify_Failed(object sender, FailedEventArgs e)
        {
            Response.Write("登录失败，" + e.Message);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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


            }

            openIdType = Page.Request.QueryString["HIGW"];

            OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(openIdType);

            if (openIdSettings == null)
            {
                Response.Write("登录失败，没有找到对应的插件配置信息。");
            }
            else
            {
                NameValueCollection values = new NameValueCollection();

                values.Add(Page.Request.Form);

                values.Add(Page.Request.QueryString);

                parameters = values;

                OpenIdNotify notify = OpenIdNotify.CreateInstance(openIdType, parameters);

                notify.Authenticated += new EventHandler<AuthenticatedEventArgs>(Notify_Authenticated);

                notify.Failed += new EventHandler<FailedEventArgs>(Notify_Failed);

                notify.Verify(0x7530, Cryptographer.Decrypt(openIdSettings.Settings));

            }
        }

        protected void SkipAlipayOpenId()
        {
            Member member = null;

            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                Member member2 = new Member(UserRole.Underling);
                member2.ParentUserId = HiContext.Current.SiteSettings.UserId;
                member = member2;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.Username = parameters["real_name"];
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "支付宝会员_" + parameters["user_id"];
            }
            member.Email = parameters["email"];
            if (string.IsNullOrEmpty(member.Email))
            {
                member.Email = GenerateUsername() + "@localhost.com";
            }
            string str = GeneratePassword();
            member.Password = str;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "支付宝会员_" + parameters["user_id"];
                member.Password = member.TradePassword = str;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = GenerateUsername();
                    member.Email = GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, parameters["CurrentOpenId"], parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(parameters["token"]))
            {
                HttpCookie cookie3 = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie3.Expires = DateTime.Now.AddMinutes(30.0);
                cookie3.Value = parameters["token"];
                HttpCookie cookie = cookie3;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (!string.IsNullOrEmpty(parameters["target_url"]))
            {
                Page.Response.Redirect(parameters["target_url"]);
            }
            Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }

        protected void SkipQQOpenId()
        {
            Member member = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                Member member2 = new Member(UserRole.Underling);
                member2.ParentUserId = HiContext.Current.SiteSettings.UserId;
                member = member2;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["NickName"];
            if (cookie != null)
            {
                member.Username = HttpUtility.UrlDecode(cookie.Value);
            }
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "腾讯会员_" + GenerateUsername(8);
            }
            member.Email = GenerateUsername() + "@localhost.com";
            string str = GeneratePassword();
            member.Password = str;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "腾讯会员_" + GenerateUsername(8);
                member.Password = member.TradePassword = str;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = GenerateUsername();
                    member.Email = GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, parameters["CurrentOpenId"], parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(parameters["token"]))
            {
                HttpCookie cookie4 = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie4.Expires = DateTime.Now.AddMinutes(30.0);
                cookie4.Value = parameters["token"];
                HttpCookie cookie3 = cookie4;
                HttpContext.Current.Response.Cookies.Add(cookie3);
            }
            if (!string.IsNullOrEmpty(parameters["target_url"]))
            {
                Page.Response.Redirect(parameters["target_url"]);
            }
            Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }
    }
}

