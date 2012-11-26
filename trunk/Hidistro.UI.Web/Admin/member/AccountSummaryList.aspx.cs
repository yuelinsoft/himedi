using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberAccount)]
    public partial class AccountSummaryList : AdminPage
    {
        string realName;
        string searchKey;

        private void btnAddBalance_Click(object sender, EventArgs e)
        {
            decimal num;
            int length = 0;
            if (txtReCharge.Text.Trim().IndexOf(".") > 0)
            {
                length = txtReCharge.Text.Trim().Substring(txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
            }
            int userId = int.Parse(currentUserId.Value);
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
                Member user = Users.GetUser(userId, false) as Member;
                if (!((user != null) && user.IsOpenBalance))
                {
                    ShowMsg("本次充值已失败，该用户不存在或预付款还没有开通", false);
                }
                else
                {
                    decimal num4 = num + user.Balance;
                    BalanceDetailInfo info2 = new BalanceDetailInfo();
                    info2.UserId = userId;
                    info2.UserName = user.Username;
                    info2.TradeDate = DateTime.Now;
                    info2.TradeType = TradeTypes.BackgroundAddmoney;
                    info2.Income = new decimal?(num);
                    info2.Balance = num4;
                    info2.Remark = Globals.HtmlEncode(txtRemark.Text.Trim());
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
                    else if (MemberHelper.AddBalance(target))
                    {
                        txtReCharge.Text = "";
                        ReBind(false);
                    }
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        public void GetBalance()
        {
            MemberQuery query = new MemberQuery();
            query.Username = searchKey;
            query.Realname = realName;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            DbQueryResult memberBlanceList = MemberHelper.GetMemberBlanceList(query);
            grdMemberAccountList.DataSource = memberBlanceList.Data;
            grdMemberAccountList.DataBind();
            pager.TotalRecords = pager.TotalRecords = memberBlanceList.TotalRecords;
            pager1.TotalRecords = pager.TotalRecords = memberBlanceList.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchKey = base.Server.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["realName"]))
                {
                    realName = base.Server.UrlDecode(Page.Request.QueryString["realName"]);
                }
                txtUserName.Text = searchKey;
                txtRealName.Text = realName;
            }
            else
            {
                searchKey = txtUserName.Text.Trim();
                realName = txtRealName.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnAddBalance.Click += new EventHandler(btnAddBalance_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!base.IsPostBack)
            {
                GetBalance();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", base.Server.UrlEncode(txtUserName.Text));
            queryStrings.Add("realName", base.Server.UrlEncode(txtRealName.Text));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }
    }
}

