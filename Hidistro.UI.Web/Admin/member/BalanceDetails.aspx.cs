
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
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberAccount)]
    public partial class BalanceDetails : AdminPage
    {

        DateTime? dataEnd;
        DateTime? dataStart;
        int typeId;
        int userId;

        private void BindBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = dataStart;
            query.ToDate = dataEnd;
            query.TradeType = (TradeTypes)typeId;
            query.PageIndex = pager.PageIndex;
            query.UserId = new int?(userId);
            query.PageSize = pager.PageSize;
            DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
            grdBalanceDetails.DataSource = balanceDetails.Data;
            grdBalanceDetails.DataBind();
            pager.TotalRecords = balanceDetails.TotalRecords;
            pager1.TotalRecords = balanceDetails.TotalRecords;
        }

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void lbtnDrawRequest_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/BalanceDrawRequest.aspx?userId=" + userId), true);
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userId"]))
                {
                    int.TryParse(Page.Request.QueryString["userId"], out userId);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["typeId"]))
                {
                    int.TryParse(Page.Request.QueryString["typeId"], out typeId);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataStart"]))
                {
                    dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataEnd"]))
                {
                    dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataEnd"])));
                }
                ViewState["UserId"] = userId;
                dropTradeType.DataBind();
                dropTradeType.SelectedValue = (TradeTypes)typeId;
                calendarStart.SelectedDate = dataStart;
                calendarEnd.SelectedDate = dataEnd;
            }
            else
            {
                userId = (int)ViewState["UserId"];
                typeId = (int)dropTradeType.SelectedValue;
                dataStart = calendarStart.SelectedDate;
                dataEnd = calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDetails.Click += new EventHandler(btnQueryBalanceDetails_Click);
            lbtnDrawRequest.Click += new EventHandler(lbtnDrawRequest_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                if (userId != 0)
                {
                    Member member = MemberHelper.GetMember(userId);
                    if (member != null)
                    {
                        litBalance.Text = member.Balance.ToString("F2");
                        litDrawBalance.Text = member.RequestBalance.ToString("F2");
                        litUserBalance.Text = (member.Balance - member.RequestBalance).ToString("F2");
                        MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(member.GradeId);
                        if (memberGrade != null)
                        {
                            litUser.Text = member.Username + "(" + memberGrade.Name + ")";
                        }
                        else
                        {
                            litUser.Text = member.Username;
                        }
                    }
                }
                BindBalanceDetails();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userId", ViewState["UserId"].ToString());
            queryStrings.Add("typeId", ((int)dropTradeType.SelectedValue).ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("dataStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", calendarEnd.SelectedDate.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

