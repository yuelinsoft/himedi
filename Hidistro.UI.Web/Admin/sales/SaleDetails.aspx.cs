using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SaleDetails)]
    public partial class SaleDetails : AdminPage
    {
        DateTime? endTime;
        DateTime? startTime;

        private void BindList()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.StartDate = startTime;
            query.EndDate = endTime;

            DbQueryResult saleOrderLineItemsStatistics = SalesHelper.GetSaleOrderLineItemsStatistics(query);
            grdOrderLineItem.DataSource = saleOrderLineItemsStatistics.Data;
            grdOrderLineItem.DataBind();
            pager.TotalRecords = saleOrderLineItemsStatistics.TotalRecords;
            grdOrderLineItem.PageSize = query.PageSize;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["startTime"]))
                {
                    startTime = new DateTime?(DateTime.Parse(Page.Request.QueryString["startTime"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["endTime"]))
                {
                    endTime = new DateTime?(DateTime.Parse(Page.Request.QueryString["endTime"]));
                }
                calendarStart.SelectedDate = startTime;
                calendarEnd.SelectedDate = endTime;
            }
            else
            {
                startTime = calendarStart.SelectedDate;
                endTime = calendarEnd.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindList();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("startTime", calendarStart.SelectedDate.ToString());
            queryStrings.Add("endTime", calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

