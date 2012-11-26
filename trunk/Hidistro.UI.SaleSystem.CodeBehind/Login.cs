namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    public class Login : HtmlTemplatedWebControl
    {
       IButton btnLogin;
       DropDownList ddlPlugins;
       static string ReturnURL = string.Empty;
       TextBox txtPassword;
       TextBox txtUserName;

        protected override void AttachChildControls()
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
            if (!string.IsNullOrEmpty(Page.Request["action"]) && (Page.Request["action"] == "Common_UserLogin"))
            {
                string str = UserLogin(Page.Request["username"], Page.Request["password"]);
                string str2 = string.IsNullOrEmpty(str) ? "Succes" : "Fail";
                Page.Response.Clear();
                Page.Response.ContentType = "application/json";
                Page.Response.Write("{\"Status\":\"" + str2 + "\",\"Msg\":\"" + str + "\"}");
                Page.Response.End();
            }
            txtUserName = (TextBox) FindControl("txtUserName");
            txtPassword = (TextBox) FindControl("txtPassword");
            btnLogin = ButtonManager.Create(FindControl("btnLogin"));
            ddlPlugins = (DropDownList) FindControl("ddlPlugins");
            if (ddlPlugins != null)
            {
                ddlPlugins.Items.Add(new ListItem("请选择登录方式", ""));
                IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
                if ((configedItems != null) && (configedItems.Count > 0))
                {
                    foreach (OpenIdSettingsInfo info in configedItems)
                    {
                        ddlPlugins.Items.Add(new ListItem(info.Name, info.OpenIdType));
                    }
                }
                ddlPlugins.SelectedIndexChanged += new EventHandler(ddlPlugins_SelectedIndexChanged);
            }

            if ( Page.Request.UrlReferrer != null &&!string.IsNullOrEmpty(Page.Request.UrlReferrer.OriginalString))
            {
                ReturnURL = Page.Request.UrlReferrer.OriginalString;
            }

            txtUserName.Focus();
            PageTitle.AddSiteNameTitle("用户登录", HiContext.Current.Context);
            btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string str = UserLogin(txtUserName.Text, txtPassword.Text);
                if (!string.IsNullOrEmpty(str))
                {
                    ShowMessage(str, false);
                }
                else
                {
                    string returnURL = Page.Request.QueryString["ReturnUrl"];
                    if (string.IsNullOrEmpty(returnURL))
                    {
                        returnURL = Globals.ApplicationPath + "/User/UserDefault.aspx";
                    }
                    else if (string.IsNullOrEmpty(ReturnURL))
                    {
                        returnURL = ReturnURL;
                    }
                    Page.Response.Redirect(returnURL);
                }
            }
        }

       void ddlPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPlugins.SelectedValue.Length > 0)
            {
                Page.Response.Redirect("OpenId/RedirectLogin.aspx?ot=" + ddlPlugins.SelectedValue);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                SkinName = "Skin-Login.html";
            }
            base.OnInit(e);
        }

       string UserLogin(string userName, string password)
        {
            string str = string.Empty;
            Member member = Users.GetUser(0, userName, false, true) as Member;
            if ((member == null) || member.IsAnonymous)
            {
                return "用户名或密码错误";
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                if (member.ParentUserId.HasValue)
                {
                    if (member.ParentUserId.Value == HiContext.Current.SiteSettings.UserId)
                    {
                        goto Label_00B2;
                    }
                }
                return "您不是本站会员，请您进行注册";
            }
            if (member.ParentUserId.HasValue && (member.ParentUserId.Value != 0))
            {
                return "您不是本站会员，请您进行注册";
            }
        Label_00B2:
            member.Password = password;
            switch (MemberProcessor.ValidLogin(member))
            {
                case LoginUserStatus.AccountPending:
                    return "用户账号还没有通过审核";

                case LoginUserStatus.InvalidCredentials:
                    return "用户名或密码错误";

                case LoginUserStatus.Success:
                {
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
                    member.GetUserCookie().WriteCookie(authCookie, 30, false);
                    ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                    CookieShoppingProvider.Instance().ClearShoppingCart();
                    HiContext.Current.User = member;
                    if (shoppingCart != null)
                    {
                        ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                    }
                    member.OnLogin();
                    return str;
                }
            }
            return "未知错误";
        }
    }
}

