namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus) this.OrderStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/UnPaymentOrderDetails.aspx?OrderId=" + this.OrderId.ToString();
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/UnShippingOrderDetails.aspx?OrderId=" + this.OrderId.ToString();
                    break;

                case OrderStatus.SellerAlreadySent:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/DeliveredOrderDetails.aspx?OrderId=" + this.OrderId.ToString();
                    break;

                case OrderStatus.Closed:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/ClosedOrderDetails.aspx?OrderId=" + this.OrderId.ToString();
                    break;

                case OrderStatus.Finished:
                    base.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/FinishedOrderDetails.aspx?OrderId=" + this.OrderId.ToString();
                    break;
            }
            base.Render(writer);
        }

        public object OrderId
        {
            get
            {
                if (this.ViewState["OrderId"] == null)
                {
                    return null;
                }
                return this.ViewState["OrderId"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["OrderId"] = value;
                }
            }
        }

        public object OrderStatusCode
        {
            get
            {
                if (this.ViewState["OrderStatusCode"] == null)
                {
                    return null;
                }
                return this.ViewState["OrderStatusCode"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["OrderStatusCode"] = value;
                }
            }
        }
    }
}

