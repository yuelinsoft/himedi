using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberRanking)]
    public partial class MemberRanking : AdminPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        string sortBy = "SaleTotals";

        private void BindMemberRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            query.SortBy = sortBy;
            query.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable memberStatistics = SalesHelper.GetMemberStatistics(query, out totalProductSales);
            grdProductSaleStatistics.DataSource = memberStatistics;
            grdProductSaleStatistics.DataBind();
            pager1.TotalRecords = pager.TotalRecords = totalProductSales;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.SortBy = sortBy;
            query.SortOrder = SortAction.Desc;
            DataTable memberStatisticsNoPage = SalesHelper.GetMemberStatisticsNoPage(query);
            string s = string.Empty + "会员,订单数,消费金额\r\n";
            foreach (DataRow row in memberStatisticsNoPage.Rows)
            {
                s = s + row["UserName"].ToString();
                s = s + "," + row["OrderCount"].ToString();
                s = s + "," + row["SaleTotals"].ToString() + "\r\n";
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberRanking.csv");
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

        private void grdProductSaleStatistics_ReBindData(object sender)
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
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sortBy"]))
                {
                    sortBy = base.Server.UrlDecode(Page.Request.QueryString["sortBy"]);
                }
                calendarStartDate.SelectedDate = dateStart;
                calendarEndDate.SelectedDate = dateEnd;
                ddlSort.SelectedValue = sortBy;
            }
            else
            {
                dateStart = calendarStartDate.SelectedDate;
                dateEnd = calendarEndDate.SelectedDate;
                sortBy = ddlSort.SelectedValue;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdProductSaleStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlSort.Items.Add(new ListItem("消费金额", "SaleTotals"));
            ddlSort.Items.Add(new ListItem("订单数", "OrderCount"));
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindMemberRanking();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("sortBy", ddlSort.SelectedValue);
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

