using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class Order_ChargesList : UserControl
    {
        OrderInfo order;

        public void LoadControls()
        {
            if ((order.OrderStatus == OrderStatus.WaitBuyerPay) || (order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
            {
                lkBtnEditshipingMode.Visible = true;
            }
            if (order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                lkBtnEditPayMode.Visible = true;
            }
            litDiscountValue.Text = "-" + Globals.FormatMoney(order.GetDiscountAmount());
            if ((!string.IsNullOrEmpty(order.DiscountName) && (order.DiscountValue > 0M)) && Enum.IsDefined(typeof(DiscountValueType), order.DiscountValueType))
            {
                litDiscountActivity.Text = order.DiscountName;
                litDiscountActivity.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.DiscountId);
            }
            else
            {
                litDiscountActivity.Text = "暂无";
                litDiscountActivity.Enabled = false;
            }
            litFreight.Text = Globals.FormatMoney(order.AdjustedFreight);
            litPayCharge.Text = Globals.FormatMoney(order.AdjustedPayCharge);
            litOptionPrice.Text = Globals.FormatMoney(order.GetOptionPrice());
            if (!string.IsNullOrEmpty(order.ActivityName))
            {
                litActivity.Text = order.ActivityName;
                litActivity.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.ActivityId);
            }
            else
            {
                litActivity.Text = "暂无";
                litActivity.Enabled = false;
            }
            if (!string.IsNullOrEmpty(order.CouponName))
            {
                litCoupon.Text = "[" + order.CouponName + "]-" + Globals.FormatMoney(order.CouponValue);
            }
            else
            {
                litCoupon.Text = "-" + Globals.FormatMoney(order.CouponValue);
            }
            litCouponValue.Text = "-" + Globals.FormatMoney(order.CouponValue);
            litDiscount.Text = Globals.FormatMoney(order.AdjustedDiscount);
            if (order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                litPoints.Text = order.GetTotalPoints().ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                litPoints.Text = order.Points.ToString(CultureInfo.InvariantCulture);
            }
            litTotalPrice.Text = Globals.FormatMoney(order.GetTotal());
            string str = string.Empty;
            if (order.OrderOptions.Count > 0)
            {
                foreach (OrderOptionInfo info in order.OrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
            }
            litOderItem.Text = str;
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadControls();
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

