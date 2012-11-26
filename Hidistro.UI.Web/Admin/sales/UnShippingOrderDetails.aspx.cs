using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public partial class UnShippingOrderDetails : OrderDetailsBasePage
    {
        public UnShippingOrderDetails()
            : base(OrderStatus.BuyerAlreadyPaid)
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

        private void LoadUserControl()
        {
            userInfo.UserId = Order.UserId;
            userInfo.Order = Order;
            itemsList.Order = Order;
            chargesList.Order = Order;
            shippingAddress.Order = Order;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
            btnMondifyShip.Click += new EventHandler(btnMondifyShip_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!Page.IsPostBack)
            {
                SetControl();
                BindUpdateSippingAddress();
                ddlshippingMode.DataBind();
                BindRemark();
                ddlshippingMode.SelectedValue = new int?(Order.ShippingModeId);
                if (Order.Gifts.Count > 0)
                {
                    hlkOrderGifts.Text = "编辑订单礼品";
                }
                hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/OrderGifts.aspx?OrderId=" + Order.OrderId;
                if (Order.GroupBuyId > 0)
                {
                    lkbtnSendGoods.Visible = Order.GroupBuyStatus == GroupBuyStatus.Success;
                }
            }
        }

        private void SetControl()
        {
            lkbtnSendGoods.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/SendOrderGoods.aspx?OrderId=" + Order.OrderId;
            int userId = Order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            Users.GetUser(userId);
            if (Users.GetUser(userId, false) is Member)
            {
                liRefund.Visible = true;
                hlkRefund.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/RefundOrder.aspx?OrderId=" + Order.OrderId;
            }
        }
    }
}

