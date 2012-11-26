using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyPromoteSales : DistributorPage
    {


        private void BindPromoteSales()
        {
            grdPromoteSales.DataSource = SubsitePromoteHelper.GetPromotions();
            grdPromoteSales.DataBind();
        }

        private void btnAddPromote_Click(object sender, EventArgs e)
        {
            string str = ((int)dropPromoteTypes.SelectedValue).ToString();
            switch (str)
            {
                case null:
                    break;

                case "2":
                    base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/AddMyDiscount.aspx", true);
                    break;

                case "3":
                    base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/AddMyFeeFree.aspx", true);
                    break;

                default:
                    if (!(str == "4"))
                    {
                        if (str == "5")
                        {
                            base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/AddMyQuantityDiscount.aspx", true);
                        }
                    }
                    else
                    {
                        base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/AddMyBuyToSend.aspx", true);
                    }
                    break;
            }
        }

        private void grdPromoteSales_ReBindData(object sender)
        {
            BindPromoteSales();
        }

        private void grdPromoteSales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = int.Parse(grdPromoteSales.DataKeys[e.Row.RowIndex].Value.ToString());
                Label label = (Label)e.Row.FindControl("lblPromoteType");
                Label label2 = (Label)e.Row.FindControl("lblPromoteTypeName");
                Literal literal = (Literal)e.Row.FindControl("ltrPromotionInfo");
                HyperLink link = (HyperLink)e.Row.FindControl("hpkPromotionProduct");
                HyperLink link2 = (HyperLink)e.Row.FindControl("hpkPromotion");
                link2.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + string.Format("/FavourableDetails.aspx?activityId={0}", num);
                link2.Target = "_blank";
                Label label3 = (Label)e.Row.FindControl("lblmemberGrades");
                IList<string> promoteMemberGrades = SubsitePromoteHelper.GetPromoteMemberGrades(num);
                string str = string.Empty;
                foreach (string str2 in promoteMemberGrades)
                {
                    str = str + str2 + ",";
                }
                str = str.Remove(str.Length - 1);
                label3.Text = str;
                if (int.Parse(label.Text, CultureInfo.InvariantCulture) == 2)
                {
                    label2.Text = "满额打折";
                    FullDiscountInfo fullDiscountInfo = SubsitePromoteHelper.GetFullDiscountInfo(num);
                    string str3 = (fullDiscountInfo.ValueType == DiscountValueType.Amount) ? "折扣金额" : "百分比折扣";
                    literal.Text = string.Concat(new object[] { "满足金额：", Globals.FormatMoney(fullDiscountInfo.Amount), "&nbsp;折扣类型:", str3, "&nbsp;折扣值：", decimal.Round(decimal.Parse(fullDiscountInfo.DiscountValue.ToString()), 2) });
                }
                if (int.Parse(label.Text, CultureInfo.InvariantCulture) == 3)
                {
                    FullFreeInfo fullFreeInfo = SubsitePromoteHelper.GetFullFreeInfo(num);
                    label2.Text = "满额免费用";
                    string str4 = "&nbsp;免费项目：";
                    if (fullFreeInfo.OptionFeeFree)
                    {
                        str4 = str4 + "订单选项费,";
                    }
                    if (fullFreeInfo.ServiceChargeFree)
                    {
                        str4 = str4 + "订单支付手续费,";
                    }
                    if (fullFreeInfo.ShipChargeFree)
                    {
                        str4 = str4 + "订单运费,";
                    }
                    literal.Text = "满足金额：" + Globals.FormatMoney(fullFreeInfo.Amount) + str4.Remove(str4.Length - 1);
                }
                if (int.Parse(label.Text, CultureInfo.InvariantCulture) == 4)
                {
                    label2.Text = "买几送几";
                    PurchaseGiftInfo purchaseGiftInfo = SubsitePromoteHelper.GetPurchaseGiftInfo(num);
                    literal.Text = string.Concat(new object[] { "购买数量：", purchaseGiftInfo.BuyQuantity, "&nbsp;赠送数量：", purchaseGiftInfo.GiveQuantity });
                    link.Visible = true;
                    link.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/promotion/MyPromotionProducts.aspx?ActiveId=" + num;
                }
                if (int.Parse(label.Text, CultureInfo.InvariantCulture) == 5)
                {
                    label2.Text = "批发打折";
                    WholesaleDiscountInfo wholesaleDiscountInfo = SubsitePromoteHelper.GetWholesaleDiscountInfo(num);
                    literal.Text = string.Concat(new object[] { "购买数量：", wholesaleDiscountInfo.Quantity, "&nbsp;折扣值：", wholesaleDiscountInfo.DiscountValue });
                    link.Visible = true;
                    link.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/promotion/MyPromotionProducts.aspx?ActiveId=" + num;
                }
            }
        }

        private void grdPromoteSales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int activityId = (int)grdPromoteSales.DataKeys[e.RowIndex].Value;
            if (SubsitePromoteHelper.DeletePromotion(activityId))
            {
                ShowMsg("成功删除了选择的促销活动", true);
                BindPromoteSales();
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdPromoteSales.RowDataBound += new GridViewRowEventHandler(grdPromoteSales_RowDataBound);
            grdPromoteSales.RowDeleting += new GridViewDeleteEventHandler(grdPromoteSales_RowDeleting);
            grdPromoteSales.ReBindData += new Grid.ReBindDataEventHandler(grdPromoteSales_ReBindData);
            btnAddPromote.Click += new EventHandler(btnAddPromote_Click);
            if (!Page.IsPostBack)
            {
                BindPromoteSales();
            }
        }
    }
}

