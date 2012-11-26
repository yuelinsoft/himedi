using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class Order_ItemsList : UserControl
    {
        OrderInfo order;

        protected override void OnLoad(EventArgs e)
        {
            dlstOrderItems.DataSource = order.LineItems.Values;
            dlstOrderItems.DataBind();
            if (order.Gifts.Count == 0)
            {
                grdOrderGift.Visible = false;
                lblOrderGifts.Visible = false;
            }
            else
            {
                grdOrderGift.DataSource = order.Gifts;
                grdOrderGift.DataBind();
            }
            litGoodsAmount.Text = Globals.FormatMoney(order.GetAmount());
            lblWeight.Text = order.Weight.ToString(CultureInfo.InvariantCulture);
        }

        public OrderInfo Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }
    }
}

