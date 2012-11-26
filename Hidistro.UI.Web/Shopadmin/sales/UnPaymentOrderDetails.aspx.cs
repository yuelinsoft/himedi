using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnPaymentOrderDetails : OrderDetailsBasePage
    {


        public UnPaymentOrderDetails()
            : base(OrderStatus.WaitBuyerPay)
        {
        }

        private void BindRemark()
        {
            spanOrderId.Text = base.Order.OrderId;
            lblorderDateForRemark.Time = base.Order.OrderDate;
            lblorderTotalForRemark.Money = base.Order.GetTotal();
            txtRemark.Text = Globals.HtmlDecode(base.Order.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = base.Order.ManagerMark;
        }

        private void BindUpdateSippingAddress()
        {
            txtShipTo.Text = Globals.HtmlDecode(base.Order.ShipTo);
            dropRegions.SetSelectedRegionId(new int?(base.Order.RegionId));
            txtAddress.Text = Globals.HtmlDecode(base.Order.Address);
            txtZipcode.Text = base.Order.ZipCode;
            txtTelPhone.Text = base.Order.TelPhone;
            txtCellPhone.Text = base.Order.CellPhone;
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            OrderInfo order = base.Order;
            order.CloseReason = ddlCloseReason.SelectedValue;
            if (SubsiteSalesHelper.CloseTransaction(order))
            {
                int userId = order.UserId;
                if (userId == 0x44c)
                {
                    userId = 0;
                }
                Messenger.OrderClosed(Users.GetUser(userId), order.OrderId, order.CloseReason);
                order.OnClosed();
                ShowMsg("关闭订单成功", true);
            }
            else
            {
                ShowMsg("关闭订单失败", false);
            }
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            OrderInfo order = base.Order;
            order.ShipTo = Globals.HtmlEncode(txtShipTo.Text.Trim());
            if (!dropRegions.GetSelectedRegionId().HasValue)
            {
                ShowMsg("收货人地址必选", false);
            }
            else
            {
                order.RegionId = dropRegions.GetSelectedRegionId().Value;
                order.Address = Globals.HtmlEncode(txtAddress.Text.Trim());
                order.TelPhone = txtTelPhone.Text.Trim();
                order.CellPhone = txtCellPhone.Text.Trim();
                order.ZipCode = txtZipcode.Text.Trim();
                order.ShippingRegion = dropRegions.SelectedRegions;
                if (string.IsNullOrEmpty(txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(txtCellPhone.Text.Trim()))
                {
                    ShowMsg("电话号码和手机号码必填其一", false);
                }
                else if (SubsiteSalesHelper.MondifyAddress(order))
                {
                    shippingAddress.LoadControl();
                    ShowMsg("修改成功", true);
                }
                else
                {
                    ShowMsg("修改失败", false);
                }
            }
        }

        private void btnMondifyPay_Click(object sender, EventArgs e)
        {
            OrderInfo order = base.Order;
            PaymentModeInfo paymentMode = SubsiteSalesHelper.GetPaymentMode(ddlpayment.SelectedValue.Value);
            order.PaymentTypeId = paymentMode.ModeId;
            order.PaymentType = paymentMode.Name;
            if (SubsiteSalesHelper.UpdateOrderPaymentType(order))
            {
                chargesList.LoadControl();
                ddlpayment.SelectedValue = new int?(base.Order.PaymentTypeId);
                ShowMsg("修改支付方式成功", true);
            }
            else
            {
                ShowMsg("修改支付方式失败", false);
            }
        }

        private void btnMondifyShip_Click(object sender, EventArgs e)
        {
            OrderInfo order = base.Order;
            ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(ddlshippingMode.SelectedValue.Value, false);
            order.ShippingModeId = shippingMode.ModeId;
            order.ModeName = shippingMode.Name;
            base.Order.ExpressCompanyName = shippingMode.ExpressCompanyName;
            base.Order.ExpressCompanyAbb = shippingMode.ExpressCompanyAbb;
            if (SubsiteSalesHelper.UpdateOrderShippingMode(order))
            {
                chargesList.LoadControl();
                ddlshippingMode.SelectedValue = new int?(base.Order.ShippingModeId);
                shippingAddress.LoadControl();
                ShowMsg("修改配送方式成功", true);
            }
            else
            {
                ShowMsg("修改配送方式失败", false);
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Length > 300)
            {
                ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                base.Order.OrderId = spanOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    base.Order.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                base.Order.ManagerRemark = Globals.HtmlEncode(txtRemark.Text.Trim());
                if (SubsiteSalesHelper.SaveRemark(base.Order))
                {
                    BindRemark();
                    ShowMsg("保存备忘录成功", true);
                }
                else
                {
                    ShowMsg("保存失败", false);
                }
            }
        }

        private void LoadControl()
        {
            lkbtnEditPrice.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/EditMyOrder.aspx?OrderId=" + base.Order.OrderId;
        }

        private void LoadUserControl()
        {
            itemsList.Order = base.Order;
            userInfo.UserId = base.Order.UserId;
            userInfo.Order = base.Order;
            chargesList.Order = base.Order;
            shippingAddress.Order = base.Order;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
            btnMondifyPay.Click += new EventHandler(btnMondifyPay_Click);
            btnMondifyShip.Click += new EventHandler(btnMondifyShip_Click);
            btnCloseOrder.Click += new EventHandler(btnCloseOrder_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!Page.IsPostBack)
            {
                LoadControl();
                BindRemark();
                BindUpdateSippingAddress();
                ddlshippingMode.DataBind();
                ddlshippingMode.SelectedValue = new int?(base.Order.ShippingModeId);
                ddlpayment.DataBind();
                ddlpayment.SelectedValue = new int?(base.Order.PaymentTypeId);
                if (base.Order.Gifts.Count > 0)
                {
                    hlkOrderGifts.Text = "编辑订单礼品";
                }
                hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/OrderGifts.aspx?OrderId=" + base.Order.OrderId;
            }
        }
    }
}

