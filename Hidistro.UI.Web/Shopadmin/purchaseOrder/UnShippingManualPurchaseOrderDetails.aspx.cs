using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
using Hidistro.Entities.Sales;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnShippingManualPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {

        public UnShippingManualPurchaseOrderDetails()
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
            if (!base.IsPostBack)
            {
                litPurchaseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
            }
        }
    }
}

