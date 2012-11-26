using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingRanking : DistributorPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        string sortBy = "SaleTotals";

        private void BindUnderlingRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            query.SortBy = sortBy;
            query.SortOrder = SortAction.Desc;
            int total = 0;
            DataTable underlingStatistics = UnderlingHelper.GetUnderlingStatistics(query, out total);
            grdProductSaleStatistics.DataSource = underlingStatistics;
            grdProductSaleStatistics.DataBind();
            calendarStartDate.SelectedDate = query.StartDate;
            calendarEndDate.SelectedDate = query.EndDate;
            pager.TotalRecords = total;
            pager1.TotalRecords = total;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = dateStart;
            query.EndDate = dateEnd;
            query.SortBy = sortBy;
            query.SortOrder = SortAction.Desc;
            DataTable underlingStatisticsNoPage = UnderlingHelper.GetUnderlingStatisticsNoPage(query);
            string s = string.Empty + "会员,订单数,消费金额\r\n";
            foreach (DataRow row in underlingStatisticsNoPage.Rows)
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
            ReloadUnderlingRanking(true);
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            ReloadUnderlingRanking(false);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdProductSaleStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
            ddlSort.Items.Add(new ListItem("消费金额", "SaleTotals"));
            ddlSort.Items.Add(new ListItem("订单数", "OrderCount"));
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindUnderlingRanking();
            }
        }

        private void ReloadUnderlingRanking(bool isSeach)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("sortBy", ddlSort.SelectedValue);
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            if (!isSeach)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

