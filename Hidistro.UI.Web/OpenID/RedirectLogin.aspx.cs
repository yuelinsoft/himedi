using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.OpenID
{

    public partial class RedirectLogin : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Context.Request.IsAuthenticated)
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

            string fullName = base.Request.QueryString["ot"];

            if (OpenIdPlugins.Instance().GetPluginItem(fullName) == null)
            {

                lblMsg.Text = "没有找到对应的插件，<a href=\"" + Globals.GetSiteUrls().Home + "\">返回首页</a>。";

            }
            else
            {

                OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(fullName);

                if (openIdSettings == null)
                {

                    lblMsg.Text = "请先配置此插件所需的信息，<a href=\"" + Globals.GetSiteUrls().Home + "\">返回首页</a>。";

                }
                else
                {

                    string returnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("OpenIdEntry_url", new object[] { fullName }));

                    OpenIdService.CreateInstance(fullName, Cryptographer.Decrypt(openIdSettings.Settings), returnUrl).Post();

                }

            }

        }

    }

}


