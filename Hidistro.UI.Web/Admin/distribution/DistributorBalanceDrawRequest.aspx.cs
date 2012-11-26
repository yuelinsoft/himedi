using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorBalanceDrawRequest)]
    public partial class DistributorBalanceDrawRequest : AdminPage
    {
        DateTime? dataEnd;
        DateTime? dataStart;
        string searchKey;
        int? userId;

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        public void GetBalanceDrawRequest()
        {
            BalanceDrawRequestQuery query = new BalanceDrawRequestQuery();
            query.FromDate = dataStart;
            query.ToDate = dataEnd;
            query.UserName = txtUserName.Text;
            query.UserId = userId;
            query.PageIndex = pager.PageIndex;
            DbQueryResult distributorBalanceDrawRequests = DistributorHelper.GetDistributorBalanceDrawRequests(query);
            grdBalanceDrawRequest.DataSource = distributorBalanceDrawRequests.Data;
            grdBalanceDrawRequest.DataBind();
            pager.TotalRecords = distributorBalanceDrawRequests.TotalRecords;
        }

        private void grdBalanceDrawRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int userId = (int)grdBalanceDrawRequest.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "UnLineReCharge")
            {
                if (DistributorHelper.DealDistributorBalanceDrawRequest(userId, true))
                {
                    GetBalanceDrawRequest();
                }
                else
                {
                    ShowMsg("预付款提现申请操作失败", false);
                }
            }
            if (e.CommandName == "RefuseRequest")
            {
                if (DistributorHelper.DealDistributorBalanceDrawRequest(userId, false))
                {
                    GetBalanceDrawRequest();
                }
                else
                {
                    ShowMsg("预付款提现申请操作失败", false);
                }
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                int num;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userId"]) && int.TryParse(Page.Request.QueryString["userId"], out num))
                {
                    userId = new int?(num);
                    Distributor distributor = DistributorHelper.GetDistributor(userId.Value);
                    if (distributor != null)
                    {
                        txtUserName.Text = distributor.Username;
                    }
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchKey = base.Server.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataStart"]))
                {
                    dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataEnd"]))
                {
                    dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataEnd"])));
                }
                if (string.IsNullOrEmpty(Page.Request.QueryString["userId"]))
                {
                    txtUserName.Text = searchKey;
                }
                calendarStart.SelectedDate = dataStart;
                calendarEnd.SelectedDate = dataEnd;
            }
            else
            {
                searchKey = txtUserName.Text;
                dataStart = calendarStart.SelectedDate;
                dataEnd = calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDrawRequest.Click += new EventHandler(btnQueryBalanceDrawRequest_Click);
            grdBalanceDrawRequest.RowCommand += new GridViewCommandEventHandler(grdBalanceDrawRequest_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                GetBalanceDrawRequest();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", txtUserName.Text);
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

