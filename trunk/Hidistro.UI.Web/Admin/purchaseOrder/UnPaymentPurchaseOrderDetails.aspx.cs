using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public partial class UnPaymentPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {
       // PurchaseOrder_Charges chargesList;
       // PurchaseOrder_Items itemsList;
       // PurchaseOrder_ShippingAddress shippingAddress;
      //  PurchaseOrder_DistributorInfo userInfo;

        public UnPaymentPurchaseOrderDetails()
            : base(OrderStatus.WaitBuyerPay)
        {
        }

        private void BindEditOrderPrice()
        {
            lblPurchaseOrderAmount.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
            lblPurchaseOrderAmount1.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
            lblPurchaseOrderAmount3.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
        }

        private void BindRemark()
        {
            spanOrderId.Text = base.purchaseOrder.OrderId;
            spanpurcharseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
            lblpurchaseDateForRemark.Time = base.purchaseOrder.PurchaseDate;
            lblpurchaseTotalForRemark.Money = base.purchaseOrder.GetPurchaseTotal();
            txtRemark.Text = Globals.HtmlDecode(base.purchaseOrder.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = base.purchaseOrder.ManagerMark;
        }

        private void BindUpdateSippingAddress()
        {
            txtShipTo.Text = Globals.HtmlDecode(base.purchaseOrder.ShipTo);
            dropRegions.SetSelectedRegionId(new int?(base.purchaseOrder.RegionId));
            txtAddress.Text = Globals.HtmlDecode(base.purchaseOrder.Address);
            txtZipcode.Text = base.purchaseOrder.ZipCode;
            txtTelPhone.Text = base.purchaseOrder.TelPhone;
            txtCellPhone.Text = base.purchaseOrder.CellPhone;
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            base.purchaseOrder.CloseReason = ddlCloseReason.SelectedValue;
            if (SalesHelper.ClosePurchaseOrder(base.purchaseOrder))
            {
                Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId={0}", base.purchaseOrder.PurchaseOrderId));
            }
            else
            {
                ShowMsg("取消采购失败", false);
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(txtPurchaseOrderDiscount.Text.Trim(), out num))
            {
                ShowMsg("请正确填写打折或者涨价金额", false);
            }
            else
            {
                base.purchaseOrder.AdjustedDiscount += num;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseOrderInfo>(base.purchaseOrder, new string[] { "ValPurchaseOrder" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                        ShowMsg(msg, false);
                        return;
                    }
                }
                if (base.purchaseOrder.GetPurchaseTotal() >= 0M)
                {
                    if (SalesHelper.UpdatePurchaseOrderAmount(base.purchaseOrder))
                    {
                        chargesList.LoadControl();
                        lblPurchaseOrderAmount.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
                        lblPurchaseOrderAmount1.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
                        lblPurchaseOrderAmount3.Text = base.purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
                        lblPurchaseOrderAmount2.Text = "0.00";
                        ShowMsg("修改成功", true);
                    }
                    else
                    {
                        ShowMsg("修改失败", false);
                    }
                }
                else
                {
                    ShowMsg("折扣值不能使得采购单总金额为负", false);
                }
            }
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            base.purchaseOrder.ShipTo = Globals.HtmlEncode(txtShipTo.Text.Trim());
            if (!dropRegions.GetSelectedRegionId().HasValue)
            {
                ShowMsg("收货人地址必选", false);
            }
            else
            {
                base.purchaseOrder.RegionId = dropRegions.GetSelectedRegionId().Value;
                base.purchaseOrder.Address = Globals.HtmlEncode(txtAddress.Text.Trim());
                base.purchaseOrder.TelPhone = txtTelPhone.Text.Trim();
                base.purchaseOrder.CellPhone = txtCellPhone.Text.Trim();
                base.purchaseOrder.ZipCode = txtZipcode.Text.Trim();
                base.purchaseOrder.ShippingRegion = dropRegions.SelectedRegions;
                if (SalesHelper.SavePurchaseOrderShippingAddress(base.purchaseOrder))
                {
                    BindUpdateSippingAddress();
                    shippingAddress.LoadControl();
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
            ShippingModeInfo shippingMode = new ShippingModeInfo();
            shippingMode = SalesHelper.GetShippingMode(ddlshippingMode.SelectedValue.Value, false);
            base.purchaseOrder.ShippingModeId = shippingMode.ModeId;
            base.purchaseOrder.ModeName = shippingMode.Name;
            if (SalesHelper.UpdatePurchaseOrderShippingMode(base.purchaseOrder))
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
                base.purchaseOrder.PurchaseOrderId = spanpurcharseOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    base.purchaseOrder.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                base.purchaseOrder.ManagerRemark = Globals.HtmlEncode(txtRemark.Text);
                if (SalesHelper.SavePurchaseOrderRemark(base.purchaseOrder))
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
            itemsList.PurchaseOrder = base.purchaseOrder;
            chargesList.PurchaseOrder = base.purchaseOrder;
            shippingAddress.PurchaseOrder = base.purchaseOrder;
            userInfo.DistributorId = base.purchaseOrder.DistributorId;
            userInfo.PurchaseOrder = base.purchaseOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnClosePurchaseOrder.Click += new EventHandler(btnClosePurchaseOrder_Click);
            btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
            btnMondifyShip.Click += new EventHandler(btnMondifyShip_Click);
            btnEditOrder.Click += new EventHandler(btnEditOrder_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!base.IsPostBack)
            {
                BindUpdateSippingAddress();
                BindEditOrderPrice();
                BindRemark();
                ddlshippingMode.DataBind();
                ddlshippingMode.SelectedValue = new int?(base.purchaseOrder.ShippingModeId);
            }
        }
    }
}

