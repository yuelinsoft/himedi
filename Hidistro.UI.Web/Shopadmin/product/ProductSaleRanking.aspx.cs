using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ProductSaleRanking : DistributorPage
    {

        DateTime? dateEnd;
        DateTime? dateStart;

        private void BindProductSaleRanking()
        {
            SaleStatisticsQuery productSale = new SaleStatisticsQuery();
            productSale.StartDate = dateStart;
            productSale.EndDate = dateEnd;
            productSale.PageSize = pager.PageSize;
            productSale.PageIndex = pager.PageIndex;
            productSale.SortBy = "ProductSaleCounts";
            productSale.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productSales = SubsiteSalesHelper.GetProductSales(productSale, out totalProductSales);
            grdProductSaleStatistics.DataSource = productSales;
            grdProductSaleStatistics.DataBind();
            pager.TotalRecords = totalProductSales;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery productSale = new SaleStatisticsQuery();
            productSale.StartDate = dateStart;
            productSale.EndDate = dateEnd;
            productSale.PageSize = pager.PageSize;
            productSale.SortBy = "ProductSaleCounts";
            productSale.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productSalesNoPage = SubsiteSalesHelper.GetProductSalesNoPage(productSale, out totalProductSales);
            string s = string.Empty + "排行,商品名称,商家编码,销售量,销售额,利润\r\n";
            foreach (DataRow row in productSalesNoPage.Rows)
            {
                s = s + row["IDOfSaleTotals"].ToString();
                s = s + "," + row["ProductName"].ToString();
                s = s + "," + row["SKU"].ToString();
                s = s + "," + row["ProductSaleCounts"].ToString();
                s = s + "," + row["ProductSaleTotals"].ToString();
                s = s + "," + row["ProductProfitsTotals"].ToString() + "\r\n";
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ProductSaleRanking.csv");
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
                    dateStart = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateEnd"]));
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
            grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdProductSaleStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindProductSaleRanking();
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

