
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class DeductSettings : AdminPage
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
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.ReferralDeduct = result;
                SettingsManager.Save(masterSettings);
                ShowMsg("成功修改了推荐人提成比例", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                txtDeduct.Text = masterSettings.ReferralDeduct.ToString();
            }
        }
    }
}

