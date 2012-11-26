using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class PurchaseOrder_Charges : UserControl
    {

        PurchaseOrderInfo purchaseOrder;

        public void LoadControl()
        {
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.Finished) || (this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                this.lblModeName.Text = this.purchaseOrder.RealModeName;
            }
            else
            {
                this.lblModeName.Text = this.purchaseOrder.ModeName;
            }
            this.litFreight.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedFreight);
            this.litOptionPrice.Text = Globals.FormatMoney(this.purchaseOrder.GetOptionPrice());
            this.litDiscount.Text = this.purchaseOrder.AdjustedDiscount.ToString();
            this.litTotalPrice.Text = Globals.FormatMoney(this.purchaseOrder.GetPurchaseTotal());
            string str = string.Empty;
            if (this.purchaseOrder.PurchaseOrderOptions.Count > 0)
            {
                foreach (PurchaseOrderOptionInfo info in this.purchaseOrder.PurchaseOrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
            }
            this.litOderItem.Text = str;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadControl();
        }

        public PurchaseOrderInfo PurchaseOrder
        {
            get
            {
                return this.purchaseOrder;
            }
            set
            {
                this.purchaseOrder = value;
            }
        }
    }
}

