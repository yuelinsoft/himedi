using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingBalanceDrawRequest : DistributorPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        int userId;
        string userName;

        public void BindBalanceDrawRequest()
        {
            BalanceDrawRequestQuery query = new BalanceDrawRequestQuery();
            if (userId > 0)
            {
                query.UserId = new int?(userId);
            }
            query.UserName = userName;
            query.FromDate = dateStart;
            query.ToDate = dateEnd;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            DbQueryResult balanceDrawRequests = UnderlingHelper.GetBalanceDrawRequests(query);
            grdBalanceDrawRequest.DataSource = balanceDrawRequests.Data;
            grdBalanceDrawRequest.DataBind();
            pager.TotalRecords = balanceDrawRequests.TotalRecords;
            pager1.TotalRecords = balanceDrawRequests.TotalRecords;
        }

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            ReloadUnderlingBalanceDrawRequest(true);
        }

        private void GetBalanceDrawRequestQuery()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userId"]))
                {
                    int.TryParse(Page.Request.QueryString["userId"], out userId);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = base.Server.UrlDecode(Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataStart"]))
                {
                    dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataEnd"]))
                {
                    dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataEnd"])));
                }
                txtUserName.Text = userName;
                calendarStart.SelectedDate = dateStart;
                calendarEnd.SelectedDate = dateEnd;
            }
            else
            {
                userName = txtUserName.Text.Trim();
            }
        }

        private void grdBalanceDrawRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int userId = (int)grdBalanceDrawRequest.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "UnLineReCharge")
            {
                if (UnderlingHelper.DealBalanceDrawRequest(userId, true))
                {
                    BindBalanceDrawRequest();
                }
                else
                {
                    ShowMsg("预付款提现申请操作失败", false);
                }
            }
            if (e.CommandName == "RefuseRequest")
            {
                if (UnderlingHelper.DealBalanceDrawRequest(userId, false))
                {
                    BindBalanceDrawRequest();
                }
                else
                {
                    ShowMsg("预付款提现申请操作失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnQueryBalanceDrawRequest.Click += new EventHandler(btnQueryBalanceDrawRequest_Click);
            grdBalanceDrawRequest.RowCommand += new GridViewCommandEventHandler(grdBalanceDrawRequest_RowCommand);
            GetBalanceDrawRequestQuery();
            if (!Page.IsPostBack)
            {
                if (int.TryParse(Page.Request.QueryString["userId"], out userId))
                {
                    Member member = UnderlingHelper.GetMember(userId);
                    if (member != null)
                    {
                        txtUserName.Text = member.Username;
                    }
                }
                BindBalanceDrawRequest();
            }
        }

        private void ReloadUnderlingBalanceDrawRequest(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtUserName.Text);
            queryStrings.Add("pageSize", hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("dataStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

