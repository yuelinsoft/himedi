using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.RefundOrder)]
    public partial class RefundOrder : AdminPage
    {
        string orderId;


        private void BtnRefund_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            if (!orderInfo.CheckAction(OrderActions.SELLER_REJECT_REFUND))
            {
                ShowMsg("未付款或不在进行中的订单没有退款操作", false);
            }
            else
            {
                int length = 0;
                decimal result = 0M;
                if (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent)
                {
                    if (txtRefundTotal.Text.Trim().IndexOf(".") > 0)
                    {
                        length = txtRefundTotal.Text.Trim().Substring(txtRefundTotal.Text.Trim().IndexOf(".") + 1).Length;
                    }
                    if (!(decimal.TryParse(txtRefundTotal.Text.Trim(), out result) && (length <= 2)))
                    {
                        ShowMsg("退款金额只能是数值，且不能超过2位小数", false);
                        return;
                    }
                    if ((result <= 0M) || (result > orderInfo.GetTotal()))
                    {
                        ShowMsg("退款金额必须大于0,小于等于实收款", false);
                        return;
                    }
                }
                if (txtRefundRemark.Text.Length > 200)
                {
                    ShowMsg("退款说明限制在200个字符以内", false);
                }
                else
                {
                    decimal money;
                    Member user = Users.GetUser(orderInfo.UserId) as Member;
                    if (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent)
                    {
                        money = result;
                    }
                    else if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed))
                    {
                        money = ((decimal)lblTotalPrice.Money) - orderInfo.NeedPrice;
                    }
                    else
                    {
                        money = (decimal)lblTotalPrice.Money;
                    }
                    BalanceDetailInfo info3 = new BalanceDetailInfo();
                    info3.UserId = orderInfo.UserId;
                    info3.UserName = orderInfo.Username;
                    info3.TradeDate = DateTime.Now;
                    info3.TradeType = TradeTypes.RefundOrder;
                    info3.Income = new decimal?(money);
                    info3.Balance = user.Balance + money;
                    info3.Remark = "订单退款到预付款";
                    BalanceDetailInfo balanceDetails = info3;
                    if (rdolist.SelectedValue == "2")
                    {
                        if (!user.IsOpenBalance)
                        {
                            ShowMsg("本次退款已失败，该用户的预付款还没有开通", false);
                        }
                        else if (MemberHelper.AddBalance(balanceDetails))
                        {
                            orderInfo.RefundAmount = money;
                            orderInfo.RefundRemark = Globals.HtmlEncode(txtRefundRemark.Text.Trim());
                            orderInfo.FinishDate = DateTime.Now;
                            orderInfo.RefundStatus = RefundStatus.Refund;
                            if (OrderHelper.RefundOrder(orderInfo))
                            {
                                Messenger.OrderRefund(user, orderInfo.OrderId, money);
                                orderInfo.OnRefund();
                                Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/sales/RefundOrderDetails.aspx?OrderId={0}", orderInfo.OrderId));
                            }
                            else
                            {
                                ShowMsg("退款失败", false);
                            }
                        }
                    }
                    else
                    {
                        orderInfo.RefundAmount = money;
                        orderInfo.RefundRemark = Globals.HtmlEncode(txtRefundRemark.Text.Trim());
                        orderInfo.FinishDate = DateTime.Now;
                        orderInfo.RefundStatus = RefundStatus.Below;
                        if (OrderHelper.RefundOrder(orderInfo))
                        {
                            Messenger.OrderRefund(user, orderInfo.OrderId, money);
                            orderInfo.OnRefund();
                            Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/sales/RefundOrderDetails.aspx?OrderId={0}", orderInfo.OrderId));
                        }
                        else
                        {
                            ShowMsg("退款失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                orderId = Page.Request.QueryString["OrderId"];
                BtnRefund.Click += new EventHandler(BtnRefund_Click);
                if (!Page.IsPostBack)
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                    if (orderInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        SetControl(orderInfo);
                    }
                }
            }
        }

        private void SetControl(OrderInfo order)
        {
            if (!order.CheckAction(OrderActions.SELLER_REJECT_REFUND))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                lblTotalPrice.Money = order.GetTotal();
                Member user = Users.GetUser(order.UserId, false) as Member;
                if (user == null)
                {
                    base.GotoResourceNotFound();
                }
                if (!user.IsOpenBalance)
                {
                    rdolist.Items[1].Enabled = false;
                    divBalance.Visible = true;
                }
                if (order.OrderStatus == OrderStatus.SellerAlreadySent)
                {
                    lblDescription.InnerHtml = "退款金额不得大于订单总金额.已发货订单允许全额或部分退款,退款后订单自动变为交易成功状态。";
                    lblTui.InnerHtml = "部分退款";
                }
                else
                {
                    lblDescription.InnerHtml = "已付款等待发货订单只允许全额退款.团购订单若不是以团购失败结束，则会扣除违约金,退款后订单自动变为关闭状态";
                    lblTui.InnerHtml = "全额退款";
                    if ((order.GroupBuyId > 0) && (order.GroupBuyStatus != GroupBuyStatus.Failed))
                    {
                        txtRefundTotal.Text = (order.GetTotal() - order.NeedPrice).ToString();
                    }
                    else
                    {
                        txtRefundTotal.Text = order.GetTotal().ToString();
                    }
                    txtRefundTotal.Enabled = false;
                }
            }
        }
    }
}

