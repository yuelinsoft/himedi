namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PuchaseStatusLabel : Label
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((OrderStatus) PuchaseStatusCode))
            {
                case OrderStatus.WaitBuyerPay:
                    base.Text = "<span class=\"colorC\">等待分销商付款</span>";
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    base.Text = "<span class=\"colorA\">分销商已付款</span>";
                    break;

                case OrderStatus.SellerAlreadySent:
                    base.Text = "<span class=\"colorE\">供应商已经发货</span>";
                    break;

                case OrderStatus.Closed:
                    base.Text = "<span class=\"colorQ\">已关闭</span>";
                    break;

                case OrderStatus.Finished:
                    base.Text = "<span class=\"colorB\">采购成功</span>";
                    break;

                case OrderStatus.History:
                    base.Text = "历史采购单";
                    break;

                default:
                    base.Text = "-";
                    break;
            }
            base.Render(writer);
        }

        public object PuchaseStatusCode
        {
            get
            {
                return ViewState["PuchaseStatusCode"];
            }
            set
            {
                ViewState["PuchaseStatusCode"] = value;
            }
        }
    }
}

