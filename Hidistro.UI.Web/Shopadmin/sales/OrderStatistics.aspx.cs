using ASPNET.WebControls;
using Hidistro.Core;
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
    public partial class OrderStatistics : DistributorPage
    {

        DateTime? dateEnd;
        DateTime? dateStart;
        string orderId;
        string shipTo;
        string userName;

        private void BindUserOrderStatistics()
        {
            UserOrderQuery userOrder = new UserOrderQuery();
            userOrder.UserName = userName;
            userOrder.ShipTo = shipTo;
            userOrder.StartDate = dateStart;
            userOrder.EndDate = dateEnd;
            userOrder.OrderId = orderId;
            userOrder.PageSize = pager.PageSize;
            userOrder.PageIndex = pager.PageIndex;
            userOrder.SortBy = grdUserOrderStatistics.SortOrderBy;

            if (grdUserOrderStatistics.SortOrder.ToLower() == "desc")
            {
                userOrder.SortOrder = SortAction.Desc;
            }
            OrderStatisticsInfo userOrders = SubsiteSalesHelper.GetUserOrders(userOrder);
            grdUserOrderStatistics.DataSource = userOrders.OrderTbl;
            grdUserOrderStatistics.DataBind();
            pager.TotalRecords = userOrders.TotalCount;
            lblPageCount.Text = string.Format("当前页共计<span style=\"color:red;\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span style=\"color:red;\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span style=\"color:red;\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
            lblSearchCount.Text = string.Format("当前查询结果共计<span style=\"color:red;\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span style=\"color:red;\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span style=\"color:red;\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            UserOrderQuery userOrder = new UserOrderQuery();
            userOrder.UserName = userName;
            userOrder.ShipTo = shipTo;
            userOrder.StartDate = dateStart;
            userOrder.EndDate = dateEnd;
            userOrder.OrderId = orderId;
            userOrder.PageSize = pager.PageSize;
            userOrder.PageIndex = pager.PageIndex;
            userOrder.SortBy = grdUserOrderStatistics.SortOrderBy;

            if (grdUserOrderStatistics.SortOrder.ToLower() == "desc")
            {
                userOrder.SortOrder = SortAction.Desc;
            }
            OrderStatisticsInfo userOrdersNoPage = SubsiteSalesHelper.GetUserOrdersNoPage(userOrder);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            builder.AppendLine("<td>订单号</td>");
            builder.AppendLine("<td>下单时间</td>");
            builder.AppendLine("<td>总订单金额</td>");
            builder.AppendLine("<td>用户名</td>");
            builder.AppendLine("<td>收货人</td>");
            builder.AppendLine("<td>利润</td>");
            builder.AppendLine("</tr>");
            foreach (DataRow row in userOrdersNoPage.OrderTbl.Rows)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["OrderDate"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Total"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["UserName"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["ShipTo"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Profits"].ToString() + "</td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("<tr>");
            builder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
            builder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
            builder.AppendLine("<td>订单毛利润共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
            builder.AppendLine("<td></td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "utf-8";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=UserOrderStatistics.xls");
            Page.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Page.Response.ContentType = "application/ms-excel";
            Page.EnableViewState = false;
            Page.Response.Write(builder.ToString());
            Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void grdUserOrderStatistics_ReBindData(object sender)
        {
            ReBind(false);
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = base.Server.UrlDecode(Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["shipTo"]))
                {
                    shipTo = base.Server.UrlDecode(Page.Request.QueryString["shipTo"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateStart"]))
                {
                    dateStart = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(DateTime.Parse(Page.Request.QueryString["dateEnd"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["orderId"]))
                {
                    orderId = Globals.UrlDecode(Page.Request.QueryString["orderId"]);
                }
                txtUserName.Text = userName;
                txtShipTo.Text = shipTo;
                calendarStartDate.SelectedDate = dateStart;
                calendarEndDate.SelectedDate = dateEnd;
                txtOrderId.Text = orderId;
            }
            else
            {
                userName = txtUserName.Text;
                shipTo = txtShipTo.Text;
                dateStart = calendarStartDate.SelectedDate;
                dateEnd = calendarEndDate.SelectedDate;
                orderId = txtOrderId.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdUserOrderStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdUserOrderStatistics_ReBindData);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindUserOrderStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtUserName.Text);
            queryStrings.Add("shipTo", txtShipTo.Text);
            queryStrings.Add("dateStart", calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("orderId", txtOrderId.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

