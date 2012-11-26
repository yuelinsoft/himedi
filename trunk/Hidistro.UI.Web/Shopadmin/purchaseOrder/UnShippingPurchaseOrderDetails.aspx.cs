using Hidistro.Core;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
using Hidistro.Entities.Sales;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnShippingPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {

        public UnShippingPurchaseOrderDetails()
            : base(OrderStatus.BuyerAlreadyPaid)
        {
        }

        private void LoadUserControl()
        {
            itemsList.PurchaseOrder = base.purchaseOrder;
            chargesList.PurchaseOrder = base.purchaseOrder;
            shippingAddress.PurchaseOrder = base.purchaseOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUserControl();
            if (!IsPostBack)
            {
                hlkOrder.Text = base.purchaseOrder.OrderId;
                hlkOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/shopadmin/sales/UnShippingOrderDetails.aspx?OrderId={0}", base.purchaseOrder.OrderId);
                litPurchaseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
            }
        }
    }
}

