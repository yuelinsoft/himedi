using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class Order_ItemsList : UserControl
    {

        private void dlstOrderItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HyperLink link = (HyperLink)e.Item.FindControl("hpkBuyToSend");

                HyperLink link2 = (HyperLink)e.Item.FindControl("hpkBuyDiscount");

                Literal literal = (Literal)e.Item.FindControl("litPurchaseGiftId");

                Literal literal2 = (Literal)e.Item.FindControl("litWholesaleDiscountId");

                if (!string.IsNullOrEmpty(literal.Text))
                {
                    link.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + string.Format("/FavourableDetails.aspx?activityId={0}", literal.Text);
                }

                if (!string.IsNullOrEmpty(literal2.Text))
                {
                    link2.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + string.Format("/FavourableDetails.aspx?activityId={0}", literal2.Text);
                }

            }

        }


        protected override void OnLoad(EventArgs e)
        {
            dlstOrderItems.ItemDataBound += new DataListItemEventHandler(dlstOrderItems_ItemDataBound);

            dlstOrderItems.DataSource = order.LineItems.Values;

            dlstOrderItems.DataBind();

            if (order.Gifts.Count == 0)
            {

                grdOrderGift.Visible = false;

                lblOrderGifts.Visible = false;

            }
            else
            {
                grdOrderGift.DataSource = order.Gifts;

                grdOrderGift.DataBind();

            }

            litGoodsAmount.Text = Globals.FormatMoney(order.GetAmount());

            lblWeight.Text = order.Weight.ToString(CultureInfo.InvariantCulture);

        }

        OrderInfo order;
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

