using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class BalanceDraw : DistributorPage
    {

        private void btnDrawNext_Click(object sender, EventArgs e)
        {
            if (SubsiteStoreHelper.DistroHasDrawRequest())
            {
                ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
            }
            else
            {
                decimal num;
                if (!decimal.TryParse(txtDrawBalance.Text.Trim(), out num))
                {
                    ShowMsg(" 提现金额只能是数值，限制在1000万以内", false);
                }
                else if (string.Compare(txtMerchantCodeCompare.Text.Trim(), txtMerchantCode.Text.Trim()) != 0)
                {
                    ShowMsg(" 两次输入的帐号不一致,请重新输入", false);
                }
                else if (num > ((decimal)lblUseableBalance.Money))
                {
                    ShowMsg(" 您的可用金额不足", false);
                }
                else if (string.IsNullOrEmpty(txtTradePassword.Text))
                {
                    ShowMsg("请输入交易密码", false);
                }
                else
                {
                    Distributor user = SubsiteStoreHelper.GetDistributor();
                    BalanceDrawRequestInfo info2 = new BalanceDrawRequestInfo();
                    info2.UserId = user.UserId;
                    info2.UserName = user.Username;
                    info2.RequestTime = DateTime.Now;
                    info2.MerchantCode = txtMerchantCode.Text.Trim();
                    info2.BankName = txtBankName.Text.Trim();
                    info2.Amount = num;
                    info2.AccountName = txtAccountName.Text.Trim();
                    info2.Remark = string.Empty;
                    BalanceDrawRequestInfo target = info2;
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDrawRequestInfo>(target, new string[] { "ValBalanceDrawRequestInfo" });
                    string msg = string.Empty;
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                        }
                        ShowMsg(msg, false);
                    }
                    else
                    {
                        user.TradePassword = txtTradePassword.Text;
                        if (Users.ValidTradePassword(user))
                        {
                            Session["BalanceDrawRequest"] = target;
                            base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ConfirmBalanceDrawRequest.aspx", true);
                        }
                        else
                        {
                            ShowMsg("交易密码不正确,请重新输入", false);
                        }
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnDrawNext.Click += new EventHandler(btnDrawNext_Click);
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

