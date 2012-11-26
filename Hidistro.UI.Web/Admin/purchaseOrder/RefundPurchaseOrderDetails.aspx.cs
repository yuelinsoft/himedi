using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.PurchaseorderRefund)]
    public partial class RefundPurchaseOrderDetails : AdminPage
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
                    PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
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
                        litOrderId.Text = purchaseOrder.OrderId;
                        litPurchaseOrderId.Text = purchaseOrder.PurchaseOrderId;
                        lblPurchaseDate.Time = purchaseOrder.PurchaseDate;
                        lblTotalPrice.Money = purchaseOrder.GetPurchaseTotal();
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

