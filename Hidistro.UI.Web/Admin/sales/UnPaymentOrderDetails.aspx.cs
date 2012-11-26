using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public partial class UnPaymentOrderDetails : OrderDetailsBasePage
    {

        public UnPaymentOrderDetails()
            : base(OrderStatus.WaitBuyerPay)
        {
        }

        private void BindRemark()
        {
            spanOrderId.Text = Order.OrderId;
            lblorderDateForRemark.Time = Order.OrderDate;
            lblorderTotalForRemark.Money = Order.GetTotal();
            txtRemark.Text = Globals.HtmlDecode(Order.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = Order.ManagerMark;
        }

        private void BindUpdateSippingAddress()
        {
            txtShipTo.Text = Globals.HtmlDecode(Order.ShipTo);
            dropRegions.SetSelectedRegionId(new int?(Order.RegionId));
            txtAddress.Text = Globals.HtmlDecode(Order.Address);
            txtZipcode.Text = Order.ZipCode;
            txtTelPhone.Text = Order.TelPhone;
            txtCellPhone.Text = Order.CellPhone;
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            Order.CloseReason = ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(Order))
            {
                int userId = Order.UserId;
                if (userId == 0x44c)
                {
                    userId = 0;
                }
                Messenger.OrderClosed(Users.GetUser(userId), Order.OrderId, Order.CloseReason);
                Order.OnClosed();
                ShowMsg("关闭订单成功", true);
                Page.Response.Redirect(Globals.ApplicationPath + "/Admin/sales/ClosedOrderDetails.aspx?OrderId=" + Order.OrderId);
            }
            else
            {
                ShowMsg("关闭订单失败", false);
            }
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            Order.ShipTo = Globals.HtmlEncode(txtShipTo.Text.Trim());
            if (!dropRegions.GetSelectedRegionId().HasValue)
            {
                ShowMsg("收货人地址必选", false);
            }
            else
            {
                Order.RegionId = dropRegions.GetSelectedRegionId().Value;
                Order.Address = Globals.HtmlEncode(txtAddress.Text.Trim());
                Order.TelPhone = txtTelPhone.Text.Trim();
                Order.CellPhone = txtCellPhone.Text.Trim();
                Order.ZipCode = txtZipcode.Text.Trim();
                Order.ShippingRegion = dropRegions.SelectedRegions;
                if (string.IsNullOrEmpty(txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(txtCellPhone.Text.Trim()))
                {
                    ShowMsg("电话号码和手机号码必填其一", false);
                }
                else if (OrderHelper.MondifyAddress(Order))
                {
                    shippingAddress.LoadControl();
                    BindUpdateSippingAddress();
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
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(ddlpayment.SelectedValue.Value);
            Order.PaymentTypeId = paymentMode.ModeId;
            Order.PaymentType = paymentMode.Name;
            if (OrderHelper.UpdateOrderPaymentType(Order))
            {
                chargesList.LoadControls();
                ShowMsg("修改支付方式成功", true);
            }
            else
            {
                ShowMsg("修改支付方式失败", false);
            }
        }

        private void btnMondifyShip_Click(object sender, EventArgs e)
        {
            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(ddlshippingMode.SelectedValue.Value, false);
            Order.ShippingModeId = shippingMode.ModeId;
            Order.ModeName = shippingMode.Name;
            Order.ExpressCompanyName = shippingMode.ExpressCompanyName;
            Order.ExpressCompanyAbb = shippingMode.ExpressCompanyAbb;
            if (OrderHelper.UpdateOrderShippingMode(Order))
            {
                chargesList.LoadControls();
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
                Order.OrderId = spanOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    Order.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                Order.ManagerRemark = Globals.HtmlEncode(txtRemark.Text);
                if (OrderHelper.SaveRemark(Order))
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
            lkbtnEditPrice.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/EditOrder.aspx?OrderId=" + Order.OrderId;
        }

        private void LoadUserControl()
        {
            itemsList.Order = Order;
            userInfo.UserId = Order.UserId;
            userInfo.Order = Order;
            chargesList.Order = Order;
            shippingAddress.Order = Order;
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
                BindUpdateSippingAddress();
                BindRemark();
                ddlshippingMode.DataBind();
                ddlshippingMode.SelectedValue = new int?(Order.ShippingModeId);
                ddlpayment.DataBind();
                ddlpayment.SelectedValue = new int?(Order.PaymentTypeId);
                if (Order.Gifts.Count > 0)
                {
                    hlkOrderGifts.Text = "编辑订单礼品";
                }
                hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/OrderGifts.aspx?OrderId=" + Order.OrderId;
            }
        }
    }
}

