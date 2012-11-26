
using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
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
    [PrivilegeCheck(Privilege.DistributorAccount)]
    public partial class DistributorBalanceDetails : AdminPage
    {
        DateTime? dataEnd;
        DateTime? dataStart;
        TradeTypes tradeType;
        int userId;

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void ddlTradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReBind(false);
        }

        private void GetBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = dataStart;
            query.ToDate = dataEnd;
            query.UserId = new int?(userId);
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.TradeType = tradeType;
            DbQueryResult distributorBalanceDetails = DistributorHelper.GetDistributorBalanceDetails(query);
            grdBalanceDetails.DataSource = distributorBalanceDetails.Data;
            grdBalanceDetails.DataBind();
            pager.TotalRecords = distributorBalanceDetails.TotalRecords;
            pager1.TotalRecords = distributorBalanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataStart"]))
                {
                    dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataEnd"]))
                {
                    dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataEnd"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["tradeType"]))
                {
                    int result = 0;
                    int.TryParse(Page.Request.QueryString["tradeType"], out result);
                    tradeType = (TradeTypes)result;
                }
                ddlTradeType.DataBind();
                ddlTradeType.SelectedValue = tradeType;
                calendarStart.SelectedDate = dataStart;
                calendarEnd.SelectedDate = dataEnd;
            }
            else
            {
                tradeType = ddlTradeType.SelectedValue;
                dataStart = calendarStart.SelectedDate;
                dataEnd = calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDetails.Click += new EventHandler(btnQueryBalanceDetails_Click);
            ddlTradeType.SelectedIndexChanged += new EventHandler(ddlTradeType_SelectedIndexChanged);
            ddlTradeType.AutoPostBack = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                LoadParameters();
                if (!base.IsPostBack)
                {
                    Distributor distributor = DistributorHelper.GetDistributor(userId);
                    if (distributor == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litUser.Text = distributor.Username;
                        GetBalanceDetails();
                    }
                }
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userId", Page.Request.QueryString["userId"]);
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("dataStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", calendarEnd.SelectedDate.ToString());
            queryStrings.Add("tradeType", ((int)ddlTradeType.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

