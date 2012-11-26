using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorReCharge)]
    public partial class DistributorReCharge : AdminPage
    {
        int userId;

        private void btnReChargeOK_Click(object sender, EventArgs e)
        {
            decimal num;
            int length = 0;
            if (txtReCharge.Text.Trim().IndexOf(".") > 0)
            {
                length = txtReCharge.Text.Trim().Substring(txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!(decimal.TryParse(txtReCharge.Text.Trim(), out num) && (length <= 2)))
            {
                ShowMsg("本次充值要给当前客户加款的金额只能是数值，且不能超过2位小数", false);
            }
            else if ((num < -10000000M) || (num > 10000000M))
            {
                ShowMsg("金额大小必须在正负1000万之间", false);
            }
            else
            {
                Distributor user = Users.GetUser(userId, false) as Distributor;
                if (user == null)
                {
                    ShowMsg("此分销商已经不存在", false);
                }
                else
                {
                    decimal num3 = num + user.Balance;
                    BalanceDetailInfo info2 = new BalanceDetailInfo();
                    info2.UserId = userId;
                    info2.UserName = user.Username;
                    info2.TradeDate = DateTime.Now;
                    info2.TradeType = TradeTypes.BackgroundAddmoney;
                    info2.Income = new decimal?(num);
                    info2.Balance = num3;
                    info2.Remark = Globals.HtmlEncode(txtRemarks.Text.Trim());
                    BalanceDetailInfo target = info2;
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDetailInfo>(target, new string[] { "ValBalanceDetail" });
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
                        if (DistributorHelper.AddBalance(target))
                        {
                            ShowMsg(string.Format("本次充值已成功，充值金额：{0}", num), true);
                        }
                        txtReCharge.Text = string.Empty;
                        txtRemarks.Text = string.Empty;
                        lblUseableBalance.Money = num3;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnReChargeOK.Click += new EventHandler(btnReChargeOK_Click);
                if (!Page.IsPostBack)
                {
                    Distributor distributor = DistributorHelper.GetDistributor(userId);
                    if (distributor == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litUserNames.Text = distributor.Username;
                        lblUseableBalance.Money = distributor.Balance - distributor.RequestBalance;
                    }
                }
            }
        }
    }
}

