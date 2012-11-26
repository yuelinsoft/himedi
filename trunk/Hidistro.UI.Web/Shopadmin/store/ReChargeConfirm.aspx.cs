using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ReChargeConfirm : DistributorPage
    {
        decimal balance;
        int paymentModeId;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(paymentModeId);
            InpourRequestInfo inpourRequest = new InpourRequestInfo();
            inpourRequest.InpourId = GenerateInpourId();
            inpourRequest.TradeDate = DateTime.Now;
            inpourRequest.InpourBlance = balance;
            inpourRequest.UserId = HiContext.Current.User.UserId;
            inpourRequest.PaymentId = paymentMode.ModeId;

            if (SubsiteStoreHelper.AddInpourBalance(inpourRequest))
            {
                string attach = "";
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
                {
                    attach = cookie.Value;
                }
                string orderId = inpourRequest.InpourId.ToString(CultureInfo.InvariantCulture);
                PaymentRequest.CreateInstance(paymentMode.Gateway, Cryptographer.Decrypt(paymentMode.Settings), orderId, inpourRequest.InpourBlance + paymentMode.CalcPayCharge(inpourRequest.InpourBlance), "预付款充值", "操作流水号-" + orderId, HiContext.Current.User.Email, inpourRequest.TradeDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
            }
        }

        private string GenerateInpourId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num2 = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num2 % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Page.Request.QueryString["modeId"], out paymentModeId);
            decimal.TryParse(Page.Request.QueryString["blance"], out balance);
            btnConfirm.Click += new EventHandler(btnConfirm_Click);
            if ((!Page.IsPostBack && (paymentModeId > 0)) && (balance > 0M))
            {
                PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(paymentModeId);
                litRealName.Text = HiContext.Current.User.Username;
                if (paymentMode != null)
                {
                    lblPaymentName.Text = paymentMode.Name;
                    lblBlance.Money = balance;
                    ViewState["PayCharge"] = paymentMode.CalcPayCharge(balance);
                    litPayCharge.Text = Globals.FormatMoney(paymentMode.CalcPayCharge(balance));
                }
            }
        }
    }
}

