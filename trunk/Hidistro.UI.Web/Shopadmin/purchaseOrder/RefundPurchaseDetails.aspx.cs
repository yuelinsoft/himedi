using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class RefundPurchaseDetails : DistributorPage
    {
        string purchaseOrderId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseOrderId = base.Request.QueryString["PurchaseOrderId"];
                if (!base.IsPostBack)
                {
                    PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                    if (purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else if (purchaseOrder.RefundStatus != RefundStatus.Refund)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litPurchaseOrderId.Text = purchaseOrder.PurchaseOrderId;
                        lblRefundDate.Time = purchaseOrder.FinishDate;
                        lblRefundAmount.Money = purchaseOrder.RefundAmount;
                        lblPaymentAmount.Money = purchaseOrder.GetPurchaseTotal() - purchaseOrder.RefundAmount;
                        litRefundRemark.Text = purchaseOrder.RefundRemark;
                    }
                }
            }
        }
    }
}

