using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web
{

    public partial class SendPayment : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.Request.QueryString["orderId"] != null)
            {
                string orderId = Page.Request.QueryString["orderId"];

                OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);

                if (orderInfo != null)
                {

                    if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
                    {
                        PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);

                        if (paymentMode == null)
                        {
                            Response.Write("<div><font color='red'>您之前选择的支付方式已经不存在</font></div>");
                        }
                        else
                        {
                            string showUrl = Globals.FullPath(Globals.GetSiteUrls().Home);

                            if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
                            {

                                showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("bank_pay", new object[] { orderInfo.OrderId }));

                            }
                            if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
                            {

                                showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("advance_pay", new object[] { orderInfo.OrderId }));

                            }

                            string attach = "";

                            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];

                            if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
                            {
                                attach = cookie.Value;
                            }

                            PaymentRequest.CreateInstance(paymentMode.Gateway, Cryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
                       
                        }
                    }
                    else
                    {

                        Page.Response.Write("订单当前状态不能支付");

                    }
                }

            }

        }

    }

}

