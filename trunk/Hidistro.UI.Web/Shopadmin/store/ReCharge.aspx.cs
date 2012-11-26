using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ReCharge : DistributorPage
    {


        private void btnReChargeNext_Click(object sender, EventArgs e)
        {
            if (radioBtnPayment.Items.Count == 0)
            {
                ShowMsg("主站没有添加支付方式", false);
            }
            else if (radioBtnPayment.SelectedValue == null)
            {
                ShowMsg("请选择支付方式", false);
            }
            else
            {
                decimal num;
                int length = 0;
                if (txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
                {
                    length = txtReChargeBalance.Text.Trim().Substring(txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
                }
                if (!(decimal.TryParse(txtReChargeBalance.Text.Trim(), out num) && (length <= 2)))
                {
                    ShowMsg("充值金额只能是数值，且不能超过2位小数", false);
                }
                else if ((num <= 0M) || (num > 10000000M))
                {
                    ShowMsg("充值金额只能是非负数值，限制在1000万以内", false);
                }
                else
                {
                    base.Response.Redirect(Globals.ApplicationPath + string.Format("/Shopadmin/store/ReChargeConfirm.aspx?modeId={0}&blance={1}", radioBtnPayment.SelectedValue, num), true);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnReChargeNext.Click += new EventHandler(btnReChargeNext_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                lblUseableBalance.Money = myAccountSummary.UseableBalance;
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                litRealName.Text = distributor.RealName;
            }
        }
    }
}

