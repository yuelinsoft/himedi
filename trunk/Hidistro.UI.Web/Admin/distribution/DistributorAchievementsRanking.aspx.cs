using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorAchievementsRanking)]
    public partial class DistributorAchievementsRanking : AdminPage
    {

        DateTime? dateEnd;
        DateTime? dateStart;


        private void BindDistributorRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            query.SortBy = "SaleTotals";
            query.SortOrder = SortAction.Desc;
            int totalDistributors = 0;
            OrderStatisticsInfo distributorStatistics = DistributorHelper.GetDistributorStatistics(query, out totalDistributors);
            grdDistributorStatistics.DataSource = distributorStatistics.OrderTbl;
            grdDistributorStatistics.DataBind();
            lblTotal.Money = distributorStatistics.TotalOfSearch;
            lblProfitTotal.Money = distributorStatistics.ProfitsOfSearch;
            pager.TotalRecords = totalDistributors;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.PageSize = pager.PageSize;
            query.SortBy = "SaleTotals";
            query.SortOrder = SortAction.Desc;
            DataTable orderTbl = DistributorHelper.GetDistributorStatisticsNoPage(query).OrderTbl;
            string s = string.Empty + "排行,分销商名称,交易量,交易金额,利润\r\n";
            foreach (DataRow row in orderTbl.Rows)
            {
                if (Convert.ToDecimal(row["SaleTotals"]) > 0M)
                {
                    s = s + row["IndexId"].ToString();
                }
                else
                {
                    s = s ?? "";
                }
                s = s + "," + row["UserName"].ToString();
                s = s + "," + row["PurchaseOrderCount"].ToString();
                s = s + "," + row["SaleTotals"].ToString();
                s = s + "," + row["Profits"].ToString();
                s = s + "\r\n";
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorRanking.CSV");
            Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Page.Response.ContentType = "application/octet-stream";
            Page.EnableViewState = false;
            Page.Response.Write(s);
            Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void grdDistributorStatistics_ReBindData(object sender)
        {
            ReBind(false);
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateStart"]))
                {
                    dateStart = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["dateEnd"]));
                }
                calendarStartDate.SelectedDate = dateStart;
                calendarEndDate.SelectedDate = dateEnd;
            }
            else
            {
                dateStart = calendarStartDate.SelectedDate;
                dateEnd = calendarEndDate.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdDistributorStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdDistributorStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindDistributorRanking();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEndDate.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

