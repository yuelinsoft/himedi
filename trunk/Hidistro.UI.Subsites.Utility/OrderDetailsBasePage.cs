    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using System;

namespace Hidistro.UI.Subsites.Utility
{
    public class OrderDetailsBasePage : DistributorPage
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
            if (string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                orderId = Page.Request.QueryString["OrderId"];
                Order = SubsiteSalesHelper.GetOrderInfo(orderId);
                if (Order == null)
                {
                    base.GotoResourceNotFound();
                }
                else if (Order.OrderStatus != pageOrderStatus)
                {
                    Redirect();
                }
                else
                {
                    base.OnInit(e);
                }
            }
        }

       void Redirect()
        {
            switch (Order.OrderStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/UnPaymentOrderDetails.aspx?OrderId=" + orderId);
                    return;

                case OrderStatus.BuyerAlreadyPaid:
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/UnShippingOrderDetails.aspx?OrderId=" + orderId);
                    return;

                case OrderStatus.SellerAlreadySent:
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/DeliveredOrderDetails.aspx?OrderId=" + orderId);
                    return;

                case OrderStatus.Closed:
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/ClosedOrderDetails.aspx?OrderId=" + orderId);
                    return;

                case OrderStatus.Finished:
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/FinishedOrderDetails.aspx?OrderId=" + orderId);
                    return;
            }
        }
    }
}

