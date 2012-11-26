using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
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
    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public partial class UnShippingPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {

        public UnShippingPurchaseOrderDetails()
            : base(OrderStatus.BuyerAlreadyPaid)
        {
        }

        private void BindRemark()
        {
            spanOrderId.Text = purchaseOrder.OrderId;
            spanpurcharseOrderId.Text = purchaseOrder.PurchaseOrderId;
            lblpurchaseDateForRemark.Time = purchaseOrder.PurchaseDate;
            lblpurchaseTotalForRemark.Money = purchaseOrder.GetPurchaseTotal();
            txtRemark.Text = Globals.HtmlDecode(purchaseOrder.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = purchaseOrder.ManagerMark;
        }

        private void BindUpdateSippingAddress()
        {
            txtShipTo.Text = Globals.HtmlDecode(purchaseOrder.ShipTo);
            dropRegions.SetSelectedRegionId(new int?(purchaseOrder.RegionId));
            txtAddress.Text = Globals.HtmlDecode(purchaseOrder.Address);
            txtZipcode.Text = purchaseOrder.ZipCode;
            txtTelPhone.Text = purchaseOrder.TelPhone;
            txtCellPhone.Text = purchaseOrder.CellPhone;
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            purchaseOrder.ShipTo = Globals.HtmlEncode(txtShipTo.Text.Trim());
            if (!dropRegions.GetSelectedRegionId().HasValue)
            {
                ShowMsg("收货人地址必选", false);
            }
            else
            {
                purchaseOrder.RegionId = dropRegions.GetSelectedRegionId().Value;
                purchaseOrder.Address = Globals.HtmlEncode(txtAddress.Text.Trim());
                purchaseOrder.TelPhone = txtTelPhone.Text.Trim();
                purchaseOrder.CellPhone = txtCellPhone.Text.Trim();
                purchaseOrder.ZipCode = txtZipcode.Text.Trim();
                purchaseOrder.ShippingRegion = dropRegions.SelectedRegions;
                if (SalesHelper.SavePurchaseOrderShippingAddress(purchaseOrder))
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
            purchaseOrder.ShippingModeId = shippingMode.ModeId;
            purchaseOrder.ModeName = shippingMode.Name;
            if (SalesHelper.UpdatePurchaseOrderShippingMode(purchaseOrder))
            {
                chargesList.LoadControl();
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
                purchaseOrder.PurchaseOrderId = spanpurcharseOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    purchaseOrder.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                purchaseOrder.ManagerRemark = Globals.HtmlEncode(txtRemark.Text);
                if (SalesHelper.SavePurchaseOrderRemark(purchaseOrder))
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
            itemsList.PurchaseOrder = purchaseOrder;
            chargesList.PurchaseOrder = purchaseOrder;
            shippingAddress.PurchaseOrder = purchaseOrder;
            userInfo.DistributorId = purchaseOrder.DistributorId;
            userInfo.PurchaseOrder = purchaseOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
            btnMondifyShip.Click += new EventHandler(btnMondifyShip_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!IsPostBack)
            {
                BindUpdateSippingAddress();
                BindRemark();
                ddlshippingMode.DataBind();
                lkbtnSendGoods.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/SendPurchaseOrderGoods.aspx?PurchaseOrderId=" + purchaseOrder.PurchaseOrderId;
                if (!(Users.GetUser(purchaseOrder.DistributorId) is Distributor))
                {
                    refundLink.Visible = false;
                }
                else
                {
                    hlkRefund.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/RefundPurchaseOrder.aspx?PurchaseOrderId=" + purchaseOrder.PurchaseOrderId;
                }
            }
        }
    }
}

