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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Shippers)]
    public partial class EditShipper : AdminPage
    {

        int shipperId;

        private void btnEditShipper_Click(object sender, EventArgs e)
        {
            ShippersInfo shipper = new ShippersInfo();
            shipper.ShipperId = shipperId;
            shipper.ShipperTag = txtShipperTag.Text.Trim();
            shipper.ShipperName = txtShipperName.Text.Trim();
            if (!ddlReggion.GetSelectedRegionId().HasValue)
            {
                ShowMsg("请选择地区", false);
            }
            else
            {
                shipper.RegionId = ddlReggion.GetSelectedRegionId().Value;
                shipper.Address = txtAddress.Text.Trim();
                shipper.CellPhone = txtCellPhone.Text.Trim();
                shipper.TelPhone = txtTelPhone.Text.Trim();
                shipper.Zipcode = txtZipcode.Text.Trim();
                shipper.Remark = txtRemark.Text.Trim();
                if (ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SalesHelper.UpdateShipper(shipper))
                    {
                        ShowMsg("成功修改了一个发货信息", true);
                    }
                    else
                    {
                        ShowMsg("修改发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["ShipperId"], out shipperId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditShipper.Click += new EventHandler(btnEditShipper_Click);
                if (!Page.IsPostBack)
                {
                    ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
                    if (shipper == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(shipper, false);
                        txtShipperTag.Text = shipper.ShipperTag;
                        txtShipperName.Text = shipper.ShipperName;
                        ddlReggion.SetSelectedRegionId(new int?(shipper.RegionId));
                        txtAddress.Text = shipper.Address;
                        txtCellPhone.Text = shipper.CellPhone;
                        txtTelPhone.Text = shipper.TelPhone;
                        txtZipcode.Text = shipper.Zipcode;
                        txtRemark.Text = shipper.Remark;
                    }
                }
            }
        }

        private bool ValidationShipper(ShippersInfo shipper)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippersInfo>(shipper, new string[] { "Valshipper" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

