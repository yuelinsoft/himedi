using Hidistro.Core.Cryptography;
using Hidistro.Membership.Context;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Shopadmin.product
{
    /// <summary>
    /// 发布到淘宝
    /// </summary>
    public partial class PublishProductToTaobao : Page
    {
        protected string productIds = "";
        protected string appkey = "";
        protected string sessionkey = "";
        protected string appsecret = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            productIds = Request.QueryString["productIds"];

            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["TopSession_" + HiContext.Current.User.UserId.ToString()];

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            appkey = Cryptographer.Decrypt(masterSettings.Topkey);

            appsecret = Cryptographer.Decrypt(masterSettings.TopSecret);

            if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
            {

                sessionkey = cookie.Value;

            }

        }

    }

}

