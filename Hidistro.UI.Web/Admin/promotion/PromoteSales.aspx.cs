using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.PromoteSales)]
    public partial class PromoteSales : AdminPage
    {
        private void BindPromoteSales()
        {
            grdPromoteSales.DataSource = PromoteHelper.GetPromotions();
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
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/AddDiscount.aspx"), true);
                    break;

                case "3":
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/AddFeeFree.aspx"), true);
                    break;

                default:
                    if (!(str == "4"))
                    {
                        if (str == "5")
                        {
                            base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/AddQuantityDiscount.aspx"), true);
                        }
                    }
                    else
                    {
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/AddBuyToSend.aspx"), true);
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int activityId = int.Parse(grdPromoteSales.DataKeys[e.Row.RowIndex].Value.ToString());
                Label label = (Label)e.Row.FindControl("lblmemberGrades");
                Label label2 = (Label)e.Row.FindControl("lblPromoteType");
                Label label3 = (Label)e.Row.FindControl("lblPromoteTypeName");
                Literal literal = (Literal)e.Row.FindControl("ltrPromotionInfo");
                HyperLink link = (HyperLink)e.Row.FindControl("hpkPromotionProduct");
                IList<string> promoteMemberGrades = PromoteHelper.GetPromoteMemberGrades(activityId);
                string str = string.Empty;
                foreach (string str2 in promoteMemberGrades)
                {
                    str = str + str2 + ",";
                }
                str = str.Remove(str.Length - 1);
                label.Text = str;
                if (int.Parse(label2.Text, CultureInfo.InvariantCulture) == 2)
                {
                    label3.Text = "满额打折";
                    FullDiscountInfo fullDiscountInfo = PromoteHelper.GetFullDiscountInfo(activityId);
                    string str3 = (fullDiscountInfo.ValueType == DiscountValueType.Amount) ? "折扣金额" : "百分比折扣";
                    literal.Text = string.Concat(new object[] { "满足金额：", Globals.FormatMoney(fullDiscountInfo.Amount), "&nbsp;折扣类型:", str3, "&nbsp;折扣值：", decimal.Round(decimal.Parse(fullDiscountInfo.DiscountValue.ToString()), 2) });
                }
                if (int.Parse(label2.Text, CultureInfo.InvariantCulture) == 3)
                {
                    FullFreeInfo fullFreeInfo = PromoteHelper.GetFullFreeInfo(activityId);
                    label3.Text = "满额免费用";
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
                if (int.Parse(label2.Text, CultureInfo.InvariantCulture) == 4)
                {
                    label3.Text = "买几送几";
                    PurchaseGiftInfo purchaseGiftInfo = PromoteHelper.GetPurchaseGiftInfo(activityId);
                    literal.Text = string.Concat(new object[] { "购买数量：", purchaseGiftInfo.BuyQuantity, "&nbsp;赠送数量：", purchaseGiftInfo.GiveQuantity });
                    link.Visible = true;
                    link.NavigateUrl = Globals.GetAdminAbsolutePath("/promotion/PromotionProducts.aspx?ActiveId=" + activityId);
                }
                if (int.Parse(label2.Text, CultureInfo.InvariantCulture) == 5)
                {
                    label3.Text = "批发打折";
                    WholesaleDiscountInfo wholesaleDiscountInfo = PromoteHelper.GetWholesaleDiscountInfo(activityId);
                    literal.Text = string.Concat(new object[] { "购买数量：", wholesaleDiscountInfo.Quantity, "&nbsp;折扣值：", wholesaleDiscountInfo.DiscountValue });
                    link.Visible = true;
                    link.NavigateUrl = Globals.GetAdminAbsolutePath("/promotion/PromotionProducts.aspx?ActiveId=" + activityId);
                }
            }
        }

        private void grdPromoteSales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int activityId = (int)grdPromoteSales.DataKeys[e.RowIndex].Value;
            if (PromoteHelper.DeletePromotion(activityId))
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

