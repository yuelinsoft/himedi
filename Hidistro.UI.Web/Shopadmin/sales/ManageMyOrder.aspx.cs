using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.Subsites.Promotions;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ManageMyOrder : DistributorPage
    {

        private void BindOrders()
        {
            OrderQuery orderQuery = GetOrderQuery();
            DbQueryResult orders = SubsiteSalesHelper.GetOrders(orderQuery);
            dlstOrders.DataSource = orders.Data;
            dlstOrders.DataBind();
            pager.TotalRecords = orders.TotalRecords;
            pager1.TotalRecords = orders.TotalRecords;
            txtUserName.Text = orderQuery.UserName;
            txtOrderId.Text = orderQuery.OrderId;
            txtProductName.Text = orderQuery.ProductName;
            txtShopTo.Text = orderQuery.ShipTo;
            calendarStartDate.SelectedDate = orderQuery.StartDate;
            calendarEndDate.SelectedDate = orderQuery.EndDate;
            lblStatus.Text = ((int)orderQuery.Status).ToString();
        }

        private void btnBatchSendGoods_Click(object sender, EventArgs e)
        {
            LoadSendGoodsPage();
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(hidOrderId.Value);
            orderInfo.CloseReason = ddlCloseReason.SelectedValue;
            if (SubsiteSalesHelper.CloseTransaction(orderInfo))
            {
                int userId = orderInfo.UserId;
                if (userId == 0x44c)
                {
                    userId = 0;
                }
                Messenger.OrderClosed(Users.GetUser(userId), orderInfo.OrderId, orderInfo.CloseReason);
                orderInfo.OnClosed();
                BindOrders();
                ShowMsg("关闭订单成功", true);
            }
            else
            {
                ShowMsg("关闭订单失败", false);
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Length > 300)
            {
                ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                OrderInfo info2 = new OrderInfo();
                info2.OrderId = hidOrderId.Value;
                OrderInfo order = info2;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    order.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                order.ManagerRemark = Globals.HtmlEncode(txtRemark.Text.Trim());
                if (SubsiteSalesHelper.SaveRemark(order))
                {
                    BindOrders();
                    ShowMsg("保存备忘录成功", true);
                }
                else
                {
                    ShowMsg("保存失败", false);
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReloadOrders(true);
        }

        private void dlstOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if ((e.CommandName == "CONFIRM_PAY") && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    int maxCount = 0;
                    int orderCount = 0;
                    int groupBuyOerderNumber = 0;
                    if (orderInfo.GroupBuyId > 0)
                    {
                        GroupBuyInfo groupBuy = SubsitePromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
                        if ((groupBuy == null) || (groupBuy.Status != GroupBuyStatus.UnderWay))
                        {
                            ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
                            return;
                        }
                        orderCount = SubsitePromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
                        maxCount = groupBuy.MaxCount;
                        groupBuyOerderNumber = orderInfo.GetGroupBuyOerderNumber();
                        if (maxCount < (orderCount + groupBuyOerderNumber))
                        {
                            ShowMsg("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
                            return;
                        }
                    }
                    if (SubsiteSalesHelper.ConfirmPay(orderInfo))
                    {
                        if ((orderInfo.GroupBuyId > 0) && (maxCount == (orderCount + groupBuyOerderNumber)))
                        {
                            SubsitePromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                        }
                        BindOrders();
                        int userId = orderInfo.UserId;
                        if (userId == 0x44c)
                        {
                            userId = 0;
                        }
                        Messenger.OrderPayment(Users.GetUser(userId), orderInfo.OrderId, orderInfo.GetTotal());
                        orderInfo.OnPayment();
                        ShowMsg("成功的确认了订单收款", true);
                    }
                    else
                    {
                        ShowMsg("确认订单收款失败", false);
                    }
                }
                else if ((e.CommandName == "FINISH_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (SubsiteSalesHelper.ConfirmOrderFinish(orderInfo))
                    {
                        BindOrders();
                        ShowMsg("成功的完成了该订单", true);
                    }
                    else
                    {
                        ShowMsg("完成订单失败", false);
                    }
                }
                else if ((e.CommandName == "CREATE_PURCHASEORDER") && orderInfo.CheckAction(OrderActions.SUBSITE_CREATE_PURCHASEORDER))
                {
                    if (SubsiteSalesHelper.CreatePurchaseOrder(orderInfo))
                    {
                        BindOrders();
                        ShowMsg("生成采购单成功", true);
                    }
                    else
                    {
                        ShowMsg(" 生成采购单失败", false);
                    }
                }
            }
        }

        private void dlstOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                OrderStatus status = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
                HyperLink link = (HyperLink)e.Item.FindControl("lkbtnEditPrice");
                HyperLink link2 = (HyperLink)e.Item.FindControl("lkbtnSendGoods");
                ImageLinkButton button = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
                ImageLinkButton button2 = (ImageLinkButton)e.Item.FindControl("lkbtnCreatePurchaseOrder");
                ImageLinkButton button3 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
                Literal literal = (Literal)e.Item.FindControl("litCloseOrder");
                int num = (int)DataBinder.Eval(e.Item.DataItem, "GroupBuyId");
                int num2 = (int)DataBinder.Eval(e.Item.DataItem, "PurchaseOrders");
                if (status == OrderStatus.WaitBuyerPay)
                {
                    literal.Visible = true;
                    link.Visible = true;
                    button.Visible = true;
                }
                if (num > 0)
                {
                    GroupBuyStatus status2 = (GroupBuyStatus)DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
                    link2.Visible = (status == OrderStatus.BuyerAlreadyPaid) && (status2 == GroupBuyStatus.Success);
                    button2.Visible = ((status == OrderStatus.BuyerAlreadyPaid) && (status2 == GroupBuyStatus.Success)) && (num2 == 0);
                }
                else
                {
                    link2.Visible = status == OrderStatus.BuyerAlreadyPaid;
                }
                button3.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderStatus"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["OrderStatus"], out result))
                {
                    query.Status = (OrderStatus)result;
                }
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(Page.Request.QueryString["GroupBuyId"]));
            }
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                ShowMsg("请选要删除的订单", false);
            }
            else
            {
                int num = SubsiteSalesHelper.DeleteOrders("'" + str.Replace(",", "','") + "'");
                BindOrders();
                ShowMsg(string.Format("成功删除了{0}个订单", num), true);
            }
        }

        private void LoadSendGoodsPage()
        {
            StringBuilder builder = new StringBuilder("~/ShopAdmin/Sales/BatchSendMyGoods.aspx?PageSize=10");
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                builder.AppendFormat("&OrderId={0}", Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                builder.AppendFormat("&PurchaseOrderId={0}", Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ProductName"]))
            {
                builder.AppendFormat("&ProductName={0}", Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ShipTo"]))
            {
                builder.AppendFormat("&ShipTo={0}", Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["UserName"]))
            {
                builder.AppendFormat("&UserName={0}", Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["StartDate"]))
            {
                builder.AppendFormat("&StartDate={0}", DateTime.Parse(Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["EndDate"]))
            {
                builder.AppendFormat("&EndDate={0}", DateTime.Parse(Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderStatus"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["OrderStatus"], out result))
                {
                    builder.AppendFormat("&OrderStatus={0}", result);
                }
            }
            Page.Response.Redirect(builder.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dlstOrders.ItemDataBound += new DataListItemEventHandler(dlstOrders_ItemDataBound);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            dlstOrders.ItemCommand += new DataListCommandEventHandler(dlstOrders_ItemCommand);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            btnCloseOrder.Click += new EventHandler(btnCloseOrder_Click);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            if (!Page.IsPostBack)
            {
                SetOrderStatusLink();
                BindOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReloadOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("UserName", Globals.UrlEncode(txtUserName.Text));
            queryStrings.Add("OrderId", Globals.UrlEncode(txtOrderId.Text));
            queryStrings.Add("ProductName", Globals.UrlEncode(txtProductName.Text));
            queryStrings.Add("ShipTo", Globals.UrlEncode(txtShopTo.Text));
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            queryStrings.Add("OrderStatus", lblStatus.Text);
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", calendarStartDate.SelectedDate.Value.ToString());
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", calendarEndDate.SelectedDate.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Shopadmin/sales/ManageMyOrder.aspx?OrderStatus={0}";
            hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            hlinkNotPay.NavigateUrl = string.Format(format, 1);
            hlinkYetPay.NavigateUrl = string.Format(format, 2);
            hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            hlinkClose.NavigateUrl = string.Format(format, 4);
            hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            hlinkHistory.NavigateUrl = string.Format(format, 0x63);
        }
    }
}

