using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{


    public partial class Order_ChargesList : UserControl
    {

        public void LoadControl()
        {
            if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                this.lkBtnEditPayMode.Visible = true;
                this.lkBtnEditshipingMode.Visible = true;
            }
            this.litDiscountValue.Text = "-" + Globals.FormatMoney(this.order.GetDiscountAmount());
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if ((!string.IsNullOrEmpty(this.order.DiscountName) && (this.order.DiscountValue > 0M)) && Enum.IsDefined(typeof(DiscountValueType), this.order.DiscountValueType))
            {
                this.litDiscountActivity.Text = this.order.DiscountName;
                this.litDiscountActivity.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), this.order.DiscountId);
                this.litDiscountActivity.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + string.Format("/FavourableDetails.aspx?activityId={0}", this.order.DiscountId);
            }
            else
            {
                this.litDiscountValue.Text = "暂无";
            }
            this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
            this.litPayCharge.Text = Globals.FormatMoney(this.order.AdjustedPayCharge);
            this.litOptionPrice.Text = Globals.FormatMoney(this.order.GetOptionPrice());
            if (!string.IsNullOrEmpty(this.order.ActivityName))
            {
                this.litActivity.Text = this.order.ActivityName;
                this.litActivity.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + string.Format("/FavourableDetails.aspx?activityId={0}", this.order.ActivityId);
            }
            else
            {
                this.litActivity.Text = "暂无";
                this.litActivity.Enabled = false;
            }
            if (!string.IsNullOrEmpty(this.order.CouponName))
            {
                this.litCoupon.Text = "[" + this.order.CouponName + "]-" + Globals.FormatMoney(this.order.CouponValue);
            }
            else
            {
                this.litCoupon.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
            }
            this.litCouponValue.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
            this.litDiscount.Text = Globals.FormatMoney(this.order.AdjustedDiscount);
            if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                this.litPoints.Text = this.order.GetTotalPoints().ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                this.litPoints.Text = this.order.Points.ToString(CultureInfo.InvariantCulture);
            }
            this.litTotalPrice.Text = Globals.FormatMoney(this.order.GetTotal());
            string str = string.Empty;
            if (this.order.OrderOptions.Count > 0)
            {
                foreach (OrderOptionInfo info in this.order.OrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
            }
            this.litOderItem.Text = str;
        }


        protected override void OnLoad(EventArgs e)
        {

            this.LoadControl();

        }

        OrderInfo order = null;
        public OrderInfo Order
        {

            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }

        }


    }



}

