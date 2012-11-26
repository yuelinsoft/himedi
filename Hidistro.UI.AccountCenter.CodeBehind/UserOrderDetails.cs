namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserOrderDetails : MemberTemplatedWebControl
    {
       GridView grdOrderGift;
       GridView grdOrderOption;
       FormatedMoneyLabel lblAdjustedDiscount;
       FormatedMoneyLabel lblCartMoney;
       FormatedMoneyLabel lblDiscount;
       FormatedMoneyLabel lblFreight;
       FormatedMoneyLabel lblOptionPrice;
       OrderStatusLabel lblOrderStatus;
       FormatedMoneyLabel lblPayCharge;
       FormatedMoneyLabel lblTotalBalance;
       FormatedMoneyLabel lbltotalPrice;
       FormatedTimeLabel litAddDate;
       Literal litAddress;
       Literal litCloseReason;
       Literal litCouponValue;
       HyperLink litDiscountName;
       Literal litEmail;
       Literal litFree;
       HyperLink litFreeName;
       Literal litModeName;
       Literal litOrderId;
       Literal litPaymentType;
       Literal litPhone;
       Literal litPoints;
       Literal litRealModeName;
       Literal litRefundOrderRemark;
       Literal litRegion;
       Literal litRemark;
       Literal litShippNumber;
       Literal litShipTo;
       Literal litTellPhone;
       FormatedMoneyLabel litTotalPrice;
       Literal litUserAddress;
       Literal litUserEmail;
       Literal litUserMSN;
       Literal litUserName;
       Literal litUserPhone;
       Literal litUserQQ;
       Literal litUserTellPhone;
       Literal litWeight;
       Literal litZipcode;
       string orderId;
       Common_OrderManage_OrderItems orderItems;
       Panel plExpress;
       Panel plOrderGift;
       Panel plOrderOption;
       Panel plOrderSended;
       Panel plRefund;
       HtmlAnchor power;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.lbltotalPrice = (FormatedMoneyLabel) this.FindControl("lbltotalPrice");
            this.litAddDate = (FormatedTimeLabel) this.FindControl("litAddDate");
            this.lblOrderStatus = (OrderStatusLabel) this.FindControl("lblOrderStatus");
            this.litCloseReason = (Literal) this.FindControl("litCloseReason");
            this.litRemark = (Literal) this.FindControl("litRemark");
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litRegion = (Literal) this.FindControl("litRegion");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litZipcode = (Literal) this.FindControl("litZipcode");
            this.litEmail = (Literal) this.FindControl("litEmail");
            this.litPhone = (Literal) this.FindControl("litPhone");
            this.litTellPhone = (Literal) this.FindControl("litTellPhone");
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.litUserAddress = (Literal) this.FindControl("litUserAddress");
            this.litUserEmail = (Literal) this.FindControl("litUserEmail");
            this.litUserPhone = (Literal) this.FindControl("litUserPhone");
            this.litUserTellPhone = (Literal) this.FindControl("litUserTellPhone");
            this.litUserQQ = (Literal) this.FindControl("litUserQQ");
            this.litUserMSN = (Literal) this.FindControl("litUserMSN");
            this.litPaymentType = (Literal) this.FindControl("litPaymentType");
            this.litModeName = (Literal) this.FindControl("litModeName");
            this.plOrderOption = (Panel) this.FindControl("plOrderOption");
            this.grdOrderOption = (GridView) this.FindControl("grdOrderOption");
            this.plOrderSended = (Panel) this.FindControl("plOrderSended");
            this.litRealModeName = (Literal) this.FindControl("litRealModeName");
            this.litShippNumber = (Literal) this.FindControl("litShippNumber");
            this.litDiscountName = (HyperLink) this.FindControl("litDiscountName");
            this.lblAdjustedDiscount = (FormatedMoneyLabel) this.FindControl("lblAdjustedDiscount");
            this.litFreeName = (HyperLink) this.FindControl("litFreeName");
            this.plExpress = (Panel) this.FindControl("plExpress");
            this.power = (HtmlAnchor) this.FindControl("power");
            this.orderItems = (Common_OrderManage_OrderItems) this.FindControl("Common_OrderManage_OrderItems");
            this.grdOrderGift = (GridView) this.FindControl("grdOrderGift");
            this.plOrderGift = (Panel) this.FindControl("plOrderGift");
            this.lblCartMoney = (FormatedMoneyLabel) this.FindControl("lblCartMoney");
            this.litPoints = (Literal) this.FindControl("litPoints");
            this.litWeight = (Literal) this.FindControl("litWeight");
            this.litFree = (Literal) this.FindControl("litFree");
            this.lblFreight = (FormatedMoneyLabel) this.FindControl("lblFreight");
            this.lblPayCharge = (FormatedMoneyLabel) this.FindControl("lblPayCharge");
            this.lblOptionPrice = (FormatedMoneyLabel) this.FindControl("lblOptionPrice");
            this.litCouponValue = (Literal) this.FindControl("litCouponValue");
            this.lblDiscount = (FormatedMoneyLabel) this.FindControl("lblDiscount");
            this.litTotalPrice = (FormatedMoneyLabel) this.FindControl("litTotalPrice");
            this.plRefund = (Panel) this.FindControl("plRefund");
            this.lblTotalBalance = (FormatedMoneyLabel) this.FindControl("lblTotalBalance");
            this.litRefundOrderRemark = (Literal) this.FindControl("litRefundOrderRemark");
            PageTitle.AddTitle("订单详细页", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                this.BindOrderBase(orderInfo);
                this.BindOrderAddress(orderInfo);
                this.BindOrderItems(orderInfo);
                this.BindOrderRefund(orderInfo);
            }
        }

       void BindOrderAddress(OrderInfo order)
        {
            this.litShipTo.Text = order.ShipTo;
            this.litRegion.Text = order.ShippingRegion;
            this.litAddress.Text = order.Address;
            this.litZipcode.Text = order.ZipCode;
            this.litEmail.Text = order.EmailAddress;
            this.litTellPhone.Text = order.TelPhone;
            this.litPhone.Text = order.CellPhone;
            Member user = HiContext.Current.User as Member;
            this.litUserName.Text = user.Username;
            this.litUserAddress.Text = RegionHelper.GetFullRegion(user.RegionId, "") + user.Address;
            this.litUserTellPhone.Text = user.TelPhone;
            this.litUserPhone.Text = user.CellPhone;
            this.litUserEmail.Text = user.Email;
            this.litUserQQ.Text = user.QQ;
            this.litUserMSN.Text = user.MSN;
            this.litPaymentType.Text = order.PaymentType + "(" + Globals.FormatMoney(order.AdjustedPayCharge) + ")";
            this.litModeName.Text = order.ModeName + "(" + Globals.FormatMoney(order.AdjustedFreight) + ")";
            this.litDiscountName.Text = order.DiscountName;
            this.litDiscountName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.DiscountId);
            this.litDiscountName.Target = "_blank";
            this.litFreeName.Text = order.ActivityName;
            this.litFreeName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.ActivityId);
            this.litFreeName.Target = "_blank";
            if ((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished))
            {
                this.plOrderSended.Visible = true;
                this.litShippNumber.Text = order.ShipOrderNumber;
                this.litRealModeName.Text = order.ExpressCompanyName;
            }
            if (order.OrderOptions.Count > 0)
            {
                this.plOrderOption.Visible = true;
                this.grdOrderOption.DataSource = order.OrderOptions;
                this.grdOrderOption.DataBind();
            }
            if (((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(order.ExpressCompanyAbb))
            {
                if (this.plExpress != null)
                {
                    this.plExpress.Visible = true;
                }
                if ((Express.GetExpressType() == "kuaidi100") && (this.power != null))
                {
                    this.power.Visible = true;
                }
            }
        }

       void BindOrderBase(OrderInfo order)
        {
            this.litOrderId.Text = order.OrderId;
            this.lbltotalPrice.Money = order.GetTotal();
            this.litAddDate.Time = order.OrderDate;
            this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
            if (order.OrderStatus == OrderStatus.Closed)
            {
                this.litCloseReason.Text = order.CloseReason;
            }
            this.litRemark.Text = order.Remark;
        }

       void BindOrderItems(OrderInfo order)
        {
            this.orderItems.DataSource = order.LineItems.Values;
            this.orderItems.DataBind();
            if (order.Gifts.Count > 0)
            {
                this.plOrderGift.Visible = true;
                this.grdOrderGift.DataSource = order.Gifts;
                this.grdOrderGift.DataBind();
            }
            this.lblCartMoney.Money = order.GetAmount();
            this.litWeight.Text = order.Weight.ToString();
            this.lblPayCharge.Money = order.AdjustedPayCharge;
            this.lblOptionPrice.Money = order.GetOptionPrice();
            this.lblFreight.Money = order.AdjustedFreight;
            this.lblAdjustedDiscount.Money = order.AdjustedDiscount;
            this.litCouponValue.Text = order.CouponName + " -" + Globals.FormatMoney(order.CouponValue);
            this.lblDiscount.Money = order.GetAmount() - order.GetDiscountedAmount();
            if (order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                this.litPoints.Text = order.GetTotalPoints().ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                this.litPoints.Text = order.Points.ToString(CultureInfo.InvariantCulture);
            }
            this.litTotalPrice.Money = order.GetTotal();
        }

       void BindOrderRefund(OrderInfo order)
        {
            if ((order.RefundStatus == RefundStatus.Refund) || (order.RefundStatus == RefundStatus.Below))
            {
                this.plRefund.Visible = true;
                this.lblTotalBalance.Money = order.RefundAmount;
                this.litRefundOrderRemark.Text = order.RefundRemark;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserOrderDetails.html";
            }
            base.OnInit(e);
        }
    }
}

