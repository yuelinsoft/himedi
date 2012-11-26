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
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductSaleStatistics)]
    public partial class ProductSaleStatistics : AdminPage
    {
        private void BindProductSaleStatistics()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            query.SortBy = "BuyPercentage";
            query.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productVisitAndBuyStatistics = SalesHelper.GetProductVisitAndBuyStatistics(query, out totalProductSales);
            grdProductSaleStatistics.DataSource = productVisitAndBuyStatistics;
            grdProductSaleStatistics.DataBind();
            pager.TotalRecords = totalProductSales;
            hidPageSize.Value = pager.PageSize.ToString();
            hidPageIndex.Value = pager.PageIndex.ToString();
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

