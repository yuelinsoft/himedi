using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.PurchaseorderSendGood)]
    public partial class SendPurchaseOrderGoods : AdminPage
    {
        string purchaseorderId;

        private void BindCharges(PurchaseOrderInfo purchaseOrder)
        {
            lblModeName.Text = purchaseOrder.ModeName;
            litFreight.Text = Globals.FormatMoney(purchaseOrder.AdjustedFreight);
            litOptionPrice.Text = Globals.FormatMoney(purchaseOrder.GetOptionPrice());
            litDiscount.Text = Globals.FormatMoney(purchaseOrder.AdjustedDiscount);
            litTotalPrice.Text = Globals.FormatMoney(purchaseOrder.GetPurchaseTotal());
            string str = string.Empty;
            if (purchaseOrder.PurchaseOrderOptions.Count > 0)
            {
                foreach (PurchaseOrderOptionInfo info in purchaseOrder.PurchaseOrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                litOderItem.Text = "（" + str + "）";
            }
        }

        private void BindExpressCompany(int modeId)
        {
            expressRadioButtonList.ExpressCompany = SalesHelper.GetExpressCompanysByMode(modeId);
            expressRadioButtonList.BindSelectExpressCompany();
        }

        private void BindShippingAddress(PurchaseOrderInfo purchaseorder)
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(purchaseorder.ShippingRegion))
            {
                shippingRegion = purchaseorder.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(purchaseorder.Address))
            {
                shippingRegion = shippingRegion + purchaseorder.Address;
            }
            if (!string.IsNullOrEmpty(purchaseorder.ZipCode))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.ZipCode;
            }
            if (!string.IsNullOrEmpty(purchaseorder.ShipTo))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.ShipTo;
            }
            if (!string.IsNullOrEmpty(purchaseorder.TelPhone))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.TelPhone;
            }
            if (!string.IsNullOrEmpty(purchaseorder.CellPhone))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.CellPhone;
            }
            litReceivingInfo.Text = shippingRegion;
        }

        private void BindUpdateSippingAddress(PurchaseOrderInfo purchaseorder)
        {
            txtShipTo.Text = purchaseorder.ShipTo;
            dropRegions.SetSelectedRegionId(new int?(purchaseorder.RegionId));
            txtAddress.Text = purchaseorder.Address;
            txtZipcode.Text = purchaseorder.ZipCode;
            txtTelPhone.Text = purchaseorder.TelPhone;
            txtCellPhone.Text = purchaseorder.CellPhone;
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseorderId);
            purchaseOrder.ShipTo = txtShipTo.Text.Trim();
            purchaseOrder.RegionId = dropRegions.GetSelectedRegionId().Value;
            purchaseOrder.Address = txtAddress.Text.Trim();
            purchaseOrder.TelPhone = txtTelPhone.Text.Trim();
            purchaseOrder.CellPhone = txtCellPhone.Text.Trim();
            purchaseOrder.ZipCode = txtZipcode.Text.Trim();
            purchaseOrder.ShippingRegion = dropRegions.SelectedRegions;
            if (SalesHelper.SavePurchaseOrderShippingAddress(purchaseOrder))
            {
                PurchaseOrderInfo purchaseorder = SalesHelper.GetPurchaseOrder(purchaseorderId);
                BindShippingAddress(purchaseorder);
                ShowMsg("修改成功", true);
            }
            else
            {
                ShowMsg("修改失败", false);
            }
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtShipOrderNumber.Text.Trim()))
            {
                ShowMsg("请填写运单号", false);
            }
            else
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseorderId);
                if (purchaseOrder != null)
                {
                    if (purchaseOrder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid)
                    {
                        ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                    }
                    else if (!radioShippingMode.SelectedValue.HasValue)
                    {
                        ShowMsg("请选择配送方式", false);
                    }
                    else if (string.IsNullOrEmpty(expressRadioButtonList.SelectedValue))
                    {
                        ShowMsg("请选择物流配送公司", false);
                    }
                    else
                    {
                        ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(radioShippingMode.SelectedValue.Value, true);
                        purchaseOrder.RealShippingModeId = radioShippingMode.SelectedValue.Value;
                        purchaseOrder.RealModeName = shippingMode.Name;
                        purchaseOrder.ExpressCompanyAbb = expressRadioButtonList.SelectedValue;
                        purchaseOrder.ExpressCompanyName = expressRadioButtonList.SelectedItem.Text;
                        purchaseOrder.ShipOrderNumber = txtShipOrderNumber.Text;
                        if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
                        {
                            ShowMsg("发货成功", true);
                        }
                        else
                        {
                            ShowMsg("发货失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseorderId = Page.Request.QueryString["PurchaseOrderId"];
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseorderId);
                if (purchaseOrder == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    itemsList.PurchaseOrder = purchaseOrder;
                    userInfo.PurchaseOrder = purchaseOrder;
                    userInfo.DistributorId = purchaseOrder.DistributorId;
                    btnSendGoods.Click += new EventHandler(btnSendGoods_Click);
                    btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
                    radioShippingMode.SelectedIndexChanged += new EventHandler(radioShippingMode_SelectedIndexChanged);
                    if (!Page.IsPostBack)
                    {
                        radioShippingMode.DataBind();
                        radioShippingMode.SelectedValue = new int?(purchaseOrder.ShippingModeId);
                        BindExpressCompany(purchaseOrder.ShippingModeId);
                        expressRadioButtonList.ExpressCompanyAbb = purchaseOrder.ExpressCompanyAbb;
                        expressRadioButtonList.ExpressCompanyName = purchaseOrder.ExpressCompanyName;
                        expressRadioButtonList.SelectedValue = purchaseOrder.RealShippingModeId.ToString();
                        BindUpdateSippingAddress(purchaseOrder);
                        BindShippingAddress(purchaseOrder);
                        BindCharges(purchaseOrder);
                        litShippingModeName.Text = purchaseOrder.ModeName;
                        litRemark.Text = purchaseOrder.Remark;
                        dropRegions.SetSelectedRegionId(new int?(purchaseOrder.RegionId));
                    }
                }
            }
        }

        private void radioShippingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioShippingMode.SelectedValue.HasValue)
            {
                BindExpressCompany(radioShippingMode.SelectedValue.Value);
            }
        }
    }
}

