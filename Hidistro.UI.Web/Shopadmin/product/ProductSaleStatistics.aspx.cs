    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ProductSaleStatistics : DistributorPage
    {

        private void BindProductSaleStatistics()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            query.SortBy = "BuyPercentage";
            query.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productVisitAndBuyStatistics = SubsiteSalesHelper.GetProductVisitAndBuyStatistics(query, out totalProductSales);
            grdProductSaleStatistics.DataSource = productVisitAndBuyStatistics;
            grdProductSaleStatistics.DataBind();
            pager.TotalRecords = totalProductSales;
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            ReBind(false);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdProductSaleStatistics_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindProductSaleStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

