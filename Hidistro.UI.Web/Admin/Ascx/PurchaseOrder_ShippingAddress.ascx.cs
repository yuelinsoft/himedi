using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class PurchaseOrder_ShippingAddress : UserControl
    {

        PurchaseOrderInfo purchaseOrder;

        public void LoadControl()
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(purchaseOrder.ShippingRegion))
            {
                shippingRegion = PurchaseOrder.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(purchaseOrder.Address))
            {
                shippingRegion = shippingRegion + PurchaseOrder.Address;
            }
            if (!string.IsNullOrEmpty(purchaseOrder.ZipCode))
            {
                shippingRegion = shippingRegion + "，" + purchaseOrder.ZipCode;
            }
            if (!string.IsNullOrEmpty(PurchaseOrder.ShipTo))
            {
                shippingRegion = shippingRegion + "，" + purchaseOrder.ShipTo;
            }
            if (!string.IsNullOrEmpty(purchaseOrder.TelPhone))
            {
                shippingRegion = shippingRegion + "，" + purchaseOrder.TelPhone;
            }
            if (!string.IsNullOrEmpty(purchaseOrder.CellPhone))
            {
                shippingRegion = shippingRegion + "，" + purchaseOrder.CellPhone;
            }
            lblShipAddress.Text = shippingRegion;
            if ((purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                lkBtnEditShippingAddress.Visible = true;
            }
            if ((purchaseOrder.PurchaseStatus == OrderStatus.Finished) || (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                litModeName.Text = purchaseOrder.RealModeName + " 发货单号：" + purchaseOrder.ShipOrderNumber;
            }
            else
            {
                litModeName.Text = purchaseOrder.ModeName;
            }
            if (!string.IsNullOrEmpty(purchaseOrder.ExpressCompanyName))
            {
                litCompanyName.Text = purchaseOrder.ExpressCompanyName;
                tr_company.Visible = true;
            }
            litRemark.Text = purchaseOrder.Remark;
            lblPurchaseDate.Time = purchaseOrder.PurchaseDate;
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadControl();
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

