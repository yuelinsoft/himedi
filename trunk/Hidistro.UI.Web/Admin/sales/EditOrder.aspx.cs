using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    //修改订单价格
    [PrivilegeCheck(Privilege.EditOrders)]
    public partial class EditOrder : AdminPage
    {
        OrderInfo order;
        string orderId;

        //页面加载
        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {

                base.GotoResourceNotFound();

            }
            else
            {

                orderId = Page.Request.QueryString["OrderId"];
                order   = OrderHelper.GetOrderInfo(orderId);

                if (!Page.IsPostBack)
                {

                    if (order == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        BindProductList(order);
                        BindOtherAmount(order);
                        BindTatolAmount(order);
                    }

                }

            }

        }

        private void BindOtherAmount(OrderInfo order)
        {
            if (order.ProcedureFeeFree)
            {
                txtAdjustedPayCharge.ReadOnly = true;
            }
            if (order.EightFree)
            {
                txtAdjustedFreight.ReadOnly = true;
            }
            txtAdjustedFreight.Text = order.AdjustedFreight.ToString("F", CultureInfo.InvariantCulture);
            txtAdjustedPayCharge.Text = order.AdjustedPayCharge.ToString("F", CultureInfo.InvariantCulture);
            oderItemAmount.Text = Globals.FormatMoney(order.GetOptionPrice());
            txtAdjustedDiscount.Text = order.AdjustedDiscount.ToString("F", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(order.PaymentType))
            {
                litPayName.Text = "(" + order.PaymentType + ")";
            }
            if (!string.IsNullOrEmpty(order.ModeName))
            {
                litShipModeName.Text = "(" + order.ModeName + ")";
            }
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
            fullDiscountAmount.Text = "-" + Globals.FormatMoney(order.GetDiscountAmount());
            if ((!string.IsNullOrEmpty(order.DiscountName) && (order.DiscountValue > 0M)) && Enum.IsDefined(typeof(DiscountValueType), order.DiscountValueType))
            {
                lkbtnFullDiscount.Text = order.DiscountName;
                lkbtnFullDiscount.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.DiscountId);
            }
            else
            {
                fullDiscountAmount.Text = "暂无";
                lkbtnFullDiscount.Enabled = false;
            }
            if (!string.IsNullOrEmpty(order.ActivityName))
            {
                lkbtnFullFree.Text = order.ActivityName;
                lkbtnFullFree.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), order.ActivityId);
            }
            else
            {
                lkbtnFullFree.Text = "暂无";
                lkbtnFullFree.Enabled = false;
            }
            if (!string.IsNullOrEmpty(order.CouponName))
            {
                couponAmount.Text = "[" + order.CouponName + "]-" + Globals.FormatMoney(order.CouponValue);
            }
            else
            {
                couponAmount.Text = "-" + Globals.FormatMoney(order.CouponValue);
            }
        }

        private void BindProductList(OrderInfo order)
        {
            grdProducts.DataSource = order.LineItems.Values;
            grdProducts.DataBind();
            grdOrderGift.DataSource = order.Gifts;
            grdOrderGift.DataBind();
        }

        private void BindTatolAmount(OrderInfo order)
        {
            decimal amount = order.GetAmount();
            lblAllPrice.Money = amount;
            lblWeight.Text = order.Weight.ToString(CultureInfo.InvariantCulture);
            litIntegral.Text = order.GetTotalPoints().ToString(CultureInfo.InvariantCulture);
            litTotal.Text = Globals.FormatMoney(order.GetTotal());
        }

        /// <summary>
        /// 修改订单金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateOrderAmount_Click(object sender, EventArgs e)
        {
            if (!order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else
            {
                decimal AdjustedFreight = 0m;
                decimal AdjustedPayCharge =0m;
                decimal AdjustedDiscount = 0m;

                if (ValidateValues(out AdjustedFreight, out AdjustedPayCharge, out AdjustedDiscount))
                {
                    string msg = string.Empty;

                    order.AdjustedFreight   = AdjustedFreight;
                    order.AdjustedPayCharge = AdjustedPayCharge;
                    order.AdjustedDiscount  = AdjustedDiscount;

                    decimal total = order.GetTotal();

                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<OrderInfo>(order, new string[] { "ValOrder" });
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                            ShowMsg(msg, false);
                            return;
                        }
                    }
                    if (total > 0M)
                    {
                        if (OrderHelper.UpdateOrderAmount(order))
                        {
                            BindTatolAmount(order);
                            ShowMsg("成功的修改了订单金额", true);
                        }
                        else
                        {
                            ShowMsg("修改订单金额失败", false);
                        }
                    }
                    else
                    {
                        ShowMsg("订单的应付总金额不应该是负数,请重新输入订单折扣", false);
                    }
                }
            }
        }

        //删除礼品
        protected void grdOrderGift_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else
            {
                int giftId = (int)grdOrderGift.DataKeys[e.RowIndex].Value;
                if (OrderHelper.DeleteOrderGift(order, giftId))
                {
                    order = OrderHelper.GetOrderInfo(orderId);
                    BindProductList(order);
                    BindTatolAmount(order);
                    ShowMsg("成功删除了一件礼品", true);
                }
                else
                {
                    ShowMsg("删除礼品失败", false);
                }
            }
        }

        /// <summary>
        /// 行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else if (e.CommandName == "setQuantity")
            {
                int num;
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                string skuId = grdProducts.DataKeys[rowIndex].Value.ToString();
                TextBox box = grdProducts.Rows[rowIndex].FindControl("txtQuantity") as TextBox;
                if (!int.TryParse(box.Text.Trim(), out num))
                {
                    ShowMsg("商品数量填写错误", false);
                }
                else if (num > OrderHelper.GetSkuStock(skuId))
                {
                    ShowMsg("此商品库存不够", false);
                }
                else if (num <= 0)
                {
                    ShowMsg("商品购买数量不能为0", false);
                }
                else if (order.LineItems[skuId].Quantity != num)
                {
                    if (OrderHelper.UpdateLineItem(skuId, order, num))
                    {
                        BindProductList(order);
                        BindTatolAmount(order);
                        ShowMsg("修改商品购买数量成功", true);
                    }
                    else
                    {
                        ShowMsg("修改商品购买数量失败", false);
                    }
                }
            }
        }

        //删除产品
        protected void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else if (order.LineItems.Values.Count <= 1)
            {
                ShowMsg("订单的最后一件商品不允许删除", false);
            }
            else if (OrderHelper.DeleteLineItem(grdProducts.DataKeys[e.RowIndex].Value.ToString(), order))
            {
                order = OrderHelper.GetOrderInfo(orderId);
                BindProductList(order);
                BindTatolAmount(order);
                ShowMsg("成功删除了一件商品", true);
            }
            else
            {
                ShowMsg("删除商品失败", false);
            }
        }



        private bool ValidateValues(out decimal adjustedFreight, out decimal adjustedPayCharge, out decimal discountAmout)
        {
            string str = string.Empty;
            if (!decimal.TryParse(txtAdjustedFreight.Text.Trim(), out adjustedFreight))
            {
                str = str + Formatter.FormatErrorMessage("运费必须在0-1000万之间");
            }
            if (!decimal.TryParse(txtAdjustedPayCharge.Text.Trim(), out adjustedPayCharge))
            {
                str = str + Formatter.FormatErrorMessage("支付费用必须在0-1000万之间");
            }
            int length = 0;
            if (txtAdjustedDiscount.Text.Trim().IndexOf(".") > 0)
            {
                length = txtAdjustedDiscount.Text.Trim().Substring(txtAdjustedDiscount.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!(decimal.TryParse(txtAdjustedDiscount.Text.Trim(), out discountAmout) && (length <= 2)))
            {
                str = str + Formatter.FormatErrorMessage("订单折扣填写错误,订单折扣只能是数值，且不能超过2位小数");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

