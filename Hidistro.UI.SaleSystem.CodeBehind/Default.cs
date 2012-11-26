using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true)]
    public class Default : HtmlTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            HiContext current = HiContext.Current;
            PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Default.html";
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.Params["OrderId"]))
            {
                this.SearchOrder();
            }
            base.OnLoad(e);
        }

        void SearchOrder()
        {
            string s = "[{";
            string orderId = this.Page.Request["OrderId"];
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                if ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished))
                {
                    string str3 = s;
                    s = str3 + "\"OrderId\":\"" + orderInfo.OrderId + "\",\"ShippingStatus\":\"已发货\",\"ShipOrderNumber\":\"" + orderInfo.ShipOrderNumber + "\",\"ShipModeName\":\"" + orderInfo.RealModeName + "\"";
                }
                else if (((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) || (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)) || (orderInfo.OrderStatus == OrderStatus.Closed))
                {
                    string str4 = s;
                    s = str4 + "\"OrderId\":\"" + orderInfo.OrderId + "\",\"ShippingStatus\":\"未发货\",\"ShipOrderNumber\":\"\",\"ShipModeName\":\"" + orderInfo.ModeName + "\"";
                }
            }
            s = s + "}]";
            this.Page.Response.ContentType = "text/plain";
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }
    }
}

