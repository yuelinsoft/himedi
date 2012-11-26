using Hidistro.Entities.Members;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyAccountSummary : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                lblAccountAmount.Money = myAccountSummary.AccountAmount;
                lblFreezeBalance.Money = myAccountSummary.FreezeBalance;
                lblUseableBalance.Money = myAccountSummary.UseableBalance;
            }
        }
    }
}

