using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnPaymentPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {

        public UnPaymentPurchaseOrderDetails()
            : base(OrderStatus.WaitBuyerPay)
        {
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = base.purchaseOrder;
            purchaseOrder.CloseReason = ddlCloseReason.SelectedValue;
            if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
            {
                ShowMsg("取消采购成功", true);
            }
            else
            {
                ShowMsg("取消采购失败", false);
            }
        }

        private void LoadUserControl()
        {
            itemsList.PurchaseOrder = base.purchaseOrder;
            chargesList.PurchaseOrder = base.purchaseOrder;
            shippingAddress.PurchaseOrder = base.purchaseOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnClosePurchaseOrder.Click += new EventHandler(btnClosePurchaseOrder_Click);
            LoadUserControl();
            if (!base.IsPostBack)
            {
                hlkOrder.Text = base.purchaseOrder.OrderId;
                hlkOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/shopadmin/sales/UnShippingOrderDetails.aspx?OrderId={0}", base.purchaseOrder.OrderId);
                litPurchaseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
                if (base.purchaseOrder.PurchaseOrderGifts.Count > 0)
                {
                    hlkOrderGifts.Text = "编辑礼品";
                }
                hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/PurchaseOrderGifts.aspx?PurchaseOrderId=" + base.purchaseOrder.PurchaseOrderId;
                lkbtnPay.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=" + base.purchaseOrder.PurchaseOrderId;
            }
        }
    }
}

