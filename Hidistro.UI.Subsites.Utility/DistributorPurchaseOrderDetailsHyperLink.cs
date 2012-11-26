using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorPurchaseOrderDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus)PurchaseStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnPaymentPurchaseOrderDetails.aspx?PurchaseOrderId=" + PurchaseOrderId;
                        break;
                    }
                case OrderStatus.BuyerAlreadyPaid:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + PurchaseOrderId;
                        break;
                    }
                case OrderStatus.SellerAlreadySent:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/DeliveredPurchaseOrderDetails.aspx?PurchaseOrderId=" + PurchaseOrderId;
                        break;
                    }
                case OrderStatus.Closed:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId=" + PurchaseOrderId;
                        break;
                    }
                case OrderStatus.Finished:
                    {
                        base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/FinishedPurchaseOrderDetails.aspx?PurchaseOrderId=" + PurchaseOrderId;
                        break;
                    }
            }
            base.Render(writer);
        }

        public object PurchaseOrderId
        {
            get
            {
                if (ViewState["PurchaseOrderId"] == null)
                {
                    return null;
                }
                return ViewState["PurchaseOrderId"];
            }
            set
            {
                if (value != null)
                {
                    ViewState["PurchaseOrderId"] = value;
                }
            }
        }

        public object PurchaseStatusCode
        {
            get
            {
                if (ViewState["purchaseStatusCode"] == null)
                {
                    return null;
                }
                return ViewState["purchaseStatusCode"];
            }
            set
            {
                if (value != null)
                {
                    ViewState["purchaseStatusCode"] = value;
                }
            }
        }
    }
}

