using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class PurchaseOrder_Charges : UserControl
    {

        PurchaseOrderInfo purchaseOrder;

        public void LoadControl()
        {
            if ((purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                lkBtnEditshipingMode.Visible = true;
            }
            if ((purchaseOrder.PurchaseStatus == OrderStatus.Finished) || (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                lblModeName.Text = purchaseOrder.RealModeName;
            }
            else
            {
                lblModeName.Text = purchaseOrder.ModeName;
            }
            litFreight.Text = Globals.FormatMoney(purchaseOrder.AdjustedFreight);
            litOptionPrice.Text = Globals.FormatMoney(purchaseOrder.GetOptionPrice());
            litDiscount.Text = Globals.FormatMoney(purchaseOrder.AdjustedDiscount);
            litTotalPrice.Text = Globals.FormatMoney(purchaseOrder.GetPurchaseTotal());
            string str = string.Empty;
            if (purchaseOrder.PurchaseOrderOptions.Count > 0)
            {
                foreach (PurchaseOrderOptionInfo info in purchaseOrder.PurchaseOrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                litOderItem.Text = "（" + str + "）";
            }
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

