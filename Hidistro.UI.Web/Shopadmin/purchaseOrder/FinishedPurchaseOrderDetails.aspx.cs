using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class FinishedPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {


        public FinishedPurchaseOrderDetails()
            : base(OrderStatus.Finished)
        {
        }

        private void LoadUserControl()
        {
            itemsList.PurchaseOrder = base.purchaseOrder;
            chargesList.PurchaseOrder = base.purchaseOrder;
            shippingAddress.PurchaseOrder = base.purchaseOrder;
            if (!string.IsNullOrEmpty(base.purchaseOrder.ExpressCompanyAbb))
            {
                plExpress.Visible = true;
                if (Express.GetExpressType() == "kuaidi100")
                {
                    power.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                LoadUserControl();
                if (base.purchaseOrder.RefundStatus == RefundStatus.Refund)
                {
                    divRefundDetails.Visible = true;
                    hlkRefundDetails.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/RefundPurchaseDetails.aspx?PurchaseOrderId=" + base.purchaseOrder.PurchaseOrderId;
                }
                hlkOrder.Text = base.purchaseOrder.OrderId;
                hlkOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/shopadmin/sales/UnShippingOrderDetails.aspx?OrderId={0}", base.purchaseOrder.OrderId);
                litPurchaseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
            }
        }
    }
}

