using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
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
    [PrivilegeCheck(Privilege.PurchaseOrderStatistics)]
    public partial class PurchaseOrderStatistics : AdminPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        string userName;

        private void BindPurchaseOrderStatistics()
        {
            UserOrderQuery order = new UserOrderQuery();
            order.UserName = userName;
            order.StartDate = dateStart;
            order.EndDate = dateEnd;
            order.PageSize = 10;
            order.PageIndex = pager.PageIndex;
            order.SortBy = grdPurchaseOrderStatistics.SortOrderBy;
            if (grdPurchaseOrderStatistics.SortOrder.ToLower() == "desc")
            {
                order.SortOrder = SortAction.Desc;
            }
            OrderStatisticsInfo purchaseOrders = DistributorHelper.GetPurchaseOrders(order);
            grdPurchaseOrderStatistics.DataSource = purchaseOrders.OrderTbl;
            grdPurchaseOrderStatistics.DataBind();
            pager.TotalRecords = purchaseOrders.TotalCount;
            lblPageCount.Text = string.Format("当前页共计<span style=\"color:red;\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span style=\"color:red;\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span style=\"color:red;\">{2}</span>元 ", purchaseOrders.OrderTbl.Rows.Count, Globals.FormatMoney(purchaseOrders.TotalOfPage), Globals.FormatMoney(purchaseOrders.ProfitsOfPage));
            lblSearchCount.Text = string.Format("当前查询结果共计<span style=\"color:red;\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span style=\"color:red;\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span style=\"color:red;\">{2}</span>元 ", purchaseOrders.TotalCount, Globals.FormatMoney(purchaseOrders.TotalOfSearch), Globals.FormatMoney(purchaseOrders.ProfitsOfSearch));
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            UserOrderQuery query2 = new UserOrderQuery();
            query2.UserName = userName;
            query2.StartDate = dateStart;
            query2.EndDate = dateEnd;
            query2.PageIndex = pager.PageIndex;
            query2.SortBy = grdPurchaseOrderStatistics.SortOrderBy;
            UserOrderQuery order = query2;
            if (grdPurchaseOrderStatistics.SortOrder.ToLower() == "desc")
            {
                order.SortOrder = SortAction.Desc;
            }
            OrderStatisticsInfo purchaseOrdersNoPage = DistributorHelper.GetPurchaseOrdersNoPage(order);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            builder.AppendLine("<td>采购单号</td>");
            builder.AppendLine("<td>订单号</td>");
            builder.AppendLine("<td>下单时间</td>");
            builder.AppendLine("<td>分销商名称</td>");
            builder.AppendLine("<td>采购单金额</td>");
            builder.AppendLine("<td>利润</td>");
            builder.AppendLine("</tr>");
            foreach (DataRow row in purchaseOrdersNoPage.OrderTbl.Rows)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["PurchaseOrderId"].ToString() + "</td>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseDate"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Distributorname"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseTotal"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseProfit"].ToString() + "</td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("<tr>");
            builder.AppendLine("<td>当前查询结果共计," + purchaseOrdersNoPage.TotalCount + "</td>");
            builder.AppendLine("<td>采购单金额共计," + purchaseOrdersNoPage.TotalOfSearch + "</td>");
            builder.AppendLine("<td>采购单毛利润共计," + purchaseOrdersNoPage.ProfitsOfSearch + "</td>");
            builder.AppendLine("<td></td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "UTF-8";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=PurchaseOrderStatistics.xls");
            Page.Response.ContentEncoding = Encoding.UTF8;
            Page.Response.ContentType = "application/ms-excel";
            Page.EnableViewState = false;
            Page.Response.Write(builder.ToString());
            Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void grdPurchaseOrderStatistics_ReBindData(object sender)
        {
            ReBind(false);
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["orderId"]))
                {
                    userName = base.Server.UrlDecode(Page.Request.QueryString["orderId"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = base.Server.UrlDecode(Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateStart"]))
                {
                    dateStart = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateEnd"]));
                }
                txtUserName.Text = userName;
                calendarStartDate.SelectedDate = dateStart;
                calendarEndDate.SelectedDate = dateEnd;
            }
            else
            {
                userName = txtUserName.Text;
                dateStart = calendarStartDate.SelectedDate;
                dateEnd = calendarEndDate.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdPurchaseOrderStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdPurchaseOrderStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindPurchaseOrderStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtUserName.Text);
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

