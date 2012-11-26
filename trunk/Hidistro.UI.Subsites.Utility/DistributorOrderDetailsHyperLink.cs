using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorOrderDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus)OrderStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/UnPaymentOrderDetails.aspx?OrderId=" + OrderId.ToString();
                        break;
                    }
                case OrderStatus.BuyerAlreadyPaid:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/UnShippingOrderDetails.aspx?OrderId=" + OrderId.ToString();
                        break;
                    }
                case OrderStatus.SellerAlreadySent:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/DeliveredOrderDetails.aspx?OrderId=" + OrderId.ToString();
                        break;
                    }
                case OrderStatus.Closed:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/ClosedOrderDetails.aspx?OrderId=" + OrderId.ToString();
                        break;
                    }
                case OrderStatus.Finished:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/FinishedOrderDetails.aspx?OrderId=" + OrderId.ToString();
                        break;
                    }
            }
            base.Render(writer);
        }

        public object OrderId
        {
            get
            {
                if (ViewState["OrderId"] == null)
                {
                    return null;
                }
                return ViewState["OrderId"];
            }
            set
            {
                if (value != null)
                {
                    ViewState["OrderId"] = value;
                }
            }
        }

        public object OrderStatusCode
        {
            get
            {
                if (ViewState["OrderStatusCode"] == null)
                {
                    return null;
                }
                return ViewState["OrderStatusCode"];
            }
            set
            {
                if (value != null)
                {
                    ViewState["OrderStatusCode"] = value;
                }
            }
        }
    }
}

