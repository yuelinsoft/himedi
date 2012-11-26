using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SiteUrlDetails : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litUserName.Text = HiContext.Current.User.Username;
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings == null)
                {
                    GotoResourceNotFound();
                }
                else
                {
                    litFirstSiteUrl.Text = siteSettings.SiteUrl;
                    litFirstRecordCode.Text = siteSettings.RecordCode;
                    litSecondSiteUrl.Text = siteSettings.SiteUrl2;
                    litSecondRecordCode.Text = siteSettings.RecordCode2;
                }
            }
        }
    }
}

