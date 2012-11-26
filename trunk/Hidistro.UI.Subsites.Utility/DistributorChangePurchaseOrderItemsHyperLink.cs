using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorChangePurchaseOrderItemsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            OrderStatus purchaseStatusCode = (OrderStatus)PurchaseStatusCode;
            if (purchaseStatusCode == OrderStatus.WaitBuyerPay)
            {
                base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ChangePurchaseOrderItems.aspx?PurchaseOrderId=" + PurchaseOrderId;
            }
            else
            {
                base.Visible = false;
                base.Text = string.Empty;
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

