using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class Order_ShippingAddress : UserControl
    {
        private OrderInfo order;

        public void LoadControl()
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(order.ShippingRegion))
            {
                shippingRegion = order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(order.Address))
            {
                shippingRegion = shippingRegion + order.Address;
            }
            if (!string.IsNullOrEmpty(order.ShipTo))
            {
                shippingRegion = shippingRegion + "   " + order.ShipTo;
            }
            if (!string.IsNullOrEmpty(order.ZipCode))
            {
                shippingRegion = shippingRegion + "   " + order.ZipCode;
            }
            if (!string.IsNullOrEmpty(order.TelPhone))
            {
                shippingRegion = shippingRegion + "   " + order.TelPhone;
            }
            if (!string.IsNullOrEmpty(order.CellPhone))
            {
                shippingRegion = shippingRegion + "   " + order.CellPhone;
            }
            lblShipAddress.Text = shippingRegion;
            if ((order.OrderStatus == OrderStatus.WaitBuyerPay) || (order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
            {
                lkBtnEditShippingAddress.Visible = true;
            }
            if ((order.OrderStatus == OrderStatus.Finished) || (order.OrderStatus == OrderStatus.SellerAlreadySent))
            {
                litModeName.Text = order.RealModeName + " 发货单号：" + order.ShipOrderNumber;
            }
            else
            {
                litModeName.Text = order.ModeName;
            }
            if (!string.IsNullOrEmpty(order.ExpressCompanyName))
            {
                litCompanyName.Text = order.ExpressCompanyName;
                tr_company.Visible = true;
            }
            litRemark.Text = order.Remark;
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadControl();
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

