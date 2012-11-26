namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;

    public class OrderDetailsBasePage : AdminPage
    {
        protected OrderInfo Order;
       string orderId;
       OrderStatus pageOrderStatus;

        public OrderDetailsBasePage(OrderStatus pageOrderStatus)
        {
            this.pageOrderStatus = pageOrderStatus;
        }

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.orderId = this.Page.Request.QueryString["OrderId"];
                this.Order = OrderHelper.GetOrderInfo(this.orderId);
                if (this.Order == null)
                {
                    base.GotoResourceNotFound();
                }
                else if (this.Order.OrderStatus != this.pageOrderStatus)
                {
                    this.Redirect();
                }
                else
                {
                    base.OnInit(e);
                }
            }
        }

       void Redirect()
        {
            switch (this.Order.OrderStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/UnPaymentOrderDetails.aspx?OrderId=" + this.orderId);
                    return;

                case OrderStatus.BuyerAlreadyPaid:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/UnShippingOrderDetails.aspx?OrderId=" + this.orderId);
                    return;

                case OrderStatus.SellerAlreadySent:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/DeliveredOrderDetails.aspx?OrderId=" + this.orderId);
                    return;

                case OrderStatus.Closed:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/ClosedOrderDetails.aspx?OrderId=" + this.orderId);
                    return;

                case OrderStatus.Finished:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/FinishedOrderDetails.aspx?OrderId=" + this.orderId);
                    return;
            }
        }
    }
}

