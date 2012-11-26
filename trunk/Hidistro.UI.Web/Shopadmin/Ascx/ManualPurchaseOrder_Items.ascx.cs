using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{

    public partial class ManualPurchaseOrder_Items : UserControl
    {

        PurchaseOrderInfo purchaseOrder;

        protected override void OnLoad(EventArgs e)
        {

            dlstOrderItems.DataSource = this.purchaseOrder.PurchaseOrderItems;

            dlstOrderItems.DataBind();

            grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;

            grdOrderGift.DataBind();

            lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();

            lblWeight.Text = this.purchaseOrder.Weight.ToString(CultureInfo.InvariantCulture);

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

