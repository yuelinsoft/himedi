namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UserOrders : MemberTemplatedWebControl
    {
       IButton btnSearch;
       WebCalendar calendarEndDate;
       WebCalendar calendarStartDate;
       OrderStautsDropDownList dropOrderStatus;
       Common_OrderManage_OrderList listOrders;
       Literal litOrderTotal;
       Pager pager;
       TextBox txtOrderId;

        protected override void AttachChildControls()
        {
            this.calendarStartDate = (WebCalendar) this.FindControl("calendarStartDate");
            this.calendarEndDate = (WebCalendar) this.FindControl("calendarEndDate");
            this.txtOrderId = (TextBox) this.FindControl("txtOrderId");
            this.dropOrderStatus = (OrderStautsDropDownList) this.FindControl("dropOrderStatus");
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
            this.listOrders = (Common_OrderManage_OrderList) this.FindControl("Common_OrderManage_OrderList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnSearch.Click += new EventHandler(this.lbtnSearch_Click);
            this.listOrders.ItemDataBound += new Common_OrderManage_OrderList.DataBindEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.ItemCommand += new Common_OrderManage_OrderList.CommandEventHandler(this.listOrders_ItemCommand);
            this.listOrders.ReBindData += new Common_OrderManage_OrderList.ReBindDataEventHandler(this.listOrders_ReBindData);
            PageTitle.AddSiteNameTitle("我的订单", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                this.BindOrders();
            }
        }

       void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
            this.listOrders.DataSource = userOrder.Data;
            this.listOrders.DataBind();
            this.txtOrderId.Text = orderQuery.OrderId;
            this.dropOrderStatus.SelectedValue = orderQuery.Status;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.pager.TotalRecords = userOrder.TotalRecords;
        }

       OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                query.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["startDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["endDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out result))
                {
                    query.Status = (OrderStatus) result;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                query.SortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out num2))
                {
                    query.SortOrder = (SortAction) num2;
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            return query;
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadUserOrders(true);
        }

        protected void listOrders_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if ((e.CommandName == "FINISH_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (TradeHelper.ConfirmOrderFinish(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMessage("成功的完成了该订单", true);
                    }
                    else
                    {
                        this.ShowMessage("完成订单失败", false);
                    }
                }
                if ((e.CommandName == "CLOSE_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
                {
                    if (TradeHelper.CloseOrder(orderInfo.OrderId))
                    {
                        this.BindOrders();
                        this.ShowMessage("成功的关闭了该订单", true);
                    }
                    else
                    {
                        this.ShowMessage("关闭订单失败", false);
                    }
                }
            }
        }

        protected void listOrders_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Row.DataItem, "OrderStatus");
                HyperLink link = (HyperLink) e.Row.FindControl("hplinkorderreview");
                HyperLink link2 = (HyperLink) e.Row.FindControl("hlinkPay");
                ImageLinkButton button = (ImageLinkButton) e.Row.FindControl("lkbtnConfirmOrder");
                ImageLinkButton button2 = (ImageLinkButton) e.Row.FindControl("lkbtnCloseOrder");
                if (link != null)
                {
                    link.Visible = status == OrderStatus.Finished;
                }
                link2.Visible = status == OrderStatus.WaitBuyerPay;
                button.Visible = status == OrderStatus.SellerAlreadySent;
                button2.Visible = status == OrderStatus.WaitBuyerPay;
            }
        }

        protected void listOrders_ReBindData(object sender)
        {
            this.ReloadUserOrders(false);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserOrders.html";
            }
            base.OnInit(e);
        }

       void ReloadUserOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("orderId", this.txtOrderId.Text.Trim());
            queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("orderStatus", ((int) this.dropOrderStatus.SelectedValue).ToString());
            queryStrings.Add("sortBy", this.listOrders.SortOrderBy);
            queryStrings.Add("sortOrder", ((int) this.listOrders.SortOrder).ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

