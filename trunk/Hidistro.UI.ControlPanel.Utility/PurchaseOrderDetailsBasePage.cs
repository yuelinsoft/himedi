namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;

    public class PurchaseOrderDetailsBasePage : AdminPage
    {
       OrderStatus pagePurchaseStatus;
        protected PurchaseOrderInfo purchaseOrder;
       string purchaseOrderId;

        public PurchaseOrderDetailsBasePage(OrderStatus pagePurchaseStatus)
        {
            this.pagePurchaseStatus = pagePurchaseStatus;
        }

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
                this.purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                if (this.purchaseOrder == null)
                {
                    base.GotoResourceNotFound();
                }
                else if (this.purchaseOrder.PurchaseStatus != this.pagePurchaseStatus)
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
            switch (this.purchaseOrder.PurchaseStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/purchaseOrder/UnPaymentPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
                    return;

                case OrderStatus.BuyerAlreadyPaid:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/purchaseOrder/UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
                    return;

                case OrderStatus.SellerAlreadySent:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/purchaseOrder/DeliveredPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
                    return;

                case OrderStatus.Closed:
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Admin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
                    return;
            }
        }
    }
}

