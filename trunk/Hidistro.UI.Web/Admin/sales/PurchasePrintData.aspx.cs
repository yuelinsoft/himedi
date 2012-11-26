using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{

    [PrivilegeCheck(Privilege.ExpressPrint)]
    public partial class PurchasePrintData : AdminPage
    {
        string purchaseOrderId;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ddlTemplates.SelectedValue == "")
            {
                ShowMsg("请选择一个快递单模板", false);
            }
            else
            {
                UpdateAddress();
                string url = string.Format("print.html?ShipperId={0}&PurchaseOrderId={1}&XmlFile={2}", ddlShoperTag.SelectedValue.ToString(CultureInfo.InvariantCulture), purchaseOrderId, ddlTemplates.SelectedValue);
                base.Response.Redirect(url);
            }
        }

        private void btnUpdateAddrdss_Click(object sender, EventArgs e)
        {
            if (UpdateAddress())
            {
                ShowMsg("修改成功", true);
            }
            else
            {
                ShowMsg("修改失败,请确认信息填写正确或采购单未发货", false);
            }
        }

        private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadShipper();
        }

        private void LoadShipper()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                litShipperName.Text = shipper.ShipperName;
                litShipperZipcode.Text = shipper.Zipcode;
                litShipperTelphone.Text = shipper.TelPhone;
                litShipperCellphone.Text = shipper.CellPhone;
                litShipperAddress.Text = shipper.Address;
                litRegion.Text = RegionHelper.GetFullRegion(shipper.RegionId, "-");
            }
        }

        private void LoadTemplates()
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
                foreach (DataRow row in isUserExpressTemplates.Rows)
                {
                    ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
                }
                pnlEmptyTemplates.Visible = false;
                pnlTemplates.Visible = true;
            }
            else
            {
                pnlEmptyTemplates.Visible = true;
                pnlTemplates.Visible = false;
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
                purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
                ddlShoperTag.AutoPostBack = true;
                ddlShoperTag.SelectedIndexChanged += new EventHandler(ddlShoperTag_SelectedIndexChanged);
                btnUpdateAddrdss.Click += new EventHandler(btnUpdateAddrdss_Click);
                btnPrint.Click += new EventHandler(btnPrint_Click);
                if (!Page.IsPostBack)
                {
                    PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                    txtShipTo.Text = purchaseOrder.ShipTo;
                    txtEmail.Text = purchaseOrder.DistributorEmail;
                    dropRegions.SetSelectedRegionId(new int?(purchaseOrder.RegionId));
                    txtAddress.Text = purchaseOrder.Address;
                    txtZipcode.Text = purchaseOrder.ZipCode;
                    txtTelphone.Text = purchaseOrder.TelPhone;
                    txtCellphone.Text = purchaseOrder.CellPhone;
                    litModeName.Text = purchaseOrder.ModeName;
                    ddlShoperTag.DataBind();
                    IList<ShippersInfo> shippers = SalesHelper.GetShippers(true);
                    foreach (ShippersInfo info2 in shippers)
                    {
                        if (info2.IsDefault)
                        {
                            ddlShoperTag.SelectedValue = info2.ShipperId;
                        }
                        if (info2.DistributorUserId == purchaseOrder.DistributorId)
                        {
                            ddlShoperTag.SelectedValue = info2.ShipperId;
                            break;
                        }
                    }
                    if (shippers.Count > 0)
                    {
                        LoadShipper();
                        pnlSender.Visible = true;
                        pnlEmptySender.Visible = false;
                    }
                    else
                    {
                        pnlSender.Visible = false;
                        pnlEmptySender.Visible = true;
                        btnPrint.Visible = false;
                    }
                    LoadTemplates();
                }
            }
        }

        private bool UpdateAddress()
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
            purchaseOrder.ShipTo = txtShipTo.Text.Trim();
            purchaseOrder.RegionId = dropRegions.GetSelectedRegionId().Value;
            purchaseOrder.Address = txtAddress.Text.Trim();
            purchaseOrder.TelPhone = txtTelphone.Text.Trim();
            purchaseOrder.CellPhone = txtCellphone.Text.Trim();
            purchaseOrder.ZipCode = txtZipcode.Text.Trim();
            purchaseOrder.ShippingRegion = dropRegions.SelectedRegions;
            return SalesHelper.SavePurchaseOrderShippingAddress(purchaseOrder);
        }
    }
}

