using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyDeductSettings : DistributorPage
    {


        private void btnOK_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (!int.TryParse(txtDeduct.Text.Trim(), out result))
            {
                ShowMsg("您输入的推荐人提成比例格式不对！", false);
            }
            else
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                siteSettings.ReferralDeduct = result;
                SettingsManager.Save(siteSettings);
                ShowMsg("成功修改了推荐人提成比例", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!Page.IsPostBack)
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                txtDeduct.Text = siteSettings.ReferralDeduct.ToString();
            }
        }
    }
}

