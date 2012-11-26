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
    public partial class ConfirmBalanceDrawRequest : DistributorPage
    {

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SubsiteStoreHelper.DistroHasDrawRequest())
            {
                ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
            }
            else if (Session["BalanceDrawRequest"] != null)
            {
                BalanceDrawRequestInfo balanceDrawRequest = (BalanceDrawRequestInfo)Session["BalanceDrawRequest"];
                if (SubsiteStoreHelper.BalanceDrawRequest(balanceDrawRequest))
                {
                    message.Visible = true;
                }
                else
                {
                    ShowMsg("写入提现信息失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!base.IsPostBack)
            {
                BalanceDrawRequestInfo info = new BalanceDrawRequestInfo();
                if (Session["BalanceDrawRequest"] != null)
                {
                    info = (BalanceDrawRequestInfo)Session["BalanceDrawRequest"];
                    Distributor user = Users.GetUser(info.UserId) as Distributor;
                    litRealName.Text = user.RealName;
                    litName.Text = info.AccountName;
                    litBankName.Text = info.BankName;
                    litBankCode.Text = info.MerchantCode;
                    lblAmount.Money = info.Amount;
                }
                else
                {
                    base.GotoResourceNotFound();
                }
            }
        }
    }
}

