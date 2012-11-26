namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderStatusLabel : Label
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus) OrderStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    base.Text = "等待买家付款";
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    base.Text = "已付款,等待发货";
                    break;

                case OrderStatus.SellerAlreadySent:
                    base.Text = "已发货";
                    break;

                case OrderStatus.Closed:
                    base.Text = "已关闭";
                    break;

                case OrderStatus.Finished:
                    base.Text = "订单已完成";
                    break;

                case OrderStatus.History:
                    base.Text = "历史订单";
                    break;

                default:
                    base.Text = "-";
                    break;
            }
            base.Render(writer);
        }

        public object OrderStatusCode
        {
            get
            {
                return ViewState["OrderStatusCode"];
            }
            set
            {
                ViewState["OrderStatusCode"] = value;
            }
        }
    }
}

