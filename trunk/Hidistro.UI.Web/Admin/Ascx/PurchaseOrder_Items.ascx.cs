using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class PurchaseOrder_Items : UserControl
    {

        PurchaseOrderInfo purchaseOrder;

        protected override void OnLoad(EventArgs e)
        {
            dlstOrderItems.DataSource = purchaseOrder.PurchaseOrderItems;
            dlstOrderItems.DataBind();
            if (purchaseOrder.PurchaseOrderGifts.Count <= 0)
            {
                giftsList.Visible = false;
            }
            else
            {
                grdOrderGift.DataSource = purchaseOrder.PurchaseOrderGifts;
                grdOrderGift.DataBind();
            }
            lblGoodsAmount.Money = purchaseOrder.GetProductAmount();
            lblWeight.Text = purchaseOrder.Weight.ToString(CultureInfo.InvariantCulture);
            purchaseOrderItemUpdateHyperLink.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
            purchaseOrderItemUpdateHyperLink.PurchaseStatusCode = purchaseOrder.PurchaseStatus;
            purchaseOrderItemUpdateHyperLink.DistorUserId = purchaseOrder.DistributorId;
            purchaseOrderItemUpdateHyperLink.DataBind();
        }

        public PurchaseOrderInfo PurchaseOrder
        {
            get
            {
                return purchaseOrder;
            }
            set
            {
                purchaseOrder = value;
            }
        }
    }
}

