using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class PrintPackingOrder : AdminPage
    {

        string orderId = string.Empty;

        private void BindOrderInfo(OrderInfo order)
        {
            litAddress.Text = order.ShippingRegion + order.Address;
            litCellPhone.Text = order.CellPhone;
            litTelPhone.Text = order.TelPhone;
            litZipCode.Text = order.ZipCode;
            litOrderId.Text = order.OrderId;
            litOrderDate.Text = order.OrderDate.ToString();
            litPayType.Text = order.PaymentType;
            litRemark.Text = order.Remark;
            litShipperMode.Text = order.RealModeName;
            litShippNo.Text = order.ShipOrderNumber;
            litSkipTo.Text = order.ShipTo;
            switch (order.OrderStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    litOrderStatus.Text = "等待付款";
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    litOrderStatus.Text = "已付款等待发货";
                    break;

                case OrderStatus.SellerAlreadySent:
                    litOrderStatus.Text = "已发货";
                    break;

                case OrderStatus.Closed:
                    litOrderStatus.Text = "已关闭";
                    break;

                case OrderStatus.Finished:
                    litOrderStatus.Text = "已完成";
                    break;
            }
        }

        private void BindOrderItems(OrderInfo order)
        {
            grdOrderItems.DataSource = order.LineItems.Values;
            grdOrderItems.DataBind();
            if (order.Gifts.Count > 0)
            {
                grdOrderGifts.DataSource = order.Gifts;
                grdOrderGifts.DataBind();
            }
            else
            {
                grdOrderGifts.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            orderId = Page.Request.Params["OrderId"];
            if (!(Page.IsPostBack || string.IsNullOrEmpty(orderId)))
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                BindOrderInfo(orderInfo);
                BindOrderItems(orderInfo);
            }
        }
    }
}

