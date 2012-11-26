using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Subsites.Utility
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class DistributorInpourReturnBasePage : DistributorPage
    {
        protected decimal Amount;
        protected string Gateway;
        protected string InpourId;
        protected InpourRequestInfo InpourRequest;
        readonly bool isBackRequest;
        protected PaymentNotify Notify;
        PaymentModeInfo paymode;

        public DistributorInpourReturnBasePage(bool _isBackRequest)
        {
            isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
            DoValidate();
        }

        protected abstract void DisplayMessage(string status);
        void DoValidate()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(Page.Request.Form);
            values2.Add(Page.Request.QueryString);
            NameValueCollection parameters = values2;
            Gateway = Page.Request.QueryString["HIGW"];
            Notify = PaymentNotify.CreateInstance(Gateway, parameters);
            if (isBackRequest)
            {
                Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[] { Gateway }));
                Notify.ReturnUrl = Notify.ReturnUrl + "?" + Page.Request.Url.Query;
            }
            InpourId = Notify.GetOrderId();
            Amount = Notify.GetOrderAmount();
            InpourRequest = SubsiteStoreHelper.GetInpouRequest(InpourId);
            if (InpourRequest == null)
            {
                ResponseStatus(true, "success");
            }
            else
            {
                Amount = InpourRequest.InpourBlance;
                paymode = SubsiteStoreHelper.GetPaymentMode(InpourRequest.PaymentId);
                if (paymode == null)
                {
                    ResponseStatus(true, "gatewaynotfound");
                }
                else
                {
                    Notify.Finished += new EventHandler<FinishedEventArgs>(Notify_Finished);
                    Notify.NotifyVerifyFaild += new EventHandler(Notify_NotifyVerifyFaild);
                    Notify.Payment += new EventHandler(Notify_Payment);
                    Notify.VerifyNotify(0x7530, Cryptographer.Decrypt(paymode.Settings));
                }
            }
        }

        void Notify_Finished(object sender, FinishedEventArgs e)
        {
            DateTime now = DateTime.Now;
            TradeTypes selfhelpInpour = TradeTypes.SelfhelpInpour;
            Distributor user = Users.GetUser(InpourRequest.UserId, false) as Distributor;
            decimal num = user.Balance + InpourRequest.InpourBlance;
            BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
            balanceDetails.UserId = InpourRequest.UserId;
            balanceDetails.UserName = user.Username;
            balanceDetails.TradeDate = now;
            balanceDetails.TradeType = selfhelpInpour;
            balanceDetails.Income = new decimal?(InpourRequest.InpourBlance);
            balanceDetails.Balance = num;
            if (paymode != null)
            {
                balanceDetails.Remark = "充值支付方式：" + paymode.Name;
            }
            if (SubsiteStoreHelper.AddBalanceDetail(balanceDetails, InpourId))
            {
                Users.ClearUserCache(user);
                ResponseStatus(true, "success");
            }
            else
            {
                SubsiteStoreHelper.RemoveInpourRequest(InpourId);
                ResponseStatus(false, "fail");
            }
        }

        void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
            ResponseStatus(false, "verifyfaild");
        }

        void Notify_Payment(object sender, EventArgs e)
        {
            ResponseStatus(false, "waitconfirm");
        }

        void ResponseStatus(bool success, string status)
        {
            if (isBackRequest)
            {
                Notify.WriteBack(Context, success);
            }
            else
            {
                DisplayMessage(status);
            }
        }
    }
}

