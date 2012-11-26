using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class TaobaoSetting : DistributorPage
    {

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if ((txtTopkey.Text.Trim().Length != 0) && (txtTopkey.Text.Trim().Length != 8))
            {
                ShowMsg("淘宝Appkey不能为空，为8位数字ID", false);
            }
            else if ((txtTopSecret.Text.Trim().Length != 0) && (txtTopSecret.Text.Trim().Length != 0x20))
            {
                ShowMsg("淘宝AppSecret不能为空,为32位字符", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);// SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(txtTopkey.Text.Trim()))
                {
                    masterSettings.Topkey = Cryptographer.Encrypt(txtTopkey.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtTopSecret.Text.Trim()))
                {
                    masterSettings.TopSecret = Cryptographer.Encrypt(txtTopSecret.Text.Trim());
                }
                SettingsManager.Save(masterSettings);
                ShowMsg("成功保存了淘宝同步设置", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            litReturnUrl.Text = Globals.FullPath(Globals.ApplicationPath + "/Shopadmin/TaobaoSessionReturn_url.aspx");
            if (!Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId); ;// SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.Topkey))
                {
                    txtTopkey.Text = Cryptographer.Decrypt(masterSettings.Topkey);
                }
                if (!string.IsNullOrEmpty(masterSettings.TopSecret))
                {
                    txtTopSecret.Text = Cryptographer.Decrypt(masterSettings.TopSecret);
                }
            }
        }
    }
}

