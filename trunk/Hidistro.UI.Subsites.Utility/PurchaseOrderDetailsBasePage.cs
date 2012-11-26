using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using System;

namespace Hidistro.UI.Subsites.Utility
{
    public class PurchaseOrderDetailsBasePage : DistributorPage
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
            if (string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
                purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                if (purchaseOrder == null)
                {
                    base.GotoResourceNotFound();
                }
                else if (purchaseOrder.PurchaseStatus != pagePurchaseStatus)
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
            switch (purchaseOrder.PurchaseStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    if (!purchaseOrder.IsManualPurchaseOrder)
                    {
                        Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnPaymentPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                        return;
                    }
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnPaymentManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                    return;

                case OrderStatus.BuyerAlreadyPaid:
                    if (!purchaseOrder.IsManualPurchaseOrder)
                    {
                        Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                        return;
                    }
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnShippingManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                    return;

                case OrderStatus.SellerAlreadySent:
                    if (!purchaseOrder.IsManualPurchaseOrder)
                    {
                        Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/DeliveredPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                        return;
                    }
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/DeliveredManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                    return;

                case OrderStatus.Closed:
                    if (!purchaseOrder.IsManualPurchaseOrder)
                    {
                        Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                        return;
                    }
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ClosedManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
                    return;
            }
        }
    }
}

