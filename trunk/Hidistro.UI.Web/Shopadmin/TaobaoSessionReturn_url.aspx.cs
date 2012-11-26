namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Core.Cryptography;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class TaobaoSessionReturn_url : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HiContext.Current.User.UserRole != UserRole.Distributor)
            {
                this.lblMsg.Text = "此系统只有分销商才能同步淘宝";
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (string.IsNullOrEmpty(masterSettings.Topkey) || string.IsNullOrEmpty(masterSettings.TopSecret))
                {
                    this.lblMsg.Text = "淘宝API接口参数没有设置";
                    this.litlink.Text = "<a href=\"" + Globals.ApplicationPath + "/Shopadmin/store/TaobaoSetting.aspx\">请点此处在淘宝同步设置选项中检查AppKey和AppSecret</a>。";
                }
                if (base.Request.QueryString["top_appkey"] != null)
                {
                    if (!VerifyTopResponse(base.Request.QueryString["top_parameters"], base.Request.QueryString["top_session"], base.Request.QueryString["top_sign"], Cryptographer.Decrypt(masterSettings.Topkey), Cryptographer.Decrypt(masterSettings.TopSecret)))
                    {
                        this.lblMsg.Text = "淘宝API接口参数错误";
                        this.litlink.Text = "<a href=\"" + Globals.ApplicationPath + "/Shopadmin/store/TaobaoSetting.aspx\">请点此处在淘宝同步设置选项中检查AppKey和AppSecret</a>。";
                    }
                    else
                    {
                        string cookieValue = base.Request.QueryString["top_session"];
                        SetCookies("TopSession_" + HiContext.Current.User.UserId.ToString(), cookieValue);
                        this.lblMsg.Text = "您已经成功登录了淘宝开放平台，并保存了其sessionkey";
                        this.litlink.Text = "您可以<a href=\"product/ToTaobaoProducts.aspx\">发布商品到淘宝</a>或<a href=\"purchaseOrder/TaobaoOrders.aspx\">从淘宝获取订单</a>了";
                        HttpCookie cookie = HiContext.Current.Context.Request.Cookies["TaobaoSessionReturnUrl_" + HiContext.Current.User.UserId.ToString()];
                        if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
                        {
                            base.Response.Redirect(cookie.Value);
                        }
                    }
                }
                else
                {
                    base.Response.Redirect("http://container.api.taobao.com/container?appkey=" + masterSettings.Topkey, true);
                }
            }
        }

        private static void SetCookies(string cookieName, string cookieValue)
        {
            HttpCookie cookie2 = new HttpCookie(cookieName);
            cookie2.Expires = DateTime.Now.AddDays(1.0);
            cookie2.Value = cookieValue;
            HttpCookie cookie = cookie2;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static bool VerifyTopResponse(string topParams, string topSession, string topSign, string appKey, string appSecret)
        {
            StringBuilder builder = new StringBuilder();
            new MD5CryptoServiceProvider();
            builder.Append(appKey).Append(topParams).Append(topSession).Append(appSecret);
            return (Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()))) == topSign);
        }
    }
}

